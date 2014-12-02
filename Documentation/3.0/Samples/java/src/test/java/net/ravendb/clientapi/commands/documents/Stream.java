package net.ravendb.clientapi.commands.documents;

import net.ravendb.abstractions.basic.CloseableIterator;
import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.RavenPagingInformation;
import net.ravendb.client.document.DocumentStore;


public class Stream {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region stream_1
    public CloseableIterator<RavenJObject> streamDocs();

    public CloseableIterator<RavenJObject> streamDocs(Etag fromEtag);

    public CloseableIterator<RavenJObject> streamDocs(Etag fromEtag, String startsWith);

    public CloseableIterator<RavenJObject> streamDocs(Etag fromEtag, String startsWith, String matches);

    public CloseableIterator<RavenJObject> streamDocs(Etag fromEtag, String startsWith, String matches, int start);

    public CloseableIterator<RavenJObject> streamDocs(Etag fromEtag, String startsWith, String matches, int start, int pageSize);

    public CloseableIterator<RavenJObject> streamDocs(Etag fromEtag, String startsWith, String matches, int start, int pageSize, String exclude);

    public CloseableIterator<RavenJObject> streamDocs(Etag fromEtag, String startsWith, String matches, int start, int pageSize, String exclude, RavenPagingInformation pagingInformation);

    public CloseableIterator<RavenJObject> streamDocs(Etag fromEtag, String startsWith, String matches, int start, int pageSize, String exclude, RavenPagingInformation pagingInformation, String skipAfter);
    //endregion

  }
  @SuppressWarnings("unused")
  public Stream() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region stream_2
      try (CloseableIterator<RavenJObject> iterator = store.getDatabaseCommands().streamDocs(null, "products/")) {
        while (iterator.hasNext()) {
          RavenJObject document = iterator.next();
        }
      }
      //endregion
    }
  }
}
