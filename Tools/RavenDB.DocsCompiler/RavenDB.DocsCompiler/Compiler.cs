// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Compiler.cs" company="Hibernating Rhinos">
//   COPYRIGHT GOES HERE
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RavenDB.DocsCompiler
{
    using System;
    using System.IO;
    using System.Linq;

    using RavenDB.DocsCompiler.MagicWorkers;
    using RavenDB.DocsCompiler.Model;
    using RavenDB.DocsCompiler.Output;

    /// <summary>
    /// The output type.
    /// </summary>
    public enum OutputType
    {
        /// <summary>
        /// HTML output.
        /// </summary>
        Html,

        /// <summary>
        /// Markdown output.
        /// </summary>
        Markdown
    }

    /// <summary>
    /// The documentation compiler.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// The file name representing a documentation list.
        /// </summary>
        private const string DocsListFileName = ".docslist";

        /// <summary>
        /// The destination full path.
        /// </summary>
        private readonly string destinationFullPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Compiler"/> class.
        /// </summary>
        /// <param name="destinationFullPath">
        /// The full path.
        /// </param>
        private Compiler(string destinationFullPath)
        {
            this.destinationFullPath = destinationFullPath;
        }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        public IDocsOutput Output { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to convert to html.
        /// </summary>
        public bool ConvertToHtml { get; set; }

        /// <summary>
        /// Gets or sets the code samples path.
        /// </summary>
        public string CodeSamplesPath { get; set; }

        /// <summary>
        /// Gets or sets the root folder.
        /// </summary>
        public Folder RootFolder { get; protected set; }

        /// <summary>
        /// Compiles the documentation in the folder.
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <param name="homeTitle">
        /// The home title.
        /// </param>
        /// <param name="versionUrl">
        /// The version url.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws an ArgumentNullException when output is null
        /// </exception>
        public static void CompileFolder(IDocsOutput output, string fullPath, string homeTitle, string versionUrl)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            var compiler = new Compiler(Path.Combine(fullPath, "docs"))
                               {
                                   Output = output,
                                   CodeSamplesPath =
                                       Path.Combine(fullPath, "code-samples")
                               };
            if (output.ContentType == OutputType.Html)
            {
                compiler.ConvertToHtml = true;
            }

            Console.WriteLine("CompileFolder - processing {0}", compiler.RootFolder);
            var generatedText =
                compiler.CompileFolder(
                    compiler.RootFolder = new Folder { Title = homeTitle, Trail = string.Empty }, versionUrl);
            var combine = Path.Combine(fullPath, "all.markdown");
            File.WriteAllText(combine, generatedText);

            compiler.Output.GenerateTableOfContents(compiler.RootFolder);
        }

        /// <summary>
        /// Compiles the folder.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <param name="versionUrl">
        /// The version url.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string CompileFolder(Folder folder, string versionUrl)
        {
            var fullFolderSlug = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
            var fullPath = Path.Combine(this.destinationFullPath, fullFolderSlug);
            if (!File.Exists(Path.Combine(fullPath, DocsListFileName)))
            {
                return string.Empty;
            }

            if (this.ConvertToHtml)
            {
                this.ProcessAsHtml(folder, versionUrl, fullPath, fullFolderSlug);
                return string.Empty;
            }

            return this.ProcessAsMarkdown(folder, versionUrl, fullPath, fullFolderSlug);
        }

        /// <summary>
        /// Processes item as HTML.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <param name="versionUrl">
        /// The version url.
        /// </param>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <param name="fullFolderSlug">
        /// The full folder slug.
        /// </param>
        private void ProcessAsHtml(Folder folder, string versionUrl, string fullPath, string fullFolderSlug)
        {
            var docs = DocsListParser.Parse(Path.Combine(fullPath, DocsListFileName)).ToArray();
            foreach (var item in docs)
            {
                Console.WriteLine("CompileFolder - processing item {0}", item.Trail);
                this.ProcessItem(folder, versionUrl, item, fullPath);
            }

            var contents = DocumentationParser.Parse(
                this,
                folder,
                Path.Combine(fullPath, "index.markdown"),
                string.IsNullOrWhiteSpace(folder.Trail) ? folder.Slug : folder.Trail + "/" + folder.Slug,
                versionUrl,
                this.ConvertToHtml);
            this.Output.SaveDocItem(
                new Document { Title = folder.Title, Content = contents, Trail = fullFolderSlug, Slug = "index" });

            this.CopyImages(folder, fullPath);
        }

        /// <summary>
        /// Process item as markdown.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <param name="versionUrl">
        /// The version url.
        /// </param>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <param name="fullFolderSlug">
        /// The full folder slug.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ProcessAsMarkdown(Folder folder, string versionUrl, string fullPath, string fullFolderSlug)
        {
            var output = string.Empty;
            output += this.ProcessIndexItem(folder, versionUrl, fullPath, fullFolderSlug);

            var docs = DocsListParser.Parse(Path.Combine(fullPath, DocsListFileName)).ToArray();
            foreach (var item in docs)
            {
                Console.WriteLine("CompileFolder - processing item {0}", item.Trail);
                output += this.ProcessItem(folder, versionUrl, item, fullPath);
            }

            this.CopyImages(folder, fullPath);
            return output;
        }

        /// <summary>
        /// Process index item.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <param name="versionUrl">
        /// The version url.
        /// </param>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <param name="fullFolderSlug">
        /// The full folder slug.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ProcessIndexItem(Folder folder, string versionUrl, string fullPath, string fullFolderSlug)
        {
            var contents = DocumentationParser.Parse(
                this,
                folder,
                Path.Combine(fullPath, "index.markdown"),
                string.IsNullOrWhiteSpace(folder.Trail) ? folder.Slug : folder.Trail + "/" + folder.Slug,
                versionUrl,
                this.ConvertToHtml);
            this.Output.SaveDocItem(
                new Document { Title = folder.Title, Content = contents, Trail = fullFolderSlug, Slug = "index" });
            return contents;
        }

        /// <summary>
        /// Processes an item.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <param name="versionUrl">
        /// The version url.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ProcessItem(Folder folder, string versionUrl, IDocumentationItem item, string fullPath)
        {
            if (item.Slug != null)
            {
                item.Slug = item.Slug.TrimStart('\\', '/');
            }

            item.Trail = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
            folder.Items.Add(item);

            var document = item as Document;
            if (document != null)
            {
                return this.CompileDocument(versionUrl, document, fullPath);
            }

            var subFolder = item as Folder;
            if (subFolder != null)
            {
                return CompileFolder(subFolder, versionUrl);
            }

            return string.Empty;
        }

        /// <summary>
        /// TCopy images.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        private void CopyImages(Folder folder, string fullPath)
        {
            // Copy images
            var imagesPath = Path.Combine(fullPath, "images");
            if (!Directory.Exists(imagesPath))
            {
                return;
            }

            var images = Directory.GetFiles(imagesPath);
            foreach (var image in images)
            {
                var imageFileName = Path.GetFileName(image);
                if (imageFileName == null)
                {
                    continue;
                }

                this.Output.SaveImage(folder, image);
            }
        }

        /// <summary>
        /// Compile a document.
        /// </summary>
        /// <param name="versionUrl">
        /// The version url.
        /// </param>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string CompileDocument(string versionUrl, Document document, string fullPath)
        {
            var strippedSlug = document.Slug.Replace(".markdown", string.Empty);
            string path = Path.Combine(fullPath, document.Slug);
            document.Content = DocumentationParser.Parse(this, null, path, document.Trail, versionUrl, this.ConvertToHtml);
            document.Slug = strippedSlug;
            this.Output.SaveDocItem(document);
            return document.Content;
        }
    }
}
