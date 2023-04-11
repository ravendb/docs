import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Employees_ByFirstName extends AbstractIndexCreationTask { }

class Employee { }

class Product { }

async function queryIndex() {
    const session = store.openSession();

    {
        //region basics_0_0
        // load all entities from 'Employees' collection
        const results = await session
            .query(Employee)
            .all(); // send query
        //endregion
    }

    {
        //region basics_0_1
        // load all entities from 'Employees' collection
        // where 'FirstName' is 'Robert'
        const results = await session
            .query(Employee)
            .whereEquals("FirstName", "Robert")
            .all(); // send query
        //endregion
    }

    {
        //region basics_0_2
        // load up to 10 entities from 'Products' collection
        // where there are more than 10 units in stock
        // skip first 5 results
        const results = await session
            .query(Product)
            .whereGreaterThan("UnitsInStock", 10)
            .skip(5)
            .take(10)
            .all(); //send query
        //endregion
    }

    {
        //region basics_0_3
        // load all entities from 'Employees' collection
        // where 'FirstName' is 'Robert'
        // using 'Employees/ByFirstName' index
        const results = await session
            .query({ indexName: "Employees/ByFirstName" })
            .whereEquals("FirstName", "Robert")
            .all(); // send query
        //endregion
    }

    {
        //region basics_0_4
        // load all entities from 'Employees' collection
        // where 'FirstName' is 'Robert'
        // using 'Employees/ByFirstName' index
        const results = await session
            .query({ indexName: "Employees/ByFirstName" })
            .whereEquals("FirstName", "Robert")
            .all(); // send query
        //endregion
    }

    {
        //region basics_3_0
        // load up entity from 'Employees' collection
        // with ID matching 'employees/1-A'
        const result = await session
            .query(Employee)
            .whereEquals("Id", "employees/1-A")
            .firstOrNull();
        //endregion
    }

}

