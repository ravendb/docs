import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    MoreLikeThisStopWords
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region more_like_this_4
class Article {
    constructor(id, name, articleBody) {
        this.id = id;
        this.name = name;
        this.articleBody = articleBody;
    }
}

class Articles_ByArticleBody extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `from doc in docs.Articles select new { 
            doc.articleBody 
        }`;

        this.store("articleBody", "Yes");
        this.analyze("articleBody", "StandardAnalyzer");
    }
}
//endregion

async function moreLikeThis() {
    {
        //region more_like_this_1
        const articles = await session
            .query({ indexName: "Articles/ByArticleBody" })
            .moreLikeThis(builder => 
                builder.usingDocument(x => 
                    x.whereEquals("id()", "articles/1")))
            .all();
        //endregion
    }

    {
        //region more_like_this_2
        const options = {
            fields: [ "articleBody" ]
        };
        const articles = await session
            .query({ indexName: "Articles/ByArticleBody" })
            .moreLikeThis(builder => builder
                .usingDocument(x => x.whereEquals("id()", "articles/1"))
                .withOptions(options))
            .all();
        //endregion
    }

    {
        //region more_like_this_3
        const stopWords = new MoreLikeThisStopWords();
        stopWords.stopWords = [ "I", "A", "Be" ];
        await session.store(stopWords, "Config/Stopwords");
        //endregion
    }
}

