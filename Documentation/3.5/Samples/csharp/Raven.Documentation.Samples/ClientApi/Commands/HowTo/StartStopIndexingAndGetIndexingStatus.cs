using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class StartStopIndexingAndGetIndexingStatus
	{
		private interface IFoo
		{
			#region start_indexing_1
			void StartIndexing(int? maxNumberOfParallelIndexTasks = null);
			#endregion

			#region stop_indexing_1
			void StopIndexing();
            #endregion

            #region get_indexing_status_1
            IndexingStatus GetIndexingStatus();
			#endregion
		}

		public StartStopIndexingAndGetIndexingStatus()
		{
			using (var store = new DocumentStore())
			{
				#region start_indexing_2
				store.DatabaseCommands.Admin.StartIndexing();
				#endregion

				#region stop_indexing_2
				store.DatabaseCommands.Admin.StopIndexing();
				#endregion

				#region get_indexing_status_2
				store.DatabaseCommands.Admin.StopIndexing();
				IndexingStatus status = store.DatabaseCommands.Admin.GetIndexingStatus(); // "Paused"
				store.DatabaseCommands.Admin.StartIndexing();
				status = store.DatabaseCommands.Admin.GetIndexingStatus(); // "Indexing"
				#endregion
			}
		}
	}
}