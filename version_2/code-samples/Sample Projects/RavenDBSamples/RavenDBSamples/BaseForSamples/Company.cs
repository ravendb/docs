namespace RavenDBSamples.BaseForSamples
{
	public class Company
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public RestaurantType Type { get; set; }
	}

	public enum RestaurantType
	{
		Italian,
		Vegetarian,
		Sushi
	}
}
