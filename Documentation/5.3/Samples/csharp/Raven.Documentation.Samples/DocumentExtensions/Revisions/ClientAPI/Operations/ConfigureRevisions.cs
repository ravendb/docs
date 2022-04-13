using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Operations.Revisions;

namespace Raven.Documentation.Samples.ClientApi.Operations.Revisions
{
    public class ConfigureRevisions
    {
        public async Task Foo() {

            using (var documentStore = new DocumentStore())
            {
                #region default-and-collection-specific-configuration
                documentStore.Maintenance.Send(new ConfigureRevisionsOperation(new RevisionsConfiguration
                {
                    // A default configuration that applies to all collections
                    Default = new RevisionsCollectionConfiguration
                    {
                        // Enable Revisions
                        Disabled = false,
                        // Do not purge revisions of documents when the documents are deleted
                        PurgeOnDelete = false,
                        // Keep the 5 most recent revisions per document, purge older revisions
                        MinimumRevisionsToKeep = 5,
                        // Keep revisions created in the last 14 days, and purge older revisions
                        MinimumRevisionAgeToKeep = TimeSpan.FromDays(14),
                        // Even if MinimumRevisionsToKeep or MinimumRevisionAgeToKeep are exceeded,
                        // purge no more than 15 revisions when the document is modified.
                        MaximumRevisionsToDeleteUponDocumentUpdate = 15
                    },
                    // Collection-specific configurations that override the default configuration
                    Collections = new Dictionary<string, RevisionsCollectionConfiguration>
                    {
                        // Enable Revisions for Users
                        {"Users", new RevisionsCollectionConfiguration {Disabled = false}},
                        // Disable Revisions for Orders
                        {"Orders", new RevisionsCollectionConfiguration {Disabled = true}}
                    }
                }));
                #endregion

                using (var session = documentStore.OpenSession())
                {
                    #region operation_sync
                    // Create a configuration for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration() 
                    {
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        MinimumRevisionsToKeep = 42,
                        PurgeOnDelete = true
                    };

                    // Create a configuration for the Products collection
                    var productsRevConfig = new RevisionsCollectionConfiguration()
                    {
                        Disabled = true
                    };

                    // Create a default collection configuration
                    var defaultRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                        MinimumRevisionsToKeep = 100,
                        PurgeOnDelete = false
                    };

                    // Combine to create a configuration for the database
                    var northwindRevConfig = new RevisionsConfiguration()
                    {
                        Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                        {
                            { "Employees", employeesRevConfig },
                            { "Products", productsRevConfig }
                        },

                        Default = defaultRevConfig
                    };

                    // Execute the operation to update the database
                    documentStore.Maintenance.Send(new ConfigureRevisionsOperation(northwindRevConfig));
                    #endregion
                }

                using (var asyncSession = documentStore.OpenAsyncSession())
                {
                    #region operation_async
                    // Create a configuration for the Employees collection
                    var employeesRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionAgeToKeep = new TimeSpan(hours: 1, minutes: 23, seconds: 45),
                        MinimumRevisionsToKeep = 42,
                        PurgeOnDelete = true
                    };

                    // Create a configuration for the Products collection
                    var productsRevConfig = new RevisionsCollectionConfiguration()
                    {
                        Disabled = true
                    };

                    // Create a default collection configuration
                    var defaultRevConfig = new RevisionsCollectionConfiguration()
                    {
                        MinimumRevisionAgeToKeep = new TimeSpan(days: 7, 0, 0, 0),
                        MinimumRevisionsToKeep = 100,
                        PurgeOnDelete = false
                    };

                    // Combine to create a configuration for the database
                    var northwindRevConfig = new RevisionsConfiguration()
                    {
                        Collections = new Dictionary<string, RevisionsCollectionConfiguration>()
                        {
                            { "Employees", employeesRevConfig },
                            { "Products", productsRevConfig }
                        },

                        Default = defaultRevConfig
                    };

                    // Execute the operation to update the database
                    await documentStore.Maintenance.SendAsync(new ConfigureRevisionsOperation(northwindRevConfig));
                    #endregion
                }
            }
        }
    }
}
