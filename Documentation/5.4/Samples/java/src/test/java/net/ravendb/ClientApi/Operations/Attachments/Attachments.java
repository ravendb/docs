package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.attachments.AttachmentType;
import net.ravendb.client.documents.operations.attachments.*;
import org.apache.commons.io.IOUtils;
import org.apache.http.client.methods.CloseableHttpResponse;

import java.io.IOException;
import java.io.InputStream;

public class Attachments {

    private interface IFoo {
        /*
        //region delete_attachment_syntax
        DeleteAttachmentOperation(String documentId, String name)

        DeleteAttachmentOperation(String documentId, String name, String changeVector)
        //endregion

        //region put_attachment_syntax
        PutAttachmentOperation(String documentId, String name, InputStream stream)

        PutAttachmentOperation(String documentId, String name, InputStream stream, String contentType)

        PutAttachmentOperation(String documentId, String name, InputStream stream, String contentType, String changeVector)
        //endregion

        //region get_attachment_syntax
        GetAttachmentOperation(String documentId, String name, AttachmentType type, String changeVector)
        //endregion
        */
    }

    private class Foo {
        //region put_attachment_return_value
        public class AttachmentDetails extends AttachmentName {
            private String changeVector;
            private String documentId;

            public String getChangeVector() {
                return changeVector;
            }

            public void setChangeVector(String changeVector) {
                this.changeVector = changeVector;
            }

            public String getDocumentId() {
                return documentId;
            }

            public void setDocumentId(String documentId) {
                this.documentId = documentId;
            }
        }

        public class AttachmentName {
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
        }
        //endregion
    }

    private class Foo2 {
        /*
        //region get_attachment_return_value
        public class CloseableAttachmentResult implements AutoCloseable {
            private AttachmentDetails details;
            private CloseableHttpResponse response;

            public InputStream getData() throws IOException {
                return response.getEntity().getContent();
            }

            public AttachmentDetails getDetails() {
                return details;
            }
        }

        public class AttachmentDetails extends Foo.AttachmentName {
            private String changeVector;
            private String documentId;

            public String getChangeVector() {
                return changeVector;
            }

            public void setChangeVector(String changeVector) {
                this.changeVector = changeVector;
            }

            public String getDocumentId() {
                return documentId;
            }

            public void setDocumentId(String documentId) {
                this.documentId = documentId;
            }
        }

        public class AttachmentName {
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
        }
        //endregion
        */
    }

    public Attachments() {
        try (IDocumentStore store = new DocumentStore()) {
            //region delete_1
            store.operations().send(
                new DeleteAttachmentOperation("orders/1-A", "invoice.pdf"));
            //endregion

            //region get_1
            store.operations().send(
                new GetAttachmentOperation("orders/1-A", "invoice.pdf", AttachmentType.DOCUMENT, null));
            //endregion

            {
                InputStream stream = null;
                //region put_1
                AttachmentDetails attachmentDetails = store
                    .operations().send(new PutAttachmentOperation("orders/1-A",
                        "invoice.pdf",
                        stream,
                        "application/pdf"));
                //endregion
            }
        }
    }
}
