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
		string Slug { get; set; }
	}
}
