using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.DataArchival;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Smuggler;
using Raven.Client.Documents.Subscriptions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DataArchival
{
    #region index_1
    public class Orders_ByOrderDate : 
        AbstractIndexCreationTask<Order, Orders_ByOrderDate.IndexEntry>
    {
        public class IndexEntry
        {
            public DateTime OrderDate { get; set; }
        }
           
        public Orders_ByOrderDate()
        {
            Map = orders => from order in orders
                select new IndexEntry
                {
                    OrderDate = order.OrderedAt
                };
            
            // Configure whether the index should process data from archived documents:
            // ========================================================================
            ArchivedDataProcessingBehavior =
                // You can set to 'ExcludeArchived', 'IncludeArchived, or 'ArchivedOnly'
                Raven.Client.Documents.DataArchival.ArchivedDataProcessingBehavior.IncludeArchived;
        }
    }
    #endregion
    
    #region index_2
    public class Orders_ByOrderDate_JS : AbstractJavaScriptIndexCreationTask
    {
        public Orders_ByOrderDate_JS()
        {
            Maps = new HashSet<string>()
            {
                @"map('Orders', function (order) {
                       return {
                           OrderDate: order.OrderedAt
                       };
                  })"
            };
            
            // Configure whether the index should process data from archived documents:
            // ========================================================================
            ArchivedDataProcessingBehavior =
                // Can set the to 'ExcludeArchived', 'IncludeArchived, or 'ArchivedOnly'
                Raven.Client.Documents.DataArchival.ArchivedDataProcessingBehavior.IncludeArchived;
        }
    }
    #endregion
    
    #region index_4
    public class Orders_ByArchivedStatus : 
        AbstractIndexCreationTask<Order, Orders_ByArchivedStatus.IndexEntry>
    {
        public class IndexEntry
        {
            public bool? isArchived { get; set; }
            public DateTime? OrderDate { get; set; }
            public string ShipToCountry { get; set; }
        }
           
        public Orders_ByArchivedStatus()
        {
            Map = orders => from order in orders
                let metadata = MetadataFor(order)
                
                // Retrieve the '@archived' metadata property from the document:
                let archivedProperty = 
                    metadata.Value<bool?>(Raven.Client.Constants.Documents.Metadata.Archived) 
                // Alternative syntax:
                // let archivedProperty =
                //     (bool?)metadata[Raven.Client.Constants.Documents.Metadata.Archived]
                      
                select new IndexEntry
                {
                    // Index whether the document is archived:
                    isArchived = archivedProperty == true,
                    
                    // Index the order date only if the document is archived:
                    OrderDate = archivedProperty == true ? order.OrderedAt : null,
                    
                    // Index the destination country only if the document is not archived:
                    ShipToCountry = archivedProperty == null ? order.ShipTo.Country : null
                };
            
            ArchivedDataProcessingBehavior =
                Raven.Client.Documents.DataArchival.ArchivedDataProcessingBehavior.IncludeArchived;
        }
    }
    #endregion
    
    #region index_5
    public class Orders_ByArchivedStatus_JS : AbstractJavaScriptIndexCreationTask
    {
        public Orders_ByArchivedStatus_JS()
        {
            Maps = new HashSet<string>()
            {
                @"map('Orders', function (order) {
                      var metadata = metadataFor(order);
                      var archivedProperty = metadata['@archived'];

                      var isArchived = (archivedProperty === true);

                      var orderDate = isArchived ? order.OrderedAt : null;
                      var shipToCountry = !isArchived ? order.ShipTo.Country : null;

                      return {
                          IsArchived: isArchived,
                          OrderDate: orderDate,
                          ShipToCountry: shipToCountry
                      };
                })"
            };
            
            ArchivedDataProcessingBehavior =
                Raven.Client.Documents.DataArchival.ArchivedDataProcessingBehavior.IncludeArchived;
        }
    }
    #endregion
    
    public class ArchivedDocsIntegration
    {
        public async Task IndexExamples()
        {
            using (var store = new DocumentStore())
            {
                #region index_3
                var indexDefinition = new IndexDefinitionBuilder<Order>()
                {
                    Map = orders => from order in orders
                        select new { order.OrderedAt }
                }
                    .ToIndexDefinition(new DocumentConventions());
            
                indexDefinition.Name = "Orders/ByOrderDate";
                
                // Configure whether the index should process data from archived documents:
                // ========================================================================
                indexDefinition.ArchivedDataProcessingBehavior =
                    // You can set to 'ExcludeArchived', 'IncludeArchived, or 'ArchivedOnly'
                    ArchivedDataProcessingBehavior.IncludeArchived;

                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region index_6
                var indexDefinition = new IndexDefinition
                {
                    Name = "Orders/ByArchivedStatus",
                    
                    Maps = new HashSet<string>
                    {
                        @"from order in docs.Orders
                          let metadata = MetadataFor(order)
                          let archivedProperty = (bool?)metadata[""@archived""]

                          select new 
                          {
                              IsArchived = archivedProperty == true,
                              OrderDate = archivedProperty == true ? order.OrderedAt : null,
                              ShipToCountry = archivedProperty == null ? order.ShipTo.Country : null
                          }"
                    },
                    
                    ArchivedDataProcessingBehavior = ArchivedDataProcessingBehavior.IncludeArchived
                };
               
                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
        }

        public async Task SubscriptionExamples()
        {
            using (var store = new DocumentStore())
            {
                #region subscription_task_generic_syntax
                var subscriptionName = store.Subscriptions
                    .Create<Order>(new SubscriptionCreationOptions<Order>()
                {
                    Name = "ArchivedOrdersSubscription",
                    // Workers that will subscribe to this subscription task
                    // will receive only archived documents from the 'Orders' collection.
                    ArchivedDataProcessingBehavior = ArchivedDataProcessingBehavior.ArchivedOnly
                    
                    // You can set the behavior to 'ExcludeArchived', 'IncludeArchived, or 'ArchivedOnly'
                });
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region subscription_task_rql_syntax
                var subscriptionName = store.Subscriptions
                    .Create(new SubscriptionCreationOptions()
                {
                    Name = "ArchivedOrdersSubscription",
                    Query = "from Orders",
                    // Workers that will subscribe to this subscription task
                    // will receive only archived documents from the 'Orders' collection.
                    ArchivedDataProcessingBehavior = ArchivedDataProcessingBehavior.ArchivedOnly
                    
                    // You can set the behavior to 'ExcludeArchived', 'IncludeArchived, or 'ArchivedOnly'
                });
                #endregion
            }
        }
        
        public async Task SmugglerExamples()
        {
            using (var store = new DocumentStore())
            {
                #region export
                var exportOperation = store.Smuggler.ExportAsync(
                    new DatabaseSmugglerExportOptions()
                    {
                        // Export only non-archived documents:
                        IncludeArchived = false
                    }, "DestinationFilePath");
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region import
                var importOperation = store.Smuggler.ImportAsync(
                    new DatabaseSmugglerImportOptions()
                    {
                        // Include archived documents in the import:
                        IncludeArchived = true
                    }, "SourceFilePath");
                #endregion
            }
        }
    }
}
