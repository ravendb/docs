import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

let query, statsCallback, callback;

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    //region stream_1
    session.advanced.stream(query);
    session.advanced.stream(query, statsCallback);
    session.advanced.stream(query, statsCallback, callback);
    //endregion
}

async function examples() {
    {
        //region stream_2
        const query = session
            .query({ indexName: "Employees/ByFirstName" })
            .whereEquals("FirstName", "Robert");

        // stream() returns Node.js Readable 
        const stream = await session.advanced.stream(query);

        stream.on("data", data => {
            // Employee { FirstName: "Robert", LastName: "Smith" id: "employees/1-A" }
        });

        stream.on("error", err => {
            // handle errors
        });

        stream.on("end", () => {
            // stream ended
        });
        //endregion
    }

    {
        //region stream_3
        const query = session.advanced
            .documentQuery(Employee)
            .whereEquals("FirstName", "Robert");

        let streamStats;
        const stream = await session.advanced
            .stream(query, stats => streamStats = stats);

        stream.on("data", data => {
            // Employee { FirstName: "Robert", LastName: "Smith" id: "employees/1-A" }
        });

        stream.on("error", err => {
            // handle errors
        });

        stream.on("end", () => {
            // stream ended
        });
        //endregion
    }

    {
        //region stream_4
        const rawQuery = session.advanced
            .rawQuery("from Employees where FirstName = 'Robert'");

        const stream = session.advanced.stream(rawQuery);

        stream.on("data", data => {
            // Employee { FirstName: "Robert", LastName: "Smith" id: "employees/1-A" }
        });

        stream.on("error", err => {
            // handle errors
        });

        stream.on("end", () => {
            // stream ended
        });

        //endregion
    }
}

class Employee {}

class Employees_ByFirstName extends AbstractIndexCreationTask {
    constructor() {
        super();
        this.map = `from employee in docs.Employees 
                select new { 
                   employee.FirstName 
                }`;
    }
}
