using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class GetIndex
    {
        public GetIndex()
        {
            using (var store = new DocumentStore())
            {
                #region get_index

                // Define the get index operation, pass the index name
                var getIndexOp = new GetIndexOperation("Orders/Totals");

                // Execute the operation by passing it to Maintenance.Send
                IndexDefinition index = store.Maintenance.Send(getIndexOp);

                // Access the index definition
                var state = index.State;
                var lockMode = index.LockMode;
                var deploymentMode = index.DeploymentMode;
                // etc.

                #endregion
            }
            
            using (var store = new DocumentStore()) 
            {
                #region get_indexes
                // Define the get indexes operation
                // Pass number of indexes to skip & number of indexes to retrieve
                var getIndexesOp = new GetIndexesOperation(0, 10);
                
                // Execute the operation by passing it to Maintenance.Send
                IndexDefinition[] indexes = store.Maintenance.Send(getIndexesOp);
                
                // Access an index definition
                var name = indexes[0].Name;
                var state = indexes[0].State;
                var lockMode = indexes[0].LockMode;
                var deploymentMode = indexes[0].DeploymentMode;
                // etc.
                #endregion
            }
        }
        
        public async Task GetIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region get_index_async
                // Define the get index operation, pass the index name
                var getIndexOp = new GetIndexOperation("Orders/Totals");
                
                // Execute the operation by passing it to Maintenance.SendAsync
                IndexDefinition index = await store.Maintenance.SendAsync(getIndexOp);

                // Access the index definition
                var state = index.State;
                var lockMode = index.LockMode;
                var deploymentMode = index.DeploymentMode;
                // etc.
                #endregion
            }
            
            using (var store = new DocumentStore()) 
            {
                #region get_indexes_async
                // Define the get indexes operation
                // Pass number of indexes to skip & number of indexes to retrieve
                var getIndexesOp = new GetIndexesOperation(0, 10);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                IndexDefinition[] indexes = await store.Maintenance.SendAsync(getIndexesOp);
                
                // Access an index definition
                var name = indexes[0].Name;
                var state = indexes[0].State;
                var lockMode = indexes[0].LockMode;
                var deploymentMode = indexes[0].DeploymentMode;
                // etc.
                #endregion
            }
        }

        private interface IFoo
        {
            /*
            #region get_index_syntax
            public GetIndexOperation(string indexName)
            #endregion
            */

            /*
            #region get_indexes_syntax
            public GetIndexesOperation(int start, int pageSize)
            #endregion
            */
        }
    }
}
