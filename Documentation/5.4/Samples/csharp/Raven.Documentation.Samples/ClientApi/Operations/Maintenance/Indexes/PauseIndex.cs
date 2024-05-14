using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{
    public class PauseIndex
    {
        private interface IFoo
        {
            /*
            #region syntax
            // class name has "Stop", but this is ok, this is the "Pause" operation
            public StopIndexOperation(string indexName)
            #endregion
            */
        }

        public PauseIndex()
        {
            using (var store = new DocumentStore())
            {
                #region pause_index
                // Define the pause index operation, pass the index name 
                var pauseIndexOp = new StopIndexOperation("Orders/Totals");

                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(pauseIndexOp);

                // At this point:
                // Index 'Orders/Totals' is paused on the preferred node
        
                // Can verify the index status on the preferred node by sending GetIndexingStatusOperation
                var indexingStatus = store.Maintenance.Send(new GetIndexingStatusOperation());
                
                var index = indexingStatus.Indexes.FirstOrDefault(x => x.Name == "Orders/Totals");
                Assert.Equal(IndexRunningStatus.Paused, index.Status);
                #endregion
            }
        }
        
        public async Task PauseIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region pause_index_async
                // Define the pause index operation, pass the index name 
                var pauseIndexOp = new StopIndexOperation("Orders/Totals");

                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(pauseIndexOp);

                // At this point:
                // Index 'Orders/Totals' is paused on the preferred node
        
                // Can verify the index status on the preferred node by sending GetIndexingStatusOperation
                var indexingStatus = await store.Maintenance.SendAsync(new GetIndexingStatusOperation());
                
                var index = indexingStatus.Indexes.FirstOrDefault(x => x.Name == "Orders/Totals");
                Assert.Equal(IndexRunningStatus.Paused, index.Status);
                #endregion
            }
        }
    }
}
