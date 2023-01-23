using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class GetIndexNames
    {
        public GetIndexNames()
        {
            using (var store = new DocumentStore()) 
            {
                #region get_index_names
                // Define the get index names operation
                // Pass number of indexes to skip & number of indexes to retrieve
                var getIndexNamesOp = new GetIndexNamesOperation(0, 10);
                
                // Execute the operation by passing it to Maintenance.Send
                string[] indexNames = store.Maintenance.Send(getIndexNamesOp);
                #endregion
            }
        }
        
        public async Task GetIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                #region get_index_names_async
                // Define the get index names operation
                // Pass number of indexes to skip & number of indexes to retrieve
                var getIndexNamesOp = new GetIndexNamesOperation(0, 10);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                string[] indexNames = await store.Maintenance.SendAsync(getIndexNamesOp);
                #endregion
            }
        }

        private interface IFoo
        {
            /*
            #region get_index_names_syntax
            public GetIndexNamesOperation(int start, int pageSize)
            #endregion
            */
        }
    }
}
