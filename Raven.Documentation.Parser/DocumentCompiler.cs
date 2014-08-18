namespace Raven.Documentation.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;

	using HtmlAgilityPack;

	using MarkdownDeep;

	using Raven.Documentation.Parser.Data;
	using Raven.Documentation.Parser.Helpers;

	internal class DocumentCompiler
	{
		private readonly Markdown _parser;

		private readonly ParserOptions _options;

		public DocumentCompiler(Markdown parser, ParserOptions options)
		{
			_parser = parser;
			_options = options;
		}

		public DocumentationPage Compile(FileInfo file, FolderItem page, string documentationVersion)
		{
			try
			{
				var key = ExtractKey(file, page, documentationVersion);
				var category = CategoryHelper.ExtractCategoryFromPath(key);
				var images = new HashSet<DocumentationImage>();

				_parser.PrepareImage = (tag, b) => PrepareImage(images, file.DirectoryName, _options.ImagesUrl, documentationVersion, tag);

				var content = File.ReadAllText(file.FullName);
				content = _parser.Transform(content);
				content = TransformBlocks(content, documentationVersion);

				var htmlDocument = HtmlHelper.ParseHtml(content);

				var title = ExtractTitle(htmlDocument);
				var textContent = ExtractTextContent(htmlDocument);

				return new DocumentationPage
				{
					Key = key,
					Title = title,
					Version = documentationVersion,
					HtmlContent = content,
					TextContent = textContent,
					Language = page.Language,
					Category = category,
					Images = images
				};
			}
			catch (Exception e)
			{
				throw new InvalidOperationException(string.Format("Could not compile '{0}'.", file.FullName), e);
			}
		}

		private static bool PrepareImage(ICollection<DocumentationImage> images, string directory, string imagesUrl, string documentationVersion, HtmlTag tag)
		{
			string src;
			if (tag.attributes.TryGetValue("src", out src))
			{
				var imagePath = Path.Combine(directory, src);

				src = src.Replace('\\', '/');
				if (src.StartsWith("images/", StringComparison.InvariantCultureIgnoreCase))
					src = src.Substring(7);

				var newSrc = string.Format("{0}/{1}", documentationVersion, src);

				tag.attributes["src"] = imagesUrl + newSrc;

				images.Add(new DocumentationImage
					           {
								   ImagePath = imagePath,
								   ImageKey = newSrc
					           });
			}

			tag.attributes["class"] = "img-responsive img-thumbnail";

			return true;
		}

		private string ExtractKey(FileInfo file, FolderItem page, string documentationVersion)
		{
			var pathToDocumentationPagesDirectory = _options.GetPathToDocumentationPagesDirectory(documentationVersion);
			var key = file.FullName.Substring(pathToDocumentationPagesDirectory.Length, file.FullName.Length - pathToDocumentationPagesDirectory.Length);
			key = key.Substring(0, key.Length - file.Extension.Length);
			key = key.Replace(@"\", @"/");
			key = key.StartsWith(@"/") ? key.Substring(1) : key;

			var extension = FileExtensionHelper.GetLanguageFileExtension(page.Language);
			if (string.IsNullOrEmpty(extension) == false)
			{
				key = key.Substring(0, key.Length - extension.Length);
			}

			return key;
		}

		private static string ExtractTextContent(HtmlDocument htmlDocument)
		{
			var relatedArticles =
				htmlDocument.DocumentNode.ChildNodes.FirstOrDefault(
					x => x.InnerText.Equals("Related articles", StringComparison.OrdinalIgnoreCase));

			if (relatedArticles == null)
				return htmlDocument.DocumentNode.InnerText;

			var nodeToRemove = relatedArticles;
			var nodesToRemove = new List<HtmlNode>();
			while (nodeToRemove != null)
			{
				nodesToRemove.Add(nodeToRemove);
				nodeToRemove = nodeToRemove.NextSibling;
			}

			foreach (var node in nodesToRemove)
			{
				htmlDocument.DocumentNode.RemoveChild(node);
			}

			return htmlDocument.DocumentNode.InnerText;
		}

		private static string ExtractTitle(HtmlDocument htmlDocument)
		{
			var node = htmlDocument.DocumentNode.ChildNodes.FirstOrDefault(x => x.Name == "h1");
			if (node == null)
				return "No title";

			return node.InnerText;
		}

		private string TransformBlocks(string content, string documentationVersion)
		{
			content = NoteBlockHelper.GenerateNoteBlocks(content);
			content = CodeBlockHelper.GenerateCodeBlocks(content, documentationVersion, _options);
			content = PanelBlockHelper.GeneratePanelBlocks(content);

			return content;
		}
	}
}