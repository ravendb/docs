namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions
{
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

			#region max_number_of_requests_per_session
			Conventions.MaxNumberOfRequestsPerSession = 30;
			#endregion

			#region save_enums_as_integers
			Conventions.SaveEnumsAsIntegers = false;
			#endregion

			#region use_optimistic_concurrency_by_default
			Conventions.DefaultUseOptimisticConcurrency = false;
			#endregion

			#region prettify_generated_linq_expressions
			Conventions.PrettifyGeneratedLinqExpressions = true;
			#endregion
		} 
	}
}