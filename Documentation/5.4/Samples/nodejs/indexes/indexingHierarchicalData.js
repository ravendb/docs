import {
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    AbstractIndexCreationTask, AbstractCsharpIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

//region class_1
class BlogPost {
    constructor(title, author, text, comments) {
        this.title = title;
        this.author = author;
        this.text = text;

        // Blog post readers can leave comments
        this.comments = comments;
    }
}

class BlogPostComment {
    constructor(author, text, comments) {
        this.author = author;
        this.text = text;

        // Allow nested comments, enabling replies to existing comments
        this.comments = comments;
    }
}
//endregion

//region index_1
class BlogPosts_ByCommentAuthor extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        this.map = `
            docs.BlogPosts.Select(post => new { 
                authors = this.Recurse(post, x => x.comments).Select(x0 => x0.author)
            })`;
    }
}
//endregion

async function sample() {
    const session = store.openSession();
    
    //region index_2
    const indexDefinition = new IndexDefinition();
    
    indexDefinition.name = "BlogPosts/ByCommentAuthor";
    indexDefinition.maps = new Set([
        `from blogpost in docs.BlogPosts
         let authors = Recurse(blogpost, (Func<dynamic, dynamic>)(x => x.comments))
         let authorNames = authors.Select(x => x.author)
         select new
         {
             Authors = authorNames
         }`
    ]);

    await store.maintenance.send(new PutIndexesOperation(indexDefinition));
    //endregion

    //region query_1
    const results = await session
        .query({ indexName: "BlogPosts/ByCommentAuthor" })
         // Query for all blog posts that contain comments by 'Moon':
        .whereEquals("authors", "Moon")
        .all();
    //endregion
}

