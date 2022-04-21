using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Revisions
{
    public class ConfigureRevisions
    {
        public async Task Foo() {

            using (var documentStore = new DocumentStore())
            {
                using (var session = documentStore.OpenSession())
                {
                    #region configure-revisions_sync
                    // Create a default configuration that will apply to all collections
                    var defaultRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 100,
                        MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                        PurgeOnDelete = false,

                        // Even if MinimumRevisionsToKeep or MinimumRevisionAgeToKeep are exceeded,
                        // purge no more than 15 revisions each time the document is modified.
                        MaximumRevisionsToDeleteUponDocumentUpdate = 15
                    };

                    // Create a collection-specific configuration
                    // that will override the default configuration for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 42,
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        PurgeOnDelete = true
                    };

                    // Create a collection-specific configuration
                    // that will override the default configuration for the Products collection
                    var productsRevConfig = new RevisionsCollectionConfiguration()
                    {
                        Disabled = true
                    };

                    // Combine the configurations in a RevisionsConfiguration object
                    var revConfig = new RevisionsConfiguration()
                    {
                        Default = defaultRevConfig,

                        Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                        {
                            { "Employees", employeesRevConfig },
                            { "Products", productsRevConfig }
                        }
                    };

                    // Execute the operation to update Revisions settings
                    documentStore.Maintenance.Send(new ConfigureRevisionsOperation(revConfig));
                    #endregion
                }

                using (var asyncSession = documentStore.OpenAsyncSession())
                {
                    #region configure-revisions_async
                    // Create a default configuration that will apply to all collections
                    var defaultRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 100,
                        MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                        PurgeOnDelete = false,

                        // Even if MinimumRevisionsToKeep or MinimumRevisionAgeToKeep are exceeded,
                        // purge no more than 15 revisions each time the document is modified.
                        MaximumRevisionsToDeleteUponDocumentUpdate = 15
                    };

                    // Create a collection-specific configuration
                    // that will override the default configuration for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 42,
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        PurgeOnDelete = true
                    };

                    // Create a collection-specific configuration
                    // that will override the default configuration for the Products collection
                    var productsRevConfig = new RevisionsCollectionConfiguration()
                    {
                        Disabled = true
                    };

                    // Combine the configurations in a RevisionsConfiguration object
                    var revConfig = new RevisionsConfiguration()
                    {
                        Default = defaultRevConfig,

                        Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                        {
                            { "Employees", employeesRevConfig },
                            { "Products", productsRevConfig }
                        }
                    };

                    // Execute the operation to update Revisions settings
                    await documentStore.Maintenance.SendAsync(new ConfigureRevisionsOperation(revConfig));
                    #endregion
                }

                // Update an existing configuration - sync
                using (var session = documentStore.OpenSession())
                {
                    // Create a default configuration that will apply to all collections
                    var defaultRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 100,
                        MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                        PurgeOnDelete = false,

                        // Even if MinimumRevisionsToKeep or MinimumRevisionAgeToKeep are exceeded,
                        // purge no more than 15 revisions each time the document is modified.
                        MaximumRevisionsToDeleteUponDocumentUpdate = 15
                    };

                    // Create a collection-specific configuration
                    // that will override the default configuration for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 42,
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        PurgeOnDelete = true
                    };

                    // Create a collection-specific configuration
                    // that will override the default configuration for the Products collection
                    var productsRevConfig = new RevisionsCollectionConfiguration()
                    {
                        Disabled = true
                    };

                    // create Revisions configuration to check what hapens when it is updated
                    RevisionsConfiguration preliminaryConfiguration = new RevisionsConfiguration()
                    {
                        Default = defaultRevConfig,

                        Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                        {
                            { "Employees", employeesRevConfig },
                            { "Products", productsRevConfig }
                        }
                    };
                    await documentStore.Maintenance.SendAsync(new ConfigureRevisionsOperation(preliminaryConfiguration));

                    #region update-existing-configuration_sync
                    // Get the current Revisions configuration from the Database Record
                    RevisionsConfiguration configuration =
                        documentStore.Maintenance.Server.Send(
                            new GetDatabaseRecordOperation(documentStore.Database)).Revisions;

                    // if there is no Revisions configuration, we create our own
                    if (configuration == null)
                    {
                        configuration = new RevisionsConfiguration()
                        {
                            Default = defaultRevConfig,

                            Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                            {
                                { "Employees", employeesRevConfig },
                                { "Products", productsRevConfig }
                            }
                        };
                    }

                    // If a Revisions configuration already exists, we modify it
                    else
                    {
                        configuration.Default = defaultRevConfig;

                        // Remove collection configurations in case they already exist, and then add them
                        configuration.Collections.Remove("Employees");
                        configuration.Collections.Add("Employees", employeesRevConfig);
                        configuration.Collections.Remove("Products");
                        configuration.Collections.Add("Products", productsRevConfig);
                    }

                    // Update the configuration
                    documentStore.Maintenance.Send(new ConfigureRevisionsOperation(configuration));
                    #endregion
                }

                // Update an existing configuration - async
                using (var session = documentStore.OpenSession())
                {
                    // Create a default configuration that will apply to all collections
                    var defaultRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 100,
                        MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                        PurgeOnDelete = false,

                        // Even if MinimumRevisionsToKeep or MinimumRevisionAgeToKeep are exceeded,
                        // purge no more than 15 revisions each time the document is modified.
                        MaximumRevisionsToDeleteUponDocumentUpdate = 15
                    };

                    // Create a collection-specific configuration
                    // that will override the default configuration for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 42,
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        PurgeOnDelete = true
                    };

                    // Create a collection-specific configuration
                    // that will override the default configuration for the Products collection
                    var productsRevConfig = new RevisionsCollectionConfiguration()
                    {
                        Disabled = true
                    };

                    // create Revisions configuration to check what hapens when it is updated
                    RevisionsConfiguration preliminaryConfiguration = new RevisionsConfiguration()
                    {
                        Default = defaultRevConfig,

                        Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                        {
                            { "Employees", employeesRevConfig },
                            { "Products", productsRevConfig }
                        }
                    };
                    await documentStore.Maintenance.SendAsync(new ConfigureRevisionsOperation(preliminaryConfiguration));

                    #region update-existing-configuration_async
                    // Get the current Revisions configuration from the Database Record
                    RevisionsConfiguration configuration =
                        documentStore.Maintenance.Server.Send(
                            new GetDatabaseRecordOperation(documentStore.Database)).Revisions;

                    // if there is no Revisions configuration, we create our own
                    if (configuration == null)
                    {
                        configuration = new RevisionsConfiguration()
                        {
                            Default = defaultRevConfig,

                            Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                            {
                                { "Employees", employeesRevConfig },
                                { "Products", productsRevConfig }
                            }
                        };
                    }

                    // If a Revisions configuration already exists, we modify it
                    else
                    {
                        configuration.Default = defaultRevConfig;

                        // Remove collection configurations in case they already exist, and then add them
                        configuration.Collections.Remove("Employees");
                        configuration.Collections.Add("Employees", employeesRevConfig);
                        configuration.Collections.Remove("Products");
                        configuration.Collections.Add("Products", productsRevConfig);
                    }

                    // Update the configuration
                    await documentStore.Maintenance.SendAsync(new ConfigureRevisionsOperation(configuration));
                    #endregion
                }
            }
        }
    }
}
