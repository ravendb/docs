using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Transformers
{
	namespace Raven.Documentation.CodeSamples.Transformers.Foo
	{
		class Foo
		{
			#region transformers_5
			public class Orders_Company : AbstractTransformerCreationTask<Order>
			{
				public Orders_Company()
				{
					TransformResults = results => from result in results select result.Company;
				}
			}

			public static void Main(string[] args)
			{
				using (var store = new DocumentStore
				{
					Url = "http://localhost:8080",
					DefaultDatabase = "Northwind"
				})
				{
					store.Initialize();

					new Orders_Company().Execute(store);

					using (var session = store.OpenSession())
					{
						IList<string> companies = session.Query<Order>()
							.Where(x => x.Freight > 100) // remember to add `Raven.Client.Linq` namespace
							.TransformWith<Orders_Company, string>()
							.ToList();
					}
				}
			}

			#endregion
		}
	}

	public class Creating
	{
		#region transformers_1
		public class Orders_Company : AbstractTransformerCreationTask<Order>
		{
			// ...
		}
		#endregion

		public async Task Sample()
		{
			using (var store = new DocumentStore())
			{
				#region transformers_2
				// deploy transformer to `DefaultDatabase` for given `DocumentStore`
				// using store `Conventions`
				new Orders_Company().Execute(store);

				// deploy asynchronously transformer to `DefaultDatabase` for given `DocumentStore`
				// using store `Conventions`
				await new Orders_Company().ExecuteAsync(store);
				#endregion

				#region transformers_3
				// deploy transformer to `Northwind` database
				// using store `Conventions`
				new Orders_Company().Execute(store.DatabaseCommands.ForDatabase("Northwind"), store.Conventions);
				#endregion

				#region transformers_4
				// deploy all transformers (and indexes) 
				// from assembly where `Orders_Company` is found
				// to `DefaultDatabase` for given `DocumentStore`
				IndexCreation.CreateIndexes(typeof(Orders_Company).Assembly, store);
				#endregion

				#region transformers_6
				store
					.DatabaseCommands
					.PutTransformer("ComboBox/Results", new TransformerDefinition
					{
						TransformResults = "from doc in results select new { doc.Id, doc.Name }"
					});
				#endregion

				#region transformers_7
				TransformerDefinition definition = new Orders_Company().CreateTransformerDefinition();
				store
					.DatabaseCommands
					.PutTransformer("Orders/Company", definition);
				#endregion
			}
		}
	}
}