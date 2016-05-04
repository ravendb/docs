using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Documentation.Parser;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Web.Helpers;
using Raven.Json.Linq;

namespace Raven.Documentation.Web.Controllers
{
    public partial class ArticlesController : BaseController
    {
        public ArticlesController(IDocumentStore store)
            : base(store)
        {
        }

        public virtual ActionResult Articles(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                var articles = DocumentSession
                    .Query<ArticlePage>()
                    .ToList();

                return View(MVC.Articles.Views.Articles, articles);
            }

            var article = DocumentSession
                .Query<ArticlePage>()
                .Where(x => x.Key == key)
                .FirstOrDefault();

            if (article == null)
                return View(MVC.Docs.Views.NotFound);

            ViewBag.ArticleKey = article.Key;
            return View(MVC.Articles.Views.Article, article);
        }

        public virtual ActionResult Generate(string key)
        {
            var parser =
                new ArticleParser(
                    new ParserOptions
                    {
                        PathToDocumentationDirectory = GetArticleDirectory(),
                        RootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")),
                        ImagesUrl = DocsController.GetImagesUrl(DocumentStore)
                    }, new NoOpGitFileInformationProvider());

            DocumentStore
                .DatabaseCommands
                .DeleteByIndex("Raven/DocumentsByEntityName", new IndexQuery { Query = "Tag:ArticlePages" })
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

            while (true)
            {
                var stats = DocumentStore.DatabaseCommands.GetStatistics();
                if (stats.StaleIndexes.Any() == false)
                    break;

                Thread.Sleep(500);
            }

            if (string.IsNullOrEmpty(key))
                return RedirectToAction(MVC.Articles.ActionNames.Articles);
            
            return RedirectToAction(MVC.Articles.ActionNames.Articles, MVC.Articles.Name, new { key = key });
        }

        private static string GetArticleDirectory()
        {
            var directory = ConfigurationManager.AppSettings["Raven/Article/Directory"];
            if (string.IsNullOrEmpty(directory) == false)
                return directory;

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\Articles\\");
        }
    }
}