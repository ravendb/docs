package net.ravendb.clientapi.commands.documents;

import java.util.List;
import java.util.Map;

import net.ravendb.abstractions.data.JsonDocument;
import net.ravendb.abstractions.data.MultiLoadResult;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.abstractions.json.linq.RavenJToken;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.RavenPagingInformation;
import net.ravendb.client.document.DocumentStore;


public class Get {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_1_0
    public JsonDocument get(String key);
    //endregion

    //region get_2_0
    public MultiLoadResult get(String[] ids, String[] includes);

    public MultiLoadResult get(String[] ids, String[] includes, String transformer);

    public MultiLoadResult get(String[] ids, String[] includes, String transformer, Map<String, RavenJToken> transformerParameters);

    public MultiLoadResult get(String[] ids, String[] includes, String transformer, Map<String, RavenJToken> transformerParameters, boolean metadataOnly);
    //endregion

    //region get_3_0
    public List<JsonDocument> getDocuments(int start, int pageSize);

    public List<JsonDocument> getDocuments(int start, int pageSize, boolean metadataOnly);
    //endregion

    //region get_4_0
    public List<JsonDocument> startsWith(String keyPrefix, String matches, int start, int pageSize);

    public List<JsonDocument> startsWith(String keyPrefix, String matches, int start, int pageSize, boolean metadataOnly);

    public List<JsonDocument> startsWith(String keyPrefix, String matches, int start, int pageSize, boolean metadataOnly, String exclude);

    public List<JsonDocument> startsWith(String keyPrefix, String matches, int start, int pageSize, boolean metadataOnly, String exclude, RavenPagingInformation pagingInformation);

    public List<JsonDocument> startsWith(String keyPrefix, String matches, int start, int pageSize, boolean metadataOnly,
      String exclude, RavenPagingInformation pagingInformation, String transformer, Map<String, RavenJToken> transformerParameters);

    public List<JsonDocument> startsWith(String keyPrefix, String matches, int start, int pageSize, boolean metadataOnly,
      String exclude, RavenPagingInformation pagingInformation, String transformer, Map<String, RavenJToken> transformerParameters, String skipAfter);
    //endregion

  }

  @SuppressWarnings("unused")
  public Get() throws Exception {
    try (IDocumentStore store = new DocumentStore().initialize()) {
      //region get_1_2
      JsonDocument document = store.getDatabaseCommands().get("products/1"); // null if does not exist
      //endregion

      //region get_2_2
      MultiLoadResult resultsWithoutIncludes = store.getDatabaseCommands().get(new String[] { "products/1", "products/2" }, null);
      //endregion

      //region get_2_3
      MultiLoadResult resultsWithIncludes = store.getDatabaseCommands().get(
        new String[] { "products/1", "products/2" },
        new String[] { "Category" });

      List<RavenJObject> results = resultsWithIncludes.getResults(); // products/1, products/2
      List<RavenJObject> includes = resultsWithIncludes.getIncludes(); // categories/1
      //endregion
    }
  }

  @SuppressWarnings("unused")
  public void missingDocuments() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region get_2_4
      // assuming that 'products/9999' does not exist
      MultiLoadResult resultsWithIncludes = store.getDatabaseCommands().get(
        new String[] { "products/1", "products/9999", "products/3" },
        null
        );
      List<RavenJObject> results = resultsWithIncludes.getResults(); // products/1, null, products/3
      List<RavenJObject> includes = resultsWithIncludes.getIncludes(); // empty
      //endregion
    }
  }

  @SuppressWarnings("unused")
  public void getDocuments() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region get_3_1
      List<JsonDocument> documents = store.getDatabaseCommands().getDocuments(0, 10, false);
      //endregion
    }
  }

  @SuppressWarnings("unused")
  public void startsWith() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region get_4_1
      // return up to 128 documents with key that starts with 'products'
      List<JsonDocument> result = store.getDatabaseCommands().startsWith("products", null, 0, 128);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region get_4_2
      // return up to 128 documents with key that starts with 'products/'
      // and rest of the key begins with "1" or "2" e.g. products/10, products/25
      List<JsonDocument> result = store.getDatabaseCommands().startsWith("products/", "1*|2*", 0, 128);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region get_4_3
      // return up to 128 documents with key that starts with 'products/'
      // and rest of the key have length of 3, begins and ends with "1"
      // and contains any character at 2nd position e.g. products/101, products/1B1
      List<JsonDocument> result = store.getDatabaseCommands().startsWith("products/", "1?1", 0, 128);
      //endregion
    }
  }

}
