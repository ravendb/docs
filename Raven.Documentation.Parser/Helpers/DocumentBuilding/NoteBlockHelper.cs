using System.Text.RegularExpressions;

namespace Raven.Documentation.Parser.Helpers.DocumentBuilding
{
    public static class NoteBlockHelper
	{
		private static readonly Regex BlockFinder = new Regex(@"{(NOTE|WARNING|INFO|DANGER|SAFE)\s+(.+)/}", RegexOptions.Compiled);

		private static readonly Regex NoteWithTitleBlockFinder = new Regex(@"{NOTE:(.+?)}(.*?){NOTE\s*/}", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex WarningWithTitleBlockFinder = new Regex(@"{WARNING:(.+?)}(.*?){WARNING\s*/}", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex InfoWithTitleBlockFinder = new Regex(@"{INFO:(.+?)}(.*?){INFO\s*/}", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex DangerWithTitleBlockFinder = new Regex(@"{DANGER:(.+?)}(.*?){DANGER\s*/}", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex SafeWithTitleBlockFinder = new Regex(@"{SAFE:(.+?)}(.*?){SAFE\s*/}", RegexOptions.Compiled | RegexOptions.Singleline);

		public static string GenerateNoteBlocks(string content)
		{
			content = NoteWithTitleBlockFinder.Replace(
				content, match => GenerateBlockWithTitle("note", match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			content = WarningWithTitleBlockFinder.Replace(
				content, match => GenerateBlockWithTitle("warning", match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			content = InfoWithTitleBlockFinder.Replace(
				content, match => GenerateBlockWithTitle("info", match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			content = DangerWithTitleBlockFinder.Replace(
				content, match => GenerateBlockWithTitle("danger", match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			content = SafeWithTitleBlockFinder.Replace(
				content, match => GenerateBlockWithTitle("safe", match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			content = BlockFinder.Replace(
				content, match => GenerateBlock(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			return content;
		}

		private static string GenerateBlockWithTitle(string blockType, string title, string blockText)
		{
			return string.Format("<div class='bs-callout bs-callout-{2}'><h4>{1}</h4>\n<p>{0}</p></div>", blockText, title, blockType.ToLowerInvariant());
		}

		private static string GenerateBlock(string blockType, string blockText)
		{
			return string.Format("<div class='bs-callout bs-callout-{2}'><h4>{1}</h4>\n<p>{0}</p></div>", blockText, GetNoteBlockHeaderText(blockType), blockType.ToLowerInvariant());
		}

		private static string GetNoteBlockHeaderText(string blockType)
		{
			switch (blockType.ToLowerInvariant())
			{
				case "safe":
					return "Safe By Default";
				case "info":
					return "Information";
				case "note":
					return "Note";
				case "warning":
					return "Warning";
				case "danger":
					return "Danger";
				default:
					return string.Empty;
			}
		}
	}
}
