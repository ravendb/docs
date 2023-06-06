using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Sharding.ShardingQuerying
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

            // Query selected shard
            using (var session = store.OpenSession())
            {
                #region query-selected-shard
                var queryResult = session.Advanced.DocumentQuery<User>()
                        // Which shard to query
                        .ShardContext(s => s.ByDocumentId("users/1"))
                        // The query
                        .SelectFields<string>("Occupation").ToList();
                #endregion
            }

            // Query selected shards
            using (var session = store.OpenSession())
            {
                #region query-selected-shards
                var queryResult = session.Advanced.DocumentQuery<User>()
                        // Which shards to query
                        .ShardContext(s => s.ByDocumentIds(new[] { "users/2", "users/3" }))
                        // The query
                        .SelectFields<string>("Occupation").ToList();
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
