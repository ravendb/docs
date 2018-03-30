using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client.Documents;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Web.Controllers
{
    public partial class AttachmentsController : BaseController
    {
        public AttachmentsController(IDocumentStore store)
            : base(store)
        {
        }

        public virtual ActionResult Get(string v, string key, string fileName)
        {
            var documentId = v == "articles"
                ? GetArticlePageId(key)
                : GetDocumentationPageId(v, key);

            if (documentId == null)
                return HttpNotFound();

            var attachment = DocumentSession.Advanced.Attachments.Get(documentId, fileName);
            if (attachment == null)
                return HttpNotFound();

            return new FileStreamResult(attachment.Stream, attachment.Details.ContentType);
        }

        private string GetArticlePageId(string key)
        {
            var articlePage = DocumentSession.Query<ArticlePage>()
                .SingleOrDefault(x => x.Key == key);

            return articlePage?.Id;
        }

        private string GetDocumentationPageId(string version, string key)
        {
            var documentationPage = DocumentSession.Query<DocumentationPage>()
                .SingleOrDefault(x => x.Version == version && x.Key == key);

            return documentationPage?.Id;
        }

        public static readonly Dictionary<string, string> FileExtensionToContentTypeMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // HTML does not have charset because the HTML is expected to declare the charset itself
            {".html", "text/html"},
            {".htm", "text/html"},
            {".css", "text/css; charset=utf-8"},
            {".js", "text/javascript; charset=utf-8"},
            {".ico", "image/vnd.microsoft.icon"},
            {".jpg", "image/jpeg"},
            {".gif", "image/gif"},
            {".png", "image/png"},
            {".xap", "application/x-silverlight-2"},
            {".json", "application/json; charset=utf-8"},
            {".eot", "application/vnd.ms-fontobject"},
            {".svg", "image/svg+xml"},
            {".ttf", "application/octet-stream"},
            {".woff", "application/font-woff"},
            {".woff2", "application/font-woff2"},
            {".appcache", "text/cache-manifest; charset=utf-8"}
        };
    }
}
