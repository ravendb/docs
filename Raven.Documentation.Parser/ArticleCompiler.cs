using System.Collections.Generic;
using System.IO;

using MarkdownDeep;

using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser
{
    public class ArticleCompiler : DocumentCompiler<ArticlePage>
    {
        public ArticleCompiler(Markdown parser, ParserOptions options, IProvideGitFileInformation repoAnalyzer)
            : base(parser, options, repoAnalyzer)
        {
        }

        protected override ArticlePage CreatePage(
            string key,
            string title,
            string documentationVersion,
            string htmlContent,
            string textContent,
            Language language,
            Category category,
            HashSet<DocumentationImage> images,
            string lastCommitSha,
            string relativePath,
            List<DocumentationMapping> mappings,
            Dictionary<string, string> metadata = null)
        {
            return new ArticlePage
            {
                Key = key,
                Title = title,
                Version = documentationVersion,
                HtmlContent = htmlContent,
                TextContent = textContent,
                Language = language,
                Category = category,
                Images = images,
                LastCommitSha = lastCommitSha,
                RelativePath = relativePath,
                Mappings = mappings,
                Metadata = metadata
            };
        }

        protected override string ExtractKey(FileInfo file, FolderItem page, string documentationVersion)
        {
            var pathToDocumentationPagesDirectory = Options.GetPathToArticlePagesDirectory();
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
    }
}