using System.Collections.Generic;

namespace RavenDB.DocsCompiler.Model
{
	public class Folder : IDocumentationItem
	{
		public Folder()
		{
			Children = new List<IDocumentationItem>();
		}

		public string Title { get; set; }
		public string Slug { get; set; }
		public ClientType Language { get; set; }
		public IDocumentationItem Parent { get; set; }
		public List<IDocumentationItem> Children { get; set; }
		public string Trail { get; set; }
		public string VirtualTrail { get; set; }
        public bool Multilanguage { get; set; }
	}
}
