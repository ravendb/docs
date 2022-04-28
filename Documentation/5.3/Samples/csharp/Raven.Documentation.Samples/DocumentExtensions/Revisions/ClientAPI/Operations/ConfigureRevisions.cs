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
                    // Define default settings that will apply to all collections
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
                    // that will override the default settings for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 42,
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        PurgeOnDelete = true
                    };

                    // Create a collection-specific configuration
                    // that will override the default settings for the Products collection
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
                    // Define default settings that will apply to all collections
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
                    // that will override the default settings for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 42,
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        PurgeOnDelete = true
                    };

                    // Create a collection-specific configuration
                    // that will override the default settings for the Products collection
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
                    // Define default settings that will apply to all collections
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
                    // that will override the default settings for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 42,
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        PurgeOnDelete = true
                    };

                    // Create a collection-specific configuration
                    // that will override the default settings for the Products collection
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

                        configuration.Collections["Employees"] = employeesRevConfig;
                        configuration.Collections["Products"] = productsRevConfig;
                    }

                    // Update the configuration
                    documentStore.Maintenance.Send(new ConfigureRevisionsOperation(configuration));
                    #endregion
                }

                // Update an existing configuration - async
                using (var session = documentStore.OpenSession())
                {
                    // Define default settings that will apply to all collections
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
                    // that will override the default settings for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionsToKeep = 42,
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        PurgeOnDelete = true
                    };

                    // Create a collection-specific configuration
                    // that will override the default settings for the Products collection
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

                        configuration.Collections["Employees"] = employeesRevConfig;
                        configuration.Collections["Products"] = productsRevConfig;
                    }

                    // Update the configuration
                    await documentStore.Maintenance.SendAsync(new ConfigureRevisionsOperation(configuration));
                    #endregion
                }


                using (var session = documentStore.OpenSession())
                {

                    #region conflict-revisions-configuration_sync
                    // Modify the document conflicts configuration
                    documentStore.Maintenance.Server.Send(
                    new ConfigureRevisionsForConflictsOperation(documentStore.Database,
                        new RevisionsCollectionConfiguration
                        {
                            // Purge conflict revisions upon their parent document deletion
                            PurgeOnDelete = true,

                            // Limit the number of conflict revisions to keep
                            MinimumRevisionsToKeep = 50
                        }));
                    #endregion
                }

                using (var asyncSession = documentStore.OpenAsyncSession())
                {

                    #region conflict-revisions-configuration_async
                    // Modify the document conflicts configuration
                    await documentStore.Maintenance.Server.SendAsync(
                    new ConfigureRevisionsForConflictsOperation(documentStore.Database,
                        new RevisionsCollectionConfiguration
                        {
                            // Purge conflict revisions upon their parent document deletion
                            PurgeOnDelete = true,

                            // Limit the number of conflict revisions to keep
                            MinimumRevisionsToKeep = 50
                        }));
                    #endregion
                }
            }
        }
    }
}
