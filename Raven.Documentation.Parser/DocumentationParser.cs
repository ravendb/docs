using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser
{
    public class DocumentationParser : ParserBase<DocumentationPage>
    {
        private readonly DocumentationDirectoryCompiler _directoryCompiler;
        private readonly TableOfContentsCompiler _tableOfContentsCompiler;

        public DocumentationParser(ParserOptions options, IProvideGitFileInformation repoAnalyzer)
            : base(options)
        {
            var documentCompiler = new DocumentCompiler(Parser, options, repoAnalyzer);
            _directoryCompiler = new DocumentationDirectoryCompiler(documentCompiler, options);
            _tableOfContentsCompiler = new TableOfContentsCompiler(options);
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
            var compilationContext = new DocumentationCompilation.Context();

            var documentationDirectories = Directory.GetDirectories(Options.PathToDocumentationDirectory);

            var tableOfContents = documentationDirectories
                .Select(documentationDirectory => new DirectoryInfo(documentationDirectory))
                .SelectMany(directory => _tableOfContentsCompiler.GenerateTableOfContents(directory, compilationContext))
                .ToList();

            var duplicatedTocs = tableOfContents.GroupBy(x => new { x.Version, x.Category })
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var duplicatedTocGroup in duplicatedTocs)
            {
                var mergedToc = MergeTocs(duplicatedTocGroup.ToList());
                ReplaceToc(mergedToc, tableOfContents);
            }

            return tableOfContents;
        }

        private TableOfContents MergeTocs(List<TableOfContents> tocs)
        {
            var toc = tocs.First();

            for (var i = 1; i < tocs.Count; i++)
            {
                MergeTocItems(tocs[i].Items, toc.Items);
            }

            return toc;
        }

        private void MergeTocItems(List<TableOfContents.TableOfContentsItem> source, List<TableOfContents.TableOfContentsItem> destination)
        {
            foreach (var sourceItem in source)
            {
                var matchedItem = destination.SingleOrDefault(x => x.Key == sourceItem.Key);

                if (matchedItem == null)
                    destination.Add(sourceItem);
                else
                    MergeTocItems(sourceItem.Items, matchedItem.Items);
            }
        }

        private void ReplaceToc(TableOfContents mergedToc, List<TableOfContents> tableOfContents)
        {
            tableOfContents.RemoveAll(x => x.Version == mergedToc.Version && x.Category == mergedToc.Category);
            tableOfContents.Add(mergedToc);
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
                .Where(documentationDirectory => Options.VersionsToParse.Count == 0 || Options.VersionsToParse.Contains(documentationDirectory.Name))
                .SelectMany(_directoryCompiler.CompileIndexFiles);
        }

        private IEnumerable<DocumentationPage> GenerateDocumentationPagesFromToc(IEnumerable<TableOfContents> tocs)
        {
            return tocs
                .Where(toc => Options.VersionsToParse.Count == 0 || Options.VersionsToParse.Contains(toc.Version))
                .SelectMany(_directoryCompiler.Compile);
        }
    }
}
