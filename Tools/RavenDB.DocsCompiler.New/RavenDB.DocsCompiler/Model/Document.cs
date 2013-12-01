using System.Collections.Generic;

namespace RavenDB.DocsCompiler.Model
{
	public class Document : IDocumentationItem
	{
		public Document()
		{
			Children = new List<IDocumentationItem>();
		}

		public string Title { get; set; }
		public string VirtualTrail { get; set; }
		public string Slug { get; set; }
		public ClientType Language { get; set; }
		public IDocumentationItem Parent { get; set; }
		public List<IDocumentationItem> Children { get; set; }
		public string Trail { get; set; }
		public string Content { get; set; }
	}
}
