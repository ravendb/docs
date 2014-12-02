package net.ravendb.clientapi.session.querying;

import java.util.List;

import com.mysema.query.annotations.QueryEntity;

import net.ravendb.abstractions.basic.Reference;
import net.ravendb.client.FieldHighlightings;
import net.ravendb.client.IDocumentQueryCustomization;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentQueryCustomizationFactory;
import net.ravendb.client.document.DocumentStore;


public class HowToUseHighlighting {
  @QueryEntity
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

  @SuppressWarnings("unused")
  private interface IFoo {
    //region highlight_1
    public IDocumentQueryCustomization highlight(String fieldName, int fragmentLength, int fragmentCount, String fragmentsField);

    public IDocumentQueryCustomization highlight(String fieldName, int fragmentLength, int fragmentCount, Reference<FieldHighlightings> highlightings);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToUseHighlighting() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region highlight_2
        QHowToUseHighlighting_SearchItem s = QHowToUseHighlighting_SearchItem.searchItem;
        Reference<FieldHighlightings> highlightingsRef = new Reference<>();
        List<SearchItem> results = session
          .query(SearchItem.class, "ContentSearchIndex")
          .customize(new DocumentQueryCustomizationFactory().highlight("Text", 128, 1, highlightingsRef))
          .search(s.text, "raven")
          .toList();

        StringBuilder builder = new StringBuilder();
        builder.append("<ul>");

        for (SearchItem result : results) {
          String[] fragments = highlightingsRef.value.getFragments(result.getId());
          builder.append("<li>" + fragments[0] + "</li>");
        }
        builder.append("</ul>");

        String ul = builder.toString();
        //endregion
      }
    }
  }
}
