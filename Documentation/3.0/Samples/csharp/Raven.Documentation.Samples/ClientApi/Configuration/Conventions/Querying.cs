namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions
{
	using Client.Document;

	public class Querying
	{
		public Querying()
		{
			var store = new DocumentStore();

			DocumentConvention Conventions = store.Conventions;

			#region find_prop_name
			Conventions.FindPropertyNameForIndex = (indexedType, indexName, path, prop) => (path + prop).Replace(",", "_").Replace(".", "_");
			Conventions.FindPropertyNameForDynamicIndex = (indexedType, indexName, path, prop) => path + prop;
			#endregion

			#region querying_consistency
			Conventions.DefaultQueryingConsistency = ConsistencyOptions.None;
			#endregion

			#region querying_consistency_2
			Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
			#endregion

			#region apply_reduce_func
			Conventions.ApplyReduceFunction = (type, resultType, results, transformResults) => 
				// carry out the reduction over results from different shards
			#endregion
				null;

			#region allow_queries_on_id
			Conventions.AllowQueriesOnId = false;
			#endregion
		} 
	}
}