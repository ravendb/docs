import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

//region lucene_1
query.whereLucene(fieldName, whereClause, exact);
//endregion

//region lucene_2
const companies = await session.advanced
    .documentQuery({ collection: "Companies" })
    .whereLucene("name", "bistro")
    .all();
//endregion
