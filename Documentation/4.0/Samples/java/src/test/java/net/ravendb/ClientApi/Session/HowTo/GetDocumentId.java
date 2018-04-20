package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class GetDocumentId {

    //region get_document_id_3
    public class Comment {
        private String author;
        private String message;

        public String getAuthor() {
            return author;
        }

        public void setAuthor(String author) {
            this.author = author;
        }

        public String getMessage() {
            return message;
        }

        public void setMessage(String message) {
            this.message = message;
        }
    }
    //endregion

    private interface IFoo {
        //region get_document_id_1
        String getDocumentId(Object entity)
        //endregion
        ;
    }

    public GetDocumentId() {
        try (IDocumentStore store = new DocumentStore()) {
            Comment comment = null;

            try (IDocumentSession session = store.openSession()) {
                //region get_document_id_2
                String commentId = session
                    .advanced()
                    .getDocumentId(comment);// e.g. comments/1-A
                //endregion
            }
        }
    }

}
