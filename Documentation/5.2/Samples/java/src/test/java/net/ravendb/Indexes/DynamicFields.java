package net.ravendb.Indexes;

import com.google.common.collect.Sets;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.AbstractJavaScriptIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Dictionary;
import java.util.List;
import java.util.ArrayList;

public class DynamicFields {

        //region dynamic_fields_1
        public class Product_1
        {
            private String id;
            
            // The KEYS under the attributes object will be dynamically indexed 
            // Fields added to this object after index creation time will also get indexed
            private Dictionary<String, Object> attributes;
            
            // get + set implementation ...
        }
        //endregion
        
        //region dynamic_fields_2_JS
        public class Products_ByAttributeKey_JS extends AbstractJavaScriptIndexCreationTask
        {        
            public Products_ByAttributeKey_JS()
            {
               setMaps(Sets.newHashSet(
                   "map('Product_1s', function (p) { " +
                   "    return { " +
                   "        _: Object.keys(p.attributes).map(key => createField(key, p.attributes[key], " +
                   "            { indexing: 'Search', storage: false, termVector: null })) " +
                   "    }; " +
                   "}) "
               ));
            }
        }
        //endregion

        //region dynamic_fields_4
        public class Product_2
        {
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
        public class Products_ByAnyField_JS extends AbstractJavaScriptIndexCreationTask
        {
            public Products_ByAnyField_JS()
            {
               setMaps(Sets.newHashSet(
                   "map('Employees', function (p) { " +
                   "    return { " +
                   "        _: Object.keys(p).map(key => createField(key, p[key], " +
                   "            { indexing: 'Search', storage: true, termVector: null })) " +
                   "    }; " +
                   "}) "
               ));
            }
        }
        //endregion
        
        //region dynamic_fields_7
        public class Product_3
        {
            private String id;
            
            // The VALUE of productType will be dynamically indexed
            private String productType;
            private int pricePerUnit;
            
            // get + set implementation ...
        }
        //endregion
                
        //region dynamic_fields_8 
        public class Products_ByProductType extends AbstractIndexCreationTask
        {
            public Products_ByProductType()
            {
                map = "docs.Product_3s.Select(p => new { " +
                      "    _ = this.CreateField(p.productType, p.pricePerUnit) " +
                      "})";
            }
        }
        //endregion
      
        //region dynamic_fields_10
        public class Product_4
        {
            private String id;
            private String name;
            
            // For each element in this list, the VALUE of property 'propName' will be dynamically indexed
            private ArrayList<Attribute> attributes;
            
            // get + set implementation ...
        }
        
        public class Attribute
        {
            private String propName;
            private String propValue;
            
            // get + set implementation ...
        }
        //endregion
        
        //region dynamic_fields_11
        public class Attributes_ByName extends AbstractIndexCreationTask
        {
            public Attributes_ByName()
            {
                map = "docs.Product_4s.Select(p => new { " +
                      "    _ = p.attributes.Select(item => this.CreateField(item.propName, item.propValue)), " +
                      "    Name = p.name " +
                      "})";
            }
        }
        //endregion

    public DynamicFields() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region dynamic_fields_3
                List<Product_1> matchingDocuments = session
                    .advanced()
                    .documentQuery(Product_1.class, Products_ByAttributeKey_JS.class)
                    .whereEquals("size", 42)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region dynamic_fields_6
                List<Product_2> matchingDocuments = session
                    .advanced()
                    .documentQuery(Product_2.class, Products_ByAnyField_JS.class)
                    .whereEquals("lastName", "Doe")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region dynamic_fields_9
                List<Product_3> matchingDocuments = session
                    .advanced()
                    .documentQuery(Product_3.class, Products_ByProductType.class)
                    .whereEquals("Electronics", 23)
                    .toList();
                //endregion
            }
            try (IDocumentSession session = store.openSession()) {
                   //region dynamic_fields_12
                   List<Product_4> matchingDocuments = session
                       .advanced()
                       .documentQuery(Product_4.class, Attributes_ByName.class)
                       .whereEquals("Width", 10)
                       .toList();
                   //endregion
            }
        }
    }

    //region syntax
    object CreateField(String fieldName, object fieldValue);

    object CreateField(String fieldName, object fieldValue, bool stored, bool analyzed);

    object CreateField(String fieldName, object fieldValue, CreateFieldOptions options);
    //endregion
}
