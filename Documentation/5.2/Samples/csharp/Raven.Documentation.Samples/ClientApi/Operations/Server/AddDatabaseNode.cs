using System.Linq;
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
                // Create AddDatabaseNodeOperation
                // Add a random node to 'Northwind' database-group
                var addDatabaseNodeOperation = new AddDatabaseNodeOperation("Northwind");
                
                // Send operation to the store
                DatabasePutResult result = store.Maintenance.Server.Send(addDatabaseNodeOperation);
                
                // Can access the new topology
                var numberOfReplicas = result.Topology.AllNodes.Count();
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region add_2
                // Create AddDatabaseNodeOperation
                // Add node C to 'Northwind' database-group
                var addDatabaseNodeOperation = new AddDatabaseNodeOperation("Northwind", "C");
                
                // Send operation to the store
                DatabasePutResult result = store.Maintenance.Server.Send(addDatabaseNodeOperation);
                #endregion
            }
        }
    }
}
