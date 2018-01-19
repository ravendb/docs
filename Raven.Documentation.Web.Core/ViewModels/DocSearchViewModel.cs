using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Web.Core.ViewModels
{
	public class DocSearchViewModel: DocumentationViewModel
	{
		public DocSearchViewModel(string version, Language language, string searchTerm, IEnumerable<IGrouping<string, ArticleSearchResult>> searchResults)
			: base(version, language)
		{
			this.SearchTerm = searchTerm;
			this.SearchResults = new List<IGrouping<string, ArticleSearchResult>>(searchResults);
		}

		public string SearchTerm { get; set; }

        public List<IGrouping<string, ArticleSearchResult>> SearchResults { get; private set; }
	}
}
