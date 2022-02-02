import {
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    AbstractIndexCreationTask, AbstractCsharpIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

//region indexes_1
class BlogPost {
    constructor(title, author, text, comments) {
        this.title = title;
        this.author = author;
        this.text = text;
        this.comments = comments;
    }
}

class BlogPostComment {
    constructor(author, text, comments) {
        this.author = author;
        this.text = text;
        this.comments = comments;
    }
}
//endregion

//region indexes_2
class BlogPosts_ByCommentAuthor extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();
        this.map = "docs.BlogPosts.Select(post => new {\n" +
            "    authors = this.Recurse(post, x => x.comments).Select(x0 => x0.author)\n" +
            "})";
    }
}
//endregion


async function sample() {
    const session = store.openSession();
    //region indexes_3
    const indexDefinition = new IndexDefinition();
    indexDefinition.name = "BlogPosts/ByCommentAuthor";
    indexDefinition.maps = new Set([
            "from post in docs.Posts" +
            "  from comment in Recurse(post, (Func<dynamic, dynamic>)(x => x.comments)) " +
            "  select new " +
            "  { " +
            "      author = comment.author " +
            "  }"
    ]);

    await store.maintenance.send(new PutIndexesOperation(indexDefinition));
    //endregion

    //region indexes_4
    const results = session
        .query({ indexName: "BlogPosts/ByCommentAuthor" })
        .whereEquals("authors", "Ayende Rahien")
        .all();
    //endregion
}

