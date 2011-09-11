using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RavenDB.DocsCompiler.MagicWorkers;
using RavenDB.DocsCompiler.Model;

namespace RavenDB.DocsCompiler
{
	public class Compiler
	{
		private const string DocsListFileName = ".docslist";

		public Folder RootFolder { get; protected set; }
		private readonly string _outputPath, _fullPath;

		private Compiler(string fullPath, string outputPath)
		{
			_fullPath = fullPath;
			_outputPath = outputPath;
		}

		public static void CompileFile(string title, string fullPath, string outputFullPath)
		{
			var template = File.ReadAllText(@"z:\Projects\RavenDB\RavenDB-docs\Tools\html-template.html");

			var contents = string.Format(template, title, DocumentationParser.Parse(fullPath));
			File.WriteAllText(outputFullPath, contents);
		}

		public static void CompileFolder(string title, string fullPath, string outputPath)
		{
			var compiler = new Compiler(fullPath, outputPath);
			compiler.CompileFolder(compiler.RootFolder = new Folder { Title = title, Trail = string.Empty });

			var menuToc = Path.Combine(outputPath, "toc.html");
			var sb = new StringBuilder();
			CreateHtmlToc(compiler.RootFolder, sb);
			File.WriteAllText(menuToc, sb.ToString());
		}

		private static void CreateHtmlToc(IDocumentationItem item, StringBuilder sb)
		{
			var folder = item as Folder;
			if (folder != null)
			{
				sb.AppendFormat(@"<li><a href=""{0}/index.html""><strong>{1}</strong></a><ul>", Path.Combine(item.Trail, item.Slug ?? string.Empty).Replace('\\', '/'), item.Title);
				sb.AppendLine();
				foreach (var documentationItem in folder.Items)
				{
					CreateHtmlToc(documentationItem, sb);
				}
				sb.AppendLine("</ul></li>");
				return;
			}

			sb.AppendFormat(@"<li><a href=""{0}"">{1}</a></li>", Path.Combine(item.Trail, item.Slug).Replace('\\', '/').Replace(".markdown", ".html"), item.Title);
			sb.AppendLine();
		}

		private void CompileFolder(Folder folder)
		{
			var fullPath = Path.Combine(_fullPath, folder.Trail, folder.Slug ?? string.Empty);
			if (!File.Exists(Path.Combine(fullPath, DocsListFileName)))
				return;

			var outputPath = Path.Combine(_outputPath, folder.Trail, folder.Slug ?? string.Empty);
			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}

			CompileFile(folder.Title, Path.Combine(fullPath, "index.markdown"), Path.Combine(outputPath, "index.html"));

			// Copy images
			// TODO: Thumbs, lightbox
			var imagesPath = Path.Combine(fullPath, "images");
			if (Directory.Exists(imagesPath))
			{
				if (!Directory.Exists(Path.Combine(outputPath, "images")))
					Directory.CreateDirectory(Path.Combine(outputPath, "images"));

				var images = Directory.GetFiles(imagesPath);
				foreach (var image in images)
				{
					var imageFileName = Path.GetFileName(image);
					if (imageFileName == null) continue;
					File.Copy(image, Path.Combine(outputPath, "images", imageFileName), true);
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
					CompileFile(document.Title, Path.Combine(fullPath, document.Slug), Path.Combine(outputPath, document.Slug.Replace(".markdown", ".html")));
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
