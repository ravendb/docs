import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function examples() {
    {
        //region regex_1
        // loads all products, which name
        // starts with 'N' or 'A'
        const products = await session.query({ collection: "Products" })
            .whereRegex("name", "^[NA]")
            .all();
        //endregion
    }
}

