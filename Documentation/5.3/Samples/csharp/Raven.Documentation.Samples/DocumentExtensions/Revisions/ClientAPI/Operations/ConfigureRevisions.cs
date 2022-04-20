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
                using (var session = documentStore.OpenSession())
                {
                    #region operation_sync
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
                    #region operation_async
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
            }
        }
    }
}
