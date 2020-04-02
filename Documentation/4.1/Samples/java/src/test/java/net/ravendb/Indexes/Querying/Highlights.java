package net.ravendb.Indexes.Querying;

import net.ravendb.Blog;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.indexes.FieldTermVector;
import net.ravendb.client.documents.queries.highlighting.HighlightingOptions;
import net.ravendb.client.documents.queries.highlighting.Highlightings;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.primitives.Reference;

import java.util.List;

public class Highlights {

    //region highlights_1
    public static class BlogPosts_ByContent extends AbstractIndexCreationTask {
        public static class Result {
            private String content;

            public String getContent() {
                return content;
            }

            public void setContent(String content) {
                this.content = content;
            }
        }

        public BlogPosts_ByContent() {
            map = "docs.Posts.Select(post => new { post.content })";
            index("content", FieldIndexing.SEARCH);
            store("content", FieldStorage.YES);
            termVector("content", FieldTermVector.WITH_POSITIONS_AND_OFFSETS);
        }
    }
    //endregion

    //region highlights_7
    public static class BlogPosts_ByCategory_Content extends AbstractIndexCreationTask {
        public static class Result {
            private String category;
            private String content;

            public String getCategory() {
                return category;
            }

            public void setCategory(String category) {
                this.category = category;
            }

            public String getContent() {
                return content;
            }

            public void setContent(String content) {
                this.content = content;
            }
        }

        public BlogPosts_ByCategory_Content() {
            map = "docs.Posts.Select(post => new { post.category, post.content })";

            reduce = "results.GroupBy(result => result.Category).Select(g => new {" +
                " category = g.Key, " +
                " Content = string.Join(\" \", g.Select(r => r.content)) " +
                "}";

            index("content", FieldIndexing.SEARCH);
            store("content", FieldStorage.YES);
            termVector("content", FieldTermVector.WITH_POSITIONS_AND_OFFSETS);
        }
    }
    //endregion

    public Highlights() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region highlights_2
                Reference<Highlightings> highlightsRef = new Reference<>();
                List<Blog> results = session
                    .advanced()
                    .documentQuery(Blog.class, BlogPosts_ByContent.class)
                    .highlight("content", 128, 1, highlightsRef)
                    .search("content", "raven")
                    .toList();

                StringBuilder builder = new StringBuilder();
                builder.append("<ul>");

                for (Blog result : results) {
                    String[] fragments = highlightsRef.value.getFragments(result.getId());
                    builder.append("<li>")
                        .append(fragments[0])
                        .append("</li>");
                }

                builder.append("</ul>");
                String ul = builder.toString();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region highlights_6_2
                Reference<Highlightings> highlightsRef = new Reference<>();
                HighlightingOptions highlightingOptions = new HighlightingOptions();
                highlightingOptions.setPreTags(new String[] { "**" });
                highlightingOptions.setPostTags(new String[] { "**" });
                List<BlogPosts_ByContent.Result> results = session
                    .query(BlogPosts_ByContent.class, BlogPosts_ByContent.class)
                    .highlight("content", 128, 1, highlightingOptions, highlightsRef)
                    .search("content", "raven")
                    .selectFields(BlogPosts_ByContent.Result.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region highlights_8_2
                // highlighting 'content', but marking 'category' as key
                Reference<Highlightings> highlightsRef = new Reference<>();
                HighlightingOptions highlightingOptions = new HighlightingOptions();
                highlightingOptions.setPreTags(new String[] { "**" });
                highlightingOptions.setPostTags(new String[] { "**" });
                highlightingOptions.setGroupKey("category");
                List<BlogPosts_ByCategory_Content.Result> results = session
                    .advanced()
                    .documentQuery(BlogPosts_ByCategory_Content.Result.class, BlogPosts_ByCategory_Content.class)
                    .highlight("content", 128, 1, highlightingOptions, highlightsRef)
                    .search("content", "raven")
                    .toList();

                // get fragments for 'news' category
                String[] newsHighlightings = highlightsRef.value.getFragments("news");
                //endregion
            }
        }
    }
}
