namespace RavenCodeSamples.Server.ScalingOut.Replication
{
	using Raven.Client.Document;

	public class ClientIntegration : CodeSampleBase
	{
		public void Sample()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region client_integration_1
				documentStore.Conventions.FailoverBehavior = FailoverBehavior.FailImmediately;

				#endregion
			}
		}
	}
}