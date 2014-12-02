package net.ravendb.clientapi.commands.transformers;

import net.ravendb.abstractions.indexing.TransformerDefinition;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class Put {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region put_1
    public String putTransformer(String name, TransformerDefinition transformerDef);
    //endregion
  }

  @SuppressWarnings("unused")
  public Put() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region put_2
      TransformerDefinition transformerDefinition = new TransformerDefinition();
      transformerDefinition.setTransformResults("from order in orders " +
        "select new " +
        "{ " +
        "    order.OrderedAt, " +
        "    order.Status, " +
        "    order.CustomerId, " +
        "    CustomerName = LoadDocument<Customer>(order.CustomerId).Name, " +
        "    LinesCount = order.Lines.Count " +
        "}");

      String transformerName = store.getDatabaseCommands().putTransformer("Order/Statistics", transformerDefinition);
      //endregion
    }
  }

}
