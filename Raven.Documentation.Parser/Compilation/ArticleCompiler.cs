using System.IO;
using HtmlAgilityPack;
using MarkdownDeep;
using Raven.Documentation.Parser.Compilation;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser
{
    public class ArticleCompiler : DocumentCompiler<ArticlePage>
    {
        private const string TitleSuffix = " | RavenDB";

        public ArticleCompiler(Markdown parser, ParserOptions options, IProvideGitFileInformation repoAnalyzer)
            : base(parser, options, repoAnalyzer)
        {
        }

        protected override ArticlePage CreatePage(CreatePageParams parameters)
        {
            return new ArticlePage
            {
                Key = parameters.Key,
                Title = parameters.Title,
                Version = parameters.DocumentationVersion,
                HtmlContent = parameters.HtmlContent,
                TextContent = parameters.TextContent,
                Language = parameters.Language,
                Category = parameters.Category,
                Images = parameters.Images,
                LastCommitSha = parameters.LastCommitSha,
                RelativePath = parameters.RelativePath,
                Mappings = parameters.Mappings,
                Metadata = parameters.Metadata,
                SeoMetaProperties = parameters.SeoMetaProperties
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

        protected override string ExtractTitle(FolderItem page, HtmlDocument htmlDocument)
        {
            string title = null;
            page.Metadata?.TryGetValue("title", out title);

            return string.IsNullOrEmpty(title) ? base.ExtractTitle(page, htmlDocument) : RemoveTitleSuffix(title);
        }

        private string RemoveTitleSuffix(string title)
        {
            if (title.EndsWith(TitleSuffix) == false)
                return title;

            var resultLength = title.Length - TitleSuffix.Length;
            return title.Substring(0, resultLength);
        }
    }
}
