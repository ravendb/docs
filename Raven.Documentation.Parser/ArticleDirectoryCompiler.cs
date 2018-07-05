using System.Collections.Generic;
using System.IO;

using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser
{
    public class ArticleDirectoryCompiler
    {
        private readonly ArticleCompiler _articleCompiler;

        private readonly ParserOptions _options;

        public ArticleDirectoryCompiler(ArticleCompiler articleCompiler, ParserOptions options)
        {
            _articleCompiler = articleCompiler;
            _options = options;
        }

        public IEnumerable<ArticlePage> Compile(DirectoryInfo directoryInfo)
        {
            return CompileArticleDirectory(_options.GetPathToArticlePagesDirectory());
        }

        private IEnumerable<ArticlePage> CompileArticleDirectory(string directory)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (item.IsFolder)
                {
                    foreach (var page in CompileArticleDirectory(Path.Combine(directory, item.Name)))
                    {
                        yield return page;
                    }

                    continue;
                }

                foreach (var pageToCompile in DocumentationDirectoryCompiler.GetPages(directory, item))
                {
                    yield return CompileArticlePage(pageToCompile, directory);
                }
            }

            var indexFilePath = Path.Combine(directory, "index" + Constants.MarkdownFileExtension);
            if (File.Exists(indexFilePath) == false)
                yield break;

            var indexItem = new FolderItem(isFolder: false)
            {
                Description = string.Empty,
                Language = Language.All,
                Name = "index"
            };

            yield return CompileArticlePage(indexItem, directory);
        }

        private ArticlePage CompileArticlePage(FolderItem page, string directory)
        {
            var path = Path.Combine(directory, page.Name + FileExtensionHelper.GetLanguageFileExtension(page.Language) + Constants.MarkdownFileExtension);
            var fileInfo = new FileInfo(path);

            if (fileInfo.Exists == false)
                throw new FileNotFoundException(string.Format("Documentation file '{0}' not found.", path));

            var compilationParams = new DocumentationCompilation.Parameters
            {
                File = fileInfo,
                Page = page,
                DocumentationVersion = "articles",
                SourceDocumentationVersion = "articles",
                Mappings = new List<DocumentationMapping>()
            };

            return _articleCompiler.Compile(compilationParams);
        }
    }
}
