using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Operations.Revisions;

namespace Raven.Documentation.Samples.ClientApi.Operations.Revisions
{
    public class HowtoConfigureRevisions
    {
        public async Task Foo() {

            using (var documentStore = new DocumentStore())
            {
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
