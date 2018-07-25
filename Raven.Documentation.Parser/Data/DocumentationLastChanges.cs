using System.Collections.Generic;

namespace Raven.Documentation.Parser.Data
{
    public class DocumentationLastChanges
    {
        public const string DefaultId = "DocumentationLastChanges/1";

        public string Id => DefaultId;

        public DocumentationLastChanges()
        {
            Changes = new Dictionary<string, string>();
        }

        public string LastCommitSha { get; set; }

        public Dictionary<string, string> Changes { get; set; }
    }
}
