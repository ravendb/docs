namespace RavenCodeSamples.ClientApi.Advanced.DatabaseCommands
{
	using Raven.Abstractions.Commands;
	using Raven.Json.Linq;

	public class Batch : CodeSampleBase
	{
		public void Sample()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				#region batch_1
				var batchResults =
					documentStore.DatabaseCommands.Batch(
						new ICommandData[]
							 {
								 new PutCommandData
									 {
										 Document = RavenJObject.FromObject(
										 new Company
											 {
												 Name = "Hibernating Rhinos"
											 }), 
											 Etag = null, 
											 Key = "rhino1", 
											 Metadata = new RavenJObject(),
									 },
								 new PutCommandData
									 {
										 Document = RavenJObject.FromObject(
										 new Company
											 {
												 Name = "Hibernating Rhinos"
											 }), 
											 Etag = null, 
											 Key = "rhino2", 
											 Metadata = new RavenJObject(),
									 },
								 new DeleteCommandData
									 {
										 Etag = null, 
										 Key = "rhino2"
									 }
							 });

				#endregion

				#region batch_2
				using (var session = documentStore.OpenAsyncSession())
				{
					var commands = new ICommandData[]
    					{
    						new PutCommandData
    						{
    							Document =
    								RavenJObject.FromObject(new Company {Name = "Hibernating Rhinos"}),
    							Etag = null,
    							Key = "rhino1",
    							Metadata = new RavenJObject(),
    						},
    						new PutCommandData
    						{
    							Document =
    								RavenJObject.FromObject(new Company {Name = "Hibernating Rhinos"}),
    							Etag = null,
    							Key = "rhino2",
    							Metadata = new RavenJObject(),
    						}
    					};

					session.Advanced.Defer(commands);
					session.Advanced.Defer(new DeleteCommandData
					{
						Etag = null,
						Key = "rhino2"
					});
				}

				#endregion
			}
		}
	}
}