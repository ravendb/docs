import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function putAttachment() {
    {
        //region put_attachment
        // Prepare content to attach
        const text = "Some content...";
        const byteArray = Buffer.from(text);
        
        // Define the put attachment operation
        const putAttachmentOp = new PutAttachmentOperation(
            "employees/1-A", "attachmentName.txt", byteArray, "text/plain");
        
        // Execute the operation by passing it to operations.send
        const attachmentDetails = await documentStore.operations.send(putAttachmentOp);
        //endregion
    }
}

//region syntax_1 
// Available overloads:
const putAttachmentOp = new PutAttachmentOperation(documentId, name, stream);
const putAttachmentOp = new PutAttachmentOperation(documentId, name, stream, contentType);
const putAttachmentOp = new PutAttachmentOperation(documentId, name, stream, contentType, changeVector);
//endregion

//region syntax_2
// The AttachmentDetails object:
// =============================
{
    // Change vector of attachment
    changeVector; // string

    // ID of the document that contains the attachment
    documentId?; // string

    // Name of attachment
    name; // string;

    // Hash of attachment
    hash; // string;

    // Content type of attachment
    contentType; // string

    // Size of attachment
    size; // number
}
//endregion
