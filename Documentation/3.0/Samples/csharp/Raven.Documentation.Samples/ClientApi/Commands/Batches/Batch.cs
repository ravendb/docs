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
									       Key = "people/1",
										   Document = RavenJObject.FromObject(new Person
											{
												FirstName = "John",
												LastName = "Doe",
												AddressId = "addresses/1"
											}),
											Metadata = new RavenJObject()
								       }, 
								   new PutCommandData
									   {
											Key = "addresses/1",
											Document = RavenJObject.FromObject(new Address
											{
												Street = "Crystal Oak Street",
											}),
											Metadata = new RavenJObject()
									   }, 
								   new DeleteCommandData
									   {
											Key = "people/2"
									   }
						       });
				#endregion
			}
		}
	}
}