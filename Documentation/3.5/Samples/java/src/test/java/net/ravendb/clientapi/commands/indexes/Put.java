package net.ravendb.clientapi.commands.indexes;

import net.ravendb.abstractions.data.IndexStats;
import net.ravendb.abstractions.data.IndexToAdd;
import net.ravendb.abstractions.indexing.FieldStorage;
import net.ravendb.abstractions.indexing.IndexDefinition;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.IndexDefinitionBuilder;
import net.ravendb.samples.northwind.QOrder;

public class Put {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region put_1_0
    public String putIndex(String name, IndexDefinition definition);

    public String putIndex(String name, IndexDefinition definition, boolean overwrite);
    //endregion

    //region put_2_0
    public String putIndex(String name, IndexDefinitionBuilder indexDef);

    public String putIndex(String name, IndexDefinitionBuilder indexDef, boolean overwrite);
    //endregion

    //region put_indexes_3_0
    public String[] putIndexes(IndexToAdd[] indexesToAdd);
    //endregion

  }

  @SuppressWarnings("unused")
  public Put() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region put_1_1
      String map = " from order in docs.Orders " +
        " select new " +
        " { " +
        "    order.Employee," +
        "    order.Company," +
        "    Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))" +
        "}";

        IndexDefinition indexDef = new IndexDefinition();
        indexDef.setMap(map);

      String indexName = store.getDatabaseCommands()
        .putIndex("Orders/Totals", indexDef);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region put_2_1
      String map = " from order in docs.Orders " +
        " select new " +
        " { " +
        "    order.Employee," +
        "    order.Company," +
        "    Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))" +
        "}";

      QOrder order = QOrder.order;
      IndexDefinitionBuilder indexDef = new IndexDefinitionBuilder();
      indexDef.setMap(map);
      indexDef.getStores().put(order.company, FieldStorage.YES);

      String indexName = store.getDatabaseCommands()
        .putIndex("Orders/Totals", indexDef);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region put_indexes_3_1
      IndexToAdd index1 = new IndexToAdd();
      index1.setName("Orders/Totals");
      index1.setPriority(IndexStats.IndexingPriority.NORMAL);
      index1.setDefinition(new IndexDefinition("from order in docs.Orders "
          + " select new "
          + "{ "
          + "   order.Employee,"
          + "   order.Company,"
          + "   Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))"
          + "}"));

      IndexDefinition indexDefinition = new IndexDefinition();
      indexDefinition.setMap("from order in docs.Orders"
          + " select new "
          + "{ "
          + "   order.Company, "
          + "   Count = 1,"
          + "   Total = order.Lines.Sum(l = >(l.Quantity * l.PricePerUnit) *  ( 1 - l.Discount)) "
          + "} ");

      indexDefinition.setReduce("from result in results "
          + "group result by result.Company into g "
          + "select new "
          + " { "
          + "    Company = g.Key,"
          + "    Count = g.Sum(x => x.Count),"
          + "    Total = g.Sum(x => x.Total) "
          + "}");


      IndexToAdd index2 = new IndexToAdd();
      index2.setName("Orders/ByCompany");
      index2.setPriority(IndexStats.IndexingPriority.NORMAL);
      index2.setDefinition(indexDefinition);

      store.getDatabaseCommands().putIndexes(new IndexToAdd[] { index1, index2});
      //endregion
    }
  }
}
