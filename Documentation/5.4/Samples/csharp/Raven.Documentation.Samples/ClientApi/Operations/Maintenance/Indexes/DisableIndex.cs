using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class DisableIndex
    {
        private interface IFoo
        {
            /*
            #region syntax
            // Available overloads:
            public DisableIndexOperation(string indexName)
            public DisableIndexOperation(string indexName, bool clusterWide)
            #endregion
            */
        }

        public DisableIndex()
        {
            using (var store = new DocumentStore())
            {
                #region disable_1
                // Define the disable index operation
                // Use this overload to disable on a single node
                var disableIndexOp = new DisableIndexOperation("Orders/Totals");
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(disableIndexOp);
                
                // At this point, the index is disabled only on the 'preferred node'
                // New data will not be indexed on this node only
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region disable_2
                // Define the disable index operation
                // Pass 'true' to disable the index on all nodes in the database-group
                var disableIndexOp = new DisableIndexOperation("Orders/Totals", true);
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(disableIndexOp);
                
                // At this point, the index is disabled on ALL nodes
                // New data will not be indexed
                #endregion
            }
        }
        
        public async Task DisableIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region disable_1_async
                // Define the disable index operation
                // Use this overload to disable on a single node
                var disableIndexOp = new DisableIndexOperation("Orders/Totals");
                
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(disableIndexOp);
                
                // At this point, the index is disabled only on the 'preferred node'
                // New data will not be indexed on this node only
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region disable_2_async
                // Define the disable index operation
                // Pass 'true' to disable the index on all nodes in the database-group
                var disableIndexOp = new DisableIndexOperation("Orders/Totals", true);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(disableIndexOp);
                
                // At this point, the index is disabled on ALL nodes
                // New data will not be indexed
                #endregion
            }
        }
    }
}
