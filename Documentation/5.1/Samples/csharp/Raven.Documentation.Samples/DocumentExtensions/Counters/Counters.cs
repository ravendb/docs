using System;
using Raven.Client.Documents;
using System.Linq;
using System.Collections.Generic;
using Raven.Client.Documents.Queries;
using System.Threading.Tasks;
using System.Threading;
using Raven.Client.Documents.Smuggler;

namespace Rvn.Ch02
{
    class Program
    {
        static void Main(string[] args)
        {
            var docStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "products"
            };
            docStore.Initialize();

            #region counters_region_CountersFor_with_document_load
            // Use CountersFor by passing it a document object

            // 1. Open a session
            using (var session = docStore.OpenSession())
            {
                // 2. Use the session to load a document.
                var document = session.Load<Product>("products/1-C");

                // 3. Create an instance of `CountersFor`
                //   Pass the document object returned from session.Load as a param.
                var documentCounters = session.CountersFor(document);

                // 4. Use `CountersFor` methods to manage the product document's Counters
                documentCounters.Delete("ProductLikes"); // Delete the "ProductLikes" Counter
                documentCounters.Increment("ProductModified", 15); // Add 15 to Counter "ProductModified"
                var counter = documentCounters.Get("DaysLeftForSale"); // Get value of "DaysLeftForSale"

                // 5. Save the changes to the session
                session.SaveChanges();
            }
            #endregion

            #region counters_region_CountersFor_without_document_load
            // Use CountersFor without loading a document

            // 1. Open a session
            using (var session = docStore.OpenSession())
            {
                // 2. pass an explicit document ID to the CountersFor constructor 
                var documentCounters = session.CountersFor("products/1-C");

                // 3. Use `CountersFor` methods to manage the product document's Counters
                documentCounters.Delete("ProductLikes"); // Delete the "ProductLikes" Counter
                documentCounters.Increment("ProductModified", 15); // Add 15 to Counter "ProductModified"
                var counter = documentCounters.Get("DaysLeftForSale"); // Get "DaysLeftForSale"'s value

                // 4. Save changes to the session
                session.SaveChanges();
            }
            #endregion

            // remove a counter from a document
            #region counters_region_Delete
            // 1. Open a session
            using (var session = docStore.OpenSession())
            {
                // 2. pass CountersFor's constructor a document ID  
                var documentCounters = session.CountersFor("products/1-C");

                // 3. Delete the "ProductLikes" Counter
                documentCounters.Delete("ProductLikes");

                // 4. Save changes to the session
                session.SaveChanges();
            }
            #endregion

            // Increment a counter's value
            #region counters_region_Increment
            // 1. Open a session
            using (var session = docStore.OpenSession())
            {
                // 2. pass CountersFor's constructor a document ID  
                var documentCounters = session.CountersFor("products/1-C");

                // 3. Use `CountersFor.Increment`
                documentCounters.Increment("ProductLikes"); // Increase "ProductLikes" by 1, or create it with a value of 1
                documentCounters.Increment("ProductDislikes", 1); // Increase "ProductDislikes" by 1, or create it with a value of 1
                documentCounters.Increment("ProductPageViews", 15); // Increase "ProductPageViews" by 15, or create it with a value of 15
                documentCounters.Increment("DaysLeftForSale", -10); // Decrease "DaysLeftForSale" by 10, or create it with a value of -10

                // 4. Save changes to the session
                session.SaveChanges();
            }
            #endregion

            // get a counter's value by the counter's name
            #region counters_region_Get
            // 1. Open a session
            using (var session = docStore.OpenSession())
            {
                // 2. pass CountersFor's constructor a document ID  
                var documentCounters = session.CountersFor("products/1-C");

                // 3. Use `CountersFor.Get` to retrieve a Counter's value
                var DaysLeft = documentCounters.Get("DaysLeftForSale");
                Console.WriteLine("Days Left For Sale: " + DaysLeft);
            }
            #endregion

            // GetAll
            #region counters_region_GetAll
            // 1. Open a session
            using (var session = docStore.OpenSession())
            {
                // 2. pass CountersFor's constructor a document ID  
                var documentCounters = session.CountersFor("products/1-C");

                // 3. Use GetAll to retrieve all of the document's Counters' names and values.
                var counters = documentCounters.GetAll();

                // list counters' names and values
                foreach (var counter in counters)
                {
                    Console.WriteLine("counter name: " + counter.Key + ", counter value: " + counter.Value);
                }
            }
            #endregion
            //Query a collection for documents with a Counter named "ProductLikes"
            #region counters_region_query
            using (var session = docStore.OpenSession())
            {
                //Select documents from the "Products" collection, with a Counter named `ProductLikes`.  
                //Querying upon Counter values (e.g. using "Where") is not possible.  
                //In this example the documents that contain the Counter are NOT returned, only Counter values.
                var query = session.Query<Product>()
                .Select(product => RavenQuery.Counter(product, "ProductLikes"));

                var queryResults = query.ToList();

                //Show ProductLikes's value, or NULL for documents with no such Counter.  
                foreach (var counterValue in queryResults)
                {
                    Console.WriteLine("ProductLikes: " + counterValue);
                }
            }
            #endregion

