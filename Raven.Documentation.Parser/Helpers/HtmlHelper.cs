namespace Raven.Documentation.Parser.Helpers
{
	using HtmlAgilityPack;

	public static class HtmlHelper
	{
		public static HtmlDocument ParseHtml(string htmlContent)
		{
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlContent);

			return htmlDocument;
		}
	}
}