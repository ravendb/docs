import { 
    AbstractIndexCreationTask, 
    DocumentStore, 
    IndexDefinition, 
    PutIndexesOperation,
    IndexDefinitionBuilder
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class SnowballAnalyzer { }

//region analyzers_1
class BlogPosts_ByTagsAndContent extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Posts.Select(post => new {     
            tags = post.tags,     
            content = post.content 
        })`;

        this.analyze("tags", "SimpleAnalyzer");
        this.analyze("content", "Raven.Sample.SnowballAnalyzer");
    }
}
//endregion

//region analyzers_3
class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Employees.Select(employee => new { " +
            "    LastName = employee.LastName, " +
            "    FirstName = employee.FirstName " +
            "})";

        this.index("FirstName", "Exact");
    }
}
//endregion

//region analyzers_4
class BlogPosts_ByContent extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Posts.Select(post => new { " +
            "    tags = post.tags, " +
            "    content = post.content " +
            "})";

        this.index("content", "Search");
    }
}
//endregion

//region analyzers_5
class BlogPosts_ByTitle extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = "docs.Posts.Select(post => new { " +
            "    tags = post.tags, " +
            "    content = post.content " +
            "})";

        this.index("content", "No");
        this.store("content", "Yes");
    }
}
//endregion

async function analyzers() {
    const session = store.openSession();

    //region analyzers_2
    const builder = new IndexDefinitionBuilder("BlogPosts/ByTagsAndContent");
    builder.map = `docs.Posts.Select(post => new {     
        tags = post.tags,     
        content = post.content 
    })`;
    builder.analyzersStrings["tags"] = "SimpleAnalyzer";
    builder.analyzersStrings["content"] = "Raven.Sample.SnowballAnalyzer";

    await store.maintenance
        .send(new PutIndexesOperation(
            builder.toIndexDefinition(store.conventions)));
    //endregion

}

