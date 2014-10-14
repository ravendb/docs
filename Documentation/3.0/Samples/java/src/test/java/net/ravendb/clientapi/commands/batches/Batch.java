package net.ravendb.clientapi.commands.batches;

import java.util.Arrays;
import java.util.List;

import net.ravendb.abstractions.commands.DeleteCommandData;
import net.ravendb.abstractions.commands.ICommandData;
import net.ravendb.abstractions.commands.PutCommandData;
import net.ravendb.abstractions.data.BatchResult;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.Supplier;


public class Batch {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region batch_1
    public BatchResult[] batch(List<ICommandData> commandDatas);
    //endregion
  }

  @SuppressWarnings("unused")
  public Batch() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region batch_2
      Product product1 = new Product();
      product1.setName("My Product");
      product1.setSupplier("suppliers/999");
      PutCommandData put1 = new PutCommandData("products/999", null, RavenJObject.fromObject(product1), new RavenJObject());

      Supplier supplier = new Supplier();
      supplier.setName("My Supplier");
      PutCommandData put2 = new PutCommandData("suppliers/999", null, RavenJObject.fromObject(supplier), new RavenJObject());

      DeleteCommandData delete = new DeleteCommandData("products/2", null);

      BatchResult[] results = store.getDatabaseCommands().batch(Arrays.<ICommandData> asList(put1, put2, delete));
      //endregion
    }
  }
}
