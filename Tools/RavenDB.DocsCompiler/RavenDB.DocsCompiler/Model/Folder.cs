using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenDB.DocsCompiler.Model
{
	public class Folder : IDocumentationItem
	{
		public string Name { get; set; }
		public string Slug { get; set; }
		public IList<Document> Documents { get; set; }
	}
}
