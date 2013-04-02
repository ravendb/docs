using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MarkdownDeep;
using RavenDB.DocsCompiler.Model;
using RavenDB.DocsCompiler.Output;

namespace RavenDB.DocsCompiler.MagicWorkers
{
	public static class DocumentationParser
	{
		static readonly Regex CodeFinder = new Regex(@"{CODE\s+(.+)/}", RegexOptions.Compiled);
		static readonly Regex CodeBlockFinder = new Regex(@"{CODE-START:(.+?)/}(.*?){CODE-END\s*/}", RegexOptions.Compiled | RegexOptions.Singleline);
		static readonly Regex NotesFinder = new Regex(@"{(NOTE|WARNING|INFO|TIP|BLOCK)\s+(.+)/}", RegexOptions.Compiled);
		static readonly Regex FilesListFinder = new Regex(@"{FILES-LIST(-RECURSIVE)?\s*/}", RegexOptions.Compiled);
		static readonly Regex FirstLineSpacesFinder = new Regex(@"^(\s|\t)+", RegexOptions.Compiled);

		public static string Parse(Compiler docsCompiler, Folder folder, string fullPath, string trail, string versionUrl, bool convertToHtml)
		{
			if (!File.Exists(fullPath))
				throw new FileNotFoundException(string.Format("{0} was not found", fullPath));

			var contents = File.ReadAllText(fullPath);
			contents = CodeBlockFinder.Replace(contents, match => GenerateCodeBlock(match.Groups[1].Value.Trim(), match.Groups[2].Value, convertToHtml));
			contents = CodeFinder.Replace(contents, match => GenerateCodeBlockFromFile(match.Groups[1].Value.Trim(), docsCompiler.CodeSamplesPath, convertToHtml));

			if (folder != null)
			{
				contents = FilesListFinder.Replace(contents, match => GenerateFilesList(folder, false));
			}

            if (convertToHtml)
            {
                contents = contents.ResolveMarkdown(docsCompiler.Output, !string.IsNullOrWhiteSpace(docsCompiler.Output.RootUrl) ? trail : string.Empty, versionUrl);
            }
			
			contents = NotesFinder.Replace(contents, match => InjectNoteBlocks(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim()));

			return contents;
		}

		private static string GenerateFilesList(Folder folder, bool recursive)
		{
			if (folder.Items == null)
				return string.Empty;

			var sb = new StringBuilder();
			foreach (var item in folder.Items)
			{
				sb.AppendFormat("* [{0}]({1})", item.Title, item.Slug);
				sb.AppendLine();
			}
			return sb.ToString();
		}

		private static string GenerateCodeBlock(string lang, string code, bool convertToHtml)
		{
            if (convertToHtml)
            {
                return string.Format("<pre class=\"brush: {2}\">{0}{1}</pre>{0}", Environment.NewLine,
                                     ConvertMarkdownCodeStatment(code).Replace("<", "&lt;"), // to support syntax highlighting on pre tags
                                     lang
                    );    
            }
            return string.Format("{0}{1}",Environment.NewLine,ConvertMarkdownCodeStatment(code)); // .Replace("<", "&lt;"));

		}

		private static string InjectNoteBlocks(string blockType, string blockText)
		{
			return string.Format(@"<div class=""{0}-block block""><span>{1}</span></div>", blockType.ToLower(), blockText);
		}

		private static string GenerateCodeBlockFromFile(string value, string codeSamplesPath, bool convertToHtml)
		{
			var values = value.Split('@');
			var section = values[0];
			var file = values[1];

			var fileContent = LocateCodeFile(codeSamplesPath, file);
            if (convertToHtml)
            {
                return "<pre class=\"brush: csharp\">" + Environment.NewLine
                   + ConvertMarkdownCodeStatment(ExtractSection(section, fileContent))
                   .Replace("<", "&lt;") // to support syntax highlighting on pre tags
                   + "</pre>";
            }
		    return Environment.NewLine
                   + ConvertMarkdownCodeStatment(ExtractSection(section, fileContent)); // .Replace("<", "&lt;");

		}

		private static string ConvertMarkdownCodeStatment(string code)
		{
			var line = code.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			var firstLineSpaces = GetFirstLineSpaces(line.FirstOrDefault());
			var firstLineSpacesLength = firstLineSpaces.Length;
			var formattedLines = line.Select(l => string.Format("    {0}", l.Substring(l.Length < firstLineSpacesLength ? 0 : firstLineSpacesLength)));
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

		private static string ExtractSection(string section, string file)
		{
			// NOTE: Nested regions are not supported
			var startText = string.Format("#region {0}", section);
			var start = file.IndexOf(startText) + startText.Length;
			var end = file.IndexOf("#endregion", start);
			var sectionContent = file.Substring(start, end - start);
			return sectionContent.Trim(Environment.NewLine.ToCharArray());
		}

		private static string LocateCodeFile(string codeSamplesPath, string file)
		{
			var codePath = Path.Combine(codeSamplesPath, file);
			if (File.Exists(codePath) == false)
				throw new FileNotFoundException(string.Format("{0} was not found", codePath));
			return File.ReadAllText(codePath);
		}

		public static string ResolveMarkdown(this string content, IDocsOutput output, string trail, string versionUrl)
		{
			// http://www.toptensoftware.com/markdowndeep/api
			var md = new Markdown
						{
							AutoHeadingIDs = true,
							ExtraMode = true,
							NoFollowLinks = false,
							SafeMode = false,
							HtmlClassTitledImages = "figure",
							UrlRootLocation = output.RootUrl,
						};

			if (!string.IsNullOrWhiteSpace(output.RootUrl))
			{
				md.PrepareLink = tag => PrepareLink(tag, output.RootUrl, trail, versionUrl);
			}

			md.PrepareImage = (tag, titledImage) => PrepareImage(output.ImagesPath, tag);

			return md.Transform(content);
		}

		private static bool PrepareLink(HtmlTag tag, string rootUrl, string trail, string versionUrl)
		{
			string href;
			if (!tag.attributes.TryGetValue("href", out href))
				return true;

			if (Uri.IsWellFormedUriString(href, UriKind.Absolute))
				return true;

			var hashIndex = href.IndexOf("#", StringComparison.InvariantCultureIgnoreCase);
			if (hashIndex != -1)
			{
				href = href.Insert(hashIndex, "?version=" + versionUrl);
			}
			else
			{
				href += "?version=" + versionUrl;
			}

			Uri uri;
			if (!string.IsNullOrWhiteSpace(trail)) trail += "/"; // make sure we don't lose the current slug
			if (!Uri.TryCreate(new Uri(rootUrl + trail, UriKind.Absolute), new Uri(href, UriKind.Relative), out uri))
			{
				// TODO: Log error
			}

			tag.attributes["href"] = uri.AbsoluteUri;

			return true;
		}

		private static bool PrepareImage(string imagesPath, HtmlTag tag)
		{
			string src;
			if (tag.attributes.TryGetValue("src", out src))
			{
				src = src.Replace('\\', '/');
				if (src.StartsWith("images/", StringComparison.InvariantCultureIgnoreCase))
					src = src.Substring(7);
				tag.attributes["src"] = imagesPath + src;
			}
			return true;
		}
	}
}