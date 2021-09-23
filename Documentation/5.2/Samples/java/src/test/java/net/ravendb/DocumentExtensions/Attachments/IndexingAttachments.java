package net.ravendb.DocumentExtensions.Attachments;

import com.google.common.collect.Lists;
import com.google.common.collect.Sets;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.AbstractJavaScriptIndexCreationTask;
import net.ravendb.client.documents.operations.attachments.AttachmentName;
import net.ravendb.client.documents.session.IDocumentSession;


import java.util.Arrays;
import java.util.Collection;
import java.util.Collections;
import java.util.List;

public class IndexingAttachments {

    private interface IFoo {

        //region syntax
        List<AttachmentName> AttachmentsFor(Object doc);
        //endregion


        /*
        //region syntax_2
        public IAttachmentObject LoadAttachment(object doc, string name);
        public IEnumerable<IAttachmentObject> LoadAttachments(object doc);
        //endregion
         */
    }

    public static class Foo {
        //region result
        private String name;
        private String hash;
        private String contentType;
        private long size;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getHash() {
            return hash;
        }

        public void setHash(String hash) {
            this.hash = hash;
        }

        public String getContentType() {
            return contentType;
        }

        public void setContentType(String contentType) {
            this.contentType = contentType;
        }

        public long getSize() {
            return size;
        }

        public void setSize(long size) {
            this.size = size;
        }

        //endregion
    }



    //region AttFor_index_JS
    public static class Employees_ByAttachmentNames extends AbstractIndexCreationTask {
        public Employees_ByAttachmentNames() {
            map = "from e in docs.Employees\n" +
                "let attachments = AttachmentsFor(e)\n" +
                "select new\n" +
                "{\n" +
                "   attachmentNames = attachments.Select(x => x.Name).ToArray() \n" +
                "}";
        }

    }
    //endregion

    //region LoadAtt_index_JS
    private class Companies_With_Attachments_JavaScript  extends AbstractJavaScriptIndexCreationTask {
        public Companies_With_Attachments_JavaScript()
        {
            setMaps(Collections.singleton(
                "map('Companies', function (company) {" +
                    "   var attachment = LoadAttachment(company, company.ExternalId);"+
                    "   return { \n" +
                    "       CompanyName: company.Name, \n" +
                    "       AttachmentName: attachment.Name, \n" +
                    "       AttachmentContentType: attachment.ContentType, \n" +
                    "       AttachmentHash: attachment.Hash, \n" +
                    "       AttachmentSize: attachment.Size, \n" +
                    "       AttachmentContent: attachment.getContentAsString('utf8')" +
                    "});"
                )
            );
        };
    }
    //endregion

    //region LoadAtts_index_JS
    private class Companies_With_All_Attachments_JS   extends AbstractJavaScriptIndexCreationTask {
        public Companies_With_All_Attachments_JS()
        {
            setMaps(Collections.singleton(
                "map('Companies', function (company) { \n"+
                    "var attachments = LoadAttachments(company); \n"+
                    "return attachments.map(attachment => ({ \n"+
                        "AttachmentName: attachment.Name, \n"+
                        "AttachmentContent: attachment.getContentAsString('utf8') \n"+
                    "})); \n"+
                "})"
                )
            );
        };
    }
    //endregion



    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {

            try (IDocumentSession session = store.openSession()) {

                //region query1
                //return all employees that have an attachment called "cv.pdf"
                List<Employee> employees = session.query(Employees_ByAttachmentNames.class)
                    .containsAny("attachmentNames", Arrays.asList("employees_cv.pdf"))
                    .selectFields(Company.class, "cv.pdf").ofType(Employee.class)
                    .toList();
                //endregion

            }
        }
    }

    class Employee{

    }
    class Company{

    }

}
