import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function deleteAttachment() {
    {
        //region delete_attachment
        // Define the delete attachment operation
        const deleteAttachmentOp = new DeleteAttachmentOperation("employees/1-A", "photo.jpg");
        
        // Execute the operation by passing it to operations.send
        await documentStore.operations.send(deleteAttachmentOp);
        //endregion
    }
}

//region syntax 
// Available overloads:
const deleteAttachmentOp = new DeleteAttachmentOperation(documentId, name);
const deleteAttachmentOp = new DeleteAttachmentOperation(documentId, name, changeVector);
//endregion
