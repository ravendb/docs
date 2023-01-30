using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class DeleteIndexErrors
    {
        public DeleteIndexErrors()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region delete_errors_all
                    // Define the delete index errors operation
                    var deleteIndexErrorsOp = new DeleteIndexErrorsOperation();
                    
                    // Execute the operation by passing it to Maintenance.Send
                    store.Maintenance.Send(deleteIndexErrorsOp);
                    
                    // All errors from ALL indexes are deleted
                    #endregion
                }

                {
                    #region delete_errors_specific
                    // Define the delete index errors operation from specific indexes
                    var deleteIndexErrorsOp = new DeleteIndexErrorsOperation(new[] { "Orders/Totals" });
                    
                    // Execute the operation by passing it to Maintenance.Send
                    // An exception will be thrown if any of the specified indexes do not exist
                    store.Maintenance.Send(deleteIndexErrorsOp);

                    // Only errors from index "Orders/Totals" are deleted
                    #endregion
                }
            }
        }
        
        public async Task DeleteIndexErrorsAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region delete_errors_all_async
                    // Define the delete index errors operation
                    var deleteIndexErrorsOp = new DeleteIndexErrorsOperation();

                    // Execute the operation by passing it to Maintenance.SendAsync
                    await store.Maintenance.SendAsync(deleteIndexErrorsOp);

                    // All errors from ALL indexes are deleted
                    #endregion
                }

                {
                    #region delete_errors_specific_async
                    // Define the delete index errors operation from specific indexes
                    var deleteIndexErrorsOp = new DeleteIndexErrorsOperation(new[] { "Orders/Totals" });
                    
                    // Execute the operation by passing it to Maintenance.SendAsync
                    // An exception will be thrown if any of the specified indexes do not exist
                    await store.Maintenance.SendAsync(deleteIndexErrorsOp);
                    
                    // Only errors from index "Orders/Totals" are deleted
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax
            // Available overloads:
            public DeleteIndexErrorsOperation()                    // Delete errors from all indexes
            public DeleteIndexErrorsOperation(string[] indexNames) // Delete errors from specific indexes
            #endregion
            */
        }
    }
}
