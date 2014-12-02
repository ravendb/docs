namespace Raven.Documentation.Parser.Data
{
	using System.Collections.Generic;

	public class TableOfContents
	{
		public TableOfContents()
		{
			Items = new List<TableOfContentsItem>();
		}

		public string Version { get; set; }

		public Category Category { get; set; }

		public List<TableOfContentsItem> Items { get; set; }

		public class TableOfContentsItem
		{
			public TableOfContentsItem()
			{
				Items = new List<TableOfContentsItem>();
			}

			public string Key { get; set; }

			public string Title { get; set; }

			public bool IsFolder { get; set; }

			public List<TableOfContentsItem> Items { get; set; }
		}
	}
}