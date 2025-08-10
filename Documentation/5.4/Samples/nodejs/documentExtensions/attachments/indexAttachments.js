import { DocumentStore, AbstractJavaScriptIndexCreationTask } from "ravendb";
const fs = require('fs');

const documentStore = new DocumentStore();
const session = store.openSession();


//region index_1
class Employees_ByAttachmentDetails extends AbstractJavaScriptIndexCreationTask {
    constructor () {
        super();

        const { attachmentsFor } = this.mapUtils();

        this.map("employees", employee => {
            // Call 'attachmentsFor' to get attachments details
            const attachments = attachmentsFor(employee);

            return {
                // Can index info from document properties:
                employeeName: employee.FirstName + " " + employee.LastName,

                // Index DETAILS of attachments:
                attachmentNames: attachments.map(x => x.Name),
                attachmentContentTypes: attachments.map(x => x.ContentType),
                attachmentSizes: attachments.map(x => x.Size)
            }
        });
    }
}
//endregion

//region index_2
class Employees_ByAttachment extends AbstractJavaScriptIndexCreationTask {
    constructor () {
        super();

        const { loadAttachment } = this.mapUtils();

        this.map("employees", employee => {
            // Call 'loadAttachment' to get attachment's details and content
            // pass the attachment name, e.g. "notes.txt"
            const attachment = loadAttachment(employee, "notes.txt");

            return {
                // Index DETAILS of attachment:
                attachmentName: attachment.Name,
                attachmentContentType: attachment.ContentType,
                attachmentSize: attachment.Size,

                // Index CONTENT of attachment:
                // Call 'getContentAsString' to access content
                attachmentContent: attachment.getContentAsString()
            }
        });

        // It can be useful configure Full-Text search on the attachment content index-field
        this.index("attachmentContent", "Search");

        // Documents with an attachment named 'notes.txt' will be indexed,
        // allowing you to query them by either the attachment's details or its content.
    }
}
//endregion

//region index_3
class Employees_ByAllAttachments extends AbstractJavaScriptIndexCreationTask {
    constructor () {
        super();

        const { loadAttachments } = this.mapUtils();

        this.map("employees", employee => {
            // Call 'loadAttachments' to get details and content for ALL attachments
            const allAttachments = loadAttachments(employee);

            // This will be a Fanout index -
            // the index will generate an index-entry for each attachment per document

            return allAttachments.map(attachment => ({

                // Index DETAILS of attachment:
                attachmentName: attachment.Name,
                attachmentContentType: attachment.ContentType,
                attachmentSize: attachment.Size,

                // Index CONTENT of attachment:
                // Call 'getContentAsString' to access content
                attachmentContent: attachment.getContentAsString()
            }));
        });

        // It can be useful configure Full-Text search on the attachment content index-field
        this.index("attachmentContent", "Search");
    }
}
//endregion

async function indexAttachments() {
    {
        //region store_attachments
        const session = documentStore.openSession();

        for (let i = 1; i <= 3; i++) {
            // Load an employee document:
            const employee = await session.load(`employees/${i}-A`);
            
            // Store the employee's notes as an attachment on the document:
            const stream = Buffer.from(employee.Notes[0]);
            session.advanced.attachments.store(`employees/${i}-A`, "notes.txt", stream, "text/plain");
        }
        
        await session.saveChanges();
        //endregion
    }
    {
        //region query_1
        const employees = await session
             // Query the index for matching employees
            .query({ indexName: "Employees/ByAttachmentDetails" })
             // Filter employee results by their attachments details
            .whereEquals("attachmentNames", "photo.jpg")
            .whereGreaterThan("attachmentSizes", 20_000)
            .all();

        // Results:
        // ========
        // Running this query on the Northwind sample data,
        // results will include 'employees/4-A' and 'employees/5-A'.
        // These 2 documents contain an attachment by name 'photo.jpg' with a matching size.
        //endregion
    }
    {
        //region query_2
        const employees = await session
            // Query the index for matching employees
            .query({indexName: "Employees/ByAttachment"})
            // Can make a full-text search
            // Looking for employees with an attachment content that contains 'Colorado' OR 'Dallas'
            .search("attachmentContent", "Colorado Dallas")
            .all();

        // Results:
        // ========
        // Results will include 'employees/1-A' and 'employees/2-A'.
        // Only these 2 documents have an attachment by name 'notes.txt'
        // that contains either 'Colorado' or 'Dallas'.
        //endregion
    }
    {
        //region query_3
        const employees = await session
            // Query the index for matching employees
            .query({indexName: "Employees/ByAllAttachments"})
            // Filter employee results by their attachments details and content
            .whereGreaterThan("attachmentSize", 20_000)
            .orElse()
            .search("attachmentContent", "Colorado Dallas")
            .all();

        // Results:
        // ========
        // Results will include:
        // 'employees/1-A' and 'employees/2-A' that match the content criteria 
        // 'employees/4-A' and 'employees/5-A' that match the size criteria
        //endregion
    }
}

//region syntax_1
attachmentsFor(document);
//endregion

//region syntax_2
// Returns a list containing the following attachment details object:
{    
    name;         // string
    hash;         // string
    contentType;  // string
    size;         // number
}
//endregion

//region syntax_3
loadAttachment(document, attachmentName);
//endregion    
    
//region syntax_4
// Returns the following attachment object:
{
    // Properties accessing DETAILS:
    // =============================
    name;         // string
    hash;         // string
    contentType;  // string
    size;         // number
    
    // Methods accessing CONTENT:
    // ==========================
    getContentAsStream();
    getContentAsString(encoding);
    getContentAsString(); // Default encoding is "utf8"
}
//endregion

//region syntax_5
loadAttachments(document);

// Returns a list containing the above attachment object per attachment.
//endregion

//endregion
