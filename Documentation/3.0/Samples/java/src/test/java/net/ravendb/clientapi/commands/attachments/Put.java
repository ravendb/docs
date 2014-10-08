package net.ravendb.clientapi.commands.attachments;

import java.io.FileInputStream;
import java.io.InputStream;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Put {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region put_1
    @Deprecated
    public void putAttachment(String key, Etag etag, InputStream data, RavenJObject metadata);
    //endregion
  }

  @SuppressWarnings("deprecation")
  public Put() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region put_2
      try (FileInputStream fis = new FileInputStream("sea.png")) {
        RavenJObject metadata = new RavenJObject();
        metadata.add("Description", "Holidays 2014");
        store
        .getDatabaseCommands()
        .putAttachment("albums/holidays/sea.jpg", null, fis, metadata);
      }
      //endregion
    }
  }
}
