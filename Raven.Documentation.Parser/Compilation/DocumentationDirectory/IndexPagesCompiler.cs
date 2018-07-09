using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser.Compilation.DocumentationDirectory
{
    internal class IndexPagesCompiler : DocumentationDirectoryCompiler
    {
        public IndexPagesCompiler(DocumentCompiler documentCompiler, ParserOptions options) : base(documentCompiler, options)
        {
        }

        public IEnumerable<DocumentationPage> Compile(DirectoryInfo directoryInfo)
        {
            var documentationVersion = directoryInfo.Name;

            var pagesDirectory = Options.GetPathToDocumentationPagesDirectory(documentationVersion);
            Debug.Assert(Directory.Exists(pagesDirectory));

            return CompileIndexFileInDirectory(pagesDirectory, documentationVersion);
        }

        private IEnumerable<DocumentationPage> CompileIndexFileInDirectory(string directory, string documentationVersion, List<DocumentationMapping> mappings = null)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (item.IsFolder == false)
                    continue;

                foreach (var page in CompileIndexFileInDirectory(Path.Combine(directory, item.Name), documentationVersion, item.Mappings))
                {
                    yield return page;
                }
            }

            var indexFilePath = Path.Combine(directory, "index" + Constants.MarkdownFileExtension);
            if (File.Exists(indexFilePath))
                yield return CompileIndexFile(directory, documentationVersion, mappings);
        }

        public DocumentationPage CompileIndexFile(string directory, string version, List<DocumentationMapping> mappings = null)
        {
            var indexItem = new FolderItem(isFolder: false)
            {
                Description = string.Empty,
                Language = Language.All,
                Name = "index"
            };

            return CompileDocumentationPage(indexItem, directory, version, mappings ?? new List<DocumentationMapping>());
        }
    }
}
