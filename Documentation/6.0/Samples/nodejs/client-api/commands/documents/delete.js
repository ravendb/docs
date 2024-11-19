import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function deleteDocumentsCommand() {
    {
        //region delete_document_1
        // Define the Delete Command
        // Pass the document ID & whether to make a concurrency check
        const command = new DeleteDocumentCommand("employees/1-A", null);

        // Send the command to the server using the RequestExecutor
        await documentStore.getRequestExecutor().execute(command);
        //endregion
    }
    {
        //region delete_document_2
        // Load a document
        const employeeDocument = await session.load('employees/2-A');
        const cv = session.advanced.getChangeVectorFor(employeeDocument);
        
        // Modify the document content and save changes
        // The change-vector of the stored document will change
        employeeDocument.Title = "Some new title";
        await session.saveChanges();

        try  {
            // Try to delete the document with the previous change-vector
            const command = new DeleteDocumentCommand("employees/2-A", cv);
            await documentStore.getRequestExecutor().execute(command);
        }
        catch (err) {
            // A concurrency exception is thrown 
            // since the change-vector of the document in the database
            // does not match the change-vector specified in the delete command
            assert.equal(err.name, "ConcurrencyException");
        }
        //endregion
    }
}

{
    //region syntax
    DeleteDocumentCommand(id, changeVector);
    //endregion
}
