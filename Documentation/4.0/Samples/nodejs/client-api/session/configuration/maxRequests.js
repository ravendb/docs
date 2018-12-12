import { DocumentStore } from "ravendb";

const store = new DocumentStore();

{
    const session = store.openSession();
    //region max_requests_1
    session.advanced.maxNumberOfRequestsPerSession = 50;
    //endregion

    //region max_requests_2
    store.conventions.maxNumberOfRequestsPerSession = 100;
    //endregion
}
