package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.IndexDefinition;
import net.ravendb.client.documents.operations.indexes.PutIndexesOperation;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Collections;
import java.util.List;

public class IndexingHierarchicalData {
    //region indexes_1
    public static class BlogPost {
        private String author;
        private String title;
        private String text;
        private List<BlogPostComment> comments;

        public String getAuthor() {
            return author;
        }

        public void setAuthor(String author) {
            this.author = author;
        }

        public String getTitle() {
            return title;
        }

        public void setTitle(String title) {
            this.title = title;
        }

        public String getText() {
            return text;
        }

        public void setText(String text) {
            this.text = text;
        }

        public List<BlogPostComment> getComments() {
            return comments;
        }

        public void setComments(List<BlogPostComment> comments) {
            this.comments = comments;
        }
    }

    public static class BlogPostComment {
        private String author;
        private String text;
        private List<BlogPostComment> comments;


        public String getAuthor() {
            return author;
        }

        public void setAuthor(String author) {
            this.author = author;
        }

        public String getText() {
            return text;
        }

        public void setText(String text) {
            this.text = text;
        }

        public List<BlogPostComment> getComments() {
            return comments;
        }

        public void setComments(List<BlogPostComment> comments) {
            this.comments = comments;
        }
    }
    //endregion

    //region indexes_2
    public static class BlogPosts_ByCommentAuthor extends AbstractIndexCreationTask {
        public BlogPosts_ByCommentAuthor() {
            map = "docs.BlogPosts.Select(post => new { " +
                "    authors = this.Recurse(post, x => x.comments).Select(x0 => x0.author) " +
                "})";
        }
    }
    //endregion


    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {

            try (IDocumentSession session = store.openSession()) {
                //region indexes_3
                IndexDefinition indexDefinition = new IndexDefinition();
                indexDefinition.setName("BlogPosts/ByCommentAuthor");
                indexDefinition.setMaps(Collections.singleton(
                    "from post in docs.Posts" +
                        "  from comment in Recurse(post, (Func<dynamic, dynamic>)(x => x.comments)) " +
                        "  select new " +
                        "  { " +
                        "      author = comment.author " +
                        "  }"
                ));
                store.maintenance().send(new PutIndexesOperation(indexDefinition));
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region indexes_4
                List<BlogPost> results = session
                    .query(BlogPost.class, BlogPosts_ByCommentAuthor.class)
                    .whereEquals("authors", "Ayende Rahien")
                    .toList();
                //endregion
            }
        }
    }
}
