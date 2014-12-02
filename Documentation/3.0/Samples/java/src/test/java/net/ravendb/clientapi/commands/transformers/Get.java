package net.ravendb.clientapi.commands.transformers;

import java.util.List;
import net.ravendb.abstractions.indexing.TransformerDefinition;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;

public class Get {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region get_1_0
    public TransformerDefinition getTransformer(String name);
    //endregion

    //region get_2_0
    public List<TransformerDefinition> getTransformers(int start, int pageSize);
    //endregion
  }

  @SuppressWarnings("unused")
  public Get() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region get_1_1
      TransformerDefinition transformer = store
        .getDatabaseCommands()
        .getTransformer("Order/Statistics"); // returns null if does not exist
      //endregion

      //region get_2_1
      List<TransformerDefinition> transformers = store.getDatabaseCommands().getTransformers(0, 128);
      //endregion
    }
  }
}
