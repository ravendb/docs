package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.indexes.FieldTermVector;
import net.ravendb.client.documents.queries.highlighting.HighlightingOptions;
import net.ravendb.client.documents.queries.highlighting.Highlightings;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.primitives.Reference;

import java.util.List;

public class HowToUseHighlighting {

    public static class ContentSearchIndex extends AbstractIndexCreationTask {
        public ContentSearchIndex() {
            map = "docs.Select(doc => new { " +
                "  text = doc.text" +
                "})";

            index("text", FieldIndexing.SEARCH);
            store("text", FieldStorage.YES);
            termVector("text", FieldTermVector.WITH_POSITIONS_AND_OFFSETS);
        }
    }

    public static class SearchItem {
        private String id;
        private String text;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getText() {
            return text;
        }

        public void setText(String text) {
            this.text = text;
        }
    }

    private class Foo {
        //region options
        private String groupKey;
        private String[] preTags;
        private String[] postTags;

        // getters and setters
        //endregion
    }

    private interface IFoo<T> {
        //region highlight_1
        IDocumentQuery<T> highlight(String fieldName,
                                    int fragmentLength,
                                    int fragmentCount,
                                    Reference<Highlightings> highlightings);

        IDocumentQuery<T> highlight(String fieldName,
                                    int fragmentLength,
                                    int fragmentCount,
                                    HighlightingOptions options,
                                    Reference<Highlightings> highlightings);

        //endregion
    }

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region highlight_2
                Reference<Highlightings> highlightingsRef = new Reference<>();
                List<SearchItem> results = session
                    .query(SearchItem.class, ContentSearchIndex.class)
                    .highlight("text", 128, 1, highlightingsRef)
                    .search("text", "raven")
                    .toList();

                StringBuilder builder = new StringBuilder();
                builder.append("<ul>");

                for (SearchItem result : results) {
                    String[] fragments = highlightingsRef.value.getFragments(result.getId());
                    builder.append("<li>")
                        .append(fragments[0])
                        .append("</li>");
                }

                builder.append("</ul>");
                String ul = builder.toString();
                //endregion
            }
        }
    }
}
