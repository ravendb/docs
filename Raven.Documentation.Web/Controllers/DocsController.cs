using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Linq;
using System.Web;
using Raven.Documentation.Web.Core.ViewModels;
using Raven.Documentation.Web.Helpers;
using Raven.Documentation.Web.Indexes;
using Raven.Documentation.Parser;
using Raven.Documentation.Parser.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Documentation.Web.Models;

namespace Raven.Documentation.Web.Controllers
{
    public partial class DocsController : BaseController
    {
        private static readonly Regex NormalizePattern = new Regex(@"
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
            return RedirectToAction(
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
            var query =
                Session.Advanced.DocumentQuery<DocumentationPage>()
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
                .Select(DocumentationLanguage.Parse)
                .ToList();

            ViewBag.CurrentVersionLanguages = versionLanguages;
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var model = filterContext.Controller.ViewData.Model;
            if (model is DocumentationViewModel)
            {
                var docVm = model as DocumentationViewModel;
                ViewBag.Language = docVm.SelectedLanguage.ToString().ToLowerInvariant();
                ViewBag.Version = docVm.SelectedVersion;
            }

            base.OnResultExecuting(filterContext);
        }

        protected new IDocumentSession Session => DocumentSession;

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
                return;

            //Cache
            if (Duration <= 0)
                return;

            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.Add(cacheDuration));
            cache.SetMaxAge(cacheDuration);
            cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            base.EndExecute(asyncResult);

            Session?.Dispose();
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

            var query = from r in Session.Query<DocumentationPages_ByKey.Result, DocumentationPages_ByKey>()
                        where r.Key == key || r.Key == indexKey
                        let id = r.Ids[version + "/" + language] ?? r.Ids[version + "/All"]
                        select new DocumentationPage_WithVersionsAndLanguages.ProjectionResult
                        {
                            Page = RavenQuery.Load<DocumentationPage>(id),
                            Languages = r.Languages
                        };

            var result = query.FirstOrDefault();
            if (result == null)
                return null;

            return new DocumentationPage_WithVersionsAndLanguages.Result(result);
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

            var query = Session.Advanced.DocumentQuery<DocumentationPage>()
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
                select new ArticleSearchResult
                {
                    Key = result.Key,
                    Title = result.Title,
                    Version = result.Version,
                    Category = result.Category.GetDescription(),
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
                ImageUrlGenerator = GetImageUrlGenerator(HttpContext, DocumentStore)
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

                if (Enum.TryParse(language, true, out Language parsedLanguage))
                {
                    query = query.Where(x => x.Language == parsedLanguage || x.Language == Language.All);
                }
            }

            var pages = query.ToList();

            var options = new ParserOptions
            {
                PathToDocumentationDirectory = GetDocumentationDirectory(),
                RootUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + "article/" + CurrentVersion + "/" + CurrentLanguage + "/",
                ImageUrlGenerator = GetImageUrlGenerator(HttpContext, DocumentStore)
            };

            var results = new DocumentationValidator(options, CurrentLanguage)
                .ValidateMappings(pages)
                .ToList();

            return View(MVC.Docs.Views.ValidateMappings, results);
        }

        private List<TableOfContents> GetTableOfContents(string version)
        {
            return Session.Query<TableOfContents>().Where(x => x.Version == version).ToList();
        }


        public virtual ActionResult OldArticlePage(string version, string language, string key)
        {
            return RedirectToAction(MVC.Docs.ActionNames.ArticlePage, MVC.Docs.Name, new { language, version, key });
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
                        ImageUrlGenerator = GetImageUrlGenerator(HttpContext, DocumentStore)
                    }, new NoOpGitFileInformationProvider());

            var query = DocumentSession
                .Advanced
                .DocumentQuery<DocumentationPage>()
                .GetIndexQuery();

            DocumentStore
                .Operations
                .Send(new DeleteByQueryOperation(query))
                .WaitForCompletion();

            query = DocumentSession
                .Advanced
                .DocumentQuery<TableOfContents>()
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

            if (string.IsNullOrEmpty(key))
                return RedirectToAction(MVC.Docs.ActionNames.ArticlePage, MVC.Docs.Name, new { language = language, version = version });

            while (true)
            {
                var stats = DocumentStore.Maintenance.Send(new GetStatisticsOperation());
                if (stats.StaleIndexes.Any() == false)
                    break;

                Thread.Sleep(500);
            }

            return RedirectToAction(
                MVC.Docs.ActionNames.ArticlePage,
                MVC.Docs.Name,
                new { language = CurrentLanguage, version = CurrentVersion, key = key });
        }

        public static ParserOptions.GenerateImageUrl GetImageUrlGenerator(HttpContextBase httpContext,
            IDocumentStore documentStore, ArticleType articleType = ArticleType.Documentation)
        {
            var url = GetImagesUrl(httpContext, documentStore, articleType);
            return (docVersion, key, fileName) => $"{url}?v={docVersion}&key={key}&fileName={fileName}";
        }

        public static string GetImagesUrl(HttpContextBase httpContext, IDocumentStore store, ArticleType articleType)
        {
            return $"{httpContext.Request.Url.GetLeftPart(UriPartial.Authority)}/attachments/{articleType.GetDescription()}/";
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
