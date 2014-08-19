using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Raven.Documentation.Parser.Helpers
{
	public class LegacyBlockHelper
	{
		private static readonly Regex FilesListFinder = new Regex(@"{FILES-LIST(-RECURSIVE)?\s*/}", RegexOptions.Compiled);

		private static readonly Regex CodeBlockFinder = new Regex(@"{CODE-START:(.+?)/}(.*?){CODE-END\s*/}", RegexOptions.Compiled | RegexOptions.Singleline);

		public static string GenerateLegacyBlocks(string pathToDirectory, string content)
		{
			var directory = new DirectoryInfo(pathToDirectory);
			var directoryName = directory.Name;

			content = FilesListFinder.Replace(content, match => GenerateFilesList(pathToDirectory, directoryName));
			content = CodeBlockFinder.Replace(content, match => ReplaceLegacyCodeBlock(match.Groups[1].Value, match.Groups[2].Value));

			return content;
		}

		private static string ReplaceLegacyCodeBlock(string languageAsString, string blockContent)
		{
			var builder = new StringBuilder();
			builder.AppendFormat("{{CODE-BLOCK:{0}}}", languageAsString.Trim());
			builder.Append(blockContent);
			builder.Append("{CODE-BLOCK/}");

			return builder.ToString();
		}

		private static string GenerateFilesList(string pathToDocumentationPage, string directoryName)
		{
			var path = Path.Combine(pathToDocumentationPage, Constants.DocListFileName);
			if (File.Exists(path) == false)
				return string.Empty;

			var builder = new StringBuilder();
			foreach (var item in DocListFileHelper.ParseDocListFile(path))
			{
				builder.AppendLine(string.Format("* [{0}]({1}/{2})", item.Description, directoryName, item.Name));
			}

			return builder.ToString();
		}
	}
}