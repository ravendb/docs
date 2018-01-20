using System.ComponentModel;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Web.Models
{
	public class ArticleModel
	{
		public ArticleModel(DocumentationPage page, TableOfContents tableOfContents)
		{
			Key = page.Key;
			//TableOfContents = tableOfContents;
			Title = page.Title;
			HtmlContent = page.HtmlContent;
		}

		public string Key { get; set; }

		public string Title { get; set; }

		public string HtmlContent { get; set; }

		public TableOfContents TableOfContents { get; set; }
	}

    public enum ArticleType
    {
        [Description("documentation")]
        Documentation,

        [Description("articles")]
        Articles
    }
}
