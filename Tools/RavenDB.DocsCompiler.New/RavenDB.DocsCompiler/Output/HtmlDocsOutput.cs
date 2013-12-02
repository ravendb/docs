using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RavenDB.DocsCompiler.Model;

namespace RavenDB.DocsCompiler.Output
{
	public class MarkdownDocsOutput : IDocsOutput
	{
		public void Dispose()
		{
		}
		public string OutputPath { get; set; }

		public OutputType ContentType { get; set; }
		public string RootUrl { get; set; }
		public string ImagesPath { get; set; }
	    public CompilationMode CompilationMode { get; set; }

	    public void SaveDocItem(Document doc)
		{
			var outputPath = Path.Combine(OutputPath, doc.Trail);
			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}

			var contents = doc.Content;
			File.WriteAllText(Path.Combine(outputPath, doc.Slug + ".markdown"), contents);
		}

		public void SaveImage(Folder ofFolder, string fullFilePath)
		{
			var outputPath = Path.Combine(OutputPath, ofFolder.Trail, ofFolder.Slug, "images");
			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}

			var destFileName = Path.Combine(outputPath, Path.GetFileName(fullFilePath));

			//Console.WriteLine("Copying {0} to {1}", fullFilePath, destFileName);

			File.Copy(fullFilePath, destFileName, true);
		}

		public void GenerateTableOfContents(IDocumentationItem rootItem)
		{
		}
	}
	public class HtmlDocsOutput : IDocsOutput
	{
		public string OutputPath { get; set; }

		public string PageTemplate { get; set; }

		public OutputType ContentType { get; set; }
		public string RootUrl { get; set; }
		public string ImagesPath { get; set; }

		public bool IsHtmlOutput
		{
			get { return ContentType == OutputType.Html; }
		}

	    public CompilationMode CompilationMode { get; set; }

	    public void SaveDocItem(Document doc)
		{
			var outputPath = Path.Combine(OutputPath, doc.VirtualTrail);
			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}

			if (IsHtmlOutput)
			{
				var contents = string.Format(PageTemplate, doc.Title, doc.Content, GenerateMenu(doc), GenerateBreadcrumbs(doc));
				File.WriteAllText(Path.Combine(outputPath, doc.Slug + ".html"), contents);
			}
		}

		private string GenerateMenu(Document doc)
		{
			var root = doc.Parent;
			while (root != null)
			{
				if (root.Parent == null)
					break;

				root = root.Parent;
			}

			if (root == null)
				return string.Empty;

			return root.Children.Aggregate(string.Empty, (current, child) => current + GenerateMenuItem(child, doc));
		}

		private string GenerateMenuItem(IDocumentationItem item, Document current)
		{
			var isOpen = IsMenuItemOpen(item, current);

			var builder = new StringBuilder();
			builder.AppendFormat("<li class='{1}'>{0}", GenerateMenuItemTitle(item, current), isOpen ? "open" : string.Empty);
			foreach (var g in item.Children.GroupBy(x => new
			                                                 {
			                                                     x.Trail,
                                                                 x.Slug
			                                                 }))
			{
			    var child = g.FirstOrDefault(x => x.Language == current.Language) ?? g.First();

			    builder.Append("<ul>");
				builder.Append(GenerateMenuItem(child, current));
				builder.Append("</ul>");
			}

			builder.Append("</li>");

			return builder.ToString();
		}

		private string GenerateMenuItemTitle(IDocumentationItem item, Document current)
		{
			var document = item as Document;
			if (document != null)
			{
				return string.Format("<a href='{0}'>{1}</a>", GenerateUrlForDocument(document, current), item.Title);
			}

			var folder = item as Folder;
			if (folder != null)
			{
				return string.Format("<span>{0}</span>", item.Title);
			}

			return string.Empty;
		}

		private string GenerateUrlForDocument(Document document, Document current)
		{
			var strippedSlug = document.Slug.Replace(".markdown", string.Empty);

			var url = "/"
						+ document.VirtualTrail
						.Replace("\\", "/")
						+ "/" + strippedSlug + ".html";

		    if (document.Language != current.Language)
		    {
		        url = url.Replace(document.Language.ToString(), "client_type");
		    }

		    return new Uri(RootUrl + url).AbsoluteUri.Replace("//", "/");
		}

		private static bool IsMenuItemOpen(IDocumentationItem item, Document current)
		{
			var isOpen = item == current;
			if (isOpen)
				return true;

			foreach (var child in item.Children)
			{
				isOpen = IsMenuItemOpen(child, current);

				if (isOpen)
					return true;
			}

			return false;
		}

		private static string GenerateBreadcrumbs(IDocumentationItem doc)
		{
			var ancestors = new List<IDocumentationItem>();

			var parent = doc.Parent;
			while (parent != null)
			{
				ancestors.Add(parent);

				parent = parent.Parent;
			}

			ancestors.Reverse();
			ancestors.Add(doc);

			var builder = new StringBuilder();

			foreach (var breadcrumb in ancestors.Skip(1))
			{
				builder.AppendFormat("<li>{0}</li>\n", breadcrumb.Title);
			}

			return builder.ToString();
		}

		public void SaveImage(Folder ofFolder, string fullFilePath)
		{
			var outputPath = Path.Combine(OutputPath, ofFolder.Trail, ofFolder.Slug, "images");
			if (!Directory.Exists(outputPath))
				Directory.CreateDirectory(outputPath);

			File.Copy(fullFilePath, Path.Combine(outputPath, Path.GetFileName(fullFilePath)), true);
		}

		public void GenerateTableOfContents(IDocumentationItem rootItem)
		{
			var menuToc = Path.Combine(OutputPath, "toc.html");
			var sb = new StringBuilder();
			CreateHtmlToc(rootItem, sb);
			File.WriteAllText(menuToc, sb.ToString());
		}

		private void CreateHtmlToc(IDocumentationItem item, StringBuilder sb)
		{
			var folder = item as Folder;
			if (folder != null)
			{
				sb.AppendFormat(@"<li><a href=""{0}/{1}/index.html""><strong>{2}</strong></a><ul>", item.Language, Path.Combine(item.Trail, item.Slug ?? string.Empty).Replace('\\', '/'), item.Title);
				sb.AppendLine();
				foreach (var documentationItem in folder.Children)
				{
					CreateHtmlToc(documentationItem, sb);
				}
				sb.AppendLine("</ul></li>");
				return;
			}

            sb.AppendFormat(@"<li><a href=""{0}/{1}"">{2}</a></li>", item.Language, Path.Combine(item.Trail, item.Slug).Replace('\\', '/').Replace(".markdown", ".html"), item.Title);
			sb.AppendLine();
		}

		public void Dispose()
		{
			// Nothing to do
		}
	}
}