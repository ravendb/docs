using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes
{
    public class IndexHasChanged
    {
        public IndexHasChanged()
        {
            using (var store = new DocumentStore())
            {
                #region index_has_changed
                // Some index definition
                var indexDefinition = new IndexDefinition
                {
                    Name = "UsersByName",
                    Maps = { "from user in docs.Users select new { user.Name }"}
                };
                
                // Define the has-changed operation, pass the index definition
                var indexHasChangedOp = new IndexHasChangedOperation(indexDefinition);
                
                // Execute the operation by passing it to Maintenance.Send
                bool indexHasChanged = store.Maintenance.Send(indexHasChangedOp);
                
                // Return values:
                // false: The definition of the index passed is the SAME as the one deployed on the server  
                // true:  The definition of the index passed is DIFFERENT than the one deployed on the server
                //        Or - index does not exist
                #endregion
            }
        }
        
        public async Task IndexHasChangedAsync()
        {
            using (var store = new DocumentStore())
            {
                #region index_has_changed_async
                // Some index definition
                var indexDefinition = new IndexDefinition
                {
                    Name = "UsersByName",
                    Maps = { "from user in docs.Users select new { user.Name }"}
                };
                
                // Define the has-changed operation, pass the index definition
                var indexHasChangedOp = new IndexHasChangedOperation(indexDefinition);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                bool indexHasChanged = await store.Maintenance.SendAsync(indexHasChangedOp);
                
                // Return values:
                // false: The definition of the index passed is the SAME as the one deployed on the server  
                // true:  The definition of the index passed is DIFFERENT than the one deployed on the server
                //        Or - index does not exist
                #endregion
            }
        }

        private interface IFoo
        {
            /*
            #region syntax
            public IndexHasChangedOperation(IndexDefinition definition)
            #endregion
            */
        }
    }
}
