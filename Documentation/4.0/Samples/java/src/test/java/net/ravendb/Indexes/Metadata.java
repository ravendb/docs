package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Date;
import java.util.List;

public class Metadata {

    private static class Product {

    }

    //region indexes_1
    public static class Products_AllProperties extends AbstractIndexCreationTask {
        public static class Result {
            private String query;

            public String getQuery() {
                return query;
            }

            public void setQuery(String query) {
                this.query = query;
            }
        }

        public Products_AllProperties() {
            map = "docs.Products.Select(product => new { " +
                // convert product to JSON and select all properties from it
                "    Query = this.AsJson(product).Select(x => x.Value) " +
                "})";

            // mark 'query' field as analyzed which enables full text search operations
            index("Query", FieldIndexing.SEARCH);;
        }
    }
    //endregion

    //region indexes_3
    public static class Products_WithMetadata extends AbstractIndexCreationTask {
        public static class Result {
            private Date lastModified;

            public Date getLastModified() {
                return lastModified;
            }

            public void setLastModified(Date lastModified) {
                this.lastModified = lastModified;
            }
        }

        public Products_WithMetadata() {
            map = "docs.Products.Select(product => new { " +
                "    Product = Product, " +
                "    Metadata = this.MetadataFor(product) " +
                "}).Select(this0 => new { " +
                "    LastModified = this0.metadata.Value<DateTime>(\"Last-Modified\") " +
                "})";
        }
    }
    //endregion

    public Metadata() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region indexes_2
                List<Product> results = session
                    .query(Products_AllProperties.Result.class, Products_AllProperties.class)
                    .whereEquals("Query", "Chocolade")
                    .ofType(Product.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region indexes_4
                List<Product> results = session
                    .query(Products_WithMetadata.Result.class, Products_WithMetadata.class)
                    .orderByDescending("LastModified")
                    .ofType(Product.class)
                    .toList();
                //endregion
            }
        }
    }
}
