import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

{
    //region syntax
    session.advanced.attachments.exists(docId, attachmentName);
    //endregion
}

async function sample() {
    //region exists
    const exists = await session
        .advanced
        .attachments
        .exists("categories/1-A", "image.jpg");
    
    if (exists) {
        // attachment 'image.jpg' exists on document 'categories/1-A'
    }
    //endregion
}
