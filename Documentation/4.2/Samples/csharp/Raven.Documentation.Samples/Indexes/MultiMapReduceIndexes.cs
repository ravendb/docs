using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.MapReduce;
using Raven.Documentation.Samples.Orders;
using Raven.Documentation.Samples;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using static Raven.Documentation.Samples.Indexes.MultiMapReduce.MultiMapReduceIndex;

namespace Raven.Documentation.Samples.Indexes.MultiMapReduce
{
    public class MultiMapReduceIndex
    {
        #region multi_map_reduce_LINQ
        public class Cities_Details :
          AbstractMultiMapIndexCreationTask<Cities_Details.IndexEntry>
        {
            public class IndexEntry
            {
                public string City;
                public int Companies, Employees, Suppliers;
            }

            public Cities_Details()
            {
                AddMap<Employee>(emps =>
                    from e in emps
                    select new IndexEntry
                    {
                        City = e.Address.City,
                        Companies = 0,
                        Suppliers = 0,
                        Employees = 1
                    }
                );

                AddMap<Company>(companies =>
                    from c in companies
                    select new IndexEntry
                    {
                        City = c.Address.City,
                        Companies = 1,
                        Suppliers = 0,
                        Employees = 0
                    }
                );

                AddMap<Supplier>(suppliers =>
                    from s in suppliers
                    select new IndexEntry
                    {
                        City = s.Address.City,
                        Companies = 0,
                        Suppliers = 1,
                        Employees = 0
                    }
                );

                Reduce = results =>
                    from result in results
                    group result by result.City
                    into g
                    select new IndexEntry
                    {
                        City = g.Key,
                        Companies = g.Sum(x => x.Companies),
                        Suppliers = g.Sum(x => x.Suppliers),
                        Employees = g.Sum(x => x.Employees)
                    };
            }
        }
    }
        #endregion

    public class MultiMapReduceIndexQuery
    {
        public MultiMapReduceIndexQuery()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region multi-map-reduce-index-query
                    IList<Cities_Details.IndexEntry> commerceDetails = session
                    .Query<Cities_Details.IndexEntry, Cities_Details>()
                    .Where(doc => doc.Companies > 5)
                    .OrderBy(x => x.City)
                    .ToList();
                    #endregion
                }
            }
        }
    }


}
