import {
    DocumentStore,
    DocumentConventions,
    AbstractJavaScriptMultiMapIndexCreationTask,
    AbstractJavaScriptIndexCreationTask,
    AbstractCsharpIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();

{
    //region class_1
    class Animal {
        constructor(name) {
            this.name = name;
        }
    }

    class Cat extends Animal { }
    //endregion

    //region class_2
    class Animal {
        constructor(name) {
            this.name = name;
        }
    }

    class Dog extends Animal { }
    //endregion
}

//region index_1
class Cats_ByName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        // Index the 'name' field from the CATS collection
        this.map('Cats', cat => {
            return {
                name: cat.name
            };
        });
    }
}
//endregion

//region index_2
class Dogs_ByName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        // Index the 'name' field from the DOGS collection
        this.map('Dogs', dog => {
            return {
                name: dog.name
            };
        });
    }
}
//endregion

//region index_3
class CatsAndDogs_ByName extends AbstractJavaScriptMultiMapIndexCreationTask  {
    constructor() {
        super();

        // Index documents from the CATS collection
        this.map('Cats', cat => {
            return {
                name: cat.name
            };
        });

        // Index documents from the DOGS collection
        this.map('Dogs', dog => {
            return {
                name: dog.name
            };
        });
    }
}
//endregion

//region index_4
class CatsAndDogs_ByName extends AbstractCsharpIndexCreationTask {
    constructor() {
        super();

        // Index documents from both the CATS collection and the DOGS collection
        this.map = `from animal in docs.WhereEntityIs("Cats", "Dogs")
                    select new {
                        animal.name
                    }`;
    }
}
//endregion

//region index_5
class Animals_ByName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        // Index documents from the ANIMALS collection
        this.map('Animals', animal => {
            return {
                name: animal.name
            };
        });
    }
}
//endregion

async function multiMapQueries() {
    const session = documentStore.openSession();

    //region query_1
    const catsAndDogs = await session
        // Query the index
        .query({ indexName: "CatsAndDogs/ByName" })
        // Look for all Cats or Dogs that are named 'Mitzy' :))
        .whereEquals("name", "Mitzy")
        .all();

    // Results will include matching documents from the CATS and DOGS collection
    //endregion

    //region query_2
    const catsAndDogs = await session
        // Query the index
        .query({ indexName: "CatsAndDogs/ByName" })
        // Look for all Cats or Dogs that are named 'Mitzy' :))
        .whereEquals("name", "Mitzy")
        .all();

    // Results will include matching documents from the CATS and DOGS collection
    //endregion

    //region query_3
    const animals = await session
        // Query the index
        .query({ indexName: "Animals/ByName" })
        // Look for all Animals that are named 'Mitzy' :))
        .whereEquals("name", "Mitzy")
        .all();

    // Results will include matching documents from the ANIMALS collection
    //endregion

    //region define_convention
    const documentStore = new DocumentStore(["serverUrl_1", "serverUrl_2", "..."], "DefaultDB");

    // Customize the findCollectionName convention 
    documentStore.conventions.findCollectionName = (type) => {
        const typeName = type.name;

        // Documents created from a 'Cat' or a 'Dog' entity will be assinged the "Animals" collection
        if (typeName === "Cat" || typeName === "Dog") {
            return "Animals";
        }

        // All other documents will be assgined the default collection name
        return DocumentConventions.defaultGetCollectionName(type);
    }
    //endregion
}
