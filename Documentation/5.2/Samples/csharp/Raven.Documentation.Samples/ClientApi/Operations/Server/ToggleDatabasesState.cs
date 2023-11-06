using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class ToggleDatabasesState
    {
        public ToggleDatabasesState()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region enable
                    // Define the toggle state operation
                    // specify the database name & pass 'false' to enable
                    var enableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", disable: false);
                    
                    // To enable multiple databases use:
                    // var enableDatabaseOp =
                    //     new ToggleDatabasesStateOperation(new [] { "DB1", "DB2", ... }, disable: false);
                    
                    // Execute the operation by passing it to Maintenance.Send
                    var toggleResult = store.Maintenance.Server.Send(enableDatabaseOp);
                    #endregion
                }
                { 
                    #region disable
                    // Define the toggle state operation
                    // specify the database name(s) & pass 'true' to disable
                    var disableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", disable: true);
                    
                    // To disable multiple databases use:
                    // var disableDatabaseOp =
                    //     new ToggleDatabasesStateOperation(new [] { "DB1", "DB2", ... }, disable: true);
                    
                    // Execute the operation by passing it to Maintenance.Send
                    var toggleResult = store.Maintenance.Server.Send(disableDatabaseOp);
                    #endregion
                }
            }
        }
        
        public async Task ToggleDatabasesStateAsync()
        {
            var store = new DocumentStore();
            
            using (store)
            {
                {
                    #region enable_async
                    // Define the toggle state operation
                    // specify the database name(s) & pass 'false' to enable
                    var enableDatabaseOp = new ToggleDatabasesStateOperation(new [] { "Foo", "Bar" }, disable: false);
                    
                    // Execute the operation by passing it to Maintenance.SendAsync
                    var toggleResult = await store.Maintenance.Server.SendAsync(enableDatabaseOp);
                    #endregion
                }
                { 
                    #region disable_async
                    // Define the toggle state operation
                    // specify the database name(s) & pass 'true' to disable
                    var disableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", disable: true);
                    
                    // Execute the operation by passing it to Maintenance.SendAsync
                    var toggleResult = await store.Maintenance.Server.SendAsync(disableDatabaseOp);
                    #endregion
                }
            }
        }
        
        private class Foo
        {
            public class ToggleDatabasesStateOperation
            {
                /*
                #region syntax_1
                // Available overloads:
                public ToggleDatabasesStateOperation(string databaseName, bool disable)
                public ToggleDatabasesStateOperation(string[] databaseNames, bool disable)
                #endregion
                */
                
                /*
                #region syntax_2
                // Executing the operation returns the following object:
                public class DisableDatabaseToggleResult
                {
                    public bool Disabled; // Is database disabled
                    public string Name;   // Name of the database
                    public bool Success;  // Has request succeeded
                    public string Reason; // Reason for success or failure
                }
                #endregion
                */
            }
        }
    }
}
