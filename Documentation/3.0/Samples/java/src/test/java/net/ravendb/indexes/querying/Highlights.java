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
    }
  }
}
