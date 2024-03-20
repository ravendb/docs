import { DocumentStore } from "ravendb";
import fs from "fs";

const documentStore = new DocumentStore();

async function getAttachment() {
    {
        //region get_attachment
        // Define the get attachment operation
        const getAttachmentOp = new GetAttachmentOperation("employees/1-A", "attachmentName.txt", "Document", null);

        // Execute the operation by passing it to operations.send
        const attachmentResult = await documentStore.operations.send(getAttachmentOp);
        
        // Retrieve attachment content:
        attachmentResult.data
            .pipe(fs.createWriteStream("attachment"))
            .on("finish", () => {
                fs.readFile("attachment", "utf8", (err, data) => {
                    if (err) {
                        console.error("Error reading file:", err);
                        return;
                    }
                    console.log("Content of attachment:", data);
                    next();
                });
            });
        //endregion
    }
}

//region syntax_1 
const getAttachmentOp = new GetAttachmentOperation(documentId, name, type, changeVector);
//endregion

//region syntax_2
class AttachmentResult {
    data;    // Stream containing the attachment content
    details; // The AttachmentDetails object
}

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
