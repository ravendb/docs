using System;
using RavenDB.DocsCompiler.Model;

namespace RavenDB.DocsCompiler.Output
{
	public interface IDocsOutput : IDisposable
	{
		void SaveDocItem(Document doc);

		void SaveImage(Folder ofFolder, string fullFilePath);

		void GenerateToc(IDocumentationItem rootItem);
	}
}
