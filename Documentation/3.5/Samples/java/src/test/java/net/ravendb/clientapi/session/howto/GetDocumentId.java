package net.ravendb.clientapi.session.howto;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class GetDocumentId {

  //region get_document_id_3
  public static class Comment {
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

  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_document_id_1
    public String getDocumentUrl(Object entity);
    //endregion
  }

  public GetDocumentId() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      Comment comment = null;
      try (IDocumentSession session = store.openSession()) {
        //region get_document_id_2
        session.advanced().getDocumentId(comment); // e.g. comments/1
        //endregion
      }
    }
  }
}
