using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Raven.Client.Connection;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Client.Embedded;
using Raven.Documentation.Web.Core.ViewModels;
using Raven.Documentation.Web.Helpers;
using Raven.Documentation.Web.Indexes;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Documentation.Parser;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Web.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Raven.Json.Linq;

namespace Raven.Documentation.Web.Controllers
{
    public partial class DocsController : BaseController
	{
	    private static Regex NormalizePattern = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

		public static string DefaultVersion = "4.0";

		public static Language DefaultLanguage = Language.Csharp;

        public int Duration { get; set; }

		public DocsController(IDocumentStore store)
			: base(store)
		{
		}

	    public virtual ActionResult Index(string language, string version)
	    {
	        return this.RedirectToAction(
	            MVC.Docs.ActionNames.ArticlePage,
	            MVC.Docs.Name,
	            new { language, version, key = "start/getting-started" });
	    }

        public virtual ActionResult ArticlePage(string version, string language, string key, int? page, string fullVersion)
		{
            if (string.Equals("latest", version, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction(MVC.Docs.ActionNames.ArticlePage, new { key, page, language, version = DocsVersion.Default });

			version = DocsVersion.Parse(version);
			var lang = DocumentationLanguage.Parse(language);
			ViewBag.Key = key;

            SetCurrentVersionLanguages(version);

		    return ViewDoc(version, key, lang);
		}

        [HttpGet]
        public virtual ActionResult Suggest(string language, string version, string searchTerm)
        {
            var tokens = GetSearchTokens(searchTerm);

            var lang = DocumentationLanguage.Parse(language);
            var ver = DocsVersion.Parse(version);
            FieldHighlightings contentHighlighting;
            var query =
                Session.Advanced.DocumentQuery<DocumentationPage>()
                .Highlight("TextContent", 128, 1, out contentHighlighting)
                .SetHighlighterTags("<span class='search-result-highlight'>", "</span>")
                    .WhereIn(x => x.Language, new[] { Language.All, lang })
                    .AndAlso()
                    .WhereEquals(x => x.Version, ver)
                    .AndAlso()
                    .OpenSubclause()
                    .Search(x => x.Title, tokens.HighImportanceTerms).Boost(50)
                    .OrElse()
                    .Search(x => x.TextContent, tokens.HighImportanceTerms).Boost(35);

            if (string.IsNullOrWhiteSpace(tokens.MediumImportanceTerms) == false)
            {
                query = query
                    .OrElse()
                    .Search(x => x.Title, tokens.MediumImportanceTerms).Boost(20)
                    .OrElse()
                    .Search(x => x.TextContent, tokens.MediumImportanceTerms).Boost(5);
            }

            if (string.IsNullOrWhiteSpace(tokens.LowImportanceTerms) == false)
            {
                query = query
                    .OrElse()
                    .Search(x => x.Title, tokens.LowImportanceTerms)
                    .OrElse()
                    .Search(x => x.TextContent, tokens.LowImportanceTerms);
            }

            var results = query
                .CloseSubclause()
                    .Select(
                        x => new SuggestItem
                        {
                            Language = (x.Language == Language.All ? Language.Csharp : x.Language).ToString().ToLowerInvariant(),
                            Key = x.Key,
                            Version = x.Version,
                            Title = x.Title
                        })
                    .Take(5)
                    .ToArray();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

	    private static SearchTokens GetSearchTokens(string searchTerm)
	    {
	        var result = new SearchTokens
	        {
	            HighImportanceTerms = string.Empty,
	            MediumImportanceTerms = string.Empty,
	            LowImportanceTerms = string.Empty
	        };

	        if (string.IsNullOrWhiteSpace(searchTerm))
	            return result;

	        result.HighImportanceTerms = searchTerm.Trim();

	        var normalizedSearchTerm = NormalizePattern.Replace(result.HighImportanceTerms, " ");
	        var parts = normalizedSearchTerm.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
	        if (parts.Length > 1)
	        {
	            var lastShortWordIndex = -1;
	            for (var index = 0; index < parts.Length; index++)
	            {
	                var merge = false;
	                var word = parts[index];
	                if (word.Length <= 2)
	                {
	                    result.LowImportanceTerms += " " + word;

	                    var diff = index - lastShortWordIndex;
	                    lastShortWordIndex = index;

	                    if (Math.Abs(diff) == 1)
	                        merge = true;
	                }

	                result.MediumImportanceTerms += (merge ? string.Empty : " ") + word;
	            }
	        }

	        result.MediumImportanceTerms = result.MediumImportanceTerms.Trim();
	        result.LowImportanceTerms = result.LowImportanceTerms.Trim();

	        return result;
	    }

	    private ActionResult ViewDoc(string version, string key, Language lang)
        {
            var viewModel = new DocPageViewModel(version, lang);
            viewModel.FillToc(GetTableOfContents(version));

            var article = GetArticle(key, version, lang);
            if (article == null)
            {
                return View("NotFound", viewModel);
            }

            FillAvailableLanguages(viewModel, article, version);

            if (article.Page == null)
            {
                return View("NotFound", viewModel);
            }

            FillAvailableVersions(viewModel, article, lang, version);

            ViewBag.Key = article.Page.Key;
            viewModel.Page = article.Page;
            return View(MVC.Docs.Views.Doc, viewModel);
        }

        private void SetCurrentVersionLanguages(string version)
        {
            var versionLanguages = DocumentSession.Query<DocumentationPages_LanguagesAndVersions.DocumentationLanguageAndVersion, DocumentationPages_LanguagesAndVersions>()
                .Where(x => x.Version == version)
                .Select(x => x.Language)
                .Distinct()
                .ToList()
                .Select(x => DocumentationLanguage.Parse(x))
                .ToList();

            ViewBag.CurrentVersionLanguages = versionLanguages;
        }

        //private void SetCurrentVersionLanguages(string version)
        //{
        //    var versionLanguages = Session.Query<DocumentationPages_LanguagesAndVersions.DocumentationLanguageAndVersion, DocumentationPages_LanguagesAndVersions>()
        //        .Where(x => x.Version == version)
        //        .Select(x => x.Language)
        //        .Distinct()
        //        .ToList()
        //        .Select(x => DocumentationLanguage.Parse(x))
        //        .ToList();

        //    ViewBag.CurrentVersionLanguages = versionLanguages;
        //}

	    protected override void OnResultExecuting(ResultExecutingContext filterContext)
	    {
	        var model = filterContext.Controller.ViewData.Model;
	        if (model is DocumentationViewModel)
	        {
	            var docVm = model as DocumentationViewModel;
	            this.ViewBag.Language = docVm.SelectedLanguage.ToString().ToLowerInvariant();
	            this.ViewBag.Version = docVm.SelectedVersion;
	        }

	        base.OnResultExecuting(filterContext);
	    }

	    protected new IDocumentSession Session => DocumentSession;

	    protected override void OnActionExecuted(ActionExecutedContext filterContext)
	    {
	        if (filterContext.Exception != null)
	            return;

	        //Cache
	        if (this.Duration <= 0)
	            return;

	        HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
	        TimeSpan cacheDuration = TimeSpan.FromSeconds(this.Duration);

	        cache.SetCacheability(HttpCacheability.Public);
	        cache.SetExpires(DateTime.Now.Add(cacheDuration));
	        cache.SetMaxAge(cacheDuration);
	        cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
	    }

	    protected override void EndExecute(IAsyncResult asyncResult)
	    {
	        base.EndExecute(asyncResult);

	        if (Session != null)
	            Session.Dispose();
	    }

	    private static void FillAvailableLanguages(DocPageViewModel viewModel, DocumentationPage_WithVersionsAndLanguages.Result article, string currentVersion)
	    {
	        if (article.Languages.ContainsKey(currentVersion))
	            viewModel.AvailableLanguages.AddRange(article.Languages[currentVersion]);
	    }

	    private void FillAvailableVersions(DocPageViewModel viewModel, DocumentationPage_WithVersionsAndLanguages.Result article, Language currentLanguage, string currentVersion)
	    {
	        var currentVersionAsFloat = float.Parse(currentVersion);
	        foreach (var v in DocsVersion.AllVersionsAsFloat)
	        {
	            if (currentVersionAsFloat == v)
	                continue;

	            DocumentationMapping map;

	            if (currentVersionAsFloat < v)
	            {
	                map = article.Page.Mappings
	                    .Where(x => x.Version > currentVersionAsFloat && x.Version <= v)
	                    .OrderBy(x => x.Version)
	                    .FirstOrDefault();
	            }
	            else
	            {
	                map = article.Page.Mappings
	                    .Where(x => x.Version < currentVersionAsFloat && x.Version <= v)
	                    .OrderBy(x => x.Version)
	                    .LastOrDefault();
	            }

	            if (map == null)
	                continue;

	            if (article.Languages.Any(versionWithLangs => float.Parse(versionWithLangs.Key) == v))
	                continue;

	            viewModel.AvailableVersions[v] = map.Key;
	        }

	        foreach (var version in FilterVersions(article.Languages, currentLanguage, currentVersion))
	        {
	            viewModel.AvailableVersions[float.Parse(version)] = article.Page.Key;
	        }
	    }

	    private IEnumerable<string> FilterVersions(Dictionary<string, Language[]> languages, Language currentLanguage, string currentVersion)
	    {
	        return languages
	            .Where(x => x.Value.Contains(currentLanguage) || x.Value.Contains(Language.All))
	            .Select(x => x.Key)
	            .Where(x => x != currentVersion);
	    }

	    private DocumentationPage_WithVersionsAndLanguages.Result GetArticle(string key, string version, Language language)
	    {
	        var indexKey = key + "/index";

	        return Session
	            .Query<DocumentationPages_ByKey.Result, DocumentationPages_ByKey>()
	            .Where(x => x.Key == key || x.Key == indexKey)
	            .TransformWith<DocumentationPage_WithVersionsAndLanguages, DocumentationPage_WithVersionsAndLanguages.Result>()
	            .AddTransformerParameter("Version", version)
	            .AddTransformerParameter("Language", language.ToString())
	            .FirstOrDefault();
	    }

        [HttpGet]
        public virtual ActionResult Search(string language, string version, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return RedirectToAction(MVC.Docs.ActionNames.Index, MVC.Docs.Name);

            if (string.Equals("latest", version, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction(MVC.Docs.ActionNames.Search, new { language, version = DocsVersion.Default, searchTerm });

            var lang = DocumentationLanguage.Parse(language);
            version = DocsVersion.Parse(version);

            SetCurrentVersionLanguages(version);

            ViewBag.Term = searchTerm;

            var tokens = GetSearchTokens(searchTerm);

            FieldHighlightings contentHighlighting;
            var query = Session.Advanced.DocumentQuery<DocumentationPage>()
                .Highlight("TextContent", 128, 1, out contentHighlighting)
                .SetHighlighterTags("<span class='search-result-highlight'>", "</span>")
                    .WhereIn(x => x.Language, new[] { Language.All, lang })
                .AndAlso()
                    .WhereEquals(x => x.Version, version)
                .AndAlso()
                    .OpenSubclause()
                    .Search(x => x.Title, tokens.HighImportanceTerms).Boost(50)
                    .OrElse()
                    .Search(x => x.TextContent, tokens.HighImportanceTerms).Boost(35);

            if (string.IsNullOrWhiteSpace(tokens.MediumImportanceTerms) == false)
            {
                query = query
                    .OrElse()
                    .Search(x => x.Title, tokens.MediumImportanceTerms).Boost(20)
                    .OrElse()
                    .Search(x => x.TextContent, tokens.MediumImportanceTerms).Boost(5);
            }

            if (string.IsNullOrWhiteSpace(tokens.LowImportanceTerms) == false)
            {
                query = query
                    .OrElse()
                    .Search(x => x.Title, tokens.LowImportanceTerms)
                    .OrElse()
                    .Search(x => x.TextContent, tokens.LowImportanceTerms);
            }

            var results = query
                .CloseSubclause()
                .Take(30)
                .ToArray();

            var transformedSearchResults =
                from result in results
                let highlightedContent = contentHighlighting.GetFragments(result.Id.ToLowerInvariant()).FirstOrDefault()
                select new ArticleSearchResult
                {
                    Key = result.Key,
                    Title = result.Title,
                    Version = result.Version,
                    Category = result.Category.GetDescription(),
                    ContentHighlight = highlightedContent
                };

            var groupedSearchResults =
                from result in transformedSearchResults
                group result by result.Category;

            var viewModel = new DocSearchViewModel(
                version,
                lang,
                searchTerm,
                groupedSearchResults);

            viewModel.FillToc(GetTableOfContents(version));

            if (viewModel.SearchResults.Count == 0)
            {
                return View("NoSearchResults", viewModel);
            }

            return View("SearchResults", viewModel);
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
							ImagesUrl = GetImagesUrl(DocumentStore)
						};

			var results = new DocumentationValidator(options, CurrentLanguage)
				.ValidateLinks(pages)
				.ToList();

			return View(MVC.Docs.Views.Validate, results);
		}

		public virtual ActionResult ValidateMappings(string language, string version, bool all)
		{
			var query = DocumentSession
				.Query<DocumentationPage>()
				.Take(int.MaxValue);

			if (all == false)
			{
				query = query.Where(x => x.Version == CurrentVersion);

				Language parsedLanguage;

				if (Enum.TryParse(language, true, out parsedLanguage))
				{
					query = query.Where(x => x.Language == parsedLanguage || x.Language == Language.All);
				}
			}

			var pages = query.ToList();

			var options = new ParserOptions
			{
				PathToDocumentationDirectory = GetDocumentationDirectory(),
				RootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "article/" + CurrentVersion + "/" + CurrentLanguage + "/",
				ImagesUrl = GetImagesUrl(DocumentStore)
			};

			var results = new DocumentationValidator(options, CurrentLanguage)
				.ValidateMappings(pages)
				.ToList();

			return View(MVC.Docs.Views.ValidateMappings, results);
		}

	    private List<TableOfContents> GetTableOfContents(string version)
	    {
	        return this.Session.Query<TableOfContents>().Where(x => x.Version == version).ToList();
	    }

        
	    public virtual ActionResult OldArticlePage(string version, string language, string key)
	    {
	        return this.RedirectToAction(MVC.Docs.ActionNames.ArticlePage, MVC.Docs.Name, new { language, version, key });
	    }


	    //private static void FillAvailableLanguages(DocPageViewModel viewModel, DocumentationPage_WithVersionsAndLanguages.Result article, string currentVersion)
	    //{
	    //    if (article.Languages.ContainsKey(currentVersion))
	    //        viewModel.AvailableLanguages.AddRange(article.Languages[currentVersion]);
	    //}

	    //private void FillAvailableVersions(DocPageViewModel viewModel, DocumentationPage_WithVersionsAndLanguages.Result article, Language currentLanguage, string currentVersion)
	    //{
	    //    var currentVersionAsFloat = float.Parse(currentVersion);
	    //    foreach (var v in DocsVersion.AllVersionsAsFloat)
	    //    {
	    //        if (currentVersionAsFloat == v)
	    //            continue;

	    //        DocumentationMapping map;

	    //        if (currentVersionAsFloat < v)
	    //        {
	    //            map = article.Page.Mappings
	    //                .Where(x => x.Version > currentVersionAsFloat && x.Version <= v)
	    //                .OrderBy(x => x.Version)
	    //                .FirstOrDefault();
	    //        }
	    //        else
	    //        {
	    //            map = article.Page.Mappings
	    //                .Where(x => x.Version < currentVersionAsFloat && x.Version <= v)
	    //                .OrderBy(x => x.Version)
	    //                .LastOrDefault();
	    //        }

	    //        if (map == null)
	    //            continue;

	    //        if (article.Languages.Any(versionWithLangs => float.Parse(versionWithLangs.Key) == v))
	    //            continue;

	    //        viewModel.AvailableVersions[v] = map.Key;
	    //    }

	    //    foreach (var version in FilterVersions(article.Languages, currentLanguage, currentVersion))
	    //    {
	    //        viewModel.AvailableVersions[float.Parse(version)] = article.Page.Key;
	    //    }
	    //}

	    //private IEnumerable<string> FilterVersions(Dictionary<string, Language[]> languages, Language currentLanguage, string currentVersion)
	    //{
	    //    return languages
	    //        .Where(x => x.Value.Contains(currentLanguage) || x.Value.Contains(Language.All))
	    //        .Select(x => x.Key)
	    //        .Where(x => x != currentVersion);
	    //}


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
							ImagesUrl = GetImagesUrl(DocumentStore)
						}, new NoOpGitFileInformationProvider());


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
                return RedirectToAction(MVC.Docs.ActionNames.ArticlePage, MVC.Docs.Name, new { language = language, version = version });

            while (true)
            {
                var stats = DocumentStore.DatabaseCommands.GetStatistics();
                if (stats.StaleIndexes.Any() == false)
                    break;

                Thread.Sleep(500);
            }

            return RedirectToAction(
                MVC.Docs.ActionNames.ArticlePage,
                MVC.Docs.Name,
                new { language = CurrentLanguage, version = CurrentVersion, key = key });
        }

		//public virtual ActionResult Welcome(string language, string version)
		//{
		//	return View(MVC.Docs.Views.);
		//}

		//public virtual ActionResult Index(string language, string version)
		//{
		//	return RedirectToAction(MVC.Docs.ActionNames.Welcome, MVC.Docs.Name, new { language = language, version = version });
		//}

		//public virtual ActionResult Client(string version, string language)
		//{
		//	var toc = DocumentSession
		//		.Query<TableOfContents>()
		//		.First(x => x.Category == Category.ClientApi && x.Version == CurrentVersion);

		//	return View(MVC.Docs.Views.Client, new PageModel(toc));
		//}

		//public virtual ActionResult Server(string version, string language)
		//{
		//	var toc = DocumentSession
		//		.Query<TableOfContents>()
		//		.First(x => x.Category == Category.Server && x.Version == CurrentVersion);

		//	return View(MVC.Docs.Views.Server, new PageModel(toc));
		//}

		//public virtual ActionResult UsersIssues(string version, string language)
		//{
		//	var toc = DocumentSession
		//		.Query<TableOfContents>()
		//		.First(x => x.Category == Category.UsersIssues && x.Version == CurrentVersion);

		//	return View(MVC.Docs.Views.Server, new PageModel(toc));
		//}

		//public virtual ActionResult Glossary(string version, string language)
		//{
		//	var toc = DocumentSession
		//		.Query<TableOfContents>()
		//		.First(x => x.Category == Category.Glossary && x.Version == CurrentVersion);

		//	return View(MVC.Docs.Views.Glossary, new PageModel(toc));
		//}

		//public virtual ActionResult Samples(string version, string language)
		//{
		//	var toc = DocumentSession
		//		.Query<TableOfContents>()
		//		.First(x => x.Category == Category.Samples && x.Version == CurrentVersion);

		//	return View(MVC.Docs.Views.Samples, new PageModel(toc));
		//}

		//public virtual ActionResult FileSystem(string version, string language)
		//{
		//	var toc = DocumentSession
		//		.Query<TableOfContents>()
		//		.First(x => x.Category == Category.FileSystem && x.Version == CurrentVersion);

		//	return View(MVC.Docs.Views.FileSystem, new PageModel(toc));
		//}

		//public virtual ActionResult Articles(string version, string language, string key)
		//{
		//	ViewBag.Key = null;

		//	var indexKey = key + "/index";

		//	var allPages = DocumentSession
		//		.Query<DocumentationPage>()
		//		.Where(x => x.Key == key || x.Key == indexKey)
		//		.Where(x => x.Version == CurrentVersion)
		//		.ToList();

		//	if (allPages.Count == 0)
		//		return View(MVC.Docs.Views.NotFound);

		//	var pages = allPages
		//		.Where(x => x.Language == Language.All || x.Language == CurrentLanguage)
		//		.ToList();

		//	var category = allPages
		//		.Select(x => x.Category)
		//		.FirstOrDefault();

		//	var toc = DocumentSession
		//		.Query<TableOfContents>()
		//		.First(x => x.Category == category && x.Version == CurrentVersion);

		//	if (pages.Count == 0)
		//		return View(MVC.Docs.Views.NotDocumented, new NotDocumentedModel(key, CurrentLanguage, allPages, toc));

		//	Debug.Assert(pages.Count <= 2);

		//	ViewBag.Key = key;

		//	var all = pages.FirstOrDefault(x => x.Language == Language.All);
		//	if (all != null)
		//		return View(MVC.Docs.Views.Article, new ArticleModel(all, toc));

		//	var page = pages.First(x => x.Language != Language.All);
		//	return View(MVC.Docs.Views.Article, new ArticleModel(page, toc));
		//}

	 //   public virtual ActionResult Migration(string version, string language)
	 //   {
	 //       var toc = DocumentSession
	 //           .Query<TableOfContents>()
	 //           .First(x => x.Category == Category.Migration && x.Version == CurrentVersion);

	 //       return View(MVC.Docs.Views.Migration, new PageModel(toc));
	 //   }

        private static DocumentationPage SelectSearchResult(IEnumerable<DocumentationPage> pages)
		{
			var list = pages.ToList();

			var all = list.FirstOrDefault(x => x.Language == Language.All);
			if (all != null)
				return all;

			return list.First(x => x.Language != Language.All);
		}

		public static string GetImagesUrl(IDocumentStore store)
		{
			var remoteStore = store as DocumentStore;
			if (remoteStore != null)
				return remoteStore.Url.ForDatabase(remoteStore.DefaultDatabase) + "/static/";

			var embeddableDocumentStore = (EmbeddableDocumentStore)store;
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

    internal class SearchTokens
    {
        public string HighImportanceTerms { get; set; }

        public string MediumImportanceTerms { get; set; }

        public string LowImportanceTerms { get; set; }
    }
}
