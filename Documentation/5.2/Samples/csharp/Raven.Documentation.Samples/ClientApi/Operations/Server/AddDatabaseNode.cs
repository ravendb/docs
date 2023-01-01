using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class AddDatabaseNode
    {
        private interface IFoo
        {
            /*
            #region syntax
            public AddDatabaseNodeOperation(string databaseName, string nodeTag = null)
            #endregion
            */
        }

        public AddDatabaseNode()
        {
            using (var store = new DocumentStore())
            {
                #region add_1
                // Create the AddDatabaseNodeOperation
                // Add a random node to 'Northwind' database-group
                var addDatabaseNodeOp = new AddDatabaseNodeOperation("Northwind");
                
                // Execute the operation by passing it to Maintenance.Server.Send
                DatabasePutResult result = store.Maintenance.Server.Send(addDatabaseNodeOp);
                
                // Can access the new topology
                var numberOfReplicas = result.Topology.AllNodes.Count();
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region add_2
                // Create the AddDatabaseNodeOperation
                // Add node C to 'Northwind' database-group
                var addDatabaseNodeOp = new AddDatabaseNodeOperation("Northwind", "C");
                
                // Execute the operation by passing it to Maintenance.Server.Send
                DatabasePutResult result = store.Maintenance.Server.Send(addDatabaseNodeOp);
                #endregion
            }
        }
        
        public async Task AddDatabaseNodeAsync()
        {
            using (var store = new DocumentStore())
            {
                #region add_1_async
                // Create the AddDatabaseNodeOperation
                // Add a random node to 'Northwind' database-group
                var addDatabaseNodeOp = new AddDatabaseNodeOperation("Northwind");
                
                // Execute the operation by passing it to Maintenance.Server.SendAsync
                DatabasePutResult result = await store.Maintenance.Server.SendAsync(addDatabaseNodeOp);
                
                // Can access the new topology
                var numberOfReplicas = result.Topology.AllNodes.Count();
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region add_2_async
                // Create the AddDatabaseNodeOperation
                // Add node C to 'Northwind' database-group
                var addDatabaseNodeOp = new AddDatabaseNodeOperation("Northwind", "C");
                
                // Execute the operation by passing it to Maintenance.Server.SendAsync
                DatabasePutResult result = await store.Maintenance.Server.SendAsync(addDatabaseNodeOp);
                #endregion
            }
        }
    }
}
