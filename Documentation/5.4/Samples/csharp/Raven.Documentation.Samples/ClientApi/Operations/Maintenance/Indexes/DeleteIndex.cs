using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class DeleteIndex
    {
        public DeleteIndex()
        {
            using (var store = new DocumentStore())
            {
                #region delete_index
                // Define the delete index operation, specify the index name
                var deleteIndexOp = new DeleteIndexOperation("Orders/Totals");
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(deleteIndexOp);
                #endregion
            }
        }
        
        public async Task DeleteIndexAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region delete_index_async
                    // Define the delete index errors operation, specify the index name
                    var deleteIndexOp = new DeleteIndexOperation("Orders/Totals");
                
                    // Execute the operation by passing it to Maintenance.SendAsync
                    await store.Maintenance.SendAsync(deleteIndexOp);
                    #endregion
                }
            }
        }

        private interface IFoo
        {
            /*
            #region syntax
            public DeleteIndexOperation(string indexName)
            #endregion
            */
        }
    }
}
