using System;
using RavenDB.DocsCompiler.Model;

namespace RavenDB.DocsCompiler.Output
{
	public interface IDocsOutput : IDisposable
	{
        string ContentType { get; set; }

		string RootUrl { get; set; }

		string ImagesPath { get; set; }

		void SaveDocItem(Document doc);

		void SaveImage(Folder ofFolder, string fullFilePath);

		void GenerateToc(IDocumentationItem rootItem);
	}
}
