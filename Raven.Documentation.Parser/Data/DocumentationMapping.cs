namespace Raven.Documentation.Parser.Data
{
	public class DocumentationMapping
	{
		public DocumentationMapping()
		{
			Language = Language.Csharp;
		}

		public float Version { get; set; }

		public Language Language { get; set; }

		public string Key { get; set; }
	}
}