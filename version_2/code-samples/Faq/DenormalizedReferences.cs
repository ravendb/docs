namespace RavenCodeSamples.Faq
{
	#region denormalized_references_1
	public interface INamedDocument
	{
		string Id { get; set; }
		string Name { get; set; }
	}

	public class Customer : INamedDocument
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}

	public class Order : INamedDocument
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public DenormalizedReference<Customer> Customer { get; set; }
	}

	#endregion

	#region denormalized_references_2
	public class DenormalizedReference<T> where T : INamedDocument
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public static implicit operator DenormalizedReference<T>(T doc)
		{
			return new DenormalizedReference<T>
					   {
						   Id = doc.Id,
						   Name = doc.Name
					   };
		}
	}
	#endregion

	public class DenormalizedReferences : CodeSampleBase
	{
		public void Sample()
		{
			#region denormalized_references_3
			Customer customer = GetCurrentCustomer();
			var order = new Order
			{
				Customer = customer // implicit operator called here
			};

			#endregion
		}

		public Customer GetCurrentCustomer()
		{
			return null;
		}
	}
}