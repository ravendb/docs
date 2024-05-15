using System.IO;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using static Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Indexes.DisableIndex;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class ToggleDatabasesState
    {
        public ToggleDatabasesState()
        {
            using (var documentStore = new DocumentStore())
            {
                {
                    #region enable
                    // Define the toggle state operation
                    // specify the database name & pass 'false' to enable
                    var enableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", disable: false);
                    
                    // To enable multiple databases use:
                    // var enableDatabaseOp =
                    //     new ToggleDatabasesStateOperation(new [] { "DB1", "DB2", ... }, disable: false);
                    
                    // Execute the operation by passing it to Maintenance.Server.Send
                    var toggleResult = documentStore.Maintenance.Server.Send(enableDatabaseOp);
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
                    
                    // Execute the operation by passing it to Maintenance.Server.Send
                    var toggleResult = documentStore.Maintenance.Server.Send(disableDatabaseOp);
                    #endregion
                }
            }
        }
        
        public async Task ToggleDatabasesStateAsync()
        {
            using (var documentStore = new DocumentStore())
            {
                {
                    #region enable_async
                    // Define the toggle state operation
                    // specify the database name(s) & pass 'false' to enable
                    var enableDatabaseOp = new ToggleDatabasesStateOperation(new [] { "Foo", "Bar" }, disable: false);
                    
                    // Execute the operation by passing it to Maintenance.Server.SendAsync
                    var toggleResult = await documentStore.Maintenance.Server.SendAsync(enableDatabaseOp);
                    #endregion
                }
                { 
                    #region disable_async
                    // Define the toggle state operation
                    // specify the database name(s) & pass 'true' to disable
                    var disableDatabaseOp = new ToggleDatabasesStateOperation("Northwind", disable: true);
                    
                    // Execute the operation by passing it to Maintenance.Server.SendAsync
                    var toggleResult = await documentStore.Maintenance.Server.SendAsync(disableDatabaseOp);
                    #endregion
                }
            }
        }


        void DisableDatabaseViaFileSystem()
        {
            using (var store = new DocumentStore())
            {
                string databasePath = new string("dbPath");
                #region disable-database-via-file-system
                // Prevent database access by creating disable.marker in its path
                var disableMarkerPath = Path.Combine(databasePath, "disable.marker");
                File.Create(disableMarkerPath).Dispose();
                #endregion
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
