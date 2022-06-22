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
                // set search engine type while creating the index
                new Product_ByAvailability(SearchEngineType.Corax).Execute(store);
                #endregion
            }
        }
    }
}
