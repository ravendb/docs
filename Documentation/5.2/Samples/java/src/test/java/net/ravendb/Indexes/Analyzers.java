package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.indexes.IndexDefinitionBuilder;
import net.ravendb.client.documents.operations.indexes.PutIndexesOperation;

public class Analyzers {

    private class SnowballAnalyzer {

    }

    //region analyzers_1
    public static class BlogPosts_ByTagsAndContent extends AbstractIndexCreationTask {
        public BlogPosts_ByTagsAndContent() {
            map = "docs.Posts.Select(post => new { " +
                "    tags = post.tags, " +
                "    content = post.content " +
                "})";
            analyze("tags", "SimpleAnalyzer");
            analyze("content", "Raven.Sample.SnowballAnalyzer");
        }
    }
    //endregion

    //region analyzers_3
    public static class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
        public Employees_ByFirstAndLastName() {
            map = "docs.Employees.Select(employee => new { " +
                "    LastName = employee.LastName, " +
                "    FirstName = employee.FirstName " +
                "})";

            index("FirstName", FieldIndexing.EXACT);
        }
    }
    //endregion

    //region analyzers_4
    public static class BlogPosts_ByContent extends AbstractIndexCreationTask {
        public BlogPosts_ByContent() {
            map = "docs.Posts.Select(post => new { " +
                "    tags = post.tags, " +
                "    content = post.content " +
                "})";

            index("content", FieldIndexing.SEARCH);
        }
    }
    //endregion

    //region analyzers_5
    public static class BlogPosts_ByTitle extends AbstractIndexCreationTask {
        public BlogPosts_ByTitle() {
            map = "docs.Posts.Select(post => new { " +
                "    tags = post.tags, " +
                "    content = post.content " +
                "})";

            index("content", FieldIndexing.NO);
            store("content", FieldStorage.YES);
        }
    }
    //endregion

    public Analyzers() {
        try (IDocumentStore store = new DocumentStore()) {
            //region analyzers_2
            IndexDefinitionBuilder builder = new IndexDefinitionBuilder("BlogPosts/ByTagsAndContent");
                builder.setMap( "docs.Posts.Select(post => new { " +
                    "    tags = post.tags, " +
                    "    content = post.content " +
                    "})");
                builder.getAnalyzersStrings().put("tags", "SimpleAnalyzer");
                builder.getAnalyzersStrings().put("content", "Raven.Sample.SnowballAnalyzer");

             store.maintenance()
                .send(new PutIndexesOperation(builder.toIndexDefinition(store.getConventions())));
            //endregion
        }
    }
}
