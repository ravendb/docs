using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using MarkdownDeep;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser.Compilation
{
    public class DocumentCompiler : DocumentCompiler<DocumentationPage>
    {
        public DocumentCompiler(Markdown parser, ParserOptions options, IProvideGitFileInformation repoAnalyzer)
            : base(parser, options, repoAnalyzer)
        {
        }

        protected override DocumentationPage CreatePage(CreatePageParams parameters)
        {
            return new DocumentationPage
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
                SeoMetaProperties = parameters.SeoMetaProperties,
                RelatedArticlesContent = parameters.RelatedArticlesContent,
                DiscussionId = parameters.DiscussionId
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

        protected abstract TPage CreatePage(CreatePageParams parameters);

        protected class CreatePageParams
        {
            public string Key { get; set; }
            public string Title { get; set; }
            public string DocumentationVersion { get; set; }
            public string HtmlContent { get; set; }
            public string TextContent { get; set; }
            public Language Language { get; set; }
            public Category Category { get; set; }
            public HashSet<DocumentationImage> Images { get; set; }
            public string LastCommitSha { get; set; }
            public string RelativePath { get; set; }
            public List<DocumentationMapping> Mappings { get; set; }
            public string RelatedArticlesContent { get; set; }
            public Dictionary<string, string> Metadata { get; set; }
            public Dictionary<string, string> SeoMetaProperties { get; set; }
            public string DiscussionId { get; set; }
        }

        public TPage Compile(CompilationUtils.Parameters parameters)
        {
            var file = parameters.File;
            var page = parameters.Page;
            var documentationVersion = parameters.DocumentationVersion;
            var sourceDocumentationVersion = parameters.SourceDocumentationVersion;
            var mappings = parameters.Mappings;

            try
            {
                var key = ExtractKey(file, page, documentationVersion);
                var category = CategoryHelper.ExtractCategoryFromPath(key);
                var images = new HashSet<DocumentationImage>();

                _parser.PrepareImage = (tag, b) => PrepareImage(images, file.DirectoryName, Options.ImageUrlGenerator, documentationVersion, page.Language, tag, key);

                var content = File.ReadAllText(file.FullName);

                var builder = new DocumentBuilder(_parser, Options, sourceDocumentationVersion, content);
                builder.TransformRawHtmlBlocks();
                builder.TransformLegacyBlocks(file);
                builder.TransformBlocks();

                content = builder.Build(page);

                var htmlDocument = HtmlHelper.ParseHtml(content);

                ProcessNonMarkdownImages(file, documentationVersion, page.Language, htmlDocument, images, key);

                var title = ExtractTitle(page, htmlDocument);

                ValidateTitle(title);

                var textContent = ExtractTextContent(htmlDocument, out var relatedArticlesContent);

                var caseSensitiveFileName = PathHelper.GetProperFilePathCapitalization(file.FullName);

                var fullName = caseSensitiveFileName ?? file.FullName;

                var repoRelativePath = _repoAnalyzer.MakeRelativePathInRepository(fullName);

                var relativeUrl = repoRelativePath.Replace(@"\", @"/");

                var lastCommit = _repoAnalyzer.GetLastCommitThatAffectedFile(repoRelativePath);

                var createPageParams = new CreatePageParams
                {
                    Key = key,
                    Title = title,
                    DocumentationVersion = documentationVersion,
                    HtmlContent = htmlDocument.DocumentNode.OuterHtml,
                    TextContent = textContent,
                    Language = page.Language,
                    Category = category,
                    Images = images,
                    LastCommitSha = lastCommit,
                    RelativePath = relativeUrl,
                    Mappings = mappings.OrderBy(x => x.Version).ToList(),
                    RelatedArticlesContent = relatedArticlesContent,
                    Metadata = page.Metadata,
                    SeoMetaProperties = page.SeoMetaProperties,
                    DiscussionId = page.DiscussionId
                };

                return CreatePage(createPageParams);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format("Could not compile '{0}'.", file.FullName), e);
            }
        }

        private void ProcessNonMarkdownImages(FileInfo file, string documentationVersion, Language lang, HtmlDocument htmlDocument,
            HashSet<DocumentationImage> images, string key)
        {
            var nonMarkdownImages = htmlDocument.DocumentNode.SelectNodes("//img[starts-with(@src, 'images/')]");
            if (nonMarkdownImages == null)
            {
                return;
            }

            foreach (var node in nonMarkdownImages)
            {
                AddNonMarkdownImage(images, file.DirectoryName, Options.ImageUrlGenerator, documentationVersion, lang, node, key);
            }
        }

        private static void AddNonMarkdownImage(ICollection<DocumentationImage> images, string directory,
            ParserOptions.GenerateImageUrl generateImageUrl, string documentationVersion, Language lang, HtmlNode node, string key)
        {
            if (node.Attributes.Contains("src"))
            {
                string src = node.Attributes["src"].Value;
                var imagePath = Path.Combine(directory, src);

                src = src.Replace('\\', '/');
                if (src.StartsWith("images/", StringComparison.InvariantCultureIgnoreCase))
                    src = src.Substring(7);

                var fileName = Path.GetFileName(src);
                var imageUrl = generateImageUrl(documentationVersion, lang, key, fileName);

                node.SetAttributeValue("src", imageUrl);

                images.Add(new DocumentationImage
                {
                    ImagePath = imagePath,
                    ImageKey = $"{documentationVersion}/{src}"
                });
            }
        }

        private static bool PrepareImage(ICollection<DocumentationImage> images, string directory,
            ParserOptions.GenerateImageUrl generateImageUrl, string documentationVersion, Language lang, HtmlTag tag, string key)
        {
            string src;
            if (tag.attributes.TryGetValue("src", out src))
            {
                var imagePath = Path.Combine(directory, src);

                if (File.Exists(imagePath) == false)
                    throw new InvalidOperationException($"Could not find image in '{imagePath}' for article '{key}'.");

                src = src.Replace('\\', '/');
                if (src.StartsWith("."))
                    throw new InvalidOperationException($"Invalid image path '{src}' in article '{key}'. It cannot start from dot ('.').");

                if (src.StartsWith("images/", StringComparison.InvariantCultureIgnoreCase))
                    src = src.Substring(7);

                var fileName = Path.GetFileName(src);
                var imageUrl = generateImageUrl(documentationVersion, lang, key, fileName);

                tag.attributes["src"] = imageUrl;

                images.Add(new DocumentationImage
                {
                    ImagePath = imagePath,
                    ImageKey = $"{documentationVersion}/{src}"
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
        
        protected virtual string ExtractTitle(FolderItem page, HtmlDocument htmlDocument)
        {
            var node = htmlDocument.DocumentNode.ChildNodes.FirstOrDefault(x => x.Name == "h1");
            if (node == null)
                return "No title";

            return WebUtility.HtmlDecode(node.InnerText);
        }

        private void ValidateTitle(string title)
        {
            if (title.Contains(" :"))
                throw new InvalidOperationException("Please remove space before the colon (\" :\") in the markdown file heading.");
        }
    }
}
