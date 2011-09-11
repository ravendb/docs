using System.Collections.Generic;
using System.IO;
using RavenDB.DocsCompiler.MagicWorkers;
using RavenDB.DocsCompiler.Model;

namespace RavenDB.DocsCompiler
{
	public static class Compiler
	{
		private const string DocsListFileName = ".docslist";

		public static void CompileFile(string title, string fullPath, string outputFullPath)
		{
			var template = File.ReadAllText(@"z:\Projects\RavenDB\RavenDB-docs\Tools\html-template.html");

			var contents = string.Format(template, title, DocumentationParser.Parse(fullPath));
			File.WriteAllText(outputFullPath, contents);
		}

		public static void CompileFolder(string title, string fullPath, string outputPath)
		{
			if (!File.Exists(Path.Combine(fullPath, DocsListFileName)))
				return;

			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}

			CompileFile(title, Path.Combine(fullPath, "index.markdown"), Path.Combine(outputPath, "index.html"));

			// Copy images
			// TODO: Thumbs, lightbox
			var imagesPath = Path.Combine(fullPath, "images");
			if (Directory.Exists(imagesPath))
			{
				if (!Directory.Exists(Path.Combine(outputPath, @"images")))
					Directory.CreateDirectory(Path.Combine(outputPath, @"images"));

				var images = Directory.GetFiles(imagesPath);
				foreach (var image in images)
				{
					File.Copy(image, Path.Combine(outputPath, @"images", Path.GetFileName(image)), true);
				}
			}

			var foldersToProcess = new List<Folder>();

			var docs = DocsListParser.Parse(Path.Combine(fullPath, DocsListFileName));
			foreach (var item in docs)
			{
				var document = item as Document;
				if (document != null)
				{
					CompileFile(document.Title, Path.Combine(fullPath, document.Slug), Path.Combine(outputPath, document.Slug.Replace(".markdown", ".html")));
					continue;
				}

				var folder = item as Folder;
				if (folder != null)
				{
					foldersToProcess.Add(folder);
					continue;
				}
			}

			foreach (var folder in foldersToProcess)
			{
				CompileFolder(folder.Name, Path.Combine(fullPath, folder.Slug.TrimStart('\\', '/')), Path.Combine(outputPath, folder.Slug.TrimStart('\\', '/')));
			}
		}
	}
}
