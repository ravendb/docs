import { 
    AbstractIndexCreationTask, 
    DocumentStore, 
    IndexDefinition, 
    PutIndexesOperation,
    IndexDefinitionBuilder
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region term_vectors_1
class BlogPosts_ByTagsAndContent extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Posts.Select(post => new {     
            tags = post.tags,     
            content = post.content 
        })`; 

        this.index("content", "Search");
        this.termVector("content", "WithPositionsAndOffsets");
    }
}
//endregion

async function termVectors() {
    //region term_vectors_2
    const builder = new IndexDefinitionBuilder("BlogPosts/ByTagsAndContent");
    builder.map = `docs.Posts.Select(post => new {     
        tags = post.tags,     
        content = post.content 
    })`; 

    builder.indexesStrings["content"] = "Search";
    builder.termVectorsStrings["content"] = "WithPositionsAndOffsets";

    const indexDefinition = builder.toIndexDefinition(store.conventions);

    await store.maintenance.send(new PutIndexesOperation(indexDefinition));
    //endregion
}
