package net.ravendb.Indexes;

import com.google.common.collect.Lists;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.operations.attachments.AttachmentName;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class IndexingAttachments {

    private interface IFoo {
        //region syntax
        List<AttachmentName> AttachmentsFor(Object doc);
        //endregion
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

    //region index
    public static class Employees_ByAttachmentNames extends AbstractIndexCreationTask {
        public Employees_ByAttachmentNames() {
            map = "from e in docs.Employees\n" +
                "let attachments = AttachmentsFor(e)\n" +
                "select new\n" +
                "{\n" +
                "   attachmentNames = attachments.Select(x => x.Name).ToArray()\n" +
                "}";
        }
    }
    //endregion

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {

            try (IDocumentSession session = store.openSession()) {
                //region query1
                //return all employees that have an attachment called "cv.pdf"
                List<Employee> employees = session.query(Employee.class, Employees_ByAttachmentNames.class)
                    .containsAny("attachmentNames", Lists.newArrayList("cv.pdf"))
                    .toList();
                //endregion
            }
        }
    }
}
