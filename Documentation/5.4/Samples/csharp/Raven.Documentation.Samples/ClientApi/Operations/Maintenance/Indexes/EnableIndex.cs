using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class EnableIndex
    {
        private interface IFoo
        {
            /*
            #region syntax
            // Available overloads:
            public EnableIndexOperation(string indexName)
            public EnableIndexOperation(string indexName, bool clusterWide)
            #endregion
            */
        }

        public EnableIndex() 
        {
            using (var store = new DocumentStore())
            {
                #region enable_1
                // Define the enable index operation
                // Use this overload to enable on a single node
                var enableIndexOp = new EnableIndexOperation("Orders/Totals");

                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(enableIndexOp);

                // At this point, the index is enabled on the 'preferred node'
                // New data will be indexed on this node
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region enable_2
                // Define the enable index operation
                // Pass 'true' to enable the index on all nodes in the database-group
                var enableIndexOp = new EnableIndexOperation("Orders/Totals", true);

                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(enableIndexOp);

                // At this point, the index is enabled on ALL nodes
                // New data will be indexed
                #endregion
            }
        }
        
        public async Task EnableIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region enable_1_async
                // Define the enable index operation
                // Use this overload to enable on a single node
                var enableIndexOp = new EnableIndexOperation("Orders/Totals");

                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(enableIndexOp);

                // At this point, the index is enabled on the 'preferred node'
                // New data will be indexed on this node
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region enable_2_async
                // Define the enable index operation
                // Pass 'true' to enable the index on all nodes in the database-group
                var enableIndexOp = new EnableIndexOperation("Orders/Totals", true);

                // Execute the operation by passing it to Maintenance.SendAsync
                await store.Maintenance.SendAsync(enableIndexOp);

                // At this point, the index is enabled on ALL nodes
                // New data will be indexed
                #endregion
            }
        }
    }
}
