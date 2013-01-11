namespace RavenCodeSamples.Server.ScalingOut.Replication
{
	using Raven.Client.Document;
	using Raven.Json.Linq;

	public class ClientIntegration : CodeSampleBase
	{
		public void Sample()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region client_integration_1
				documentStore.Conventions.FailoverBehavior = FailoverBehavior.FailImmediately;

				#endregion

				#region client_integration_2

				documentStore.DatabaseCommands.Put("Raven/ServerPrefixForHilo", null,
				                                   new RavenJObject
					                                {
						                                {"ServerPrefix", "NorthServer/"}
					                                },
				                                   new RavenJObject());
				#endregion
			}
		}
	}
}