import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

async function examples() {
    {
        //region lazy_1
        const employeeLazy = session
            .advanced
            .lazily
            .load("employees/1-A");

        const employee = await employeeLazy.getValue(); // load operation will be executed here
        //endregion
    }

    {
        //region lazy_2
        const employeesLazy = session
            .query({ collection: "Employees" })
            .lazily();

        await session.advanced.eagerly.executeAllPendingLazyOperations(); // query will be executed here

        const employees = await employeesLazy.getValue();
        //endregion
    }
}
    
    
