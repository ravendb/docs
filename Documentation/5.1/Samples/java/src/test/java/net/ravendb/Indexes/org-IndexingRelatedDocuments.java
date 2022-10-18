package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.IndexDefinition;
import net.ravendb.client.documents.operations.indexes.PutIndexesOperation;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Collections;
import java.util.List;

public class IndexingRelatedDocuments {

    private class Product {

    }

    //region indexing_related_documents_4
    public static class Book {
        private String id;
        private String name;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }

    public static class Author {
        private String id;
        private String name;
        private List<String> bookIds;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public List<String> getBookIds() {
            return bookIds;
        }

        public void setBookIds(List<String> bookIds) {
            this.bookIds = bookIds;
        }
    }
    //endregion

    //region indexing_related_documents_2
    public static class Products_ByCategoryName extends AbstractIndexCreationTask {
        public Products_ByCategoryName() {
            map = "docs.Products.Select(product => new { " +
                "    CategoryName = (this.LoadDocument(product.Category, \"Categories\")).Name " +
                "})";
        }
    }
    //endregion

    //region indexing_related_documents_5
    public static class Authors_ByNameAndBooks extends AbstractIndexCreationTask {
        public Authors_ByNameAndBooks() {
            map = "docs.Authors.Select(author => new { " +
                "    name = author.name, " +
                "    books = author.bookIds.Select(x => (this.LoadDocument(x, \"Books\")).name) " +
                "})";
        }
    }
    //endregion

    public IndexingRelatedDocuments() {
        try (IDocumentStore store = new DocumentStore()) {
            //region indexing_related_documents_3
            IndexDefinition indexDefinition = new IndexDefinition();
            indexDefinition.setName("Products/ByCategoryName");
            indexDefinition.setMaps(Collections.singleton("from product in products " +
                "   select new " +
                "   { " +
                "       CategoryName = LoadDocument(product.Category, \"\"Categories\"\").Name " +
                "   }"));

            store.maintenance().send(new PutIndexesOperation(indexDefinition));
            //endregion


            try (IDocumentSession session = store.openSession()) {
                //region indexing_related_documents_7
                List<Product> results = session
                    .query(Product.class, Products_ByCategoryName.class)
                    .whereEquals("CategoryName", "Beverages")
                    .toList();
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region indexing_related_documents_6
            IndexDefinition indexDefinition = new IndexDefinition();
            indexDefinition.setName("Authors/ByNameAndBooks");
            indexDefinition.setMaps(Collections.singleton("from author in docs.Authors " +
                "     select new " +
                "     { " +
                "         name = author.name, " +
                "         books = author.bookIds.Select(x => LoadDocument(x, \"\"Books\"\").id) " +
                "     }"));
            store.maintenance().send(new PutIndexesOperation(indexDefinition));
            //endregion

            try (IDocumentSession session = store.openSession()) {
                //region indexing_related_documents_8
                List<Author> results = session
                    .query(Author.class, Authors_ByNameAndBooks.class)
                    .whereEquals("name", "Andrzej Sapkowski")
                    .whereEquals("books", "The Witcher")
                    .toList();
                //endregion
            }
        }

    }

}
