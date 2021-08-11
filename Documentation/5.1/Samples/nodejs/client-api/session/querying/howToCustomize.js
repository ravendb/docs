import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

let action, queryCustomization, seed, waitTimeout;

{
    const query = session.query({ collection: "test" });

    //region customize_1_0
    query.on("beforeQueryExecuted", action);
    //endregion

    //region customize_1_0_0
    query.on("afterQueryExecuted", action);
    //endregion


    //region customize_1_0_1
    query.on("afterStreamExecuted", action);
    //endregion


    //region customize_2_0
    queryCustomization.noCaching();
    //endregion

    //region customize_3_0
    queryCustomization.noTracking();
    //endregion

    //region customize_4_0
    queryCustomization.randomOrdering([seed]);
    //endregion

    //region customize_8_0
    queryCustomization.waitForNonStaleResults([waitTimeout]);
    //endregion
}

class Employee {}

async function customizeExamples() {
    {
        //region customize_1_1
        await session
            .query(Employee)
            .on("beforeQueryExecuted", indexQuery => {
                // set 'pageSize' to 10
                indexQuery.pageSize = 10;  
            })
            .all();
        //endregion
    }

    {
        //region customize_1_1_0
        let queryDuration;

        await session.query(Employee)
            .on("afterQueryExecuted", result => {
                queryDuration = result.durationInMs;
            })
            .all();
        //endregion
    }

    {
        //region customize_1_1_1
        let totalStreamedResultsSize;

        await session.query(Employee)
            .on("afterStreamExecuted", result => {
                totalStreamedResultsSize += result.size;
            });

        //endregion
    }

    {
        //region customize_2_1
        session.advanced.on("beforeQuery",
            event => event.queryCustomization.noCaching());

        const results = await session.query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }

    {
        //region customize_3_1
        session.advanced.on("beforeQuery",
            event => event.queryCustomization.noTracking());

        const results = await session.query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }

    {
        //region customize_4_1
        session.advanced.on("beforeQuery",
            event => event.queryCustomization.randomOrdering());

        // result will be ordered randomly each time
        const results = await session.query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }

    {
        //region customize_8_1
        session.advanced.on("beforeQuery",
            event => event.queryCustomization.waitForNonStaleResults());

        const results = await session.query({ collection: "Employees" })
            .whereEquals("FirstName", "Robert")
            .all();
        //endregion
    }

}
