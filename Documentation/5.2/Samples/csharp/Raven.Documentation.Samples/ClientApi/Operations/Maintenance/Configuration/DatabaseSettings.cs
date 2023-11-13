using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide.Operations.Configuration;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Configuration
{
    public class DatabaseSettings
    {
        public void PutDatabaseSettings()
        {
            var documentStore = new DocumentStore();
            using (documentStore)
            {
                #region put_database_settings
                // 1. Modify the database settings:
                // ================================
                
                // Define the settings dictionary with the key-value pairs to set, for example:
                var settings = new Dictionary<string, string>
                {
                    ["Databases.QueryTimeoutInSec"] = "350", 
                    ["Indexing.Static.DeploymentMode"] = "Rolling"
                };

                // Define the put database settings operation,
                // specify the database name & pass the settings dictionary
                var putDatabaseSettingsOp = new PutDatabaseSettingsOperation(documentStore.Database, settings);
                    
                // Execute the operation by passing it to Maintenance.Send
                documentStore.Maintenance.Send(putDatabaseSettingsOp);
                    
                // 2. RELOAD the database for the change to take effect:
                // =====================================================
                
                // Disable database
                var disableDatabaseOp = new ToggleDatabasesStateOperation(documentStore.Database, true);
                documentStore.Maintenance.Server.Send(disableDatabaseOp);

                // Enable database
                var enableDatabaseOp = new ToggleDatabasesStateOperation(documentStore.Database, false);
                documentStore.Maintenance.Server.Send(enableDatabaseOp); 
                #endregion
            }
        }
        
        public async Task PutDatabaseSettingsAsync()
        {
            var documentStore = new DocumentStore();
            
            using (documentStore)
            {
                #region put_database_settings_async
                // 1. Modify the database settings:
                // ================================
                
                // Define the settings dictionary with the key-value pairs to set, for example:
                var settings = new Dictionary<string, string>
                {
                    ["Databases.QueryTimeoutInSec"] = "350", 
                    ["Indexing.Static.DeploymentMode"] = "Rolling"
                };

                // Define the put database settings operation,
                // specify the database name & pass the settings dictionary
                var putDatabaseSettingsOp = new PutDatabaseSettingsOperation(documentStore.Database, settings);
                    
                // Execute the operation by passing it to Maintenance.SendAsync
                await documentStore.Maintenance.SendAsync(putDatabaseSettingsOp);
                    
                // 2. RELOAD the database for the change to take effect:
                // =====================================================
                
                // Disable database
                var disableDatabaseOp = new ToggleDatabasesStateOperation(documentStore.Database, true);
                await documentStore.Maintenance.Server.SendAsync(disableDatabaseOp);

                // Enable database
                var enableDatabaseOp = new ToggleDatabasesStateOperation(documentStore.Database, false);
                await documentStore.Maintenance.Server.SendAsync(enableDatabaseOp); 
                #endregion
            }
        }
        
        public void GetDatabaseSettings()
        {
            var documentStore = new DocumentStore();
            using (documentStore)
            {
                #region get_database_settings
                // Define the get database settings operation, specify the database name
                var getDatabaseSettingsOp = new GetDatabaseSettingsOperation(documentStore.Database);
                    
                // Execute the operation by passing it to Maintenance.Send
                var customizedSettings = documentStore.Maintenance.Send(getDatabaseSettingsOp);
                
                // Get the customized value
                var customizedValue = customizedSettings.Settings["Databases.QueryTimeoutInSec"];
                #endregion
            }
        }
        
        public async Task GetDatabaseSettingsAsync()
        {
            var documentStore = new DocumentStore();
            
            using (documentStore)
            {
                #region get_database_settings_async
                // Define the get database settings operation, specify the database name
                var getDatabaseSettingsOp = new GetDatabaseSettingsOperation(documentStore.Database);
                    
                // Execute the operation by passing it to Maintenance.SendAsync
                var customizedSettings = await documentStore.Maintenance.SendAsync(getDatabaseSettingsOp);
                
                // Get the customized value
                var customizedValue = customizedSettings.Settings["Databases.QueryTimeoutInSec"];
                #endregion
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax_1
            PutDatabaseSettingsOperation(string databaseName, Dictionary<string, string> configurationSettings)
            #endregion
            */
            
            /*
            #region syntax_2
            GetDatabaseSettingsOperation(string databaseName)
            #endregion
            */
            
            /*
            #region syntax_3
            // Executing the operation returns the following object:
            public class DatabaseSettings
            {
                // Configuration settings that have been customized
                public Dictionary<string, string> Settings { get; set; }
            }
            #endregion
            */
        }
    }
}
