import {
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    IndexDefinitionBuilder, AbstractJavaScriptIndexCreationTask, AbstractCsharpIndexCreationTask
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Syntax {
    //region syntax_JS
    createField(fieldName, fieldValue, options); // returns object
    //endregion
}

// region Example1
{
    //region dynamic_fields_1
    class Product {
        constructor(id, attributes) {
            this.id = id;

            // The KEYS under the attributes object will be dynamically indexed 
            // Fields added to this object after index creation time will also get indexed
            this.attributes = attributes;
        }
    }
    //endregion

    //region dynamic_fields_2_JS
    class Products_ByAttributeKey_JS extends AbstractJavaScriptIndexCreationTask {
        constructor() {
            super();

            const { createField } = this.mapUtils();

            this.map("Products", p => {
                return {
                    // Call 'createField' to generate dynamic-index-fields from the attributes object keys
                    // Using '_' is just a convention. Any other string can be used instead of '_'

                    // The actual field name will be the key
                    // The actual field terms will be derived from p.attributes[key]
                    _: Object.keys(p.attributes).map(key => createField(key, p.attributes[key], {
                        indexing: "Search",
                        storage: false,
                        termVector: null
                    }))
                };
            });
        }
    }
    //endregion

    function QueryDynamicFields() {
        //region dynamic_fields_3
        const matchingDocuments = session.query({indexName: 'Products_ByAttributeKey'})
             // 'size' is a dynamic-index-field that was indexed from the attributes object
            .whereEquals('size', 42)
            .all();
        //endregion
    }
}
//endregion

// region Example2
{
    //region dynamic_fields_4
    class Product {
        constructor(id, firstName, lastName, title) {
            this.id = id;

            // All KEYS in the document will be dynamically indexed 
            // Fields added to the document after index creation time will also get indexed
            this.firstName = firstName;
            this.lastName = lastName;
            this.title = title;
            // ...
        }
    }
    //endregion

    //region dynamic_fields_5_JS
    class Products_ByAnyField_JS extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            const { createField } = this.mapUtils();

            this.map("Products", p => {
                return {
                    // This will index EVERY FIELD under the top level of the document
                    _: Object.keys(p).map(key => createField(key, p[key], {
                        indexing: "Search",
                        storage: true,
                        termVector: null
                    }))
                };
            });
        }
    }
    //endregion

    function QueryDynamicFields() {
        //region dynamic_fields_6
        const matchingDocuments = session.query({ indexName: 'Products_ByAnyField_JS' })
             // 'lastName' is a dynamic-index-field that was indexed from the document
            .whereEquals('lastName', 'Doe')
            .all();
        //endregion
    }
}
//endregion

// region Example3
{
    //region dynamic_fields_7
    class Product {
        constructor(id, productType, pricePerUnit) {
            this.id = id;

            // The VALUE of productType will be dynamically indexed
            this.productType = productType;
            this.pricePerUnit = pricePerUnit;
        }
    }
    //endregion

    //region dynamic_fields_8
    class Products_ByProductType extends AbstractCsharpIndexCreationTask {
        constructor () {
            super();

            // The field name will be the value of document field 'productType'
            // The field terms will be derived from document field 'pricePerUnit'
            this.map = "docs.Products.Select(p => new { " +
                "    _ = this.CreateField(p.productType, p.pricePerUnit) " +
                "})";
        }
    }
    //endregion

    //region dynamic_fields_8_JS
    class Products_ByProductType_JS extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            const { createField } = this.mapUtils();

            this.map("Products", p => {
                return {
                    _: [
                        // The field name will be the value of document field 'productType'
                        // The field terms will be derived from document field 'pricePerUnit'
                        createField(p.productType, p.pricePerUnit, {
                            indexing: "Search",
                            storage: false,
                            termVector: null
                        })
                    ]
                };
            });
        }
    }
    //endregion
    
    function QueryDynamicFields() {
        //region dynamic_fields_9
        const matchingDocuments = session.query({ indexName: 'Products_ByProductType' })
             // 'Electronics' is the dynamic-index-field that was indexed from document field 'productType'
            .whereEquals('Electronics', 23)
            .all();
        //endregion
    }
}
//endregion

// region Example4
{
    //region dynamic_fields_10
    class Product {
        constructor(id, name, attributes) {
            this.id = id;
            this.name = name;

            // For each element in this list, the VALUE of property 'propName' will be dynamically indexed
            // e.g. Color, Width, Length (in ex. below) will become dynamic-index-fields
            this.attributes = attributes;
        }
    }

    class Attribute {
        constructor(propName, propValue) {
            this.propName = propName;
            this.propValue = propValue;
        }
    }
    //endregion

    //region dynamic_fields_11
    class Attributes_ByName extends AbstractCsharpIndexCreationTask
    {
        constructor () {
            super();

            // For each attribute item, the field name will be the value of field 'propName'
            // The field terms will be derived from field 'propValue'
            // A regular-index-field (Name) is defined as well
            this.map =
                "docs.Products.Select(p => new { " +
                "    _ = p.attributes.Select(item => this.CreateField(item.propName, item.propValue)), " +
                "    Name = p.name " +
                "})";
        }
    }
    //endregion

    //region dynamic_fields_11_JS
    class Attributes_ByName_JS extends AbstractJavaScriptIndexCreationTask {
        constructor () {
            super();

            const { createField } = this.mapUtils();

            this.map("Products", p => {
                return {
                    // For each item, the field name will be the value of field 'propName'
                    // The field terms will be derived from field 'propValue'
                    _: p.attributes.map(item => createField(item.propName, item.propValue, {
                        indexing: "Search",
                        storage: true,
                        termVector: null
                    })),

                    // A regular-index-field can be defined as well:
                    Name: p.name
                };
            });
        }
    }
    //endregion
    
    function QueryDynamicFields() {
        //region dynamic_fields_12
        const matchingDocuments = session.query({ indexName: 'Attributes/ByName' })
             // 'Width' is a dynamic-index-field that was indexed from the attributes list
            .whereEquals('Width', 10)
            .all();
        //endregion
    }
}
//endregion
