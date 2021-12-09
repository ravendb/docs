import {AbstractCsharpIndexCreationTask, DocumentStore} from "ravendb";

const store = new DocumentStore('http://127.0.0.1:8080', 'Northwind');
const session = store.openSession();

class CEmployees_ByAttachmentNames {}
class Company {
}
class Employee {
}

{
    //region AttFor_index_JS
    class Employees_ByAttachmentNames extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();
            this.map = `docs.Employees.Select(e => 
                    let attachments = AttachmentsFor(e);
                    new {    
                            AttachmentNames = attachments.Select(x => x.Name).ToArray()
                    })`;
        }
    }

    //endregion
}
{
    //region LoadAtt_index_JS
    class Companies_With_Attachments_JavaScript  extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();
            this.map = `docs.Companies.Select(company => 
                   let attachment = LoadAttachment(company, company.ExternalId);
                    new {
                        CompanyName: company.Name,
                        AttachmentName: attachment.Name,
                        AttachmentContentType: attachment.ContentType,
                        AttachmentHash: attachment.Hash,
                        AttachmentSize: attachment.Size,
                        AttachmentContent: attachment.getContentAsString('utf8')
                  
                 })`
        }
    }
    //endregion
}
{
    //region LoadAtts_index_JS
    class Companies_With_All_Attachments_JS  extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();
            this.map = `docs.Companies.Select(company => 
                        var attachments = LoadAttachments(company);
                 return attachments.map(attachment => ({
                     AttachmentName: attachment.Name,
                     AttachmentContent: attachment.getContentAsString('utf8')
                  }));
            )`
        }
    }
    //endregion
}



//region query1
    //return all employees that have an attachment called "cv.pdf"
    let employees = session.query({ collection: "Employees_ByAttachmentNames" })
        .containsAny("attachmentNames", ["employees_cv.pdf"])
        .selectFields<Company>( "cv.pdf", Employee).all()
//endregion