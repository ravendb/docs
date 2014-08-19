using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.Transformers
{
	public class Metadata
	{
		#region transformers_1
		public class Products_AsDocument : AbstractTransformerCreationTask<Product>
		{
			public class Result
			{
				public RavenJObject RawDocument { get; set; }
			}

			public Products_AsDocument()
			{
				TransformResults = products => from product in products
								select new
								{
									RawDocument = AsDocument(product)
								};
			}
		}
		#endregion

		#region transformers_3
		public class Products_WithMetadata : AbstractTransformerCreationTask<Product>
		{
			public class Result
			{
				public Product Product { get; set; }

				public string EntityName { get; set; }

				public string ClrType { get; set; }
			}

			public Products_WithMetadata()
			{
				TransformResults = products => from product in products
								let metadata = MetadataFor(product)
								select new
								{
									Product = product,
									EntityName = metadata.Value<string>("Raven-Entity-Name"),
									ClrType = metadata.Value<string>("Raven-Clr-Type")
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
					#region transformers_2
					IList<Products_AsDocument.Result> results = session
						.Query<Product>()
						.TransformWith<Products_AsDocument, Products_AsDocument.Result>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region transformers_4
					IList<Products_WithMetadata.Result> results = session
						.Query<Product>()
						.TransformWith<Products_WithMetadata, Products_WithMetadata.Result>()
						.ToList();
					#endregion
				}
			}
		}
	}
}