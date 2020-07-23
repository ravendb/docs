namespace Raven.Documentation.Parser.Data
{
	using System.Collections.Generic;

	public class TableOfContents
	{
		public TableOfContents()
		{
			this.Items = new List<TableOfContentsItem>();
		}

		public string Version { get; set; }

		public Category Category { get; set; }
        
        public int? Position { get; set; }

		public List<TableOfContentsItem> Items { get; set; }

		public class TableOfContentsItem
		{
			public TableOfContentsItem()
			{
				this.Items = new List<TableOfContentsItem>();
				this.Languages = new List<Language>();
			}

			public string Key { get; set; }

			public string Title { get; set; }

            public string SourceVersion { get; set; }

			public bool IsFolder { get; set; }

			public List<TableOfContentsItem> Items { get; set; }

			public List<Language> Languages { get; set; }
		}
	}
}
