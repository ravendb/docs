using System.Collections.Generic;

namespace RavenDBSamples.BaseForSamples
{
	public class Order
	{
		public List<Item> Items { get; set; }
		public string UserId { get; set; }
		public string Title { get; set; }
	}

	public class Item
	{
		public string Id { get; set; }
	}

	public class Product
	{
		public string Name { get; set; }
	}
}