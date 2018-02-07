using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class ToggleDatabasesState
    {
        private class Foo
        {
            public class ToggleDatabasesStateOperation
            {
                /*
                #region toggle_1
                public ToggleDatabasesStateOperation(string databaseName, bool disable)
                #endregion

                #region toggle_2
                public ToggleDatabasesStateOperation(string[] databaseNames, bool disable) 
                #endregion
                */
            }

        }

        public ToggleDatabasesState()
        {
            using (var store = new DocumentStore())
            {
                { 
                    #region toggle_1_0
                    // disable Northwind database
                    ToggleDatabasesStateOperation toggleOperation = 
                        new ToggleDatabasesStateOperation("Northwind", 
                            disable: true);
                    store.Maintenance.Server.Send(toggleOperation);
                    #endregion
                }

                {
                    #region toggle_2_0
                    // enable Foo and Bar databases
                    ToggleDatabasesStateOperation toggleOperation = 
                        new ToggleDatabasesStateOperation(new [] { "Foo", "Bar" }, 
                            disable: false);
                    store.Maintenance.Server.Send(toggleOperation);
                    #endregion
                }
            }
        }
    }
}
