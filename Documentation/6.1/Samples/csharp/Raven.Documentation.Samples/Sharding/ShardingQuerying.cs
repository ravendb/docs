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
                var queryResults = session.Query<User>()
                     // Call 'ShardContext' to select which shard to query
                     // RavenDB will query only the shard containing document "users/1"
                    .Customize(x => x.ShardContext(s => s.ByDocumentId("users/1")))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToList();
                #endregion
            }
            
            // Query selected shard - DocumentQuery
            using (var session = store.OpenSession())
            {
                #region query_selected_shard_2
                var queryResult = session.Advanced.DocumentQuery<User>()
                    // Call 'ShardContext' to select which shard to query
                    .ShardContext(s => s.ByDocumentId("users/1"))
                    // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToList();
                #endregion
            }
            
            // Query selected shard - Query
            using (var session = store.OpenSession())
            {
                #region query_selected_shard_3
                var queryResults = session.Query<User>()
                     // Call 'ShardContext' to select which shards to query
                     // RavenDB will query only the shards containing documents "users/2" & "users/3"
                    .Customize(x => x.ShardContext(s => s.ByDocumentIds(new[] { "users/2", "users/3" })))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToList();
                #endregion
            }

            // Query selected shards - DocumentQuery
            using (var session = store.OpenSession())
            {
                #region query_selected_shard_4
                var queryResult = session.Advanced.DocumentQuery<User>()
                     // Call 'ShardContext' to select which shards to query
                    .ShardContext(s => s.ByDocumentIds(new[] {"users/2", "users/3"}))
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
                var queryResults = await asyncSession.Query<User>()
                     // Call 'ShardContext' to select which shard to query
                    .Customize(x => x.ShardContext(s => s.ByDocumentId("users/1")))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToListAsync();
                #endregion
            }
            
            // Query selected shard - DocumentQuery
            using (var asyncSession = store.OpenAsyncSession())
            {
                #region query_selected_shard_2_async
                var queryResult = await asyncSession.Advanced.AsyncDocumentQuery<User>()
                    // Call 'ShardContext' to select which shard to query
                    .ShardContext(s => s.ByDocumentId("users/1"))
                    // The query predicate
                    .WhereEquals(x => x.Name, "Joe")
                    .ToListAsync();
                #endregion
            }
            
            // Query selected shard - Query
            using (var asyncSession = store.OpenAsyncSession())
            {
                #region query_selected_shard_3_async
                var queryResults = await asyncSession.Query<User>()
                     // Call 'ShardContext' to select which shards to query
                    .Customize(x => x.ShardContext(s => s.ByDocumentIds(new[] { "users/2", "users/3" })))
                     // The query predicate
                    .Where(x => x.Name == "Joe")
                    .ToListAsync();
                #endregion
            }

            // Query selected shards - DocumentQuery
            using (var asyncSession = store.OpenAsyncSession())
            {
                #region query_selected_shard_4_async
                var queryResult = await asyncSession.Advanced.AsyncDocumentQuery<User>()
                     // Call 'ShardContext' to select which shards to query
                    .ShardContext(s => s.ByDocumentIds(new[] {"users/2", "users/3"}))
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
