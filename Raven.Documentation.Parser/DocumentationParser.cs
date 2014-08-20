namespace Raven.Documentation.Parser
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using MarkdownDeep;

	using Raven.Documentation.Parser.Data;

	public class DocumentationParser
	{
		private readonly ParserOptions _options;

		private readonly DirectoryCompiler _directoryCompiler;

		public DocumentationParser(ParserOptions options)
		{
			_options = options;

			var parser = new Markdown
							 {
								 AutoHeadingIDs = true,
								 ExtraMode = true,
								 NoFollowLinks = false,
								 SafeMode = false,
								 HtmlClassTitledImages = "figure text-center",
								 UrlRootLocation = options.RootUrl
							 };

			var documentCompiler = new DocumentCompiler(parser, options);
			_directoryCompiler = new DirectoryCompiler(documentCompiler, options);
		}

		public IEnumerable<DocumentationPage> Parse()
		{
			var documentationDirectories = Directory.GetDirectories(_options.PathToDocumentationDirectory);
			return documentationDirectories
				.Select(documentationDirectory => new DirectoryInfo(documentationDirectory))
				.Where(documentationDirectory => _options.VersionsToParse.Count == 0 || _options.VersionsToParse.Contains(documentationDirectory.Name))
				.SelectMany(_directoryCompiler.Compile);
		}

		public IEnumerable<TableOfContents> GenerateTableOfContents()
		{
			var documentationDirectories = Directory.GetDirectories(_options.PathToDocumentationDirectory);
			return documentationDirectories
				.Select(documentationDirectory => new DirectoryInfo(documentationDirectory))
				.SelectMany(_directoryCompiler.GenerateTableOfContents);
		}
	}
}