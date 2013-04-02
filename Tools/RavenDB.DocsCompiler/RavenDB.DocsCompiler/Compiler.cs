using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RavenDB.DocsCompiler.MagicWorkers;
using RavenDB.DocsCompiler.Model;
using RavenDB.DocsCompiler.Output;

namespace RavenDB.DocsCompiler
{
	public class Compiler
	{
		private const string DocsListFileName = ".docslist";

		public IDocsOutput Output { get; set; }

        public bool ConvertToHTML { get; set; }

		public string CodeSamplesPath { get; set; }

		public Folder RootFolder { get; protected set; }
		private readonly string _fullPath;

		private Compiler(string fullPath)
		{
			_fullPath = fullPath;
		}

		public static void CompileFolder(IDocsOutput output, string fullPath, string homeTitle, string versionUrl)
		{
			if (output == null)
				throw new ArgumentNullException("output");
            

			var compiler = new Compiler(Path.Combine(fullPath, "docs"))
			               	{
			               		Output = output,
			               		CodeSamplesPath = Path.Combine(fullPath, "code-samples")
			               	};
            if (output.ContentType.Equals("html", StringComparison.InvariantCultureIgnoreCase))
            {
                compiler.ConvertToHTML = true;
            }

		    Console.WriteLine("CompileFolder - processing {0}", compiler.RootFolder);
			var generatedText = compiler.CompileFolder(compiler.RootFolder = new Folder { Title = homeTitle, Trail = string.Empty }, versionUrl);
		    var combine = Path.Combine(fullPath, "all.markdown");
		    File.WriteAllText(combine, generatedText );

			compiler.Output.GenerateToc(compiler.RootFolder);
		}

		private string CompileFolder(Folder folder, string versionUrl)
		{
			var fullFolderSlug = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
			var fullPath = Path.Combine(_fullPath, fullFolderSlug);
			if (!File.Exists(Path.Combine(fullPath, DocsListFileName)))
				return "";

            if (ConvertToHTML)
            {
                processAsHTML(folder, versionUrl, fullPath, fullFolderSlug);
                return "";
            }
            else
            {
                return processAsMarkdown(folder, versionUrl, fullPath, fullFolderSlug);
            }
			
			
		}

	    private void processAsHTML(Folder folder, string versionUrl, string fullPath, string fullFolderSlug)
	    {
	        var docs = DocsListParser.Parse(Path.Combine(fullPath, DocsListFileName)).ToArray();
	        foreach (var item in docs)
	        {
	            Console.WriteLine("CompileFolder - processing item {0}", item.Trail);
	            processItem(folder, versionUrl, item, fullPath);
	        }

	        var contents = DocumentationParser.Parse(this, folder, Path.Combine(fullPath, "index.markdown"),
	                                                 string.IsNullOrWhiteSpace(folder.Trail)
	                                                     ? folder.Slug
	                                                     : folder.Trail + "/" + folder.Slug,
	                                                 versionUrl, ConvertToHTML);
	        Output.SaveDocItem(new Document
	            {
	                Title = folder.Title,
	                Content = contents,
	                Trail = fullFolderSlug,
	                Slug = "index"
	            });

	        CopyImages(folder, fullPath);
	        return;
	    }

        private string processAsMarkdown(Folder folder, string versionUrl, string fullPath, string fullFolderSlug)
        {
            var output = "";
            output += processIndexItem(folder, versionUrl, fullPath, fullFolderSlug);

            var docs = DocsListParser.Parse(Path.Combine(fullPath, DocsListFileName)).ToArray();
            foreach (var item in docs)
            {
                Console.WriteLine("CompileFolder - processing item {0}", item.Trail);
                output += processItem(folder, versionUrl, item, fullPath);
            }

            

            CopyImages(folder, fullPath);
            return output;
        }

	    private string processIndexItem(Folder folder, string versionUrl, string fullPath, string fullFolderSlug)
	    {
	        var contents = DocumentationParser.Parse(this, folder, Path.Combine(fullPath, "index.markdown"),
	                                                 string.IsNullOrWhiteSpace(folder.Trail)
	                                                     ? folder.Slug
	                                                     : folder.Trail + "/" + folder.Slug,
	                                                 versionUrl, ConvertToHTML);
	        Output.SaveDocItem(new Document
	            {
	                Title = folder.Title,
	                Content = contents,
	                Trail = fullFolderSlug,
	                Slug = "index"
	            });
	        return contents;
	    }

	    private string processItem(Folder folder, string versionUrl, IDocumentationItem item, string fullPath)
	    {
	        if (item.Slug != null)
	            item.Slug = item.Slug.TrimStart('\\', '/');
	        item.Trail = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
	        folder.Items.Add(item);

	        var document = item as Document;
	        if (document != null)
	        {
                return CompileDocument(versionUrl, document, fullPath);
	        }

	        var subFolder = item as Folder;
	        if (subFolder != null)
	        {
	            return CompileFolder(subFolder, versionUrl);
	        }
	        return "";
	    }

	    private void CopyImages(Folder folder, string fullPath)
	    {
            // Copy images
	        var imagesPath = Path.Combine(fullPath, "images");
	        if (!Directory.Exists(imagesPath))
                return;
	        var images = Directory.GetFiles(imagesPath);
	        foreach (var image in images)
	        {
	            var imageFileName = Path.GetFileName(image);
	            if (imageFileName == null) continue;
	            Output.SaveImage(folder, image);
	        }
	    }

	    private string CompileDocument(string versionUrl, Document document, string fullPath)
	    {
	        var strippedSlug = document.Slug.Replace(".markdown", string.Empty);
	        string path = Path.Combine(fullPath, document.Slug);
	        document.Content = DocumentationParser.Parse(this, null, path, document.Trail,
	                                                     versionUrl, ConvertToHTML);
	        document.Slug = strippedSlug;
	        Output.SaveDocItem(document);
	        return document.Content;
	    }
	}
}
