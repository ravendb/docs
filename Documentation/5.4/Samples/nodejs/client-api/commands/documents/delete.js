import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function deleteDocumentsCommand() {
    {
        //region delete
        // Define the Delete Command
        // Pass the document ID & whether to make concurrency checks
        const deleteDocCmd = new DeleteDocumentCommand("employees/1-A", null);

        // Send the command to the server using the RequestExecutor
        await documentStore.getRequestExecutor().execute(deleteDocCmd);
        //endregion
    }
}

{
    //region syntax
    DeleteDocumentCommand(id, changeVector);
    //endregion
}
