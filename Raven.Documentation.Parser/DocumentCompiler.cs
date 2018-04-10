namespace Raven.Documentation.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using HtmlAgilityPack;

    using MarkdownDeep;

    using Raven.Documentation.Parser.Data;
    using Raven.Documentation.Parser.Helpers;

    public class DocumentCompiler : DocumentCompiler<DocumentationPage>
    {
        public DocumentCompiler(Markdown parser, ParserOptions options, IProvideGitFileInformation repoAnalyzer)
            : base(parser, options, repoAnalyzer)
        {
        }

        protected override DocumentationPage CreatePage(
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
            string relatedArticlesContent,
            Dictionary<string, string> metadata = null,
            Dictionary<string, string> seoMetaProperties = null)
        {
            return new DocumentationPage
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
                Metadata = metadata,
                SeoMetaProperties = seoMetaProperties,
                RelatedArticlesContent = relatedArticlesContent
            };
        }
    }

    public abstract class DocumentCompiler<TPage> where TPage : DocumentationPage
    {
        private readonly Markdown _parser;

        protected readonly ParserOptions Options;

        private readonly IProvideGitFileInformation _repoAnalyzer;

        protected DocumentCompiler(Markdown parser, ParserOptions options, IProvideGitFileInformation repoAnalyzer)
        {
            _parser = parser;
            Options = options;
            _repoAnalyzer = repoAnalyzer;
        }

        protected abstract TPage CreatePage(string key, string title, string documentationVersion, string htmlContent,
            string textContent, Language language, Category category, HashSet<DocumentationImage> images,
            string lastCommitSha, string relativePath, List<DocumentationMapping> mappings,
            string relatedArticlesContent,
            Dictionary<string, string> metadata = null,
            Dictionary<string, string> seoMetaProperties = null);

        public TPage Compile(FileInfo file, FolderItem page, string documentationVersion, List<DocumentationMapping> mappings)
        {
            try
            {
                var key = ExtractKey(file, page, documentationVersion);
                var category = CategoryHelper.ExtractCategoryFromPath(key);
                var images = new HashSet<DocumentationImage>();

                _parser.PrepareImage = (tag, b) => PrepareImage(images, file.DirectoryName, Options.ImagesUrl, documentationVersion, tag, key);

                var content = File.ReadAllText(file.FullName);

                var builder = new DocumentBuilder(_parser, Options, documentationVersion, content);
                builder.TransformRawHtmlBlocks();
                builder.TransformLegacyBlocks(file);
                builder.TransformBlocks();

                content = builder.Build(page);

                var htmlDocument = HtmlHelper.ParseHtml(content);

                ProcessNonMarkdownImages(file, documentationVersion, htmlDocument, images);

                var title = ExtractTitle(htmlDocument);
                var textContent = ExtractTextContent(htmlDocument, out var relatedArticlesContent);

                var caseSensitiveFileName = PathHelper.GetProperFilePathCapitalization(file.FullName);

                var fullName = caseSensitiveFileName ?? file.FullName;

                var repoRelativePath = _repoAnalyzer.MakeRelativePathInRepository(fullName);

                var relativeUrl = repoRelativePath.Replace(@"\", @"/");

                var lastCommit = _repoAnalyzer.GetLastCommitThatAffectedFile(repoRelativePath);

                return CreatePage(
                    key,
                    title,
                    documentationVersion,
                    htmlDocument.DocumentNode.OuterHtml,
                    textContent,
                    page.Language,
                    category,
                    images,
                    lastCommit,
                    relativeUrl,
                    mappings.OrderBy(x => x.Version).ToList(),
                    relatedArticlesContent,
                    page.Metadata,
                    page.SeoMetaProperties);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format("Could not compile '{0}'.", file.FullName), e);
            }
        }

        private void ProcessNonMarkdownImages(FileInfo file, string documentationVersion, HtmlDocument htmlDocument,
            HashSet<DocumentationImage> images)
        {
            var nonMarkdownImages = htmlDocument.DocumentNode.SelectNodes("//img[starts-with(@src, 'images/')]");
            if (nonMarkdownImages == null)
            {
                return;
            }

            foreach (var node in nonMarkdownImages)
            {
                AddNonMarkdownImage(images, file.DirectoryName, Options.ImagesUrl, documentationVersion, node);
            }
        }

        private static bool AddNonMarkdownImage(ICollection<DocumentationImage> images, string directory, string imagesUrl,
            string documentationVersion, HtmlNode node)
        {
            if (node.Attributes.Contains("src"))
            {
                string src = node.Attributes["src"].Value;
                var imagePath = Path.Combine(directory, src);

                src = src.Replace('\\', '/');
                if (src.StartsWith("images/", StringComparison.InvariantCultureIgnoreCase))
                    src = src.Substring(7);

                var newSrc = string.Format("{0}/{1}", documentationVersion, src);

                node.SetAttributeValue("src", imagesUrl + newSrc);

                images.Add(new DocumentationImage
                {
                    ImagePath = imagePath,
                    ImageKey = newSrc
                });
            }

            return true;
        }

        private static bool PrepareImage(ICollection<DocumentationImage> images, string directory, string imagesUrl, string documentationVersion, HtmlTag tag, string key)
        {
            string src;
            if (tag.attributes.TryGetValue("src", out src))
            {
                var imagePath = Path.Combine(directory, src);

                src = src.Replace('\\', '/');
                if (src.StartsWith("."))
                    throw new InvalidOperationException($"Invalid image path '{src}' in article '{key}'. It cannot start from dot ('.').");

                if (src.StartsWith("images/", StringComparison.InvariantCultureIgnoreCase))
                    src = src.Substring(7);

                var newSrc = string.Format("{0}/{1}/{2}", documentationVersion, key, src);

                tag.attributes["src"] = imagesUrl + newSrc;

                images.Add(new DocumentationImage
                {
                    ImagePath = imagePath,
                    ImageKey = newSrc
                });
            }

            tag.attributes["class"] = "img-responsive img-thumbnail";

            return true;
        }

        protected virtual string ExtractKey(FileInfo file, FolderItem page, string documentationVersion)
        {
            var pathToDocumentationPagesDirectory = Options.GetPathToDocumentationPagesDirectory(documentationVersion);
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

        private static string ExtractTextContent(HtmlDocument htmlDocument, out string relatedArticlesHtmlContent)
        {
            var relatedArticles =
                htmlDocument.DocumentNode.ChildNodes.FirstOrDefault(
                    x => x.InnerText.Equals("Related articles", StringComparison.OrdinalIgnoreCase));

            if (relatedArticles == null)
            {
                relatedArticlesHtmlContent = null;
                return htmlDocument.DocumentNode.InnerText;
            }

            var nodeToRemove = relatedArticles;
            var nodesToRemove = new List<HtmlNode>();
            while (nodeToRemove != null)
            {
                nodesToRemove.Add(nodeToRemove);
                nodeToRemove = nodeToRemove.NextSibling;
            }

            foreach (var node in nodesToRemove)
            {
                htmlDocument.DocumentNode.RemoveChild(node);
            }

            relatedArticlesHtmlContent = string.Join(Environment.NewLine, nodesToRemove.Skip(1).Select(x => x.OuterHtml));

            return htmlDocument.DocumentNode.InnerText;
        }

        private static string ExtractTitle(HtmlDocument htmlDocument)
        {
            var node = htmlDocument.DocumentNode.ChildNodes.FirstOrDefault(x => x.Name == "h1");
            if (node == null)
                return "No title";

            return node.InnerText;
        }
    }
}
