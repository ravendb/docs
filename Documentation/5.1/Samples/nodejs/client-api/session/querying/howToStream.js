import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

let query, statsCallback, callback;

{
    //region syntax
     await session.advanced.stream(query, [statsCallback]);
    //endregion
}

async function streamingExamples() {
    {
        //region stream_1
        // Define a query on a collection
        const query = session.query({ collection: "employees" })
            .whereEquals('FirstName', 'Robert');

        // Call stream() to execute the query, it returns a Node.js ReadableStream.
        // Parms: pass the query and an optional callback for getting the query stats.
        let streamQueryStats;
        const queryStream = await session.advanced.stream(query, s => streamQueryStats = s);

        // Two options to get query stats:
        // * Pass a callback to stream() with an 'out param' that will be filled with query stats.
        //   This param can then be accessed in the 'end' event.
        // * Or: Use an event listener, listen to the 'stats' event, as described below.

        // Handle stream events with callback functions:        

        // Process the item received:
        queryStream.on("data", resultItem => {
            // Get the employee entity from the result item.
            // Note: This entity will Not be tracked by the session.
            const employee = resultItem.document;

            // The resultItem also provides the following:
            const employeeId = resultItem.id;
            const documentMetadata = resultItem.metadata;
            const documentChangeVector = resultItem.changeVector;
        });
        
        // Can get query stats by using an event listener:
        queryStream.once("stats", queryStats => {
            // Get number of total results
            const totalResults = queryStats.totalResults;
            // Get the Auto-Index that was used/created with this dynamic query
            const indexUsed = queryStats.indexName;
        });

        // Stream emits an 'end' event when there is no more data to read:
        queryStream.on("end", () => {            
            // Get info from 'streamQueryStats', the stats object
            const totalResults = streamQueryStats.totalResults;
            const indexUsed = streamQueryStats.indexName;
        });

        queryStream.on("error", err => {
            // Handle errors
        });
        //endregion
    }
    {
        //region stream_2
        // Define a raw query using RQL
        const rawQuery = session.advanced
            .rawQuery("from Employees where FirstName = 'Robert'");

        // Call stream() to execute the query
        const queryStream = await session.advanced.stream(rawQuery);

        // Handle stats & stream events as described in the dynamic query example above.
        //endregion
    }
    {
        //region stream_3
        // Define a query with projected results
        // Each query result is not an Employee document but an entity containing selected fields only.
        const projectedQuery =session.query({collection: 'employees'})
            .selectFields(['FirstName', 'LastName']);
       
        // Call stream() to execute the query
        const queryStream = await session.advanced.stream(projectedQuery);

        queryStream.on("data", resultItem => {
            // entity contains only the projected fields
            const employeeName = resultItem.document;
        });

        // Handle stats & stream events as described in the dynamic query example above.
        //endregion
    }
    {
        //region stream_4
        // Define a query on an index
        const query = session.query({ indexName: "Employees/ByFirstName" })
            .whereEquals("FirstName", "Robert");

        // Call stream() to execute the query
        const queryStream = await session.advanced.stream(query);

        // Can get info about the index used from the stats
        queryStream.once("stats", queryStats => {
            const indexUsed = queryStats.indexName;
            const isIndexStale = queryStats.stale;
            const lastTimeIndexWasUpdated = queryStats.indexTimestamp;
        });
        
        // Handle stats & stream events as described in the dynamic query example above.
        //endregion
    }
    {
        //region stream_5
        // Define a query with a 'select' clause to project the results.
        
        // The related Company & Employee documents are 'loaded',
        // and returned in the projection together with the Order document itself.
        
        // Each query result is not an Order document
        // but an entity containing the document & the related documents. 
        const rawQuery = session.advanced
            .rawQuery(`from Orders as o
                       where o.ShipTo.City = 'London'
                       load o.Company as c, o.Employee as e
                       select {
                           order: o,
                           company: c,
                           employee: e
                       }`);

        // Call stream() to execute the query
        const queryStream = await session.advanced.stream(rawQuery);

        queryStream.on("data", resultItem => {
            const theOrderDocument = resultItem.document.order;
            const theCompanyDocument = resultItem.document.company;
            const theEmployeeDocument = resultItem.document.employee;
        });
        
        // Handle stats & stream events as described in the dynamic query example above.
        //endregion
    }
}

//region stream_4_index
// The index:
class Employees_ByFirstName extends AbstractJavaScriptIndexCreationTask {

    constructor () {
        super();

        this.map("Employees", employee => {
            return {
                firstName: employee.FirstName
            }
        });
    }
}
//endregion
