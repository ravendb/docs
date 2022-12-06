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
                // Add random node to 'Northwind' database-group
                DatabasePutResult result = store.Maintenance.Server.Send(new AddDatabaseNodeOperation("Northwind"));
                
                // Can access the new topology
                var numberOfReplicas = result.Topology.AllNodes.Count();

                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region add_2
                // Add node C to 'Northwind' database-group
                DatabasePutResult result = store.Maintenance.Server.Send(new AddDatabaseNodeOperation("Northwind", "C"));
                #endregion
            }
        }
    }
}
