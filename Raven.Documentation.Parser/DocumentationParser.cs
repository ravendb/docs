namespace Raven.Documentation.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using MarkdownDeep;

    using Raven.Documentation.Parser.Data;

    public class DocumentationParser
    {
	    private readonly ParserOptions _options;

	    private Markdown _parser;

        private DocumentCompiler _documentCompiler;

        private readonly DirectoryCompiler _directoryCompiler;

        public DocumentationParser(ParserOptions options)
        {
	        _options = options;
	        _parser = new Markdown
                          {
                              AutoHeadingIDs = true,
                              ExtraMode = true,
                              NoFollowLinks = false,
                              SafeMode = false,
                              HtmlClassTitledImages = "figure",
                              UrlRootLocation = options.RootUrl
                          };

            _documentCompiler = new DocumentCompiler(_parser, options);
            _directoryCompiler = new DirectoryCompiler(_documentCompiler, options);
        }

        public IEnumerable<DocumentationPage> Parse()
        {
            var documentationDirectories = Directory.GetDirectories(_options.PathToDocumentationDirectory);
            return documentationDirectories
                .Select(documentationDirectory => new DirectoryInfo(documentationDirectory))
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