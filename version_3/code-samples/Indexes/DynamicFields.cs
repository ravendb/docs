using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes
{
	public class DynamicFields
	{
		#region dynamic_fields_1
		public class Product
		{
			public string Id { get; set; }
			public List<Attribute> Attributes { get; set; }
		}

		public class Attribute
		{
			public string Name { get; set; }
			public string Value { get; set; }
		}
		#endregion

		#region dynamic_fields_2
		public class Product_ByAttribute : AbstractIndexCreationTask<Product>
		{
			public Product_ByAttribute()
			{
				Map = products => from p in products
								  select new
								  {
									  _ = p.Attributes
										 .Select(attribute =>
											 CreateField(attribute.Name, attribute.Value, false, true))
								  };
			}
		}

		#endregion

		public void Dynamic()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region dynamic_fields_3
					var products = session.Advanced.LuceneQuery<Product>("Product/ByAttribute")
						.WhereEquals("Color", "Red")
						.ToList();

					#endregion
				}
			}
		}
	}
}