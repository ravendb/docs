package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.*;
import org.apache.commons.lang3.NotImplementedException;

import java.time.Duration;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Events {

    private static final Logger log = null;

    private static class Product {
        private int unitsInStock;
        private String name;
        private boolean discontinued;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public int getUnitsInStock() {
            return unitsInStock;
        }

        public void setUnitsInStock(int unitsInStock) {
            this.unitsInStock = unitsInStock;
        }

        public boolean isDiscontinued() {
            return discontinued;
        }

        public void setDiscontinued(boolean discontinued) {
            this.discontinued = discontinued;
        }
    }

    //region on_before_store_event
    private void onBeforeStoreEvent(Object sender, BeforeStoreEventArgs args) {
        if (args.getEntity() instanceof Product) {
            Product product = (Product) args.getEntity();
            if (product.unitsInStock == 0) {
                product.setDiscontinued(true);
            }
        }
    }
    //endregion

    //region on_before_delete_event
    private void onBeforeDeleteEvent(Object sender, BeforeDeleteEventArgs args) {
        throw new NotImplementedException("Sample");
    }
    //endregion

    //region on_before_query_execute_event
    private void onBeforeQueryEvent(Object sender, BeforeQueryEventArgs args) {
        args.getQueryCustomization().noCaching();
    }
    //endregion

    private class Foo {
        //region on_before_query_execute_event_2
        private void onBeforeQueryEvent(Object sender, BeforeQueryEventArgs args) {
            args.getQueryCustomization().waitForNonStaleResults(Duration.ofSeconds(30));
        }
        //endregion
    }
    //region on_after_save_changes_event
    private void onAfterSaveChangesEvent(Object sender, AfterSaveChangesEventArgs args) {
        if (log.isLoggable(Level.INFO)) {
            log.info("Document " + args.getDocumentId() + " was saved");
        }
    }
    //endregion

    public Events() {
        try (IDocumentStore store = new DocumentStore()) {
            store.addAfterSaveChangesListener(this::onAfterSaveChangesEvent);
            store.addBeforeDeleteListener(this::onBeforeDeleteEvent);
            store.addBeforeQueryListener(this::onBeforeQueryEvent);

            //region store_session
            // subscribe to the event
            store.addBeforeStoreListener(this::onBeforeStoreEvent);

            try (IDocumentSession session = store.openSession()) {
                Product product1 = new Product();
                product1.setName("RavenDB v3.5");
                product1.setUnitsInStock(0);

                session.store(product1);

                Product product2 = new Product();
                product2.setName("RavenDB v4.0");
                product2.setUnitsInStock(1000);
                session.store(product2);

                session.saveChanges();  // Here the method is invoked
            }
            //endregion

            //region delete_session
            // subscribe to the event
            store.addBeforeDeleteListener(this::onBeforeDeleteEvent);

            // open a session and delete entity
            try (IDocumentSession session = store.openSession()) {
                Product product = session.load(Product.class, "products/1-A");

                session.delete(product);

                session.saveChanges(); // NotImplementedException will be thrown here
            }
            //endregion

        }
    }

}
