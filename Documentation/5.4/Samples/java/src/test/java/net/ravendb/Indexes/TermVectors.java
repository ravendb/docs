package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.*;
import net.ravendb.client.documents.operations.indexes.PutIndexesOperation;

public class TermVectors {

    public static class Foo {
        //region term_vectors_3
        public enum FieldTermVector {
            /**
             * Do not store term vectors
             */
            NO,

            /**
             * Store the term vectors of each document. A term vector is a list of the document's
             * terms and their number of occurrences in that document.
             */
            YES,
            /**
             * Store the term vector + token position information
             */
            WITH_POSITIONS,
            /**
             * Store the term vector + Token offset information
             */
            WITH_OFFSETS,

            /**
             * Store the term vector + Token position and offset information
             */
            WITH_POSITIONS_AND_OFFSETS
        }
        //endregion
    }



    //region term_vectors_1
    public static class BlogPosts_ByTagsAndContent extends AbstractIndexCreationTask {
        public BlogPosts_ByTagsAndContent() {
            map = "docs.Posts.Select(post => new { " +
                "    Tags = post.Tags, " +
                "    Content = post.Content " +
                "})";

            index("Content", FieldIndexing.SEARCH);
            termVector("Content", FieldTermVector.WITH_POSITIONS_AND_OFFSETS);
        }
    }
    //endregion

    public TermVectors() {
        try (IDocumentStore store = new DocumentStore()) {
            //region term_vectors_2
            IndexDefinitionBuilder builder = new IndexDefinitionBuilder("BlogPosts/ByTagsAndContent");
            builder.setMap("docs.Posts.Select(post => new { " +
                "    Tags = post.Tags, " +
                "    Content = post.Content " +
                "})");

            builder.getIndexesStrings().put("Content", FieldIndexing.SEARCH);
            builder.getTermVectorsStrings().put("Content", FieldTermVector.WITH_POSITIONS_AND_OFFSETS);

            IndexDefinition indexDefinition = builder.toIndexDefinition(store.getConventions());

            store.maintenance().send(new PutIndexesOperation(indexDefinition));
            //endregion
        }
    }
}
