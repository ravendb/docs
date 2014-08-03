namespace Raven.Documentation.Parser.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;

	using Raven.Documentation.Parser.Data;

	public class CodeBlockHelper
	{
		private static readonly Regex CodeWithoutLanguageFinder = new Regex(@"{CODE\s+(.+)/}", RegexOptions.Compiled);

		private static readonly Regex CodeWithLanguageFinder = new Regex(@"{CODE:(.+?)\s+(.+)/}", RegexOptions.Compiled);

		private static readonly Regex CodeBlockFinder = new Regex(@"{CODE-BLOCK:(.+?)}(.*?){CODE-BLOCK/}", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex CodeTabsFinder = new Regex(@"{CODE-TABS}(.*?){CODE-TABS/}", RegexOptions.Compiled | RegexOptions.Singleline);

		private static readonly Regex CodeTabFinder = new Regex(@"{CODE-TAB:(.+?)\s+(.+)/}", RegexOptions.Compiled);

		private static readonly Regex CodeTabBlockFinder = new Regex(@"{CODE-TAB-BLOCK:(.+?)}(.*?){CODE-TAB-BLOCK/}", RegexOptions.Compiled | RegexOptions.Singleline);

		public static string GenerateCodeBlocks(string content, double documentationVersion, ParserOptions options)
		{
			content = CodeTabsFinder.Replace(content, match => GenerateCodeTabsBlock(match.Groups[1].Value.Trim(), documentationVersion, options));
			content = CodeWithLanguageFinder.Replace(content, match => GenerateCodeBlockFromFile(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim(), documentationVersion, options));
			content = CodeWithoutLanguageFinder.Replace(content, match => GenerateCodeBlockFromFile("csharp", match.Groups[1].Value.Trim(), documentationVersion, options));
			content = CodeBlockFinder.Replace(content, match => GenerateCodeBlock(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			return content;
		}

		private static string GenerateCodeBlock(string languageAsString, string content)
		{
			var language = (Language)Enum.Parse(typeof(Language), languageAsString, true);

			var builder = new StringBuilder();
			builder.AppendLine(string.Format("<pre class='prettyprint {0}'>", ConvertLanguageToCssClass(language)));
			builder.AppendLine(content);
			builder.AppendLine("</pre>");
			return builder.ToString();
		}

		private static string GenerateCodeBlockFromFile(string languageAsString, string value, double documentationVersion, ParserOptions options)
		{
			var language = (Language)Enum.Parse(typeof(Language), languageAsString, true);
			var samplesDirectory = options.GetPathToDocumentationSamplesDirectory(language, documentationVersion);

			var values = value.Split('@');
			var section = values[0];
			var file = values[1];

			var builder = new StringBuilder();
			builder.AppendLine(string.Format("<pre class='prettyprint {0}'>", ConvertLanguageToCssClass(language)));
			switch (language)
			{
				case Language.Csharp:
					builder.AppendLine(ExtractSectionFromCsharpFile(section, Path.Combine(samplesDirectory, file)));
					break;
				case Language.Java:
					builder.AppendLine(ExtractSectionFromJavaFile(section, Path.Combine(samplesDirectory, file)));
					break;
				default:
					throw new NotSupportedException(language.ToString());
			}
			builder.AppendLine("</pre>");

			return builder.ToString();
		}

		private static string GenerateCodeTabsBlock(string content, double documentationVersion, ParserOptions options)
		{
			var tabs = new List<CodeTab>();
			var matches = CodeTabFinder.Matches(content);
			foreach (Match match in matches)
				tabs.Add(GenerateCodeTabFromFile(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim(), documentationVersion, options));

			matches = CodeTabBlockFinder.Matches(content);
			foreach (Match match in matches)
				tabs.Add(GenerateCodeTab(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			var builder = new StringBuilder();
			builder.AppendLine("<div class='code-tabs'>");
			builder.AppendLine("<ul class='nav nav-tabs'>");
			foreach (var tab in tabs)
				builder.AppendLine(string.Format("<li class='code-tab'><a href='#{0}' data-toggle='tab'>{1}</a></li>", tab.Id, ConvertLanguageToDisplayName(tab.Language)));
			builder.AppendLine("</ul>");

			builder.AppendLine("<div class='tab-content'>");
			foreach (var tab in tabs)
			{
				builder.AppendLine(string.Format("<div class='tab-pane code-tab' id='{0}'>", tab.Id));
				builder.AppendLine(string.Format("<pre class='prettyprint {0}'>", ConvertLanguageToCssClass(tab.Language)));
				builder.AppendLine(tab.Content);
				builder.AppendLine("</pre>");
				builder.AppendLine("</div>");
			}
			builder.AppendLine("</div>");
			builder.AppendLine("</div>");

			return builder.ToString();
		}

		private static CodeTab GenerateCodeTab(string languageAsString, string content)
		{
			var language = (Language)Enum.Parse(typeof(Language), languageAsString, true);

			return new CodeTab { Content = content, Language = language, Id = Guid.NewGuid().ToString("N") };
		}

		private static CodeTab GenerateCodeTabFromFile(string languageAsString, string value, double documentationVersion, ParserOptions options)
		{
			var language = (Language)Enum.Parse(typeof(Language), languageAsString, true);
			var samplesDirectory = options.GetPathToDocumentationSamplesDirectory(language, documentationVersion);

			var values = value.Split('@');
			var section = values[0];
			var file = values[1];

			string content;
			switch (language)
			{
				case Language.Csharp:
					content = ExtractSectionFromCsharpFile(section, Path.Combine(samplesDirectory, file));
					break;
				case Language.Java:
					content = ExtractSectionFromJavaFile(section, Path.Combine(samplesDirectory, file));
					break;
				default:
					throw new NotSupportedException(language.ToString());
			}

			return new CodeTab { Content = content, Language = language, Id = Guid.NewGuid().ToString("N") };
		}

		private static string ExtractSectionFromJavaFile(string section, string filePath)
		{
			if (File.Exists(filePath) == false)
				throw new FileNotFoundException(string.Format("File '{0}' does not exist.", filePath), filePath);

			var content = File.ReadAllText(filePath);
			var startText = string.Format("//region {0}", section);
			var indexOfStart = content.IndexOf(startText, StringComparison.OrdinalIgnoreCase);
			if (indexOfStart == -1)
				throw new InvalidOperationException(string.Format("Section '{0}' not found in '{1}'.", section, filePath));

			var start = content.IndexOf(startText, StringComparison.OrdinalIgnoreCase) + startText.Length;
			var end = content.IndexOf("//endregion", start, StringComparison.OrdinalIgnoreCase);
			var sectionContent = content.Substring(start, end - start);
			if (sectionContent.EndsWith("//"))
				sectionContent = sectionContent.TrimEnd(new[] { '/' });

			return sectionContent.Trim(Environment.NewLine.ToCharArray());
		}

		private static string ExtractSectionFromCsharpFile(string section, string filePath)
		{
			if (File.Exists(filePath) == false)
				throw new FileNotFoundException(string.Format("File '{0}' does not exist.", filePath), filePath);

			var content = File.ReadAllText(filePath);
			var startText = string.Format("#region {0}", section);
			var indexOfStart = content.IndexOf(startText, StringComparison.OrdinalIgnoreCase);
			if (indexOfStart == -1)
				throw new InvalidOperationException(string.Format("Section '{0}' not found in '{1}'.", section, filePath));

			var start = content.IndexOf(startText, StringComparison.OrdinalIgnoreCase) + startText.Length;
			var end = content.IndexOf("#endregion", start, StringComparison.OrdinalIgnoreCase);
			var sectionContent = content.Substring(start, end - start);
			if (sectionContent.EndsWith("//"))
				sectionContent = sectionContent.TrimEnd(new[] { '/' });

			sectionContent = sectionContent.Trim(Environment.NewLine.ToCharArray());

			while (sectionContent.StartsWith("\t\t\t"))
			{
				sectionContent = sectionContent.Replace("\t\t\t", "\t\t");
			}

			sectionContent = sectionContent
				.Replace("<", "&lt;")
				.Replace(">", "&gt;");

			return sectionContent
				.TrimEnd('\t')
				.TrimEnd(Environment.NewLine.ToCharArray());
		}

		private static string ConvertLanguageToCssClass(Language language)
		{
			switch (language)
			{
				case Language.Csharp:
					return "lang-cs";
				case Language.Java:
					return "lang-java";
				case Language.Http:
					return "lang-js";
				default:
					throw new NotSupportedException(language.ToString());
			}
		}

		private static string ConvertLanguageToDisplayName(Language language)
		{
			switch (language)
			{
				case Language.Csharp:
					return "C#";
				case Language.Java:
					return "Java";
				case Language.Http:
					return "HTTP";
				default:
					throw new NotSupportedException(language.ToString());
			}
		}
	}
}