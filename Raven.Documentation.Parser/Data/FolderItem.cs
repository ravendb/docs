using System.Collections.Generic;

namespace Raven.Documentation.Parser.Data
{
    public class FolderItem
    {
        public FolderItem(bool isFolder)
        {
            IsFolder = isFolder;
            Mappings = new List<DocumentationMapping>();
        }

        public FolderItem(FolderItem item)
        {
            IsFolder = item.IsFolder;
            Name = item.Name;
            Description = item.Description;
            Language = item.Language;
            Mappings = item.Mappings;
            DiscussionId = item.DiscussionId;
        }

        public bool IsFolder { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Language Language { get; set; }

        public string LastSupportedVersion { get; set; }

        public string DiscussionId { get; set; }

        public Dictionary<string, string> Metadata { get; set; }

        public Dictionary<string, string> SeoMetaProperties { get; set; }

        public List<DocumentationMapping> Mappings { get; set; }
    }
}
