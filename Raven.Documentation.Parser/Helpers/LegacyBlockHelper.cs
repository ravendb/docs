using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Raven.Documentation.Parser.Helpers
{
	public class LegacyBlockHelper
	{
		private static readonly Regex FilesListFinder = new Regex(@"{FILES-LIST(-RECURSIVE)?\s*/}", RegexOptions.Compiled);

		public static string GenerateLegacyBlocks(string pathToDirectory, string content)
		{
			var directory = new DirectoryInfo(pathToDirectory);
			var directoryName = directory.Name;

			return FilesListFinder.Replace(content, match => GenerateFilesList(pathToDirectory, directoryName));
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