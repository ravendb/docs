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
            object CreateField(string fieldName, object fieldValue);

            object CreateField(string fieldName, object fieldValue, bool stored, bool analyzed);

            object CreateField(string fieldName, object fieldValue, CreateFieldOptions options);
            #endregion
        }

        #region dynamic_fields_1
        public class Product_1
        {
            public string Id { get; set; }
            
            // The KEYS under the Attributes object will be dynamically indexed 
            // Fields added to this object after index creation time will also get indexed
            public Dictionary<string, object> Attributes { get; set; }
        }
        #endregion
        
        #region dynamic_fields_2
        public class Products_ByAttributeKey : AbstractIndexCreationTask<Product_1>
        {
            public class IndexEntry
            {
                // The dynamic-index-field declaration
                // Using '_' is just a convention. Any other string can be used instead of '_'
                // The actual field name generated is defined by method CreateField below
                public object _ { get; set; }
            }

            public Products_ByAttributeKey()
            {
                Map = products => from p in products
                    select new Products_ByAttributeKey.IndexEntry
                    {
                        // Define the dynamic-index-field
                        // Call 'CreateField' to generate dynamic-index-fields from the Attributes object keys
                        
                        // The field name will be item.Key
                        // The field terms will be derived from item.Value
                        _ = p.Attributes.Select(item => CreateField(item.Key, item.Value))
                    };
            }
        }
        #endregion

        #region dynamic_fields_2_JS
        public class Products_ByAttributeKey_JS : AbstractJavaScriptIndexCreationTask
        {
            public Products_ByAttributeKey_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Product_1s', function (p) {
                        return {
                            _: Object.keys(p.Attributes).map(key => createField(key, p.Attributes[key],
                                { indexing: 'Search', storage: true, termVector: null }))
                        };
                    })"
                };
            }
        }
        #endregion
        
        #region dynamic_fields_4
        public class Product_2
        {
            public string Id { get; set; }

            // All KEYS in the document will be dynamically indexed 
            // Fields added to the document after index creation time will also get indexed
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Title { get; set; }
            // ...
        }
        #endregion
        
        #region dynamic_fields_5_JS
        public class Products_ByAnyField_JS : AbstractJavaScriptIndexCreationTask
        {
            public class IndexEntry
            {
                public object _ { get; set; }
            }

            public Products_ByAnyField_JS()
            {
                // this will index EVERY FIELD under the top level of the document
                Maps = new HashSet<string>
                {
                    @"map('Product_2s', function (p) {
                          return {
                              _: Object.keys(p).map(key => createField(key, p[key],
                                  { indexing: 'Search', storage: true, termVector: null }))
                          }
                     })"
                };
            }
        }
        #endregion
        
        #region dynamic_fields_7
        public class Product_3
        {
            public string Id { get; set; }
            
            // The VALUE of ProductType will be dynamically indexed
            public string ProductType { get; set; }
            public int PricePerUnit { get; set; }
        }
        #endregion
        
        #region dynamic_fields_8
        public class Products_ByProductType : AbstractIndexCreationTask<Product_3>
        {
            public class IndexEntry
            {
                public object _ { get; set; }
            }

            public Products_ByProductType()
            {
                Map = products => from p in products
                    select new Products_ByProductType.IndexEntry
                    {
                        // Define the dynamic-index-field
                        // Call 'CreateField' to generate dynamic-index-fields
                        
                        // The field name will be the value of document field 'ProductType'
                        // The field terms will be derived from document field 'PricePerUnit'
                        _ = CreateField(p.ProductType, p.PricePerUnit)
                    };
            }
        }
        #endregion
        
        #region dynamic_fields_8_JS
        public class Products_ByName_JS : AbstractJavaScriptIndexCreationTask
        {
            public Products_ByName_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Product_3s', function (p) {
                        return {
                            _: createField(p.ProductType, p.PricePerUnit,
                                { indexing: 'Search', storage: true, termVector: null })
                        };
                    })"
                };
            }
        }
        #endregion

        #region dynamic_fields_10
        public class Product_4
        {
            public string Id { get; set; }
            public string Name { get; set; }
            
            // For each element in this list, the VALUE of property 'PropName' will be dynamically indexed
            public List<Attribute> Attributes { get; set; }
        }

        public class Attribute
        {
            public string PropName { get; set; }
            public string PropValue { get; set; }
        }
        #endregion
        
        #region dynamic_fields_11
        public class Attributes_ByName : AbstractIndexCreationTask<Product_4>
        {
            public class IndexEntry
            {
                public object _ { get; set; }
                public string Name { get; set; }
            }

            public Attributes_ByName()
            {
                Map = products => from a in products
                    select new Attributes_ByName.IndexEntry
                    {
                        // Define the dynamic-index-field by calling CreateField
                        // A dynamic-index-field will be generated for each item in the Attributes list
                        
                        // For each item, the field name will be the value of field 'PropName'
                        // The field terms will be derived from field 'PropValue'
                        _ = a.Attributes.Select(item => CreateField(item.PropName, item.PropValue)),
                        
                        // A regular index field can be defined as well:
                        Name = a.Name
                    };
            }
        }
        #endregion
        
        #region dynamic_fields_11_JS
        public class Attributes_ByName_JS : AbstractJavaScriptIndexCreationTask
        {
            public Attributes_ByName_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Product_4s', function (p) {
                        return {
                            _: p.Attributes.map(item => createField(item.PropName, item.PropValue,
                                { indexing: 'Search', storage: true, termVector: null })),
                           Name: p.Name
                        };
                    })"
                };
            }
        }
        #endregion

        public void QueryDynamicFields()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region dynamic_fields_3
                    IList<Product_1> matchingDocuments = session
                        .Advanced
                        .DocumentQuery<Product_1, Products_ByAttributeKey>()
                         // 'Size' is a dynamic-index-field that was indexed from the Attributes object
                        .WhereEquals("Size", 42)
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region dynamic_fields_6
                    IList<Product_2> matchingDocuments = session
                        .Advanced
                        .DocumentQuery<Product_2, Products_ByAnyField_JS>()
                        // 'LastName' is a dynamic-index-field that was indexed from the document
                        .WhereEquals("LastName", "Doe")
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region dynamic_fields_9
                    IList<Product_3> matchingDocuments = session
                        .Advanced
                        .DocumentQuery<Product_3, Products_ByProductType>()
                         // 'Electronics' is the dynamic-index-field that was indexed from document field 'ProductType'
                        .WhereEquals("Electronics", 23)
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region dynamic_fields_12
                    IList<Product_4> matchingDocuments = session
                        .Advanced
                        .DocumentQuery<Product_4, Attributes_ByName>()
                        // 'Width' is a dynamic-index-field that was indexed from the Attributes list
                        .WhereEquals("Width", 10)
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
