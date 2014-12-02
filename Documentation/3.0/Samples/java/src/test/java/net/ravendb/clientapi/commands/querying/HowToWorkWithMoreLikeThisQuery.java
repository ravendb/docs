package net.ravendb.clientapi.commands.querying;

import net.ravendb.abstractions.data.MoreLikeThisQuery;
import net.ravendb.abstractions.data.MultiLoadResult;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class HowToWorkWithMoreLikeThisQuery {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region more_like_this_1
    public MultiLoadResult moreLikeThis(MoreLikeThisQuery query);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToWorkWithMoreLikeThisQuery() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region more_like_this_2
      // Search for similar documents to 'articles/1'
      // using 'Articles/MoreLikeThis' index and search only field 'Body'
      MoreLikeThisQuery moreLikeThisQuery = new MoreLikeThisQuery();
      moreLikeThisQuery.setIndexName("Articles/MoreLikeThis");
      moreLikeThisQuery.setDocumentId("articles/1");
      moreLikeThisQuery.setFields(new String[] { "Body" });
      MultiLoadResult result = store.getDatabaseCommands().moreLikeThis(moreLikeThisQuery);
      //endregion
    }
  }
}
