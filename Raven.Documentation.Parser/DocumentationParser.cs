using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Compilation;
using Raven.Documentation.Parser.Compilation.DocumentationDirectory;
using Raven.Documentation.Parser.Compilation.ToC;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser
{
    public class DocumentationParser : ParserBase<DocumentationPage>
    {
        private readonly TableOfContentsCompiler _tableOfContentsCompiler;
        private readonly TocBasedPagesCompiler _tocBasedPagesCompiler;
        private readonly IndexPagesCompiler _indexPagesCompiler;

        public DocumentationParser(ParserOptions options, IProvideGitFileInformation repoAnalyzer)
            : base(options)
        {
            var documentCompiler = new DocumentCompiler(Parser, options, repoAnalyzer);
            _tableOfContentsCompiler = new TableOfContentsCompiler(options);
            _tocBasedPagesCompiler = new TocBasedPagesCompiler(documentCompiler, Options);
            _indexPagesCompiler = new IndexPagesCompiler(documentCompiler, Options);
        }

        public override ParserOutput Parse()
        {
            var tableOfContents = GenerateTableOfContents().ToList();
            var docs = GenerateDocumentationPages(tableOfContents);

            return new ParserOutput
            {
                TableOfContents = tableOfContents,
                Pages = docs
            };
        }

        private IEnumerable<TableOfContents> GenerateTableOfContents()
        {
            var compilationContext = new CompilationUtils.Context();

            var documentationDirectories = Directory.GetDirectories(Options.PathToDocumentationDirectory);

            var compiledTocs = documentationDirectories
                .Select(documentationDirectory => new DirectoryInfo(documentationDirectory))
                .Select(directory => _tableOfContentsCompiler.Compile(directory, compilationContext))
                .ToList();

            var mergedTocs = TableOfContentsMerger.Merge(compiledTocs);
            return mergedTocs;
        }

        private IEnumerable<DocumentationPage> GenerateDocumentationPages(IEnumerable<TableOfContents> tocs)
        {
            foreach (var indexPage in GenerateDocumentationPagesFromIndex())
                yield return indexPage;

            foreach (var pageFromToc in GenerateDocumentationPagesFromToc(tocs))
                yield return pageFromToc;
        }

        private IEnumerable<DocumentationPage> GenerateDocumentationPagesFromIndex()
        {
            var documentationDirectories = Directory.GetDirectories(Options.PathToDocumentationDirectory);

            return documentationDirectories
                .Select(documentationDirectory => new DirectoryInfo(documentationDirectory))
                .Where(documentationDirectory => Options.CanCompileVersion(documentationDirectory.Name))
                .SelectMany(_indexPagesCompiler.Compile);
        }

        private IEnumerable<DocumentationPage> GenerateDocumentationPagesFromToc(IEnumerable<TableOfContents> tocs)
        {
            return tocs
                .Where(toc => Options.CanCompileVersion(toc.Version))
                .SelectMany(_tocBasedPagesCompiler.Compile);
        }
    }
}
