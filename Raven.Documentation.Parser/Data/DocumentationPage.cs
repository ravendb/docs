namespace Raven.Documentation.Parser.Data
{
	using System.Collections.Generic;

	public class DocumentationPage
    {
		public DocumentationPage()
		{
			Images = new List<DocumentationImage>();
		}

        public string Version { get; set; }

	    public string HtmlContent { get; set; }

	    public string TextContent { get; set; }

	    public string Title { get; set; }

		public string Key { get; set; }

	    public string Id { get; set; }

		public Language Language { get; set; }

	    public Category Category { get; set; }

		public List<DocumentationImage> Images { get; set; }
    }
}