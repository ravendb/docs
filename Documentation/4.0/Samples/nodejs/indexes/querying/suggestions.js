import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    MoreLikeThisStopWords,
    QueryData
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{

    //region suggestions_1
    class Products_ByName extends AbstractIndexCreationTask {
        constructor() {
            super();

            this.map = `from product in docs.Products select new { 
                product.Name 
            }`;

            this.index("Name", "Search"); // (optional) splitting name into multiple tokens
            this.suggestion("Name"); // configuring suggestions
        }
    }
    //endregion

    class Product { }

    async function suggestions() {
        
            {
                //region suggestions_2
                const product = await session
                    .query({ indexName: "Products/ByName" })
                    .search("Name", "chaig")
                    .firstOrNull();
                //endregion
            }

            {
                //region suggestions_3
                const suggestionResult = await session
                    .query({ indexName: "Products/ByName" })
                    .suggestUsing(builder => builder.byField("name", "chaig"))
                    .execute();

                console.log("Did you mean?");

                for (const suggestion of suggestionResult["name"].suggestions) {
                    console.log("\t" + suggestion);
                }
                //endregion
            }
 

        
            {
                //region query_suggestion_over_multiple_words
                const resultsByMultipleWords = await session
                    .query({ indexName: "Products/ByName" })
                    .suggestUsing(builder =>
                        builder.byField("name", [ "chaig", "tof" ])
                               .withOptions({
                                    accuracy: 0.4,
                                    pageSize: 5,
                                    distance: "JaroWinkler",
                                    sortMode: "Popularity"
                               }))
                    .execute();

                console.log("Did you mean?");

                for (const suggestion of resultsByMultipleWords["name"].suggestions) {
                    console.log("\t" + suggestion);
                }
                //endregion
            }
 
    }
}
