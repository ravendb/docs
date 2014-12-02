namespace RavenCodeSamples.Faq
{
	using System.Collections.Generic;

	using Raven.Imports.Newtonsoft.Json;

	#region polymorphism_1
	public class Sale
	{
		public Sale()
		{
			Items = new List<SaleItem>();
		}
		public string Id { get; set; }
		public List<SaleItem> Items { get; private set; }
	}

	public abstract class SaleItem
	{
		public decimal Amount { get; set; }
	}

	public class ProductSaleItem : SaleItem
	{
		public string ProductNumber { get; set; }
	}

	public class DiscountSaleItem : SaleItem
	{
		public string DiscountText { get; set; }
	}

	#endregion

	public class Polymorphism : CodeSampleBase
	{
		public void Sample()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region polymorphism_2
				using (var session = documentStore.OpenSession())
				{
					var sale = new Sale();
					sale.Items.Add(new ProductSaleItem { Amount = 1.99m, ProductNumber = "123" });
					sale.Items.Add(new DiscountSaleItem { Amount = -0.10m, DiscountText = "Hanukkah Discount" });
					session.Store(sale);
					session.SaveChanges();
				}

				#endregion

				#region polymorphism_3
				documentStore.Conventions.CustomizeJsonSerializer = serializer => serializer.TypeNameHandling = TypeNameHandling.All;

				#endregion
			}
		}
	}
}