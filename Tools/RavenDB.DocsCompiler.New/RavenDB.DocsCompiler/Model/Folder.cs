using System.Collections.Generic;

namespace RavenDB.DocsCompiler.Model
{
	public class Folder : IDocumentationItem
	{
		public Folder()
		{
			Items = new List<IDocumentationItem>();
		}

		public string Title { get; set; }
		public string Slug { get; set; }
		public string Trail { get; set; }
		public IList<IDocumentationItem> Items { get; private set; }
	}
}
