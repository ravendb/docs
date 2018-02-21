using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Raven.Documentation.Parser.Helpers
{
	public class LegacyBlockHelper
	{
		private static readonly Regex FilesListFinder = new Regex(@"{FILES-LIST(-RECURSIVE)?\s*/}", RegexOptions.Compiled);

		private static readonly Regex CodeBlockFinder = new Regex(@"{CODE-START:(.+?)/}(.*?){CODE-END\s*/}", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex RawHtmlBlockFinder = new Regex(@"{RAW\s*}([\S\s]*?){RAW/}", RegexOptions.Compiled);

        private static readonly Regex RawHtmlPlaceholderRegex = new Regex($@"{RawHtmlPlaceholderPrefix}[0-9]*");

	    private const string RawHtmlPlaceholderPrefix = "_RAW_HTML_PLACEHOLDER_";

		public static string GenerateLegacyBlocks(string pathToDirectory, string content)
		{
			var directory = new DirectoryInfo(pathToDirectory);
			var directoryName = directory.Name;

			content = FilesListFinder.Replace(content, match => GenerateFilesList(pathToDirectory, directoryName));
			content = CodeBlockFinder.Replace(content, match => ReplaceLegacyCodeBlock(match.Groups[1].Value, match.Groups[2].Value));

			return content;
		}

	    public static string ReplaceRawHtmlWithPlaceholders(string content, out IDictionary<string, string> rawHtmlPlaceholders)
	    {
            var htmlPlaceholders = new Dictionary<string, string>();

	        var placeholderIndex = 0;
            content = RawHtmlBlockFinder.Replace(content, match =>
            {
                var html = match.Groups[1].Value;
                var placeholder = GetPlaceholder(placeholderIndex);
                htmlPlaceholders[placeholder] = html;
                placeholderIndex++;
                return placeholder;
            });

	        rawHtmlPlaceholders = htmlPlaceholders;

	        return content;
	    }

	    public static string ReplaceRawHtmlPlaceholdersAfterMarkdownTransformation(string content,
	        IDictionary<string, string> rawHtmlPlaceholders)
	    {
	        int index = 0;
	        return RawHtmlPlaceholderRegex.Replace(content, match =>
	        {
	            var result = rawHtmlPlaceholders[GetPlaceholder(index)];
	            index++;
	            return result;
	        });
	    }

	    private static string GetPlaceholder(int index)
	    {
	        return $"{RawHtmlPlaceholderPrefix}{index}";
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
			var path = Path.Combine(pathToDocumentationPage, Constants.DocumentationFileName);
			if (File.Exists(path) == false)
				return string.Empty;

			var builder = new StringBuilder();
			foreach (var item in DocumentationFileHelper.ParseFile(path))
			{
				builder.AppendLine(string.Format("* [{0}]({1}/{2})", item.Description, directoryName, item.Name));
			}

			return builder.ToString();
		}
	}
}
