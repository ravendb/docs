import {AbstractCsharpIndexCreationTask, DocumentStore} from "ravendb";

const store = new DocumentStore('http://127.0.0.1:8080', 'Northwind');
const session = store.openSession();

class CEmployees_ByAttachmentNames {
}

class Company {
}

class Employee {
}

{
    //region AttFor_index_JS
    class Employees_ByAttachmentNames extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();
            this.map = `docs.Employees.Select(e => new {
                e = e,
                attachments = this.AttachmentsFor(e)
            }).Select(this0 => new {
                AttachmentNames = Enumerable.ToArray(this0.attachments.Select(x => x.Name))
            })`;
        }
    }

    //endregion
}
{
    //region LoadAtt_index_JS
    class Companies_With_Attachments_JavaScript extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();
            this.map = `docs.Companies.Select(company => new {
                company = company,
                attachment = this.LoadAttachment(company, (System.String) company.ExternalId)
            }).Select(this0 => new {
                CompanyName = this0.company.Name,
                AttachmentName = this0.attachment.Name,
                AttachmentContentType = this0.attachment.ContentType,
                AttachmentHash = this0.attachment.Hash,
                AttachmentSize = this0.attachment.Size,
                AttachmentContent = this0.attachment.GetContentAsString(Encoding.UTF8)
            })`;
        }
    }

    //endregion
}
{
    //region LoadAtts_index_JS
    class Companies_With_All_Attachments_JS extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();
            this.map = `docs.Companies.Select(company => new {
                company = company,
                attachments = this.LoadAttachments(company)
            }).SelectMany(this0 => this0.attachments, (this0, attachment) => new {
                AttachmentName = attachment.Name,
                AttachmentContent = attachment.GetContentAsString(Encoding.UTF8)
            })`;
        }
    }

    //endregion
}


//region query1
//return all employees that have an attachment called "cv.pdf"
const employees = await session.query(Employees_ByAttachmentNames)
    .containsAny("attachmentNames", ["employees_cv.pdf"])
    .selectFields("cv.pdf")
    .all();
//endregion