import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let moreLikeThis, builder, documentJson, filterBuilder, options;

    const query = session.query();
    //region more_like_this_1
    query.moreLikeThis(moreLikeThis);

    query.moreLikeThis(builder);
    //endregion

    query.moreLikeThis(builder => {
        //region more_like_this_3
        builder.usingAnyDocument();

        builder.usingDocument(documentJson);

        builder.usingDocument(filterBuilder);

        builder.withOptions(options);
        //endregion
    });
}

async function sample() {
    {
        //region more_like_this_4
        // Search for similar articles to 'articles/1'
        // using 'Articles/MoreLikeThis' index and search only field 'body'
        const options = { fields: [ "body" ] };

        const articles = await session
            .query({ indexName:  "Articles/MoreLikeThis" })
            .moreLikeThis(builder => builder
                .usingDocument(x => x.whereEquals("id()", "articles/1"))
                .withOptions(options))
            .all();
        //endregion
    }

    {
        //region more_like_this_6
        // Search for similar articles to 'articles/1'
        // using 'Articles/MoreLikeThis' index and search only field 'body'
        // where article category is 'IT'
        const options = { fields: [ "body" ] };
        const articles = await session
            .query({ indexName: "Articles/MoreLikeThis" })
            .moreLikeThis(builder => builder
                .usingDocument(x => x.whereEquals("id()", "articles/1"))
                .withOptions(options))
            .whereEquals("category", "IT")
            .all();
        //endregion
    }
}
