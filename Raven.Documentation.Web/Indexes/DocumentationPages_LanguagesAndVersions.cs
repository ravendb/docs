using System.Linq;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Web.Indexes
{
    public class DocumentationPages_LanguagesAndVersions : AbstractIndexCreationTask<DocumentationPage, DocumentationPages_LanguagesAndVersions.DocumentationLanguageAndVersion>
    {
        public class DocumentationLanguageAndVersion
        {
            public string Language { get; set; }

            public string Version { get; set; }
        }

        public DocumentationPages_LanguagesAndVersions()
        {
            Map = pages =>
                from page in pages
                from supportedVersion in page.SupportedVersions
                select new DocumentationLanguageAndVersion
                {
                    Language = page.Language.ToString(),
                    Version = supportedVersion
                };

            Store("Language", FieldStorage.Yes);
        }
    }
}
