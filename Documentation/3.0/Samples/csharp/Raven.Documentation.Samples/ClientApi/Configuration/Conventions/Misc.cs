namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions
{
	using Abstractions.Replication;
	using Client.Connection;
	using Client.Document;

	public class Misc
	{
		public Misc()
		{
			var store = new DocumentStore();

			DocumentConvention Conventions = store.Conventions;

			#region disable_profiling
			Conventions.DisableProfiling = true;
			#endregion


			#region enlist_in_dist_tx
			Conventions.EnlistInDistributedTransactions = true;
			#endregion

			#region failover_behavior
			Conventions.FailoverBehavior = FailoverBehavior.AllowReadsFromSecondaries;
			#endregion

			#region replication_informer
			Conventions.ReplicationInformerFactory = (url, jsonRequestFactory) => 
				new ReplicationInformer(Conventions, jsonRequestFactory);
			#endregion

			#region max_number_of_requests_per_session
			Conventions.MaxNumberOfRequestsPerSession = 30;
			#endregion

			#region save_enums_as_integers
			Conventions.SaveEnumsAsIntegers = false;
			#endregion

			#region use_optimistic_concurrency_by_default
			Conventions.DefaultUseOptimisticConcurrency = false;
			#endregion
		} 
	}
}