package net.ravendb.clientapi.commands.attachments.howto;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.data.JsonDocumentMetadata;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Update {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region update_1
    @Deprecated
    public void updateAttachmentMetadata(String key, Etag etag, RavenJObject metadata);
    //endregion
  }

  @SuppressWarnings("deprecation")
  public Update() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region update_2
      JsonDocumentMetadata attachment = store.getDatabaseCommands().head("albums/holidays/sea.jpg");
      RavenJObject metadata = attachment.getMetadata();
      metadata.add("Description", "Holidays 2012");

      store.getDatabaseCommands().updateAttachmentMetadata("albums/holidays/sea.jpg", attachment.getEtag(), metadata);
      //endregion
    }
  }
}
