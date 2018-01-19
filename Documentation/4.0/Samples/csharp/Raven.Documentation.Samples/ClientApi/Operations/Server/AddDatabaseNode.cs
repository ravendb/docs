using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{

    public class AddDatabaseNode
    {
        private interface IFoo
        {
            /*
            #region add_1
            public AddDatabaseNodeOperation(string databaseName, string node = null)
            #endregion
            */
        }

        public AddDatabaseNode()
        {

            using (var store = new DocumentStore())
            {
                #region add_2
                // add random node to 'Northwind' database group
                store.Maintenance.Server.Send(new AddDatabaseNodeOperation("Northwind"));
                #endregion

                #region add_3
                // add node C to 'Northwind' database group
                store.Maintenance.Server.Send(new AddDatabaseNodeOperation("Northwind", "C"));
                #endregion
            }

        }
    }
}
