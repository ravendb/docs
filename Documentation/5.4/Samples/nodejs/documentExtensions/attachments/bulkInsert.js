import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function bulkInsertAttachments() {
    {
        //region bulk_insert_attachment
        // Open a session
        const session = documentStore.openSession();

        // Choose user profiles for which to attach a file
        const users = await session.query({ collection: "users" })
            .whereLessThan("age", 30)
            .all();

        // Prepare content that will be attached
        const text = "Some contents here";
        const byteArray = Buffer.from(text);
        
        // Create a bulkInsert instance
        const bulkInsert = documentStore.bulkInsert();

        try {
            for (let i = 0; i < users.length; i++) {

                // Call `attachmentsFor`, pass the document ID for which to attach the file
                const attachmentsBulkInsert = bulkInsert.attachmentsFor(users[i].id);
                
                // Call 'store' to attach the byte array to the bulkInsert instance
                await attachmentsBulkInsert.store("attachmentName", byteArray);
            }
        } finally {
            // Persist the data - call finish
            await bulkInsert.finish();
        }
        //endregion
    }
}

//region syntax_1
attachmentsFor(id);
//endregion

//region syntax_2
store(name, bytes);
store(name, bytes, contentType);
//endregion

//region user_class
class User {
    constructor(
        id = null,
        age = 0,
        name = ''
    ) {
        Object.assign(this, {
            id,
            age,
            name
        });
    }
}
//endregion
