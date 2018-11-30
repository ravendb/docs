import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    let fieldName, searchTerms, operator, boost;

    const query = session.query();
    //region search_1
    query.search(fieldName, searchTerms);
    query.search(fieldName, searchTerms, operator);
    query.boost(boost);
    //endregion
}

async function examples() {
    {
        //region search_4
        const users = await session.query({ collection: "Users" })
            .search("name", "a*")
            .all();
        //endregion
    }

    {
        //region search_2
        const users = await session.query({ indexName: "Users/ByNameAndHobbies" })
            .search("name", "Adam")
            .search("hobbies", "sport")
            .all();
        //endregion
    }

    {
        //region search_3
        const users = await session
            .query({ indexName: "Users/ByHobbies" })
            .search("hobbies", "I love sport")
            .boost(10)
            .search("hobbies", "but also like reading books")
            .boost(5)
            .all();
        //endregion
    }
}
