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

			compiler.CompileFolder(compiler.RootFolder = new Folder { Title = homeTitle, Trail = string.Empty }, versionUrl);

			compiler.Output.GenerateToc(compiler.RootFolder);
		}

		private void CompileFolder(Folder folder, string versionUrl)
		{
			var fullFolderSlug = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
			var fullPath = Path.Combine(_fullPath, fullFolderSlug);
			if (!File.Exists(Path.Combine(fullPath, DocsListFileName)))
				return;

			var docs = DocsListParser.Parse(Path.Combine(fullPath, DocsListFileName)).ToArray();
			foreach (var item in docs)
			{
				if (item.Slug != null)
					item.Slug = item.Slug.TrimStart('\\', '/');
				item.Trail = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
				folder.Items.Add(item);

				var document = item as Document;
				if (document != null)
				{
					var strippedSlug = document.Slug.Replace(".markdown", string.Empty);
					document.Content = DocumentationParser.Parse(this, null, Path.Combine(fullPath, document.Slug), document.Trail, versionUrl);
					document.Slug = strippedSlug;
					Output.SaveDocItem(document);
					continue;
				}

				var subFolder = item as Folder;
				if (subFolder != null)
				{
					CompileFolder(subFolder, versionUrl);
					continue;
				}
			}

			var contents = DocumentationParser.Parse(this, folder, Path.Combine(fullPath, "index.markdown"),
			                                         string.IsNullOrWhiteSpace(folder.Trail) ? folder.Slug : folder.Trail + "/" + folder.Slug,
													 versionUrl);
			Output.SaveDocItem(new Document
			{
				Title = folder.Title,
				Content = contents,
				Trail = fullFolderSlug,
				Slug = "index"
			});

			// Copy images
			var imagesPath = Path.Combine(fullPath, "images");
			if (Directory.Exists(imagesPath))
			{
				var images = Directory.GetFiles(imagesPath);
				foreach (var image in images)
				{
					var imageFileName = Path.GetFileName(image);
					if (imageFileName == null) continue;
					Output.SaveImage(folder, image);
				}
			}
		}
	}
}
