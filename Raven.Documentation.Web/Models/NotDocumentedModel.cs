namespace Raven.Documentation.Web.Models
{
	using System.Collections.Generic;
	using System.Linq;

	using Raven.Documentation.Parser.Data;

	public class NotDocumentedModel
	{
		public NotDocumentedModel()
		{
			AvailableLanguages = new List<Language>();
		}

		public NotDocumentedModel(string articleKey, Language currentLanguage, IEnumerable<DocumentationPage> pages, TableOfContents tableOfContents)
		{
			ArticleKey = articleKey;
			CurrentLanguage = currentLanguage;
			TableOfContents = tableOfContents;
			AvailableLanguages = pages.Select(x => x.Language).ToList();
		}

		public string ArticleKey { get; set; }

		public Language CurrentLanguage { get; set; }

		public IList<Language> AvailableLanguages { get; set; }

		public TableOfContents TableOfContents { get; set; }
	}
}