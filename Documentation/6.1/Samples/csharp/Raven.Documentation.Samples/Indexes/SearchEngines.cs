using System;
using System.IO;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.SearchEngine
{
    public class SearchEngine
    {
        private class Product
        {
            public Product(string name, string brand)
            {
                Name = name;
                Brand = brand;
            }

            public string Id { get; set; }
            public string Name { get; set; }
            public string Brand { get; set; }
        }

        #region index-definition_set-search-engine-type
        private class Product_ByAvailability : AbstractIndexCreationTask<Product>
        {
            public Product_ByAvailability(SearchEngineType type)
            {
                // Any Map/Reduce segments here
                Map = products => from p in products
                                  select new
                                  {
                                      p.Name,
                                      p.Brand
                                  };

                // The preferred search engine type
                SearchEngineType = type;
            }
        }
        #endregion

        public void indexAvailableProducts()
        {
            var productId = "Products/1-A";

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    session.Store(new Product("prodName", "prodBrand"), productId);
                    session.SaveChanges();
                }

                #region index-definition_select-while-creating-index
                // Set search engine type while creating the index
                new Product_ByAvailability(SearchEngineType.Corax).Execute(store);
                #endregion
            }
        }

        public void indexOrderByLocation()
        {
            using (var store = new DocumentStore())
            {

                new Order_ByLocation(SearchEngineType.Corax).Execute(store);
            }
        }

        #region index-definition_disable-indexing-for-specified-field
        private class Order_ByLocation : AbstractIndexCreationTask<Order>
        {
            public Order_ByLocation(SearchEngineType type)
            {
                Map = orders => from o in orders
                                select new
                                {
                                    o.ShipTo.Location
                                };

                SearchEngineType = type;

                // Disable Indexing for this field
                Index("Location", FieldIndexing.No);

                // Enable storing the field's contents
                // (this is mandatory if its indexing is disabled)
                Store("Location", FieldStorage.Yes);
            }
        }
        #endregion

        public class Order
        {
            public string Id { get; set; }
            public string Company { get; set; }
            public string Employee { get; set; }
            public DateTime OrderedAt { get; set; }
            public DateTime RequireAt { get; set; }
            public DateTime? ShippedAt { get; set; }
            public Address ShipTo { get; set; }
            public string ShipVia { get; set; }
            public decimal Freight { get; set; }
        }

        public class Address
        {
            public string Id { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public int ZipCode { get; set; }
            public object Location { get; internal set; }
        }


    }
}
