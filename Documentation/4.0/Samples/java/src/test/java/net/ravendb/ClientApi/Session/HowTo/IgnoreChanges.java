package net.ravendb.ClientApi.Session.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class IgnoreChanges {

    private class Product {
        private int unitsInStock;

        public int getUnitsInStock() {
            return unitsInStock;
        }

        public void setUnitsInStock(int unitsInStock) {
            this.unitsInStock = unitsInStock;
        }
    }

    public IgnoreChanges() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region ignore_changes_2
                Product product = session.load(Product.class, "products/1-A");
                session.advanced().ignoreChangesFor(product);
                product.unitsInStock++; //this will be ignored for SaveChanges
                session.saveChanges();
                //endregion
            }
        }
    }

    private interface IFoo {
        //region ignore_changes_1
        void ignoreChangesFor(Object entity);
        //endregion
    }

}
