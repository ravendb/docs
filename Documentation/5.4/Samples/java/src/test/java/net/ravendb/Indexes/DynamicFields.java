package net.ravendb.Indexes;

import com.google.common.collect.Sets;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.AbstractJavaScriptIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Dictionary;
import java.util.List;

public class DynamicFields {

    public class Example1 {

        //region dynamic_fields_1
        public class Product {
            private String id;

            // The KEYS under the attributes object will be dynamically indexed
            // Fields added to this object after index creation time will also get indexed
            private Dictionary<String, Object> attributes;

            // get + set implementation ...
        }
        //endregion

        //region dynamic_fields_2_JS
        public class Products_ByAttributeKey_JS extends AbstractJavaScriptIndexCreationTask {
            public Products_ByAttributeKey_JS() {
                // Call 'createField' to generate dynamic-index-fields from the attributes object keys
                // Using '_' is just a convention. Any other string can be used instead of '_'

                // The actual field name will be the key
                // The actual field terms will be derived from p.attributes[key]
                setMaps(Sets.newHashSet(
                    "map('Products', function (p) { " +
                    "    return { " +
                    "        _: Object.keys(p.attributes).map(key => createField(key, p.attributes[key], " +
                    "            { indexing: 'Search', storage: false, termVector: null })) " +
                    "    }; " +
                    "}) "
                ));
            }
        }
        //endregion

        public Example1() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region dynamic_fields_3
                    List<Product> matchingDocuments = session
                        .query(Product.class, Products_ByAttributeKey_JS.class)
                        .whereEquals("size", 42)
                        .toList();
                    //endregion
                }
            }
        }
    }

    public class Example2 {

        //region dynamic_fields_4
        public class Product {
            private String id;

            // All KEYS in the document will be dynamically indexed
            // Fields added to the document after index creation time will also get indexed
            private String firstName;
            private String lastName;
            private String title;
            // ...

            // get + set implementation ...
        }
        //endregion

        //region dynamic_fields_5_JS
        public class Products_ByAnyField_JS extends AbstractJavaScriptIndexCreationTask {
            public Products_ByAnyField_JS() {
            
                // This will index EVERY FIELD under the top level of the document
                setMaps(Sets.newHashSet(
                    "map('Products', function (p) { " +
                    "    return { " +
                    "        _: Object.keys(p).map(key => createField(key, p[key], " +
                    "            { indexing: 'Search', storage: true, termVector: null })) " +
                    "    }; " +
                    "}) "
                ));
            }
        }
        //endregion

        public Example2() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region dynamic_fields_6
                    List<Product> matchingDocuments = session
                        .query(Product.class, Products_ByAnyField_JS.class)
                        .whereEquals("lastName", "Doe")
                        .toList();
                    //endregion
                }
            }
        }
    }

    public class Example3 {

        //region dynamic_fields_7
        public class Product {
            private String id;

            // The VALUE of productType will be dynamically indexed
            private String productType;
            private int pricePerUnit;

            // get + set implementation ...
        }
        //endregion

        //region dynamic_fields_8
        public class Products_ByProductType extends AbstractIndexCreationTask {
            public Products_ByProductType() {
            
                // The field name will be the value of document field 'productType'
                // The field terms will be derived from document field 'pricePerUnit'
                map = "docs.Products.Select(p => new { " +
                      "    _ = this.CreateField(p.productType, p.pricePerUnit) " +
                      "})";
            }
        }
        //endregion

        public Example3() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region dynamic_fields_9
                    List<Product> matchingDocuments = session
                        .query(Product.class, Products_ByProductType.class)
                        .whereEquals("Electronics", 23)
                        .toList();
                    //endregion
                }
            }
        }
    }

    public class Example4 {

        //region dynamic_fields_10
        public class Product {
            private String id;
            private String name;

            // For each element in this list, the VALUE of property 'propName' will be dynamically indexed
            // e.g. Color, Width, Length (in ex. below) will become dynamic-index-fields
            private List<Attribute> attributes;

            // get + set implementation ...
        }

        public class Attribute {
            private String propName;
            private String propValue;

            // get + set implementation ...
        }
        //endregion

        //region dynamic_fields_11
        public class Attributes_ByName extends AbstractIndexCreationTask {
            public Attributes_ByName() {

                // For each attribute item, the field name will be the value of field 'propName'
                // The field terms will be derived from field 'propValue'
                // A regular-index-field (Name) is defined as well
                map =
                    "docs.Products.Select(p => new { " +
                    "    _ = p.attributes.Select(item => this.CreateField(item.propName, item.propValue)), " +
                    "    Name = p.name " +
                    "})";
            }
        }
        //endregion

        public Example4() {
            try (IDocumentStore store = new DocumentStore()) {
                try (IDocumentSession session = store.openSession()) {
                    //region dynamic_fields_12
                    List<Product> matchingDocuments = session
                        .query(Product.class, Attributes_ByName.class)
                        .whereEquals("Width", 10)
                        .toList();
                    //endregion
                }
            }
        }
    }
}
