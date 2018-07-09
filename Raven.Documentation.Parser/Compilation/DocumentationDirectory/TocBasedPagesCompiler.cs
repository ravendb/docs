using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser.Compilation.DocumentationDirectory
{
    internal class TocBasedPagesCompiler : DocumentationDirectoryCompiler
    {
        public TocBasedPagesCompiler(DocumentCompiler documentCompiler, ParserOptions options) : base(documentCompiler, options)
        {
        }

        public IEnumerable<DocumentationPage> Compile(TableOfContents tableOfContents)
        {
            foreach (var tableOfContentsItem in tableOfContents.Items)
            {
                var tocItemCompilationResult = CompileTableOfContentsItem(tableOfContentsItem, tableOfContents.Version);

                foreach (var documentationPage in tocItemCompilationResult)
                {
                    yield return documentationPage;
                }
            }
        }

        private IEnumerable<DocumentationPage> CompileTableOfContentsItem(TableOfContents.TableOfContentsItem tocItem, string version)
        {
            if (tocItem.IsFolder)
            {
                foreach (var innerTocItem in tocItem.Items)
                {
                    foreach (var innerTocItemCompiled in CompileTableOfContentsItem(innerTocItem, version))
                    {
                        yield return innerTocItemCompiled;
                    }
                }

                yield break;
            }

            var directory = tocItem.GetSourceBaseDirectory(Options);
            var pagesToCompile = GetPagesForTocItem(tocItem, directory);

            foreach (var pageToCompile in pagesToCompile)
            {
                yield return CompileDocumentationPage(pageToCompile, directory, version, pageToCompile.Mappings, tocItem.SourceVersion);
            }
        }

        private IEnumerable<FolderItem> GetPagesForTocItem(TableOfContents.TableOfContentsItem tocItem, string directory)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            var keySuffix = tocItem.GetKeySuffix();

            var matchingFolderItems = DocumentationFileHelper.ParseFile(docsFilePath)
                .Where(x => x.Name == keySuffix)
                .SelectMany(x => GetPages(directory, x));

            foreach (var folderItem in matchingFolderItems)
            {
                yield return folderItem;
            }
        }
    }
}
