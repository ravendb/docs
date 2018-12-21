import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    MoreLikeThisStopWords,
    QueryData
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region search_20_2
class Users_ByName extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Users.Select(user => new {    
            name = user.name
        })`;

        this.index("name", "Search");
    }
}
//endregion

//region search_21_2
class Users_Search extends AbstractIndexCreationTask {
    constructor() {
        super();

        this.map = `docs.Users.Select(user => new {    
            query = new object[] {        
                user.name,        
                user.hobbies,        
                user.age    
            }}))`;

        this.index("query", "Search");
    }
}
//endregion

//region linq_extensions_search_user_class
class User {
    constructor(id, name, age, hobbies) {
        this.id = id;
        this.name = name;
        this.age = age;
        this.hobbies = hobbies;
    }
}
//endregion

async function searching() {
    
    {
        //region search_3_0
        const users = await session
            .query(User)
            .search("name", "John Adam")
            .all();
        //endregion
    }

    {
        //region search_4_0
        const users = await session
            .query(User)
            .search("hobbies", "looking for someone who likes sport books computers")
            .all();
        //endregion
    }

    {
        //region search_5_0
        const users = await session
            .query(User)
            .search("name", "Adam")
            .search("hobbies", "sport")
            .all();
        //endregion
    }

    {
        //region search_6_0
        const users = await session
            .query(User)
            .search("hobbies", "I love sport")
            .boost(10)
            .search("hobbies", "but also like reading books")
            .boost(5)
            .all();
        //endregion
    }

    {
        //region search_7_0
        const users = await session
            .query(User)
            .search("hobbies", "computers")
            .search("name", "James")
            .whereEquals("age", 20)
            .all();
        //endregion
    }

    {
        //region search_8_0
        const users = await session
            .query(User)
            .search("name", "Adam")
            .andAlso()
            .search("hobbies", "sport")
            .all();
        //endregion
    }

    {
        //region search_9_0
        const users = session
            .query(User)
            .not()
            .search("name", "James")
            .all();
        //endregion
    }

    {
        //region search_10_1
        const users = await session
            .query(User)
            .search("name", "Adam")
            .andAlso()
            .not()
            .search("hobbies", "sport")
            .all();
        //endregion
    }

    {
        //region search_11_0
        const users = await session
            .query(User)
            .search("name", "Jo* Ad*")
            .all();
        //endregion
    }

    {
        //region search_12_0
        const users = await session
            .query(User)
            .search("name", "*oh* *da*")
            .all();
        //endregion
    }

    {
        //region search_20_0
        const users = await session
            .query({ indexName: "Users/ByName" })
            .search("name", "John")
            .all();
        //endregion
    }

    {
        //region search_21_0
        const users = session
            .query({ indexName: "Users/Search" })
            .search("query", "John")
            .all();
        //endregion
    }

}

