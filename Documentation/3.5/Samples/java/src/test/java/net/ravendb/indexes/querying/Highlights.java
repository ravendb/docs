package net.ravendb.indexes.querying;

import java.util.List;

import com.mysema.query.types.Expression;
import com.mysema.query.types.path.ListPath;

import net.ravendb.abstractions.basic.Reference;
import net.ravendb.abstractions.indexing.FieldIndexing;
import net.ravendb.abstractions.indexing.FieldStorage;
import net.ravendb.abstractions.indexing.FieldTermVector;
import net.ravendb.client.FieldHighlightings;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.BlogPost;
import net.ravendb.samples.QBlogPost;


public class Highlights {

  @SuppressWarnings("unused")
  private interface IHighlights<TSelf> {

    //region highlights_3
    public TSelf highlight(String fieldName, int fragmentLength, int fragmentCount, String fragmentsField);

    public TSelf highlight(String fieldName, int fragmentLength, int fragmentCount, Reference<FieldHighlightings> highlightings);

    public <TValue> TSelf highlight(Expression<?> propertySelector, int fragmentLength, int fragmentCount, ListPath<?, ?> fragmentsPropertySelector);

    public <TValue> TSelf highlight(Expression<?> propertySelector, int fragmentLength, int fragmentCount, Reference<FieldHighlightings> highlightings);
    //endregion

    //region highlights_4
    public TSelf setHighlighterTags(String preTag, String postTag);

    public TSelf setHighlighterTags(String[] preTags, String[] postTags);
    //endregion
  }

  //region highlights_1
  public static class BlogPosts_ByContent extends AbstractIndexCreationTask {
    public BlogPosts_ByContent() {
      QBlogPost b = QBlogPost.blogPost;
      map =
       " from post in posts " +
       " select new         " +
       "  {                 " +
       "      post.Content  " +
       "  };";
      index(b.content, FieldIndexing.ANALYZED);
      store(b.content, FieldStorage.YES);
      termVector(b.content, FieldTermVector.WITH_POSITIONS_AND_OFFSETS);
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
      QBlogPost b = QBlogPost.blogPost;
      map =
       " from post in posts " +
       " select new         " +
       "  {                 " +
       "      post.Category, " +
       "      post.Content  " +
       "  };";

      reduce =
        " from result in results " +
        " group result by result.Category into g " +
        " select new " +
        " { " +
        "    Category = g.Key, " +
        "    Content = string.Join(\" \", g.Select(r => r.Content " +
        " }";
      index(b.content, FieldIndexing.ANALYZED);
      store(b.content, FieldStorage.YES);
      termVector(b.content, FieldTermVector.WITH_POSITIONS_AND_OFFSETS);
    }
  }
  //endregion

  @SuppressWarnings("unused")
  public Highlights() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region highlights_2
        Reference<FieldHighlightings> highlightingsRef = new Reference<>();

        List<BlogPost> results = session
          .advanced()
          .documentQuery(BlogPost.class, BlogPosts_ByContent.class)
          .highlight("Content", 128, 1, highlightingsRef)
          .search("Content", "raven")
          .toList();

        StringBuilder builder = new StringBuilder();
        builder.append("<ul>\n");
        for (BlogPost result : results) {
          String[] fragments = highlightingsRef.value.getFragments(result.getId());
          builder.append("<li>");
          builder.append(fragments[0]);
          builder.append("</li>");
        }

        String ul = builder.append("</ul>").toString();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region highlights_5
        Reference<FieldHighlightings> highlightingsRef = new Reference<>();

        List<BlogPost> results = session
          .advanced()
          .documentQuery(BlogPost.class, BlogPosts_ByContent.class)
          .highlight("Content", 128, 1, highlightingsRef)
          .setHighlighterTags("**", "**")
          .search("Content", "raven")
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region highlights_6
        Reference<FieldHighlightings> highlightingsRef = new Reference<>();

        List<BlogPosts_ByCategory_Content.Result> results = session
          .advanced()
          .documentQuery(BlogPost.class, BlogPosts_ByCategory_Content.class)
          .highlight("Content", 128, 1, highlightingsRef)
          .setHighlighterTags("**", "**")
          .search("Content", "raven")
          .selectFields(BlogPosts_ByCategory_Content.Result.class)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region highlights_8
        Reference<FieldHighlightings> highlightingsRef = new Reference<>();

        List<BlogPosts_ByCategory_Content.Result> results = session
          .advanced()
          .documentQuery(BlogPosts_ByCategory_Content.Result.class, BlogPosts_ByCategory_Content.class)
          .highlight("Content", "Category", 128, 1, highlightingsRef) // highlighting 'Content', but marking 'Category' as key
          .setHighlighterTags("**", "**")
          .search("Content", "raven")
          .selectFields(BlogPosts_ByCategory_Content.Result.class)
          .toList();

        String[] newsHighlightings = highlightingsRef.value.getFragments("News"); // get fragments for 'News' category
        //endregion
      }
    }
  }
}
