using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Documentation.Parser;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Web.Helpers;
using Raven.Documentation.Web.Models;

namespace Raven.Documentation.Web.Controllers
{
    public partial class ArticlesController : BaseController
    {
        public ArticlesController(IDocumentStore store)
            : base(store)
        {
        }

        [Route("/articles/{*key}")]
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
                .FirstOrDefault(x => x.Key == key);

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
                        ImageUrlGenerator = DocsController.GetImageUrlGenerator(HttpContext, DocumentStore, ArticleType.Articles)
                    }, new NoOpGitFileInformationProvider());

            var query = DocumentSession
                .Advanced
                .DocumentQuery<ArticlePage>()
                .GetIndexQuery();

            DocumentStore
                .Operations
                .Send(new DeleteByQueryOperation(query))
                .WaitForCompletion();

            DocumentSession.SaveChanges();

            var toDispose = new List<IDisposable>();

            try
            {
                foreach (var page in parser.Parse())
                {
                    DocumentSession.Store(page);

                    foreach (var image in page.Images)
                    {
                        var fileInfo = new FileInfo(image.ImagePath);
                        if (fileInfo.Exists == false)
                            continue;

                        var file = fileInfo.OpenRead();
                        toDispose.Add(file);

                        var documentId = page.Id;
                        var fileName = Path.GetFileName(image.ImagePath);

                        DocumentSession.Advanced.Attachments.Store(documentId, fileName, file, AttachmentsController.FileExtensionToContentTypeMapping[fileInfo.Extension]);
                    }
                }

                foreach (var toc in parser.GenerateTableOfContents())
                {
                    DocumentSession.Store(toc);
                }

                DocumentSession.SaveChanges();
            }
            finally
            {
                foreach (var disposable in toDispose)
                    disposable?.Dispose();
            }

            while (true)
            {
                var stats = DocumentStore.Maintenance.Send(new GetStatisticsOperation());
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
