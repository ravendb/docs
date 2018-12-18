import { 
    DocumentStore,
    AbstractIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

class Product { }

//region indexes_1
class Products_AllProperties extends AbstractIndexCreationTask {

    constructor() {
        super();
        this.map = "docs.Products.Select(product => new { " +
            // convert product to JSON and select all properties from it
            "    Query = this.AsJson(product).Select(x => x.Value) " +
            "})";

        // mark 'query' field as analyzed which enables full text search operations
        this.index("Query", "Search");
    }
}
//endregion

//region indexes_3
class Products_WithMetadata extends AbstractIndexCreationTask {
    constructor() {
        super();
        this.map = "docs.Products.Select(product => new { " +
            "    Product = product, " +
            "    Metadata = this.MetadataFor(product) " +
            "}).Select(this0 => new { " +
            "    LastModified = this0.Metadata.Value<DateTime>(\"Last-Modified\") " +
            "})";
    }
}
//endregion

async function example() {
    
    const session = store.openSession();

    {
        //region indexes_2
        const results = await session
            .query({ indexName: "Products/AllProperties" })
            .whereEquals("Query", "Chocolade")
            .ofType(Product)
            .all();
        //endregion
    }

        //region indexes_4
        const results = await session
            .query({ indexName: "Products/WithMetadata" })
            .orderByDescending("LastModified")
            .ofType(Product)
            .all();
        //endregion
}

