package net.ravendb.clientapi.session.howto;

import net.ravendb.abstractions.commands.DeleteCommandData;
import net.ravendb.abstractions.commands.ICommandData;
import net.ravendb.abstractions.commands.PutCommandData;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.Supplier;


public class Defer {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region defer_1
    public void defer(ICommandData... commands);
    //endregion
  }

  public Defer() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region defer_2
        Product product1 = new Product();
        product1.setName("My Product");
        product1.setSupplier("suppliers/999");

        PutCommandData command1 = new PutCommandData();
        command1.setKey("products/999");
        command1.setDocument(RavenJObject.fromObject(product1));
        command1.setMetadata(new RavenJObject());

        session.advanced().defer(command1);

        Supplier supplier = new Supplier();
        supplier.setName("My Supplier");

        PutCommandData command2 = new PutCommandData();
        command2.setKey("suppliers/999");
        command2.setDocument(RavenJObject.fromObject(supplier));
        command2.setMetadata(new RavenJObject());

        session.advanced().defer(command2);

        DeleteCommandData deleteCommandData = new DeleteCommandData();
        deleteCommandData.setKey("products/1");
        session.advanced().defer(deleteCommandData);
        //endregion
      }
    }
  }
}
