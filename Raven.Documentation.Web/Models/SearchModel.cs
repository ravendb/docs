namespace Raven.Documentation.Web.Models
{
	using System.Collections.Generic;
	using Raven.Documentation.Parser.Data;

	public class SearchModel
	{
		private readonly Dictionary<Category, List<Result>> _results;

		public Language CurrentLanguage { get; private set; }

		public SearchModel(Dictionary<Category, List<Result>> results, Language currentLanguage)
		{
			_results = results;
			CurrentLanguage = currentLanguage;
		}

		public Dictionary<Category, List<Result>> Results
		{
			get
			{
				return _results;
			}
		}

		public class Result
		{
			public string Title { get; set; }

			public Category Category { get; set; }

			public string Key { get; set; }
		}
	}
}