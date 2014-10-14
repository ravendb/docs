package net.ravendb.clientapi.commands.documents;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.data.PutResult;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Category;


public class Put {
  public interface IFoo {
    //region put_1
    public PutResult put(String key, Etag guid, RavenJObject document, RavenJObject metadata);
    //endregion

  }

  public Put() throws Exception {
    try (IDocumentStore store = new DocumentStore().initialize()) {
      //region put_3
      Category category = new Category();
      category.setName("My Category");
      category.setDescription("My Category description");

      store.getDatabaseCommands().put(
        "categories/999",
        null,
        RavenJObject.fromObject(category),
        new RavenJObject());
      //endregion
    }
  }

}
