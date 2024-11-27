import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function putDocumentsCommand() {
    {
        //region put_document_1
        // Define the json document to 'put'
        const jsonDocument = {
            name: "My category",
            description: "My category description",
            "@metadata": {
                "@collection": "categories"
            }
        }
        
        // Define the 'PutDocumentCommand'
        // Pass the document ID, whether to make concurrency checks, 
        // and the json document to store
        const command = new PutDocumentCommand("categories/999", null, jsonDocument);

        // Call 'execute' on the Store Request Executor to send the command to the server
        await documentStore.getRequestExecutor().execute(command);

        // Access the command result
        const result = command.result;
        const theDocumentID = result.id;
        const theDocumentCV = result.changeVector;
        
        assert.strictEqual(theDocumentID, "categories/999");
        //endregion
    }
    {
        //region put_document_2
        const session = documentStore.openSession();
        
        // Create a new entity
        const category = new Category();
        category.name = "My category";
        category.description = "My category description";

        // To be able to specify under which collection the document should be stored 
        // you need to convert the entity to a json document first.

        // Passing the entity as is instead of the json document
        // will result in storing the document under the "@empty" collection.

        const documentInfo = new DocumentInfo();
        documentInfo.collection = "categories"; // The target collection
        const jsonDocument = EntityToJson.convertEntityToJson(
            category, documentStore.conventions, documentInfo);

        // Define the 'PutDocumentCommand'
        // Pass the document ID, whether to make concurrency checks,
        // and the json document to store
        const command = new PutDocumentCommand("categories/999", null, jsonDocument);

        // Call 'execute' on the Session Request Executor to send the command to the server
        await session.advanced.requestExecutor.execute(command);

        // Access the command result
        const result = command.result;
        const theDocumentID = result.id;
        const theDocumentCV = result.changeVector;

        assert.strictEqual(theDocumentID, "categories/999");
        //endregion
    }
}

{
    //region syntax_1
    PutDocumentCommand(id, changeVector, document);
    //endregion
    
    //region syntax_2
    // Executing `PutDocumentCommand` returns the following object:
    {
        // The document id under which the entity was stored
        id; // string
        
        // The change vector assigned to the stored document
        changeVector; // string
    }
    //endregion
}
