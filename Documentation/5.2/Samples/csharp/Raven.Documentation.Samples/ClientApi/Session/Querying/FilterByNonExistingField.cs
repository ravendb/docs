using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    class FilterByNonExistingField
    {
        public async void Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region whereNotExists_1
                    List<Order> ordersWithoutFreightField = session
                        .Advanced
                         // Define a DocumentQuery on 'Orders' collection
                        .DocumentQuery<Order>()
                         // Search for documents that do Not contain field 'Freight'
                        .Not.WhereExists("Freight")
                         // Execute the query
                        .ToList();
                    
                    // Results will be only the documents that do Not contain the 'Freight' field in 'Orders' collection 
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereNotExists_1_async
                    List<Order> ordersWithoutFreightField = await asyncSession
                        .Advanced
                         // Define a DocumentQuery on 'Orders' collection
                        .AsyncDocumentQuery<Order>()
                         // Search for documents that do Not contain field 'Freight'
                        .Not.WhereExists("Freight")
                         // Execute the query
                        .ToListAsync();
                    
                    // Results will be only the documents that do Not contain the 'Freight' field in 'Orders' collection 
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region whereNotexists_2
                    // Query the index
                    // ===============
                    
                    List<Order> ordersWithoutFreightField = session
                        .Advanced
                         // Define a DocumentQuery on the index
                        .DocumentQuery<Order, Orders_ByFreight>()
                         // Verify the index is not stale (optional)
                        .WaitForNonStaleResults()
                         // Search for documents that do Not contain field 'Freight'
                        .Not.WhereExists(x => x.Freight)
                         // Execute the query
                        .ToList();
                    
                    // Results will be only the documents that do Not contain the 'Freight' field in 'Orders' collection 
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereNotexists_2_async
                    // Query the index
                    // ===============
                    
                    List<Order> ordersWithoutFreightField = await asyncSession
                        .Advanced
                         // Define a DocumentQuery on the index
                        .AsyncDocumentQuery<Order, Orders_ByFreight>()
                         // Verify the index is not stale (optional)
                        .WaitForNonStaleResults()
                         // Search for documents that do Not contain field 'Freight'
                        .Not.WhereExists(x => x.Freight)
                         // Execute the query
                        .ToListAsync();

                    // Results will be only the documents that do Not contain the 'Freight' field in 'Orders' collection 
                    #endregion
                }
            }
        }
    }
    
    #region the_index
    // Define a static index on the 'Orders' collection
    // ================================================
    
    public class Orders_ByFreight : AbstractIndexCreationTask<Order, Orders_ByFreight.IndexEntry>
    {
        public class IndexEntry
        {
            // Define the index-fields
            public decimal Freight { get; set; }
            public string Id { get; set; }
        }
        
        public Orders_ByFreight()
        {
            // Define the index Map function
            Map = orders => from doc in orders
                select new IndexEntry
                {
                    // Index a field that might be missing in SOME documents
                    Freight = doc.Freight,
                    // Index a field that exists in ALL documents in the collection
                    Id = doc.Id
                };
        }
    }
    #endregion
}
