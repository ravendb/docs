import { DocumentStore, SuggestionWithTerm } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let suggestion, suggestionBuilder, fieldName, term, terms, options;

    const query = session.query();
    //region suggest_1
    query.suggestUsing(suggestion);

    query.suggestUsing(suggestionBuilder);
    //endregion

    query.suggestUsing(suggestionBuilder => {
        //region suggest_2
        suggestionBuilder.byField(fieldName, term);

        suggestionBuilder.byField(fieldName, terms);

        suggestionBuilder.withOptions(options);
        //endregion
    });
}

async function sample() {
    {
        //region suggest_5
        const suggestions = await session
            .query({ indexName: "Employees/ByFullName" })
            .suggestUsing(builder =>
                builder.byField("FullName", "johne")
                    .withOptions({
                        accuracy: 0.4,
                        pageSize: 5,
                        distance: "JaroWinkler",
                        sortMode: "Popularity"
                    }))
            .execute();
        //endregion
    }

    {
        //region suggest_8
        const suggestionWithTerm = new SuggestionWithTerm("fullName");
        suggestionWithTerm.term = "johne";

        const suggestions = await session
            .query({ indexName: "Employees/ByFullName" })
            .suggestUsing(suggestionWithTerm)
            .execute();
        //endregion
    }
}
