using System;
using System.IO;
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
        public ClientType ClientType { get; set; }
        public string RootUrl { get; set; }
        public string ImagesPath { get; set; }
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
        public ClientType ClientType { get; set; }
	    public string RootUrl { get; set; }
		public string ImagesPath { get; set; }

	    public bool IsHtmlOutput
	    {
            get { return ContentType == OutputType.Html; }
	    }

		public void SaveDocItem(Document doc)
		{
			var outputPath = Path.Combine(OutputPath, doc.Trail);
			if (!Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(outputPath);
			}
            
            if (IsHtmlOutput)
            {
                var contents = string.Format(PageTemplate, doc.Title, doc.Content);
                File.WriteAllText(Path.Combine(outputPath, doc.Slug + ".html"), contents);
            }			
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
				sb.AppendFormat(@"<li><a href=""{0}/{1}/index.html""><strong>{2}</strong></a><ul>", ClientType.ToString().ToLowerInvariant(), Path.Combine(item.Trail, item.Slug ?? string.Empty).Replace('\\', '/'), item.Title);
				sb.AppendLine();
				foreach (var documentationItem in folder.Items)
				{
					CreateHtmlToc(documentationItem, sb);
				}
				sb.AppendLine("</ul></li>");
				return;
			}

            sb.AppendFormat(@"<li><a href=""{0}/{1}"">{2}</a></li>", ClientType.ToString().ToLowerInvariant(), Path.Combine(item.Trail, item.Slug).Replace('\\', '/').Replace(".markdown", ".html"), item.Title);
			sb.AppendLine();
		}

		public void Dispose()
		{
			// Nothing to do
		}
	}
}