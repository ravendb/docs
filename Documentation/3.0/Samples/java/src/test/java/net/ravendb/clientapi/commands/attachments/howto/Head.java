package net.ravendb.clientapi.commands.attachments.howto;

import java.util.List;

import net.ravendb.abstractions.data.Attachment;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;

@SuppressWarnings("deprecation")
public class Head {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region head_1_0
    @Deprecated
    public Attachment headAttachment(String key);
    //endregion

    //region head_2_0
    @Deprecated
    public List<Attachment> getAttachmentHeadersStartingWith(String idPrefix, int start, int pageSize);
    //endregion
  }

  @SuppressWarnings("unused")
  public Head() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region head_1_1
      Attachment attachment = store.getDatabaseCommands().headAttachment("albums/holidays/sea.jpg"); // null if does not exist
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region head_2_1
      List<Attachment> attachments = store.getDatabaseCommands().getAttachmentHeadersStartingWith("albums/holidays/", 0, 10);
      //endregion
    }
  }
}
