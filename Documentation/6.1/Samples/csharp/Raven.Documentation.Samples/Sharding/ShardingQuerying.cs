using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Sharding
{
    public class ShardingQuerying
    {
        public ShardingQuerying()
        {
            using var store = new DocumentStore();

            // OrderBy with no limit
            using (var session = store.OpenSession())
            {
                #region OrderBy_with-no-limit
                var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                        .OrderBy(x => x.Name)
                        .ToList();
                #endregion
            }

            // OrderBy with limit
            using (var session = store.OpenSession())
            {
                #region OrderBy_with-limit
                var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                        .OrderBy(x => x.Name)
                        .Take(3) // this limit will apply while retrieving the items
                        .ToList();
                #endregion
            }

            // Compute sum by retrieved results
            using (var session = store.OpenSession())
            {
                #region compute-sum-by-retrieved-results
                var queryResult = session.Query<UserMapReduce.Result, UserMapReduce>()
                        .OrderBy(x => x.Sum)
                        .Take(3) // this limit will only apply after retrieving all items
                        .ToList();
                #endregion
            }

            using (var session = store.OpenSession())
            {
                #region Query_basic-paging
                IList<Product> results = session
                    .Query<Product, Products_ByUnitsInStock>()
                    .Statistics(out QueryStatistics stats) // fill query statistics
                    .Where(x => x.UnitsInStock > 10)
                    .Skip(700) // skip the first 7 pages (700 results)
                    .Take(100) // get pages 701-800
                    .ToList();

                long totalResults = stats.TotalResults;
                #endregion
            }

            using (var session = store.OpenSession())
            {
                #region DocumentQuery_basic-paging
                IList<Product> results = session
                    .Advanced
                    .DocumentQuery<Product, Products_ByUnitsInStock>()
                    .Statistics(out QueryStatistics stats) // fill query statistics
                    .WhereGreaterThan(x => x.UnitsInStock, 10)
                    .Skip(700) // skip the first 7 pages (700 results)
                    .Take(100) // get pages 701-800
                    .ToList();

                long totalResults = stats.TotalResults;
                #endregion
            }
        }

        public void QuerySelectedShards()
        {
            using var store = new DocumentStore();
            
            // Query selected shard - Query
            using (var session = store.OpenSession())
            {
                #region query_selected_shard_1
                // Query for 'User' documents from a specific shard:
                // =================================================
                var userDocuments = session.Query<User>()
                     // Call 'ShardContext' to select which shard to query
                     // RavenDB will query only the shard containing document "companies/1"
                    .Customize(x => x.ShardContext(s => s.ByDocumentId("companies/1")))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToList();

                // Variable 'userDocuments' will include all documents of type 'User'
                // that match the query predicate and reside on the shard containing document 'companies/1'.
                
                // Query for ALL documents from a specific shard:
                // ==============================================
                var allDocuments = session.Query<object>() // query with <object>
                    .Customize(x => x.ShardContext(s => s.ByDocumentId("companies/1")))
                    .ToList();
                
                // Variable 'allDocuments' will include ALL documents
                // that reside on the shard containing document 'companies/1'.
                #endregion
            }
            
            // Query selected shard - DocumentQuery
            using (var session = store.OpenSession())
            {
                #region query_selected_shard_2
                // Query for 'User' documents from a specific shard:
                // =================================================
                var userDocuments = session.Advanced.DocumentQuery<User>()
                    // Call 'ShardContext' to select which shard to query
                    .ShardContext(s => s.ByDocumentId("companies/1"))
                    // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToList();
                
                // Query for ALL documents from a specific shard:
                // ==============================================
                var allDocuments = session.Advanced.DocumentQuery<object>()
                    .ShardContext(s => s.ByDocumentId("companies/1"))
                    .ToList();
                #endregion
            }
            
            // Query selected shard - Query
            using (var session = store.OpenSession())
            {
                #region query_selected_shard_3
                // Query for 'User' documents from the specified shards:
                // =====================================================
                var userDocuments = session.Query<User>()
                     // Call 'ShardContext' to select which shards to query
                     // RavenDB will query only the shards containing documents "companies/2" & "companies/3"
                    .Customize(x => x.ShardContext(s => s.ByDocumentIds(new[] { "companies/2", "companies/3" })))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToList();

                // Variable 'userDocuments' will include all documents of type 'User' that match the query predicate
                // and reside on either the shard containing document 'companies/2'
                // or the shard containing document 'companies/3'.

                // To get ALL documents from the designated shards instead of just 'User' documents,
                // query with `session.Query<object>`. 
                #endregion
            }

            // Query selected shards - DocumentQuery
            using (var session = store.OpenSession())
            {
                #region query_selected_shard_4
                // Query for 'User' documents from the specified shards:
                // =====================================================
                var userDocuments = session.Advanced.DocumentQuery<User>()
                     // Call 'ShardContext' to select which shards to query
                    .ShardContext(s => s.ByDocumentIds(new[] {"companies/2", "companies/3"}))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToList();
                #endregion
            }
        }
        
        public async Task QuerySelectedShardsAsync()
        {
            using var store = new DocumentStore();
            
            // Query selected shard - Query
            using (var asyncSession = store.OpenAsyncSession())
            {
                #region query_selected_shard_1_async
                // Query for 'User' documents from a specific shard:
                // =================================================
                var userDocuments = await asyncSession.Query<User>()
                     // Call 'ShardContext' to select which shard to query
                    .Customize(x => x.ShardContext(s => s.ByDocumentId("companies/1")))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToListAsync();
                
                // Query for ALL documents from a specific shard:
                // ==============================================
                var allDocuments = await asyncSession.Query<object>()
                    .Customize(x => x.ShardContext(s => s.ByDocumentId("companies/1")))
                    .ToListAsync();
                #endregion
            }
            
            // Query selected shard - DocumentQuery
            using (var asyncSession = store.OpenAsyncSession())
            {
                #region query_selected_shard_2_async
                // Query for 'User' documents from a specific shard:
                // =================================================
                var userDocuments = await asyncSession.Advanced.AsyncDocumentQuery<User>()
                    // Call 'ShardContext' to select which shard to query
                    .ShardContext(s => s.ByDocumentId("companies/1"))
                    // The query predicate
                    .WhereEquals(x => x.Name, "Joe")
                    .ToListAsync();
                
                // Query for ALL documents from a specific shard:
                // ==============================================
                var allDocuments = await asyncSession.Advanced.AsyncDocumentQuery<object>()
                    .ShardContext(s => s.ByDocumentId("companies/1"))
                    .ToListAsync();
                #endregion
            }
            
            // Query selected shard - Query
            using (var asyncSession = store.OpenAsyncSession())
            {
                #region query_selected_shard_3_async
                // Query for 'User' documents from the specified shards:
                // =====================================================
                var userDocuments = await asyncSession.Query<User>()
                     // Call 'ShardContext' to select which shards to query
                    .Customize(x => x.ShardContext(s => s.ByDocumentIds(new[] { "companies/2", "companies/3" })))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToListAsync();
                #endregion
            }

            // Query selected shards - DocumentQuery
            using (var asyncSession = store.OpenAsyncSession())
            {
                #region query_selected_shard_4_async
                // Query for 'User' documents from the specified shards:
                // =====================================================
                var userDocuments = await asyncSession.Advanced.AsyncDocumentQuery<User>()
                     // Call 'ShardContext' to select which shards to query
                    .ShardContext(s => s.ByDocumentIds(new[] {"companies/2", "companies/3"}))
                     // The query predicate
                    .WhereEquals(x => x.Name, "Joe")
                    .ToListAsync();
                #endregion
            }
        }
        
        #region index-for-paging-sample
        public class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
        {
            public Products_ByUnitsInStock()
            {
                Map = products => from product in products
                                  select new
                                  {
                                      UnitsInStock = product.UnitsInStock
                                  };
            }
        }
        #endregion

        private class UserMapReduce : AbstractIndexCreationTask<User, UserMapReduce.Result>
        {
            public class Result
            {
                public string Name;
                public int Sum;
            }

            public UserMapReduce()
            {
                Map = users =>
                    from user in users
                    select new Result
                    {
                        Name = user.Name,
                        Sum = user.Count
                    };

                #region map-reduce-index
                Reduce = results =>
                    from result in results
                    group result by result.Name
                    into g
                    select new Result
                    {
                        // Group-by field (reduce key)
                        Name = g.Key,
                        // Computation field
                        Sum = g.Sum(x => x.Sum)
                    };
                #endregion
            }
        }

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string AddressId { get; set; }
            public int Count { get; set; }
            public int Age { get; set; }
        }
    }
}
