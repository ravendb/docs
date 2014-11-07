namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions
{
	using Abstractions.Replication;
	using Client.Connection;
	using Client.Document;

	public class Replication
	{
		public Replication()
		{
			var store = new DocumentStore();

			DocumentConvention Conventions = store.Conventions;

			#region failover_behavior
			Conventions.FailoverBehavior = FailoverBehavior.AllowReadsFromSecondaries;
			#endregion

			#region replication_informer
			Conventions.ReplicationInformerFactory = (url, jsonRequestFactory) =>
				new ReplicationInformer(Conventions, jsonRequestFactory);
			#endregion

			#region index_and_transformer_replication_mode
			Conventions.IndexAndTransformerReplicationMode = IndexAndTransformerReplicationMode.Indexes |
			                                                 IndexAndTransformerReplicationMode.Transformers;
			#endregion
		} 
	}
}