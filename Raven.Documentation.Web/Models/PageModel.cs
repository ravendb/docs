namespace Raven.Documentation.Web.Models
{
	using Raven.Documentation.Parser.Data;

	public class PageModel
	{
		public PageModel(TableOfContents tableOfContents)
		{
			TableOfContents = tableOfContents;
		}

		public TableOfContents TableOfContents { get; set; }
	}
}