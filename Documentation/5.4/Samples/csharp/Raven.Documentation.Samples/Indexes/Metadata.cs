using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class Metadata
	{
		#region indexes_1
        public class Products_AllProperties : AbstractIndexCreationTask<Product, Products_AllProperties.Result>
        {
            public class Result
            {
                public string Query { get; set; }
            }

            public Products_AllProperties()
            {
                Map = products => from product in products
                    select new
                    {
                        // convert product to JSON and select all properties from it
                        Query = AsJson(product).Select(x => x.Value)
                    };

                // mark 'Query' field as analyzed which enables full text search operations
                Index(x => x.Query, FieldIndexing.Search);
            }
        }
		#endregion

		#region indexes_3
		public class Products_WithMetadata : AbstractIndexCreationTask<Product>
		{
			public class Result
			{
				public DateTime LastModified { get; set; }
			}

			public Products_WithMetadata()
			{
				Map = products => from product in products
							let metadata = MetadataFor(product)
							select new
							{
								LastModified = metadata.Value<DateTime>("@last-modified")
							};
			}
		}
		#endregion

		public Metadata()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region indexes_2
					IList<Product> results = session
						.Query<Products_AllProperties.Result, Products_AllProperties>()
						.Where(x => x.Query == "Chocolade")
						.OfType<Product>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_4
					IList<Product> results = session
						.Query<Products_WithMetadata.Result, Products_WithMetadata>()
						.OrderByDescending(x => x.LastModified)
						.OfType<Product>()
						.ToList();
					#endregion
				}
			}
		}
	}
}
