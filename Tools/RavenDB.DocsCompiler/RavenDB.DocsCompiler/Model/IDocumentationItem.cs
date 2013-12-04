using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenDB.DocsCompiler.Model
{
	public interface IDocumentationItem
	{
		string Title { get; set; }
		string Trail { get; set; }
		string VirtualTrail { get; set; }
		string Slug { get; set; }

		ClientType Language { get; set; }

		IDocumentationItem Parent { get; set; }
		List<IDocumentationItem> Children { get; set; } 
	}
}
