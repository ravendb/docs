import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

class Syntax {
    //region whereExists_syntax
    whereExists(fieldName);
    //endregion
}

async function whereExists() {

    //region whereExists_1
    // Only documents that contain field 'firstName' will be returned
    
    await session.query({ collection: "Employees" })
        .whereExists("firstName");
        .all();
    //endregion

    //region whereExists_2
    // Only documents that contain the 'latitude' property in the specified path will be returned
    
    await session.query({ collection: "Employees" })
        .whereExists("address.location.latitude");
        .all();
    //endregion
}
