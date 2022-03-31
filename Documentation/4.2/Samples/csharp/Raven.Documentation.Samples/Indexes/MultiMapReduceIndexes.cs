using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.MapReduce;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;


namespace Raven.Documentation.Samples.Indexes.MultiMap
{
    #region map_reduce_0_0
    public class Products_ByCategory : AbstractIndexCreationTask<Product, Products_ByCategory.Result>
    {
        public class Result
        {
            public string Category { get; set; }

            public int Count { get; set; }
        }

        public Products_ByCategory()
        {
            Map = products => from product in products
                              let categoryName = LoadDocument<Category>(product.Category).Name
                              select new
                              {
                                  Category = categoryName,
                                  Count = 1
                              };

            Reduce = results => from result in results
                                group result by result.Category into g
                                select new
                                {
                                    Category = g.Key,
                                    Count = g.Sum(x => x.Count)
                                };
        }
    }
    #endregion



    public class MultiMapReduceIndex
    {
        #region Multi-Map-Reduce-LINQ
        public class CityCommerceDetails : AbstractMultiMapIndexCreationTask<CityCommerceDetails.IndexEntry>
        {
            public class IndexEntry
            {
                public string CityName { get; set; }
                public int NumberOfCompaniesInCity { get; set; }
                public int NumberOfSuppliersInCity { get; set; }
                public int NumberOfItemsShippedToCity { get; set; }
            }

            public CityCommerceDetails()
            {
                AddMap<Company>(companies => 
                    from company in companies
                    select new IndexEntry
                    {
                        CityName = company.Address.City,
                        NumberOfCompaniesInCity = 1,
                        NumberOfSuppliersInCity = 0,
                        NumberOfItemsShippedToCity = 0
                    });

                AddMap<Supplier>(suppliers => 
                    from supplier in suppliers
                    select new IndexEntry
                    {
                        CityName = supplier.Address.City,
                        NumberOfCompaniesInCity = 0,
                        NumberOfSuppliersInCity = 1,
                        NumberOfItemsShippedToCity = 0
                    });

                AddMap<Order>(orders => 
                    from order in orders
                    select new IndexEntry
                    {
                        CityName = order.ShipTo.City,
                        NumberOfCompaniesInCity = 0,
                        NumberOfSuppliersInCity = 0,
                        NumberOfItemsShippedToCity = order.Lines.Sum(x => x.Quantity)
                    });

                Reduce = results => 
                    from result in results
                    group result by result.CityName into g
                    select new
                    {
                        CityName = g.Key,
                        NumberOfCompaniesInCity = g.Sum(x => x.NumberOfCompaniesInCity),
                        NumberOfSuppliersInCity = g.Sum(x => x.NumberOfSuppliersInCity),
                        NumberOfItemsShippedToCity = g.Sum(x => x.NumberOfItemsShippedToCity)
                    };
            }
        }
        #endregion

        #region multi-map-reduce-index-query
        List<CityCommerceDetails.IndexEntry> commerceDetails;

        using (IDocumentSession session = DocumentStoreHolder.Store.OpenSession())
        {
            commerceDetails = session.Query<CityCommerceDetails.IndexEntry, CityCommerceDetails>()
                .Where(doc => doc.NumberOfCompaniesInCity > minCompaniesCount ||
                              doc.NumberOfItemsShippedToCity > minItemsCount)
                .OrderBy(x => x.CityName)
                .ToList();

}
        #endregion
    }
}
