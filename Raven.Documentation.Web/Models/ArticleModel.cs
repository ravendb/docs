namespace Raven.Documentation.Web.Models
{
	using Raven.Documentation.Parser.Data;

	public class ArticleModel
	{
		public ArticleModel(DocumentationPage page, TableOfContents tableOfContents)
		{
			Key = page.Key;
			TableOfContents = tableOfContents;
			Title = page.Title;
			HtmlContent = page.HtmlContent;
		}

		public string Key { get; set; }

		public string Title { get; set; }

		public string HtmlContent { get; set; }

		public TableOfContents TableOfContents { get; set; }
	}
}