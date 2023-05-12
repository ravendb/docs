using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class Sorting
    {
        #region static_sorting1
        public class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
        {
            public Products_ByUnitsInStock()
            {
                Map = products => from product in products
                                  select new
                                  {
                                      product.UnitsInStock
                                  };
            }
        }
        #endregion

        #region static_sorting2
        public class Products_ByName : AbstractIndexCreationTask<Product>
        {
            public Products_ByName()
            {
                Map = products => from product in products
                                  select new
                                  {
                                      product.Name
                                  };

                Analyzers.Add(x => x.Name, "Raven.Server.Documents.Indexes.Persistence.Lucene.Analyzers.Collation.Cultures.SvCollationAnalyzer, Raven.Server");
            }
        }

        #endregion

        public void QueryWithOrderBy()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region static_sorting3
                IList<Product> results = session
                    .Query<Product>()
                    .OrderBy(product => product.UnitsInStock)
                    .ToList();
                #endregion
            }
        }
    }
}
