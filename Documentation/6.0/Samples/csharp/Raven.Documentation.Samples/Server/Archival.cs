using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.DataArchival;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.DataArchival;
using Raven.Client.Documents.Operations.Expiration;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Smuggler;
using Raven.Client.Documents.Subscriptions;
using Raven.Client.Util;
using static Raven.Client.Constants;
using Sparrow;

namespace Raven.Documentation.Samples.Server
{
    public class Archival
    {
        private class User
        {
            public string Name { get; set; }
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region overrideDefaultIndexConfiguration
                store.Maintenance.Send(new PutIndexesOperation(new[] {
                new IndexDefinition
                {
                    Maps = {
                        //...
                            },

                    Name = "indexName",

                    // Process archived documents
                    ArchivedDataProcessingBehavior = ArchivedDataProcessingBehavior.IncludeArchived
                }}));
                #endregion

                #region applyAdditionalLogic
                store.Maintenance.Send(new PutIndexesOperation(new[] {
                new IndexDefinition
                {
                    Maps = {
                                // This will apply only to non-archived documents
                                // (whose @archived property is null)
                                "from o in docs where o[\"@metadata\"][\"@archived\"] == null select new" +
                                "{" +
                                "    Name = o.Name" +
                                "}"
                            }
                }}));
                #endregion

                #region useIndexDefinitionBuilder
                var indexDefinition = new IndexDefinitionBuilder<Company>
                {
                    Map = companies => from company in companies where company.Name == 
                                       "Company Name" select new { company.Name },
                }.ToIndexDefinition(store.Conventions);

                indexDefinition.Name = "indexName";

                // Process only archived documents
                indexDefinition.ArchivedDataProcessingBehavior = 
                        ArchivedDataProcessingBehavior.ArchivedOnly;

                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region dataSubscriptionDefinition
                var subsId = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions
                {
                    Query = "from Companies",
                    Name = "Created",
                    // Process only archived documents
                    ArchivedDataProcessingBehavior = ArchivedDataProcessingBehavior.ArchivedOnly
                });
                #endregion

                string path = "/";
                #region smugglerOption
                var operation =
                await store.Smuggler.ExportAsync
                        (new DatabaseSmugglerExportOptions { IncludeArchived = true }, path);
                #endregion
            };

            using (var store = new DocumentStore())
            {
                var time = SystemTime.UtcNow.AddMinutes(5).ToString();

                #region archiveByPatch
                // Archive all the documents in a collection
                var operation = await store.Operations.SendAsync(new PatchByQueryOperation(new IndexQuery()
                {
                    // provide the time in UTC format, e.g. 2024-01-01T12:00:00.000Z
                    Query = "from Companies update { archived.archiveAt(this, \"" + time + "\") }"
                }));
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region unarchiveByPatch
                // Unarchive all archived document in a collection
                var operation =
                    await store.Operations.SendAsync(
                        new PatchByQueryOperation(new IndexQuery()
                        {
                            Query = @"from Companies update 
                            {
                                archived.unarchive(this)
                            }"
                        }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region unarchiveUsingAutoIndex
                var operation =
                    await store.Operations.SendAsync(
                        new PatchByQueryOperation(new IndexQuery()
                        {
                            // This query uses an index, and if the index excludes 
                            // archived docs - unarchiving will fail.
                            Query = @"from Companies where Name == 'shoes' update
                                    {
                                        archived.unarchive(this)
                                    }"
                        }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region unarchiveCollectionQuery
                var operation =
                    await store.Operations.SendAsync(
                        new PatchByQueryOperation(new IndexQuery()
                        {
                            // This collection query will not exclude archived docs,
                            // and the inner logic will select docs and unarchive them.  
                            Query = @"from Companies as company update 
                                      {
                                        if (company.Name == 'shoes')
                                        archived.unarchive(this)
                                      }"
                        }));
                #endregion
            }


            using (var store = new DocumentStore())
            {
                #region enableArchivingAndSetFrequency
                var configuration = new DataArchivalConfiguration
                {
                    // Enable archiving
                    Disabled = false,
                    // Scan for documents scheduled for archiving every 180 seconds 
                    ArchiveFrequencyInSec = 180
                };

                var result = await store.Maintenance.SendAsync(
                                    new ConfigureDataArchivalOperation(configuration));
                #endregion
            }
        }
    }

    public class Company
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class IndexEntry
    {
        public string AddressText { get; set; }
    }

    #region useAbstractIndexCreationTask
    public class AllCompanies_AddressText : AbstractIndexCreationTask<Company>
    {
        public AllCompanies_AddressText()
        {
            Map = companies => from company in companies
                               select new IndexEntry
                               {
                                   AddressText = company.Address
                               };

            ArchivedDataProcessingBehavior =
                Raven.Client.Documents.DataArchival.ArchivedDataProcessingBehavior.IncludeArchived;
        }
    }
    #endregion
}