            //Query a collection for documents with a Counter named "ProductLikes"
            using (var session = docStore.OpenSession())
            {

                #region counters_region_load_include1
                //include single Counters
                var productPage = session.Load<Product>("products/1-C", includeBuilder =>
                        includeBuilder.IncludeCounter("ProductLikes")
                                      .IncludeCounter("ProductDislikes")
                                      .IncludeCounter("ProductDownloads"));
                #endregion
            }

            using (var session = docStore.OpenSession())
            {
                #region counters_region_load_include2
                //include multiple Counters
                //note that you can combine the inclusion of Counters and documents.
                var productPage = session.Load<Product>("orders/1-A", includeBuilder =>
                        includeBuilder.IncludeDocuments("products/1-C")
                                      .IncludeCounters(new[] { "ProductLikes", "ProductDislikes" }));
                #endregion
            }
            using (var session = docStore.OpenSession())
            {
                #region counters_region_query_include_single_Counter
                //include a single Counter
                var query = session.Query<Product>()
                        .Include(includeBuilder =>
                         includeBuilder.IncludeCounter("ProductLikes"));
                #endregion
            }

            using (var session = docStore.OpenSession())
            {
                #region counters_region_query_include_multiple_Counters
                //include multiple Counters
                var query = session.Query<Product>()
                        .Include(includeBuilder =>
                        includeBuilder.IncludeCounters(new[] { "ProductLikes", "ProductDownloads" }));
                #endregion
            }

            using (var session = docStore.OpenSession())
            {
                #region counters_region_rawqueries_counter
                //Various RQL expressions sent to the server using counter()
                //Returned Counter value is accumulated
                var rawquery1 = session.Advanced
                    .RawQuery<CounterResult>("from products as p select counter(p, \"ProductLikes\")")
                    .ToList();

                var rawquery2 = session.Advanced
                    .RawQuery<CounterResult>("from products select counter(\"ProductLikes\") as ProductLikesCount")
                    .ToList();

                var rawquery3 = session.Advanced
                    .RawQuery<CounterResult>("from products where PricePerUnit > 50 select Name, counter(\"ProductLikes\")")
                    .ToList();
                #endregion

                #region counters_region_rawqueries_counterRaw
                //An RQL expression sent to the server using counterRaw()
                //Returned Counter value is distributed
                var query = session.Advanced
                    .RawQuery<CounterResultRaw>("from users as u select counterRaw(u, \"downloads\")")
                    .ToList();
                #endregion
            }




        }

        private class CounterResult
        {
            public long? ProductPrice { get; set; }
            public long? ProductLikes { get; set; }
            public string ProductSection { get; set; }
        }

        private class CounterResultRaw
        {
            public Dictionary<string, long> Downloads { get; set; }
        }


        public class Product
        {
            public string Id { get; set; }
            public string CustomerId { get; set; }
            public DateTime Started { get; set; }
            public DateTime? Ended { get; set; }
            public double PricePerUnit { get; set; }
            public string Issue { get; set; }
            public int Votes { get; set; }
        }

        #region counters_region_CounterItem
        // The value given to a Counter by each node, is placed in a CounterItem object.
        public struct CounterItem
        {
            public string Name;
            public string DocId;
            public string ChangeVector;
            public long Value;
        }
        #endregion


        private interface IFoo
        {
            #region Increment-definition
            void Increment(string counterName, long incrementValue = 1);
            #endregion

            #region Delete-definition
            void Delete(string counterName);
            #endregion

            #region Get-definition
            long Get(string counterName);
            #endregion

            #region GetAll-definition
            Dictionary<string, long?> GetAll();
            #endregion
        }



        public async Task Sample()
        {
            var token = new CancellationToken();

            using (var store = new DocumentStore())
            {
                // export only Indexes and Documents to a given file
                var exportOperation = await store
                    .Smuggler
                    .ExportAsync(
                        new DatabaseSmugglerExportOptions
                        {
                            #region smuggler_options
                            OperateOnTypes = DatabaseItemType.Indexes
                                             | DatabaseItemType.Documents
                                             | DatabaseItemType.Counters
                            #endregion
                        },
                        @"C:\ravendb-exports\Northwind.ravendbdump",
                        token);
            }
        }
    }
}
