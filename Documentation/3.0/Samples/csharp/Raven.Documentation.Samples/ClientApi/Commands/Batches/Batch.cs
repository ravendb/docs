using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Batches
{
	using System.Collections.Generic;

	using Raven.Abstractions.Commands;
	using Raven.Abstractions.Data;
	using Raven.Client.Document;
	using Raven.Json.Linq;

	public class Batch
	{
		private interface IFoo
		{
			#region batch_1
			BatchResult[] Batch(IEnumerable<ICommandData> commandDatas);
			#endregion
		}

		public Batch()
		{
			using (var store = new DocumentStore())
			{
				#region batch_2
				var results = store
					.DatabaseCommands
					.Batch(new ICommandData[]
						       {
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
											Key = "products/2"
									   }
						       });
				#endregion
			}
		}
	}
}