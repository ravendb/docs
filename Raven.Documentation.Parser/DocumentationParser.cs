using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser
{
    public class DocumentationParser : ParserBase<DocumentationPage>
    {
        private readonly DocumentationDirectoryCompiler _directoryCompiler;

        public DocumentationParser(ParserOptions options, IProvideGitFileInformation repoAnalyzer)
            : base(options)
        {
            var documentCompiler = new DocumentCompiler(Parser, options, repoAnalyzer);
            _directoryCompiler = new DocumentationDirectoryCompiler(documentCompiler, options);
        }

        public override IEnumerable<DocumentationPage> Parse()
        {
            var documentationDirectories = Directory.GetDirectories(Options.PathToDocumentationDirectory);
            var compilationContext = new DocumentationCompilation.Context();

            return documentationDirectories
                .Select(documentationDirectory => new DirectoryInfo(documentationDirectory))
                .Where(documentationDirectory => Options.VersionsToParse.Count == 0 || Options.VersionsToParse.Contains(documentationDirectory.Name))
                .SelectMany(directoryInfo => _directoryCompiler.Compile(directoryInfo, compilationContext));
        }

        public override IEnumerable<TableOfContents> GenerateTableOfContents()
        {
            var documentationDirectories = Directory.GetDirectories(Options.PathToDocumentationDirectory);
            return documentationDirectories
                .Select(documentationDirectory => new DirectoryInfo(documentationDirectory))
                .SelectMany(_directoryCompiler.GenerateTableOfContents);
        }
    }
}
