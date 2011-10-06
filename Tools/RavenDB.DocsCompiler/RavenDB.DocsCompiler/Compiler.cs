using System;
using System.Collections.Generic;
using System.IO;
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

		public Folder RootFolder { get; protected set; }
		private readonly string _fullPath;

		private Compiler(string fullPath)
		{
			_fullPath = fullPath;
		}

		//public void CompileFile(string title, string fullPath, string outputFullPath)
		//{
		//    var template = File.ReadAllText(@"z:\Projects\RavenDB\RavenDB-docs\Tools\html-template.html");

		//    var contents = string.Format(template, title, DocumentationParser.Parse(fullPath));
		//    File.WriteAllText(outputFullPath, contents);
		//}

		public static void CompileFolder(IDocsOutput output, string fullPath, string homeTitle)
		{
			if (output == null)
				throw new ArgumentNullException("output");

			var compiler = new Compiler(fullPath);
			compiler.Output = output;
			compiler.CompileFolder(compiler.RootFolder = new Folder { Title = homeTitle, Trail = string.Empty });

			compiler.Output.GenerateToc(compiler.RootFolder);
		}

		private void CompileFolder(Folder folder)
		{
			var fullFolderSlug = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);

			var fullPath = Path.Combine(_fullPath, fullFolderSlug);
			if (!File.Exists(Path.Combine(fullPath, DocsListFileName)))
				return;

			var contents = DocumentationParser.Parse(Path.Combine(fullPath, "index.markdown"));
			Output.SaveDocItem(new Document
			                   	{
			                   		Title = folder.Title,
			                   		Content = contents,
									Trail = fullFolderSlug,
			                   		Slug = "index"
			                   	});

			// Copy images
			// TODO: Thumbs, lightbox
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

			var docs = DocsListParser.Parse(Path.Combine(fullPath, DocsListFileName));

			foreach (var item in docs)
			{
				if (item.Slug != null)
					item.Slug = item.Slug.TrimStart('\\', '/');
				item.Trail = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
				folder.Items.Add(item);

				var document = item as Document;
				if (document != null)
				{
					document.Content = DocumentationParser.Parse(Path.Combine(fullPath, document.Slug));
					document.Slug = document.Slug.Replace(".markdown", "");
					Output.SaveDocItem(document);
					continue;
				}

				var subFolder = item as Folder;
				if (subFolder != null)
				{
					CompileFolder(subFolder);
					continue;
				}
			}
		}
	}
}
