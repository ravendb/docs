package net.ravendb.clientapi.commands.attachments;

import net.ravendb.abstractions.data.Attachment;
import net.ravendb.abstractions.data.AttachmentInformation;
import net.ravendb.abstractions.data.Etag;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


@SuppressWarnings("deprecation")
public class Get {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_1_0
    @Deprecated
    public Attachment getAttachment(String key);
    //endregion

    //region get_2_0
    @Deprecated
    public AttachmentInformation[] getAttachments(int start, Etag startEtag, int pageSize);
    //endregion
  }

  @SuppressWarnings("unused")
  public Get() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region get_1_1
      Attachment attachment = store.getDatabaseCommands().getAttachment("albums/holidays/sea.jpg"); // null if does not exist
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region get_2_1
      AttachmentInformation[] attachments = store.getDatabaseCommands().getAttachments(0, Etag.empty(), 10);
      //endregion
    }
  }
}
