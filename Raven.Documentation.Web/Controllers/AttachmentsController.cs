using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Raven.Client.Documents;
using Raven.Documentation.Web.Core;
using Raven.Documentation.Web.Models;

namespace Raven.Documentation.Web.Controllers
{
    public partial class AttachmentsController : BaseController
    {
        public const string AttachmentsPrefix = "attachments/";

        public static readonly string AttachmentsForDocumentationPrefix = AttachmentsPrefix + ArticleType.Documentation.GetDescription();

        public static readonly string AttachmentsForArticlesPrefix = AttachmentsPrefix + ArticleType.Articles.GetDescription();
        
        public AttachmentsController(IDocumentStore store)
            : base(store)
        {
        }

        public virtual ActionResult Get(string type, string id)
        {
            var attachment = DocumentSession.Advanced.Attachments.Get(AttachmentsPrefix + type, id);
            if (attachment == null)
                return HttpNotFound();

            return new FileStreamResult(attachment.Stream, attachment.Details.ContentType);
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

    public class Attachment
    {
    }
}
