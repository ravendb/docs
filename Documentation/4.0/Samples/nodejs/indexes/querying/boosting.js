import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class User {
    constructor(id, name, age, hobbies) {
        this.id = id;
        this.name = name;
        this.age = age;
        this.hobbies = hobbies;
    }
}

async function boosting() {
    {
        //region boosting_1_0
        const users = await session
            .query(User)
            .search("hobbies", "I love sports")
            .boost(10)
            .search("hobbies", "but also like reading books")
            .boost(5)
            .all();
        //endregion
    }

    {
        //region boosting_2_1
        const users = await session
            .query(User)
            .whereStartsWith("name", "G")
            .boost(10)
            .whereStartsWith("name", "A")
            .boost(5)
            .all();
        //endregion
    }
}

