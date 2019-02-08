using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Raven.Documentation.Parser.Helpers.DocumentBuilding
{
    public class PanelBlockHelper
	{
		private static readonly Regex PanelBlockWithTitleFinder = new Regex(@"{PANEL:(.+?)}(.*?){PANEL/}", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex PanelBlockFinder = new Regex(@"{PANEL}(.*?){PANEL/}", RegexOptions.Compiled | RegexOptions.Singleline);

		public static string GeneratePanelBlocks(string content)
		{
			content = PanelBlockFinder.Replace(content, match => GeneratePanelBlock(null, match.Groups[1].Value.Trim()));
			content = PanelBlockWithTitleFinder.Replace(content, match => GeneratePanelBlock(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			return content;
		}

		private static string GeneratePanelBlock(string title, string content)
		{
			var builder = new StringBuilder();
			builder.AppendLine("<div class='panel panel-default'>");
			if (string.IsNullOrEmpty(title) == false)
			{
				builder.AppendLine("<div class='panel-heading'>");
				builder.AppendLine(string.Format("<h2 id='{0}' class='panel-title'>", ConvertTilteToId(title)));
				builder.AppendLine(title);
				builder.AppendLine("</h2>");
				builder.AppendLine("</div>");
			}

			builder.AppendLine("<div class='panel-body'>");
			builder.AppendLine(content);
			builder.AppendLine("</div>");
			builder.AppendLine("</div>");

			return builder.ToString();
		}

		private static string ConvertTilteToId(string title)
		{
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(title);

			var parts = htmlDocument.DocumentNode.InnerText.Split(new[] { ' ' });
			return string.Join("-", parts).ToLowerInvariant();
		}
	}
}
