import { 
    DocumentStore,
    AbstractIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

class Product { }
class DateTime{}

//region index_1
class Products_WithMetadata extends AbstractIndexCreationTask {
    constructor() {
        super();
        this.map = "docs.Products.Select(product => new {\n" +
            "    Product = product,\n" +
            "    Metadata = this.MetadataFor(product)\n" +
            "}).Select(this_0 => new {\n" +
            "       LastModified = this_0.Metadata.Value<DateTime>(\'Last-Modified'\)\n"+
            "   })";
    }
}
//endregion

async function example() {
    
    const session = store.openSession();

        //region query_1
        const results = await session
            .query({ indexName: "Products/WithMetadata" })
            .orderByDescending("LastModified")
            .ofType(Product)
            .all();
        //endregion
}

