import * as assert from "assert";
import { 
    DocumentConventions,
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    AbstractIndexCreationTask
} from "ravendb";

const store = new DocumentStore();

{
    //region multi_map_1
    const indexDefinition = new IndexDefinition();
    indexDefinition.name = "Animals/ByName";
    indexDefinition.maps = new Set([
        "docs.Cats.Select(c => new { name = c.name})",
        "docs.Dogs.Select(c => new { name = c.name})"
    ]);
    //endregion

    async function poly() {
        const session = store.openSession();
        //region multi_map_2
        const results = await session
            .query({ indexName: "Animals/ByName" })
            .whereEquals("name", "Mitzy")
            .all();
        //endregion
    }
}

class Animal { }

class Cat extends Animal { }

class Dog extends Animal { }

async function otherWays() {
    //region other_ways_1
    
    {
        const store = new DocumentStore();
        store.conventions.findCollectionName = clazz => {
            if (clazz instanceof Animal) {
                return "Animals";
            }

            return DocumentConventions.defaultGetCollectionName(clazz);
        };
    }
    //endregion
}


