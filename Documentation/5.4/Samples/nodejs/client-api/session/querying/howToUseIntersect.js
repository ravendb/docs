import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    const query = session.query();
    //region intersect_1
    query.intersect();
    //endregion
}

async function sample() {
    //region intersect_2
    // return all T-shirts that are manufactured by 'Raven'
    // and contain both 'Small Blue' and 'Large Gray' types
    const tShirts = await session
        .query({ indexName: "TShirts/ByManufacturerColorSizeAndReleaseYear" })
        .whereEquals("manufacturer", "Raven")
        .intersect()
        .whereEquals("color", "Blue")
        .andAlso()
        .whereEquals("size", "Small")
        .intersect()
        .whereEquals("color", "Gray")
        .andAlso()
        .whereEquals("size", "Large")
        .toList();
    //endregion
}
