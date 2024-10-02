using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Helpers.DocumentBuilding
{
    public class CodeBlockHelper
    {
        private const string CodeBlockPlaceholderPrefix = "_CODE_BLOCK_PLACEHOLDER_";

        private const string CodeTabsPlaceholderPrefix = "_CODE_TABS_PLACEHOLDER_";

        private const string CodeWithoutLanguagePlaceholderPrefix = "_CODE_WITHOUT_LANGUAGE_PLACEHOLDER_";

        private const string CodeWithLanguagePlaceholderPrefix = "_CODE_WITH_LANGUAGE_PLACEHOLDER_";

        private static readonly Regex CodeBlockPlaceholderFinder = new Regex($@"{CodeBlockPlaceholderPrefix}[0-9]*");

        private static readonly Regex CodeTabsPlaceholderFinder = new Regex($@"{CodeTabsPlaceholderPrefix}[0-9]*");

        private static readonly Regex CodeWithoutLanguagePlaceholderFinder = new Regex($@"{CodeWithoutLanguagePlaceholderPrefix}[0-9]*");

        private static readonly Regex CodeWithLanguagePlaceholderFinder = new Regex($@"{CodeWithLanguagePlaceholderPrefix}[0-9]*");

        private static readonly Regex CodeWithoutLanguageFinder = new Regex(@"{CODE\s+(.+?)/}", RegexOptions.Compiled);

        private static readonly Regex CodeWithLanguageFinder = new Regex(@"{CODE:(.+?)\s+(.+)/}", RegexOptions.Compiled);

        private static readonly Regex CodeBlockFinder = new Regex(@"{CODE-BLOCK:(.+?)}(.*?){CODE-BLOCK/}", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex CodeTabsFinder = new Regex(@"{CODE-TABS}(.*?){CODE-TABS/}", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex CodeTabFinder = new Regex(@"{CODE-TAB:(.+?)\s+(.+)/}", RegexOptions.Compiled);

        private static readonly Regex CodeTabBlockFinder = new Regex(@"{CODE-TAB-BLOCK:(.+?)}(.*?){CODE-TAB-BLOCK/}", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex ContentFrameFinder = new Regex(@"{CONTENT-FRAME:(.+?)}(.*?){CONTENT-FRAME/}", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex FirstLineSpacesFinder = new Regex(@"^(\s|\t)+", RegexOptions.Compiled);

        public static string ReplaceCodeBlocksWithPlaceholders(string content, out IDictionary<string, string> placeholders)
        {
            var localPlaceholders = new Dictionary<string, string>();

            content = CodeTabsFinder.Replace(content, match => GenerateCodeTabsBlockPlaceholder(match, localPlaceholders));
            content = CodeWithLanguageFinder.Replace(content, match => GenerateCodePlaceholder(match, localPlaceholders));
            content = CodeWithoutLanguageFinder.Replace(content, match => GenerateCodeWithoutLanguagePlaceholder(match, localPlaceholders));
            content = CodeBlockFinder.Replace(content, match => GenerateCodeBlockPlaceholder(match, localPlaceholders));

            placeholders = localPlaceholders;
            return content;
        }

        private static string GenerateCodeWithoutLanguagePlaceholder(Match match, Dictionary<string, string> placeholders)
        {
            var key = $"{CodeWithoutLanguagePlaceholderPrefix}{placeholders.Count}";
            placeholders[key] = match.Value;

            return key;
        }

        private static string GenerateCodePlaceholder(Match match, Dictionary<string, string> placeholders)
        {
            var key = $"{CodeWithLanguagePlaceholderPrefix}{placeholders.Count}";
            placeholders[key] = match.Value;

            return key;
        }

        private static string GenerateCodeTabsBlockPlaceholder(Match match, Dictionary<string, string> placeholders)
        {
            var key = $"{CodeTabsPlaceholderPrefix}{placeholders.Count}";
            placeholders[key] = match.Value;

            return key;
        }

        private static string GenerateCodeBlockPlaceholder(Match match, Dictionary<string, string> placeholders)
        {
            var key = $"{CodeBlockPlaceholderPrefix}{placeholders.Count}";
            placeholders[key] = match.Value;

            return key;
        }

        public static string GenerateCodeBlocks(string content, string documentationVersion, ParserOptions options, IDictionary<string, string> placeholders)
        {
            content = CodeTabsPlaceholderFinder.Replace(content, match => placeholders[match.Value]);
            content = CodeWithLanguagePlaceholderFinder.Replace(content, match => placeholders[match.Value]);
            content = CodeWithoutLanguagePlaceholderFinder.Replace(content, match => placeholders[match.Value]);
            content = CodeBlockPlaceholderFinder.Replace(content, match => placeholders[match.Value]);

            content = CodeTabsFinder.Replace(content, match => GenerateCodeTabsBlock(match.Groups[1].Value.Trim(), documentationVersion, options, placeholders));
            content = CodeWithLanguageFinder.Replace(content, match => GenerateCodeBlockFromFile(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim(), documentationVersion, options));
            content = CodeWithoutLanguageFinder.Replace(content, match => GenerateCodeBlockFromFile("csharp", match.Groups[1].Value.Trim(), documentationVersion, options));
            content = CodeBlockFinder.Replace(content, match => GenerateCodeBlock(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim(), placeholders));

            return content;
        }

        public static string GenerateContentFrames(string content)
        {
            content = ContentFrameFinder.Replace(content, match => GenerateContentFrame(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

            return content;
        }

        private static string GenerateContentFrame(string title, string content)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<div class='content-frame'>");
            if (string.IsNullOrWhiteSpace(title) == false)
                builder.AppendLine($"<h4>{title}</h4>");

            builder.AppendLine(content);
            builder.AppendLine("</div>");
            return builder.ToString();
        }

        private static string GenerateCodeBlock(string languageAsString, string content, IDictionary<string, string> placeholders)
        {
            var language = (CodeBlockLanguage)Enum.Parse(typeof(CodeBlockLanguage), languageAsString, true);

            content = NormalizeContent(content);

            var builder = new StringBuilder();
            builder.AppendLine(string.Format("<pre class='line-numbers'><code class='{0}'>{1}</code></pre>", ConvertLanguageToCssClass(language), content));
            return builder.ToString();
        }

        private static string GenerateCodeBlockFromFile(string languageAsString, string value, string documentationVersion, ParserOptions options)
        {
            var language = (Language)Enum.Parse(typeof(Language), languageAsString, true);
            var samplesDirectory = options.GetPathToCodeDirectory(language, documentationVersion);

            var values = value.Split('@');
            var section = values[0];
            var file = values[1];

            string content;
            var builder = new StringBuilder();
            switch (language)
            {
                case Language.Csharp:
                    content = ExtractSectionFromCsharpFile(section, Path.Combine(samplesDirectory, file));
                    break;
                case Language.Java:
                case Language.NodeJs:
                    content = ExtractSectionFromFile(section, Path.Combine(samplesDirectory, file));
                    break;
                case Language.Python:
                    content = ExtractSectionFromPythonFile(section, Path.Combine(samplesDirectory, file));
                    break;
                case Language.Php:
                    content = ExtractSectionFromPhpFile(section, Path.Combine(samplesDirectory, file));
                    break;
                default:
                    throw new NotSupportedException(language.ToString());
            }

            builder.AppendLine(string.Format("<pre class='line-numbers'><code class='{0}'>{1}</code></pre>", ConvertLanguageToCssClass(language), content));

            return builder.ToString();
        }

        private static string GenerateCodeTabsBlock(string content, string documentationVersion, ParserOptions options, IDictionary<string, string> placeholders)
        {
            var tabs = new List<CodeTabBase>();
            var matchList = new SortedList<int, (Match Match, bool IsFromFile)>();
            
            var matches = CodeTabFinder.Matches(content);
            foreach (Match match in matches)
                matchList.Add(match.Index, (match, true));

            matches = CodeTabBlockFinder.Matches(content);
            foreach (Match match in matches)
                matchList.Add(match.Index, (match, false));

            foreach (var kvp in matchList)
            {
                var isFromFile = kvp.Value.IsFromFile;
                var match = kvp.Value.Match;

                var languageAndTitle = match.Groups[1].Value.Trim();
                var parts = languageAndTitle.Split(':');
                var languageAsString = parts[0];
                var title = parts.Length > 1 ? parts[1] : null;
                var value = match.Groups[2].Value.Trim();

                if (isFromFile)
                {
                    tabs.Add(GenerateCodeTabFromFile(languageAsString, title, value, documentationVersion, options));
                    continue;
                }

                tabs.Add(GenerateCodeTabBlock(languageAsString, title, value));
            }

            var builder = new StringBuilder();
            builder.AppendLine("<div class='code-tabs'>");
            builder.AppendLine("<ul class='nav nav-tabs'>");
            for (int index = 0; index < tabs.Count; index++)
            {
                var tab = tabs[index];
                builder.AppendLine(string.Format("<li class='code-tab'><a href='#{0}' class='nav-link {2}' data-toggle='tab'>{1}</a></li>", tab.Id, tab.Title ?? tab.GetLanguageDisplayName(), index == 0 ? "active" : string.Empty));
            }
            builder.AppendLine("</ul>");

            builder.AppendLine("<div class='tab-content'>");
            for (int index = 0; index < tabs.Count; index++)
            {
                var tab = tabs[index];
                builder.AppendLine(string.Format("<div class='tab-pane code-tab {1}' id='{0}'>", tab.Id, index == 0 ? "active" : string.Empty));
                builder.AppendLine(string.Format("<pre class='line-numbers'><code class='{0}'>{1}</code></pre>", tab.GetLanguageCssClass(), tab.Content));
                builder.AppendLine("</div>");
            }
            builder.AppendLine("</div>");
            builder.AppendLine("</div>");

            return builder.ToString();
        }

        private static CodeTabBlock GenerateCodeTabBlock(string languageAsString, string title, string content)
        {
            var language = (CodeBlockLanguage)Enum.Parse(typeof(CodeBlockLanguage), languageAsString, true);

            content = content
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");

            return new CodeTabBlock(ConvertLanguageToCssClass, null) { Title = title, Content = content, Language = language, Id = Guid.NewGuid().ToString("N") };
        }

        private static CodeTab GenerateCodeTabFromFile(string languageAsString, string title, string value, string documentationVersion, ParserOptions options)
        {
            var language = (Language)Enum.Parse(typeof(Language), languageAsString, true);
            var samplesDirectory = options.GetPathToCodeDirectory(language, documentationVersion);

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
                case Language.NodeJs:
                    content = ExtractSectionFromFile(section, Path.Combine(samplesDirectory, file));
                    break;
                case Language.Python:
                    content = ExtractSectionFromPythonFile(section, Path.Combine(samplesDirectory, file));
                    break;
                case Language.Php:
                    content = ExtractSectionFromPhpFile(section, Path.Combine(samplesDirectory, file));
                    break;
                default:
                    throw new NotSupportedException(language.ToString());
            }

            return new CodeTab(ConvertLanguageToCssClass, ConvertLanguageToDisplayName) { Title = title, Content = content, Language = language, Id = Guid.NewGuid().ToString("N") };
        }

        private static string ExtractSectionFromFile(string section, string filePath)
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

            return NormalizeContent(sectionContent);
        }

        private static string ExtractSectionFromPythonFile(string section, string filePath)
        {
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException(string.Format("File '{0}' does not exist.", filePath), filePath);

            var content = File.ReadAllText(filePath);
            var startText = string.Format("# region {0}", section);
            var indexOfStart = content.IndexOf(startText, StringComparison.OrdinalIgnoreCase);
            if (indexOfStart == -1)
                throw new InvalidOperationException(string.Format("Section '{0}' not found in '{1}'.", section, filePath));

            var start = content.IndexOf(startText, StringComparison.OrdinalIgnoreCase) + startText.Length;
            var end = content.IndexOf("# endregion", start, StringComparison.OrdinalIgnoreCase);
            var sectionContent = content.Substring(start, end - start);
            if (sectionContent.EndsWith("//"))
                sectionContent = sectionContent.TrimEnd(new[] { '/' });

            return NormalizeContent(sectionContent);
        }

        private static string ExtractSectionFromPhpFile(string section, string filePath)
        {
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException(string.Format("File '{0}' does not exist.", filePath), filePath);

            var content = File.ReadAllText(filePath);
            var startText = string.Format("# region {0}", section);
            var indexOfStart = content.IndexOf(startText, StringComparison.OrdinalIgnoreCase);
            if (indexOfStart == -1)
                throw new InvalidOperationException(string.Format("Section '{0}' not found in '{1}'.", section, filePath));

            var start = content.IndexOf(startText, StringComparison.OrdinalIgnoreCase) + startText.Length;
            var end = content.IndexOf("# endregion", start, StringComparison.OrdinalIgnoreCase);
            var sectionContent = content.Substring(start, end - start);
            if (sectionContent.EndsWith("//"))
                sectionContent = sectionContent.TrimEnd(new[] { '/' });

            return NormalizeContent(sectionContent);
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

            return NormalizeContent(sectionContent);
        }

        private static string NormalizeContent(string content)
        {
            content = content
                .Replace("</p>\n<p>", "\n")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");

            content = content
                .TrimStart(' ')
                .TrimEnd('\t', ' ')
                .TrimEnd(Environment.NewLine.ToCharArray())
                .TrimStart(Environment.NewLine.ToCharArray());

            var line = content.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
            var firstLineSpaces = GetFirstLineSpaces(line.FirstOrDefault());
            var firstLineSpacesLength = firstLineSpaces.Length;
            var formattedLines = line
                .Select(l => string.Format("{0}", l.Substring(l.Length < firstLineSpacesLength ? 0 : firstLineSpacesLength)));

            return string.Join(Environment.NewLine, formattedLines);
        }

        private static string GetFirstLineSpaces(string firstLine)
        {
            if (firstLine == null)
                return string.Empty;

            var match = FirstLineSpacesFinder.Match(firstLine);
            if (match.Success)
            {
                return firstLine.Substring(0, match.Length);
            }

            return string.Empty;
        }

        private static string ConvertLanguageToCssClass(Language language)
        {
            switch (language)
            {
                case Language.Csharp:
                    return "language-csharp";
                case Language.Java:
                    return "language-java";
                case Language.Http:
                case Language.NodeJs:
                    return "language-javascript";
                case Language.Python:
                    return "language-python";
                case Language.Php:
                    return "language-php";
                default:
                    throw new NotSupportedException(language.ToString());
            }
        }

        private static string ConvertLanguageToCssClass(CodeBlockLanguage language)
        {
            switch (language)
            {
                case CodeBlockLanguage.Csharp:
                    return "language-csharp";
                case CodeBlockLanguage.Java:
                    return "language-java";
                case CodeBlockLanguage.Http:
                    return "language-http";
                case CodeBlockLanguage.Json:
                    return "language-json";
                case CodeBlockLanguage.Plain:
                    return "language-none";
                case CodeBlockLanguage.Xml:
                    return "language-xml";
                case CodeBlockLanguage.Python:
                    return "language-python";
                case CodeBlockLanguage.Php:
                    return "language-php";
                case CodeBlockLanguage.Bash:
                    return "language-bash";
                case CodeBlockLanguage.Batch:
                    return "language-batch";
                case CodeBlockLanguage.Git:
                    return "language-git";
                case CodeBlockLanguage.Go:
                    return "language-go";
                case CodeBlockLanguage.Html:
                    return "language-html";
                case CodeBlockLanguage.JavaScript:
                    return "language-javascript";
                case CodeBlockLanguage.PowerShell:
                    return "language-powershell";
                case CodeBlockLanguage.Ruby:
                    return "language-ruby";
                case CodeBlockLanguage.Sql:
                    return "language-sql";
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
                case Language.NodeJs:
                    return "Node.js";
                case Language.Http:
                    return "HTTP";
                case Language.Python:
                    return "Python";
                case Language.Php:
                    return "Php";
                default:
                    throw new NotSupportedException(language.ToString());
            }
        }
    }
}
