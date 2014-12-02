using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Helpers
{
	public class DocListFileHelper
	{
		private static readonly Regex DocsListLine = new Regex(@"^([\w\-/\.]{2,})\t(.+)$", RegexOptions.Compiled | RegexOptions.Multiline);

		public static IEnumerable<FolderItem> ParseDocListFile(string docListFilePath)
		{
			var contents = File.ReadAllText(docListFilePath);

			var matches = DocsListLine.Matches(contents);
			foreach (Match match in matches)
			{
				var name = match.Groups[1].Value.Trim();
				var description = match.Groups[2].Value.Trim();
				var isFolder = name.StartsWith("/");
				var item = new FolderItem(isFolder)
				{
					Language = Language.All,
					Description = description,
					Name = isFolder ? name.Substring(1, name.Length - 1) : name.Substring(0, name.Length - Constants.MarkdownFileExtension.Length)
				};

				yield return item;
			}
		} 
	}
}