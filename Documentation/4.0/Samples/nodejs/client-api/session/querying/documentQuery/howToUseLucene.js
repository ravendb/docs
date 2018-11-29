import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();
const query = session.query();

let fieldName, whereClause, exact;

//region lucene_1
query.whereLucene(fieldName, whereClause, exact);
//endregion

async function lucene_2() {
    //region lucene_2
    const companies = await session.advanced
        .documentQuery({ collection: "Companies" })
        .whereLucene("Name", "bistro")
        .all();
    //endregion
}
