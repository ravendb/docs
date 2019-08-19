import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

let documentType, indexName, collection, options;

const documentStore = new DocumentStore();
const session = documentStore.openSession();

class Employees_ByName extends AbstractIndexCreationTask {}
class Employee {}

//region query_1_0
session.query(documentType);
session.query(options);
//endregion

async function examples() {
    {
        //region query_1_1
        // load all entities from 'Employees' collection
        const employees = await session.query(Employee)
            .all();

        // or without passing type
        const employees2 = await session.query({ collection: "Employees" })
            .all();

        //endregion
    }

    {
        //region query_1_2
        // load all entities from 'Employees' collection
        // where FirstName equals 'Robert'
        const employees = await session.query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }

    {
        //region query_1_4
        // load all entities from 'Employees' collection
        // where firstName equals 'Robert'
        // using 'Employees/ByName' index
        const employees = await session.query({ indexName: "Employees/ByName" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }

    {
        //region query_1_6
        // load all employees hired between
        // 1/1/2002 and 12/31/2002
        const employees = await session.advanced.documentQuery(Employee)
            .whereBetween("HiredAt",
                new Date("2002-01-01"), new Date("2002-12-31"))
            .all();
        //endregion
    }

    {
        //region query_1_7
        // load all entities from 'Employees' collection
        // where FirstName equals 'Robert
        const employees = await session.advanced
            .rawQuery("from Employees where FirstName = 'Robert'")
            .all();
        //endregion
    }

    //region query_1_8
    class Pet {
        constructor(name) {
            this.name = name;
        }
    }

    class Person {
        constructor(name, pet) {
            this.name = name;
            this.pet = pet;
        }
    }

    documentStore.conventions.registerEntityType(Person);
    documentStore.conventions.registerEntityType(Pet);
    // ...

    documentStore.initialize();
    //endregion
}
