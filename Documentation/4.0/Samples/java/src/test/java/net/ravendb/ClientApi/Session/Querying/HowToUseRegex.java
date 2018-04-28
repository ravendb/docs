package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class HowToUseRegex {

    private static class User {
        private String firstName;

        public String getFirstName() {
            return firstName;
        }

        public void setFirstName(String firstName) {
            this.firstName = firstName;
        }
    }

    private static class Product {

    }

    public void examples() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region regex_1
                // loads all products, which name
                // starts with 'N' or 'A'
                List<Product> products = session.query(Product.class)
                    .whereRegex("name", "^[NA]")
                    .toList();
                //endregion
            }
        }
    }
}
