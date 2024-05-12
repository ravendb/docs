using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class PauseIndexing
    {
        private interface IFoo
        {
            /*
            #region syntax
            // class name has "Stop", but this is ok, this is the "Pause" operation
            public StopIndexingOperation()
            #endregion
            */
        }

        public PauseIndexing()
        {
            using (var store = new DocumentStore())
            {
                #region pause_indexing
                // Define the pause indexing operation 
                var pauseIndexingOp = new StopIndexingOperation();

                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(pauseIndexingOp);

                // At this point:
                // All indexes in the default database will be 'paused' on the preferred node
        
                // Can verify indexing status on the preferred node by sending GetIndexingStatusOperation
                var indexingStatus = store.Maintenance.Send(new GetIndexingStatusOperation());
                Assert.Equal(IndexRunningStatus.Paused, indexingStatus.Status);
                #endregion
            }
        }
        
        public async Task PauseIndexingAsync()
        {
            using (var store = new DocumentStore())
            {
                #region pause_indexing_async
                // Define the pause indexing operation 
                var pauseIndexingOp = new StopIndexingOperation();

                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(pauseIndexingOp);

                // At this point:
                // All indexes in the default database will be 'paused' on the preferred node
        
                // Can verify indexing status on the preferred node by sending GetIndexingStatusOperation
                var indexingStatus = await store.Maintenance.SendAsync(new GetIndexingStatusOperation());
                Assert.Equal(IndexRunningStatus.Paused, indexingStatus.Status);
                #endregion
            }
        }
    }
}
