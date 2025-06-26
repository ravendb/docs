using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;

namespace Raven.Documentation.Samples.DataArchival
{
    public class Unarchiving
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                #region unarchiving
                // Define the patch query string
                string patchByQuery = @"
                    // The patch query:
                    // ================
                    from Orders
                    update 
                    {
                        // The patch script:
                        // =================
                        archived.unarchive(this)
                    }";
                
                // Define the patch operation, pass the patch string 
                var patchByQueryOp = new PatchByQueryOperation(patchByQuery);
    
                // Execute the operation by passing it to Operations.Send
                store.Operations.Send(patchByQueryOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region unarchiving_async
                // Define the patch query string
                string patchByQuery = @"
                    from Orders
                    update 
                    {
                        archived.unarchive(this)
                    }";
                
                // Define the patch operation, pass the patch string 
                var patchByQueryOp = new PatchByQueryOperation(patchByQuery);
    
                // Execute the operation by passing it to Operations.Send
                await store.Operations.SendAsync(patchByQueryOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region unarchiving_overload
                // Define the patch query string
                string patchByQuery = @"
                    from Orders
                    update 
                    {
                        archived.unarchive(this)
                    }";
                
                // Define the patch operation, pass the patch string 
                var patchByQueryOp = new PatchByQueryOperation(new IndexQuery()
                {
                    Query = patchByQuery
                });
    
                // Execute the operation by passing it to Operations.Send
                store.Operations.Send(patchByQueryOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region unarchiving_overload_async
                // Define the patch query string
                string patchByQuery = @"
                    from Orders
                    update 
                    {
                        archived.unarchive(this)
                    }";
                
                // Define the patch operation, pass the patch string 
                var patchByQueryOp = new PatchByQueryOperation(new IndexQuery()
                {
                    Query = patchByQuery
                });
    
                // Execute the operation by passing it to Operations.Send
                await store.Operations.SendAsync(patchByQueryOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region unarchive_using_auto_index
                string patchByQuery = @"
                    // This filtering query creates an auto-index:
                    // ===========================================
                    from Orders
                    where ShipTo.Country == 'USA'
                    update 
                    {
                        archived.unarchive(this)
                    }";
                
                var patchByQueryOp = new PatchByQueryOperation(patchByQuery);
                store.Operations.Send(patchByQueryOp);
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region unarchive_using_filter_in_script
                string patchByQuery = @"
                    // Perform a collection query:
                    // ===========================
                    from Orders as order
                    update 
                    { 
                        // Filter documents inside the script:
                        // ===================================
                        if (order.ShipTo.City == 'New York')
                        {
                            archived.unarchive(this)
                        }
                    }";
                
                var patchByQueryOp = new PatchByQueryOperation(patchByQuery);
                store.Operations.Send(patchByQueryOp);
                #endregion
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax
            archived.unarchive(doc)
            #endregion
            */
        }
    }
}

