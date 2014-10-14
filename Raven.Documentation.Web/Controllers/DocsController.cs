using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Raven.Client.Connection;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Documentation.Web.Helpers;

namespace Raven.Documentation.Web.Controllers
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Web.Mvc;

	using Raven.Abstractions.Data;
	using Raven.Client;
	using Raven.Documentation.Parser;
	using Raven.Documentation.Parser.Data;
	using Raven.Documentation.Web.Models;
	using Raven.Json.Linq;

	public partial class DocsController : BaseController
	{
		public static string DefaultVersion = "3.0";

		public static Language DefaultLanguage = Language.Csharp;

		public DocsController(IDocumentStore store)
			: base(store)
		{
		}

		public virtual ActionResult Search(string language, string version, string value)
		{
			if (string.IsNullOrEmpty(value))
				return RedirectToAction(MVC.Docs.ActionNames.Index, MVC.Docs.Name);

			FieldHighlightings contentHighlighting;
			var results = DocumentSession.Advanced.DocumentQuery<DocumentationPage>()
				.Highlight("TextContent", 128, 1, out contentHighlighting)
				.SetHighlighterTags("<span class='label label-warning'>", "</span>")
				.WhereIn(x => x.Language, new[] { Language.All, CurrentLanguage })
				.AndAlso()
				.WhereEquals(x => x.Version, CurrentVersion)
				.AndAlso()
				.OpenSubclause()
				.Search(x => x.Title, value).Boost(15)
				.OrElse()
				.Search(x => x.TextContent, value)
				.CloseSubclause()
				.Take(30)
				.ToArray();

			var pages =
				results
					.GroupBy(x => x.Key)
					.Select(SelectSearchResult)
					.Select(x => new SearchModel.Result
									 {
										 Key = x.Key,
										 Title = x.Title,
										 //ContentHighlights = contentHighlighting.GetFragments(x.Id.ToLowerInvariant()),
										 Category = x.Category
									 })
					.GroupBy(x => x.Category)
					.ToDictionary(x => x.Key, x => x.Take(10).ToList());

			return View(MVC.Docs.Views.Search, new SearchModel(pages, CurrentLanguage));
		}

		public virtual ActionResult Validate(string language, string version, bool all)
		{
			if (DebugHelper.IsDebug() == false)
				return RedirectToAction(MVC.Docs.ActionNames.Index, MVC.Docs.Name);

			var query = DocumentSession
				.Query<DocumentationPage>()
				.Take(1024);

			if (all == false) 
				query = query.Where(x => x.Version == CurrentVersion);

			var pages = query.ToList();

			var options = new ParserOptions
						{
							PathToDocumentationDirectory = GetDocumentationDirectory(),
							RootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "article/" + CurrentVersion + "/" + CurrentLanguage + "/",
							ImagesUrl = GetImagesUrl()
						};

			var results = new DocumentationValidator(options)
				.ValidateLinks(pages)
				.ToList();

			return View(MVC.Docs.Views.Validate, results);
		}

		public virtual ActionResult Generate(string language, string version, string key, bool all)
		{
			if (DebugHelper.IsDebug() == false)
				return RedirectToAction(MVC.Docs.ActionNames.Index, MVC.Docs.Name);

			var versionsToParse = new List<string>();
			if (all == false)
				versionsToParse.Add(CurrentVersion);

			var parser =
				new DocumentationParser(
					new ParserOptions
						{
							PathToDocumentationDirectory = GetDocumentationDirectory(),
							VersionsToParse = versionsToParse,
							RootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")),
							ImagesUrl = GetImagesUrl()
						});

			foreach (var attachment in DocumentStore.DatabaseCommands.GetAttachments(0, Etag.Empty, 1024))
			{
				DocumentStore.DatabaseCommands.DeleteAttachment(attachment.Key, null);
			}

			DocumentStore
				.DatabaseCommands
				.DeleteByIndex("Raven/DocumentsByEntityName", new IndexQuery { Query = "Tag:DocumentationPages OR Tag:TableOfContents" })
				.WaitForCompletion();

			foreach (var page in parser.Parse())
			{
				DocumentSession.Store(page);

				Parallel.ForEach(
					page.Images,
					image =>
						{
							if (System.IO.File.Exists(image.ImagePath) == false)
								return;

							using (var file = System.IO.File.OpenRead(image.ImagePath))
							{
								DocumentStore.DatabaseCommands.PutAttachment(image.ImageKey, null, file, new RavenJObject());
							}
						});
			}

			foreach (var toc in parser.GenerateTableOfContents())
			{
				DocumentSession.Store(toc);
			}

			DocumentSession.SaveChanges();

			if (string.IsNullOrEmpty(key))
				return RedirectToAction(MVC.Docs.ActionNames.Welcome, MVC.Docs.Name, new { language = language, version = version });

			while (true)
			{
				var stats = DocumentStore.DatabaseCommands.GetStatistics();
				if (stats.StaleIndexes.Any() == false)
					break;

				Thread.Sleep(500);
			}

			return RedirectToAction(
				MVC.Docs.ActionNames.Articles,
				MVC.Docs.Name,
				new { language = CurrentLanguage, version = CurrentVersion, key = key });
		}

		public virtual ActionResult Welcome(string language, string version)
		{
			return View(MVC.Docs.Views.Welcome);
		}

		public virtual ActionResult Index(string language, string version)
		{
			return RedirectToAction(MVC.Docs.ActionNames.Welcome, MVC.Docs.Name, new { language = language, version = version });
		}

		public virtual ActionResult Start(string version, string language)
		{
			var toc = DocumentSession
				.Query<TableOfContents>()
				.First(x => x.Category == Category.Start && x.Version == CurrentVersion);

			return View(MVC.Docs.Views.Start, new PageModel(toc));
		}

		public virtual ActionResult Client(string version, string language)
		{
			var toc = DocumentSession
				.Query<TableOfContents>()
				.First(x => x.Category == Category.ClientApi && x.Version == CurrentVersion);

			return View(MVC.Docs.Views.Client, new PageModel(toc));
		}

		public virtual ActionResult Server(string version, string language)
		{
			var toc = DocumentSession
				.Query<TableOfContents>()
				.First(x => x.Category == Category.Server && x.Version == CurrentVersion);

			return View(MVC.Docs.Views.Server, new PageModel(toc));
		}

		public virtual ActionResult Glossary(string version, string language)
		{
			var toc = DocumentSession
				.Query<TableOfContents>()
				.First(x => x.Category == Category.Glossary && x.Version == CurrentVersion);

			return View(MVC.Docs.Views.Glossary, new PageModel(toc));
		}

		public virtual ActionResult Articles(string version, string language, string key)
		{
			ViewBag.Key = null;

			var indexKey = key + "/index";

			var allPages = DocumentSession
				.Query<DocumentationPage>()
				.Where(x => x.Key == key || x.Key == indexKey)
				.Where(x => x.Version == CurrentVersion)
				.ToList();

			if (allPages.Count == 0)
				return View(MVC.Docs.Views.NotFound);

			var pages = allPages
				.Where(x => x.Language == Language.All || x.Language == CurrentLanguage)
				.ToList();

			var category = allPages
				.Select(x => x.Category)
				.FirstOrDefault();

			var toc = DocumentSession
				.Query<TableOfContents>()
				.First(x => x.Category == category && x.Version == CurrentVersion);

			if (pages.Count == 0)
				return View(MVC.Docs.Views.NotDocumented, new NotDocumentedModel(key, CurrentLanguage, allPages, toc));

			Debug.Assert(pages.Count <= 2);

			ViewBag.Key = key;

			var all = pages.FirstOrDefault(x => x.Language == Language.All);
			if (all != null)
				return View(MVC.Docs.Views.Article, new ArticleModel(all, toc));

			var page = pages.First(x => x.Language != Language.All);
			return View(MVC.Docs.Views.Article, new ArticleModel(page, toc));
		}

		private static DocumentationPage SelectSearchResult(IEnumerable<DocumentationPage> pages)
		{
			var list = pages.ToList();

			var all = list.FirstOrDefault(x => x.Language == Language.All);
			if (all != null)
				return all;

			return list.First(x => x.Language != Language.All);
		}

		private string GetImagesUrl()
		{
			var remoteStore = DocumentStore as DocumentStore;
			if (remoteStore != null)
				return remoteStore.Url.ForDatabase(remoteStore.DefaultDatabase) + "/static/";

			var embeddableDocumentStore = (EmbeddableDocumentStore)DocumentStore;
			if (embeddableDocumentStore.Url != null)
				return embeddableDocumentStore.Url.ForDatabase(embeddableDocumentStore.DefaultDatabase) + "/static/";

			return null;
		}

		private static string GetDocumentationDirectory()
		{
			var directory = ConfigurationManager.AppSettings["Raven/Documentation/Directory"];
			if (string.IsNullOrEmpty(directory) == false)
				return directory;

			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\Documentation\\");
		}
	}
}