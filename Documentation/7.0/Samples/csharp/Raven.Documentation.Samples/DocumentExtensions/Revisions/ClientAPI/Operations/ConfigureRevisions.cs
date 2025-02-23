using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Operations
{
    public class ConfigRevisions
    {
        public ConfigRevisions()
        {
            // REPLACE
            using (var documentStore = new DocumentStore())
            {
                #region replace_configuration
                // ==============================================================================
                // Define default settings that will apply to ALL collections
                // Note: this is optional
                var defaultRevConfig = new RevisionsCollectionConfiguration()
                {
                    MinimumRevisionsToKeep = 100,
                    MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                    MaximumRevisionsToDeleteUponDocumentUpdate = 15,
                    PurgeOnDelete = false,
                    Disabled = false
                    
                    // With this configuration:
                    // ------------------------
                    // * A revision will be created anytime a document is modified or deleted.
                    // * Revisions of a deleted document can be accessed in the Revisions Bin view.
                    // * At least 100 of the latest revisions will be kept.
                    // * Older revisions will be removed if they exceed 7 days on next revision creation.
                    // * A maximum of 15 revisions will be deleted each time a document is updated,
                    //   until the defined '# of revisions to keep' limit is reached.
                };

                // ==============================================================================
                // Define a specific configuration for the EMPLOYEES collection
                // This will override the default settings 
                var employeesRevConfig = new RevisionsCollectionConfiguration()
                {
                    MinimumRevisionsToKeep = 50,
                    MinimumRevisionAgeToKeep = new TimeSpan(hours: 12, minutes: 0, seconds: 0),
                    PurgeOnDelete = true,
                    Disabled = false
                    
                    // With this configuration:
                    // ------------------------
                    // * A revision will be created anytime an Employee document is modified.
                    // * When a document is deleted all its revisions will be removed.
                    // * At least 50 of the latest revisions will be kept.
                    // * Older revisions will be removed if they exceed 12 hours on next revision creation.
                };

                // ==============================================================================
                // Define a specific configuration for the PRODUCTS collection
                // This will override the default settings 
                var productsRevConfig = new RevisionsCollectionConfiguration()
                {
                    Disabled = true 
                    // No revisions will be created for the Products collection,
                    // even though default configuration is enabled
                };

                // ==============================================================================
                // Combine all configurations in the RevisionsConfiguration object
                var revisionsConfig = new RevisionsConfiguration()
                {
                    Default = defaultRevConfig,
                    Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                    {
                        { "Employees", employeesRevConfig },
                        { "Products", productsRevConfig }
                    }
                };
                
                // ==============================================================================
                // Define the configure revisions operation, pass the configuration
                var configureRevisionsOp = new ConfigureRevisionsOperation(revisionsConfig);
                
                // Execute the operation by passing it to Maintenance.Send
                // Any existing configuration will be replaced with the new configuration passed
                documentStore.Maintenance.Send(configureRevisionsOp);
                #endregion
            }
            
            // MODIFY
            using (var documentStore = new DocumentStore())
            {
                var defaultRevConfig = new RevisionsCollectionConfiguration()
                {
                    MinimumRevisionsToKeep = 100,
                    MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                    MaximumRevisionsToDeleteUponDocumentUpdate = 15,
                    PurgeOnDelete = false,
                    Disabled = false
                };

                var employeesRevConfig = new RevisionsCollectionConfiguration()
                {
                    MinimumRevisionsToKeep = 50,
                    MinimumRevisionAgeToKeep = new TimeSpan(hours: 12, minutes: 0, seconds: 0),
                    PurgeOnDelete = true
                };

                var productsRevConfig = new RevisionsCollectionConfiguration()
                {
                    Disabled = true
                };

                #region modify_configuration
                // ==============================================================================
                // Define the get database record operation:
                var getDatabaseRecordOp = new GetDatabaseRecordOperation(documentStore.Database);
                // Get the current revisions configuration from the database record:
                RevisionsConfiguration revisionsConfig =
                    documentStore.Maintenance.Server.Send(getDatabaseRecordOp).Revisions;

                // ==============================================================================
                // If no revisions configuration exists, then create a new configuration
                if (revisionsConfig == null)
                {
                    revisionsConfig = new RevisionsConfiguration()
                    {
                        Default = defaultRevConfig,
                        Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                        {
                            { "Employees", employeesRevConfig },
                            { "Products", productsRevConfig }
                        }
                    };
                }
                
                // ==============================================================================
                // If a revisions configuration already exists, then modify it
                else
                {
                    revisionsConfig.Default = defaultRevConfig;
                    revisionsConfig.Collections["Employees"] = employeesRevConfig;
                    revisionsConfig.Collections["Products"] = productsRevConfig;
                }

                // ==============================================================================
                // Define the configure revisions operation, pass the configuration
                var configureRevisionsOp = new ConfigureRevisionsOperation(revisionsConfig);

                // Execute the operation by passing it to Maintenance.Send
                // The existing configuration will be updated
                documentStore.Maintenance.Send(configureRevisionsOp);
                #endregion
            }
        }

        public async Task ConfigRevisionsAsync()
        {
            // REPLACE
            using (var documentStore = new DocumentStore())
            {
                #region replace_configuration_async
                // ==============================================================================
                // Define default settings that will apply to ALL collections
                // Note: this is optional
                var defaultRevConfig = new RevisionsCollectionConfiguration()
                {
                    MinimumRevisionsToKeep = 100,
                    MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                    MaximumRevisionsToDeleteUponDocumentUpdate = 15,
                    PurgeOnDelete = false,
                    Disabled = false,
                    
                    // With this configuration:
                    // ------------------------
                    // * A revision will be created anytime a document is modified or deleted.
                    // * Revisions of a deleted document can be accessed in the Revisions Bin view.
                    // * At least 100 of the latest revisions will be kept.
                    // * Older revisions will be removed if they exceed 7 days on next revision creation.
                    // * A maximum of 15 revisions will be deleted each time a document is updated,
                    //   until the defined '# of revisions to keep' limit is reached.
                };

                // ==============================================================================
                // Define a specific configuration for the EMPLOYEES collection
                // This will override the default settings 
                var employeesRevConfig = new RevisionsCollectionConfiguration()
                {
                    MinimumRevisionsToKeep = 50,
                    MinimumRevisionAgeToKeep = new TimeSpan(hours: 12, minutes: 0, seconds: 0),
                    PurgeOnDelete = true,
                    Disabled = false
                   
                    // With this configuration:
                    // ------------------------
                    // * A revision will be created anytime an Employee document is modified.
                    // * When a document is deleted all its revisions will be removed.
                    // * At least 50 of the latest revisions will be kept.
                    // * Older revisions will be removed if they exceed 12 hours on next revision creation.
                };
                    
                // ==============================================================================
                // Define a specific configuration for the PRODUCTS collection
                // This will override the default settings 
                var productsRevConfig = new RevisionsCollectionConfiguration()
                {
                    Disabled = true 
                    // No revisions will be created for the Products collection,
                    // even though default configuration is enabled
                };

                // ==============================================================================
                // Combine all configurations in the RevisionsConfiguration object
                var revisionsConfig = new RevisionsConfiguration()
                {
                    Default = defaultRevConfig,
                    Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                    {
                        { "Employees", employeesRevConfig },
                        { "Products", productsRevConfig }
                    }
                };

                // ==============================================================================
                // Define the configure revisions operation, pass the configuration
                var configureRevisionsOp = new ConfigureRevisionsOperation(revisionsConfig);
               
                // Execute the operation by passing it to Maintenance.SendAsync
                // Any existing configuration will be replaced with the new configuration passed
                await documentStore.Maintenance.SendAsync(configureRevisionsOp);
                #endregion
            }
            
            // MODIFY
            using (var documentStore = new DocumentStore())
            {
                var defaultRevConfig = new RevisionsCollectionConfiguration()
                {
                    MinimumRevisionsToKeep = 100,
                    MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                    MaximumRevisionsToDeleteUponDocumentUpdate = 15,
                    PurgeOnDelete = false,
                    Disabled = false
                };

                var employeesRevConfig = new RevisionsCollectionConfiguration()
                {
                    MinimumRevisionsToKeep = 50,
                    MinimumRevisionAgeToKeep = new TimeSpan(hours: 12, minutes: 0, seconds: 0),
                    PurgeOnDelete = true
                };

                var productsRevConfig = new RevisionsCollectionConfiguration()
                {
                    Disabled = true
                };

                #region modify_configuration_async
                // ==============================================================================
                // Define the get database record operation:
                var getDatabaseRecordOp = new GetDatabaseRecordOperation(documentStore.Database);
                // Get the current revisions configuration from the database record:
                RevisionsConfiguration revisionsConfig =
                    documentStore.Maintenance.Server.Send(getDatabaseRecordOp).Revisions;

                // ==============================================================================
                // If no revisions configuration exists, then create a new configuration
                if (revisionsConfig == null)
                {
                    revisionsConfig = new RevisionsConfiguration()
                    {
                        Default = defaultRevConfig,
                        Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                        {
                            { "Employees", employeesRevConfig },
                            { "Products", productsRevConfig }
                        }
                    };
                }
                
                // ==============================================================================
                // If a revisions configuration already exists, then modify it
                else
                {
                    revisionsConfig.Default = defaultRevConfig;
                    revisionsConfig.Collections["Employees"] = employeesRevConfig;
                    revisionsConfig.Collections["Products"] = productsRevConfig;
                }

                // ==============================================================================
                // Define the configure revisions operation, pass the configuration
                var configureRevisionsOp = new ConfigureRevisionsOperation(revisionsConfig);

                // Execute the operation by passing it to Maintenance.SendAsync
                // The existing configuration will be updated
                await documentStore.Maintenance.SendAsync(configureRevisionsOp);
                #endregion
            }
        }
    }

    public class Syntax
    {
        public interface IFoo
        {
            /*
            #region syntax_1
            public ConfigureRevisionsOperation(RevisionsConfiguration configuration);
            #endregion
            */
        }

        #region syntax_2
        public class RevisionsConfiguration
        {
            public RevisionsCollectionConfiguration Default;
            public Dictionary<string, RevisionsCollectionConfiguration> Collections;
        }
        #endregion
        
        #region syntax_3
        public class RevisionsCollectionConfiguration
        {
            public long? MinimumRevisionsToKeep { get; set; }
            public TimeSpan? MinimumRevisionAgeToKeep { get; set; }
            public long? MaximumRevisionsToDeleteUponDocumentUpdate { get; set; }
            public bool PurgeOnDelete { get; set; }
            public bool Disabled { get; set; }
        }
        #endregion
    }
}
