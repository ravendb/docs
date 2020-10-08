using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;


namespace Raven.Documentation.Samples.Indexes
{
    public class DynamicFields
    {
        private interface IFoo
        {
            #region syntax
            object CreateField(string name, object value);

            object CreateField(string name, object value, bool stored, bool analyzed);

            object CreateField(string name, object value, CreateFieldOptions options);
            #endregion
        }

        #region dynamic_fields_1
        public class Product
        {
            public string Id { get; set; }
            public List<Attribute> Attributes { get; set; }
        }

        public class Attribute
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
        #endregion

        #region dynamic_fields_2
        public class Products_ByAttribute : AbstractIndexCreationTask<Product>
        {
            public class Result
            {
                public string Color { get; set; }
                public string Size { get; set; }
            }

            public Products_ByAttribute()
            {
                Map = products => from p in products
                                  select new
                                  {
                                      _ = p.Attributes
                                          .Select(attribute =>
                                              CreateField(attribute.Name, attribute.Value, false, true))
                                  };
            }
        }
        #endregion

        #region dynamic_fields_JS_index
        private class CreateFieldItems_JavaScript : AbstractJavaScriptIndexCreationTask
        {
            public CreateFieldItems_JavaScript()
            {
                Maps = new HashSet<string>
                {
                    @"map('Products', function (p) {
                        return {
                            _: p.Attributes.foreach(x => createField(x.Name, x.Value, { 
                                   indexing: 'Exact',
                                   storage: true,
                                   termVector: null
                               })
                        };
                    })",
                };
            }
        }
        #endregion

        public void Dynamic()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region dynamic_fields_3
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByAttribute>()
                        .WhereEquals("Color", "Red")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region dynamic_fields_4
                    IList<Product> results = session
                        .Query<Products_ByAttribute.Result, Products_ByAttribute>()
                        .Where(x => x.Color == "Red")
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
