namespace RavenDB.DocsCompiler.Model
{
	public class Document : IDocumentationItem
	{
		public string Title { get; set; }
		public string Slug { get; set; }
		public string Trail { get; set; }
		public string Content { get; set; }
	}
}
