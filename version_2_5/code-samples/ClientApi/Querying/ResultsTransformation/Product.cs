namespace RavenCodeSamples.ClientApi.Querying.ResultsTransformation
{
	#region product_class
	public class Product
	{
		public string Id { get; set; }
		public string ArticleNumber { get; set; }
		public string Name { get; set; }
		public string Manufacturer { get; set; }
		public string Description { get; set; }
		public int QuantityInWarehouse { get; set; }
	}
	#endregion
}