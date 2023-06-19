import { DocumentStore } from "ravendb";

const store = new DocumentStore();

async function getCount() {

    const session = store.openSession();

    //region getCount
    // Get the number of revisions for document 'companies/1-A"
    const revisionsCount = await session.advanced.revisions.getCountFor("companies/1-A");
    //endregion
}

{
    const session = store.openSession();
    
    //region syntax
    await session.advanced.revisions.getCountFor(id);
    //endregion
}
