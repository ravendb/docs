namespace RavenCodeSamples.Server.ScalingOut.Replication
{
	using Raven.Abstractions.Replication;

	public class FromEmbeddedServer : CodeSampleBase
	{
		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region from_embedded_server_1
				using (var session = store.OpenSession())
				{
					session.Store(new ReplicationDocument
					{
						Destinations =
							{
								new ReplicationDestination
									{
										Url = "https://ravendb-replica"
									},
							}
					});

					session.SaveChanges();
				}

				#endregion
			}
		}
	}
}