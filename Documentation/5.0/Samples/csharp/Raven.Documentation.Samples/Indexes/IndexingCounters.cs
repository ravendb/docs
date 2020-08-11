using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Counters;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class IndexingCounters
    {
        private interface IFoo
        {
            #region syntax
            IEnumerable<string> CounterNamesFor(object doc);
            #endregion
        }

        #region index_0
        public class Companies_ByCounterNames : AbstractIndexCreationTask<Company>
        {
            public class Result
            {
                public string[] CounterNames { get; set; }
            }

            public Companies_ByCounterNames()
            {
                Map = employees => from e in employees
                                   let counterNames = CounterNamesFor(e)
                                   select new Result
                                   {
                                       CounterNames = counterNames.ToArray()
                                   };
            }
        }
        #endregion

        #region index_1
        private class MyCounterIndex : AbstractCountersIndexCreationTask<Company>
        {
            public MyCounterIndex()
            {
                AddMap("Likes", counters => from counter in counters
                                                select new
                                                {
                                                    Likes = counter.Value,
                                                    Name = counter.Name,
                                                    User = counter.DocumentId
                                                });
            }
        }
        #endregion

        #region index_2
        private class MyCounterIndex_AllCounters : AbstractCountersIndexCreationTask<Company>
        {
            public MyCounterIndex_AllCounters()
            {
                AddMapForAll(counters => from counter in counters
                                         select new
                                         {
                                             Count = counter.Value,
                                             Name = counter.Name,
                                             User = counter.DocumentId
                                         });
            }
        }
        #endregion

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query1
                    // return all companies that have 'Likes' counter
                    List<Company> companies = session
                        .Query<Companies_ByCounterNames.Result, Companies_ByCounterNames>()
                        .Where(x => x.CounterNames.Contains("Likes"))
                        .OfType<Company>()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query2
                    // return all companies that have 'Likes' counter
                    List<Company> companies = await asyncSession
                        .Query<Companies_ByCounterNames.Result, Companies_ByCounterNames>()
                        .Where(x => x.CounterNames.Contains("Likes"))
                        .OfType<Company>()
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query3
                    // return all companies that have 'Likes' counter
                    List<Company> companies = session
                        .Advanced
                        .DocumentQuery<Company, Companies_ByCounterNames>()
                        .ContainsAny("CounterNames", new[] { "Likes" })
                        .ToList();
                    #endregion
                }
            }
        }

        #region counter_entry
        public class CounterEntry
        {
            public string DocumentId { get; set; }
            public string Name { get; set; }
            public long Value { get; set; }
        }
        #endregion
    }
}
