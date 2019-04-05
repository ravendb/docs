using System.Collections.Generic;

namespace Raven.Documentation.Parser.Data
{
    public class DocumentationFile
    {
        public DocumentationFile()
        {
            Mappings = new List<DocumentationMapping>();
        }

        public string Path { get; set; }

        public string Name { get; set; }

        public string LastSupportedVersion { get; set; }

        public string DiscussionId { get; set; }

        public Dictionary<string, string> Metadata { get; set; }

        public Dictionary<string, string> SeoMetaProperties { get; set; }

        public List<DocumentationMapping> Mappings { get; set; }
    }
}
