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

        public IEnumerable<ArticlePage> Compile(DirectoryInfo directoryInfo, DocumentationCompilation.Context context)
        {
            return CompileArticleDirectory(_options.GetPathToArticlePagesDirectory(), context);
        }

        private IEnumerable<ArticlePage> CompileArticleDirectory(string directory, DocumentationCompilation.Context context)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (item.IsFolder)
                {
                    foreach (var page in CompileArticleDirectory(Path.Combine(directory, item.Name), context))
                    {
                        yield return page;
                    }

                    continue;
                }

                foreach (var pageToCompile in DocumentationDirectoryCompiler.GetPages(directory, item))
                {
                    yield return CompileArticlePage(pageToCompile, directory, context);
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

            yield return CompileArticlePage(indexItem, directory, context);
        }

        private ArticlePage CompileArticlePage(FolderItem page, string directory, DocumentationCompilation.Context compilationContext)
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
                Mappings = new List<DocumentationMapping>()
            };

            return _articleCompiler.Compile(compilationParams, compilationContext);
        }
    }
}
