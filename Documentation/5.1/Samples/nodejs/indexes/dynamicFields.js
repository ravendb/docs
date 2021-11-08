import {
    AbstractIndexCreationTask,
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    IndexDefinitionBuilder, AbstractJavaScriptIndexCreationTask
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region dynamic_fields_1

class Product {
    constructor(id, attributes) {
        this.id = id;
        this.attributes = attributes;
    }
}

class Attribute {
    constructor(name, value) {
        this.name = name;
        this.value = value;
    }
}
//endregion

//region dynamic_fields_2
class Products_ByAttribute extends AbstractIndexCreationTask {

    constructor() {
        super();

        this.map = `docs.Products.Select(p => new {     
            _ = p.attributes.Select(attribute => 
                this.CreateField(attribute.name, attribute.value, false, true)) 
        })`;
    }
}
//endregion

async function example() {
    {
        //region dynamic_fields_4
        const results = await session
            .query({ indexName: "Products/ByAttribute" })
            .whereEquals("color", "red")
            .ofType(Product)
            .all();
        //endregion
    }
}
class CreateFields {
//region syntax
    CreateField(name, value);

    CreateField(name, value, stored, analyzed);

    CreateField(name, value, options);
//endregion
}

//region dynamic_fields_JS_index
class CreateFieldItems_JavaScript  extends AbstractJavaScriptIndexCreationTask {

    constructor() {
        super();

        this.map = `docs.Products.Select(p => new {     
            _ = p.attributes.foreach(x => createField(x.Name, x.Value, { 
                    indexing: 'Exact',
                    storage: true,
                    termVector: null
                }))
        })`;
    }
}
//endregion