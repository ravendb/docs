using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class SetLockMode
    {
        public SetLockMode()
        {
            using (var store = new DocumentStore())
            {
                #region set_lock_single
                // Define the set lock mode operation
                // Pass index name & lock mode
                var setLockModeOp = new SetIndexesLockOperation("Orders/Totals", IndexLockMode.LockedIgnore);

                // Execute the operation by passing it to Maintenance.Send
                // An exception will be thrown if index does not exist
                store.Maintenance.Send(setLockModeOp);
                
                // Lock mode is now set to 'LockedIgnore'
                // Any modifications done now to the index will Not be applied, and will Not throw
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region set_lock_multiple
                // Define the index list and the new lock mode:
                var parameters = new SetIndexesLockOperation.Parameters {
                    IndexNames = new[] {"Orders/Totals", "Orders/ByCompany"}, 
                    Mode = IndexLockMode.LockedError
                };
                    
                // Define the set lock mode operation, pass the parameters
                var setLockModeOp = new SetIndexesLockOperation(parameters);
                
                // Execute the operation by passing it to Maintenance.Send
                // An exception will be thrown if any of the specified indexes do not exist
                store.Maintenance.Send(setLockModeOp);
                
                // Lock mode is now set to 'LockedError' on both indexes
                // Any modifications done now to either index will throw
                #endregion
            }
        }
        
        public async Task SetLockModeAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region set_lock_single_async
                    // Define the set lock mode operation
                    // Pass index name & lock mode
                    var setLockModeOp = new SetIndexesLockOperation("Orders/Totals", IndexLockMode.LockedIgnore);

                    // Execute the operation by passing it to Maintenance.SendAsync
                    // An exception will be thrown if index does not exist
                    await store.Maintenance.SendAsync(setLockModeOp);
                    
                    // Lock mode is now set to 'LockedIgnore'
                    // Any modifications done now to the index will Not be applied, and will Not throw
                    #endregion
                }

                {
                    #region set_lock_multiple_async
                    // Define the index list and the new lock mode:
                    var parameters = new SetIndexesLockOperation.Parameters {
                        IndexNames = new[] {"Orders/Totals", "Orders/ByCompany"}, 
                        Mode = IndexLockMode.LockedError
                    };
                    
                    // Define the set lock mode operation, pass the parameters
                    var setLockModeOp = new SetIndexesLockOperation(parameters);
                
                    // Execute the operation by passing it to Maintenance.SendAsync
                    // An exception will be thrown if any of the specified indexes do not exist
                    await store.Maintenance.SendAsync(setLockModeOp);
                    
                    // Lock mode is now set to 'LockedError' on both indexes
                    // Any modifications done now to either index will throw
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax_1
            // Available overloads:
            public SetIndexesLockOperation(string indexName, IndexLockMode mode);
            public SetIndexesLockOperation(Parameters parameters);
            #endregion
            */
        }

        private class Mode
        {
            #region syntax_2
            public enum IndexLockMode
            {
                Unlock,
                LockedIgnore,
                LockedError
            }
            #endregion
        }

        #region syntax_3
        public class Parameters
        {
            public string[] IndexNames { get; set; }
            public IndexLockMode Mode { get; set; }
        }
        #endregion
    }
}
