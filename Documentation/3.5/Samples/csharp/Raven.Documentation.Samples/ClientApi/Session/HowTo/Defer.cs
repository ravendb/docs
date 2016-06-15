using Raven.Abstractions.Commands;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
	public class Defer
	{
		private interface IFoo
		{
			#region defer_1
			void Defer(params ICommandData[] commands);
			#endregion
		}

		public Defer()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region defer_2
					session
						.Advanced
						.Defer(
							new PutCommandData
								{
									Key = "products/999",
									Document = RavenJObject.FromObject(new Product
																		   {
																			   Name = "My Product", 
																			   Supplier = "suppliers/999"
																		   }),
									Metadata = new RavenJObject()
								},
							new PutCommandData
								{
									Key = "suppliers/999",
									Document = RavenJObject.FromObject(new Supplier
																		   {
																			   Name = "My Supplier",
																		   }),
									Metadata = new RavenJObject()
								},
							new DeleteCommandData
								{
									Key = "products/1"
								});
					#endregion
				}
			}
		}
	}
}