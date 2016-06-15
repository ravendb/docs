package net.ravendb.clientapi.session.howto;

import net.ravendb.abstractions.data.MoreLikeThisQuery;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.client.indexes.AbstractTransformerCreationTask;

public class MoreLikeThis {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region more_like_this_1
    public <T> T[] moreLikeThis(Class<T> entityClass, Class<? extends AbstractIndexCreationTask> indexCreator, String documentId);

    public <T> T[] moreLikeThis(Class<T> entityClass, Class<? extends AbstractIndexCreationTask> indexCreator, MoreLikeThisQuery parameters);

    public <T> T[] moreLikeThis(Class<T> entityClass, String index, String documentId);

    public <T> T[] moreLikeThis(Class<T> entityClass, Class<? extends AbstractIndexCreationTask> indexCreator, Class<? extends AbstractTransformerCreationTask> transformerClass, String documentId);

    public <T> T[] moreLikeThis(Class<T> entityClass, Class<? extends AbstractIndexCreationTask> indexCreator, Class<? extends AbstractTransformerCreationTask> transformerClass, MoreLikeThisQuery parameters);

    public <T> T[] moreLikeThis(Class<T> entityClass, String index, String transformer, String documentId);

    public <T> T[] moreLikeThis(Class<T> entityClass, String index, String transformer, MoreLikeThisQuery parameters);
    //endregion
  }

  private static class Article {
    //empty by design
  }

  @SuppressWarnings("unused")
  public MoreLikeThis() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region more_like_this_2
        // Search for similar articles to 'articles/1'
        // using 'Articles/MoreLikeThis' index and search only field 'Body'
        MoreLikeThisQuery moreLikeThisQuery = new MoreLikeThisQuery();
        moreLikeThisQuery.setIndexName("Articles/MoreLikeThis");
        moreLikeThisQuery.setDocumentId("articles/1");
        moreLikeThisQuery.setFields(new String[] { "Body" });

        Article[] articles = session.advanced()
          .moreLikeThis(Article.class, "Articles/MoreLikeThis", null, moreLikeThisQuery);
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region more_like_this_4
        // Search for similar articles to 'articles/1'
        // using 'Articles/MoreLikeThis' index and search only field 'Body'
        // where article category is 'IT'
        MoreLikeThisQuery moreLikeThisQuery = new MoreLikeThisQuery();
        moreLikeThisQuery.setIndexName("Articles/MoreLikeThis");
        moreLikeThisQuery.setDocumentId("articles/1");
        moreLikeThisQuery.setFields(new String[] { "Body" });
        moreLikeThisQuery.setAdditionalQuery("Category:IT");
        session.advanced().moreLikeThis(Article.class, "Articles/MoreLikeThis", null, moreLikeThisQuery);
        //endregion
      }
    }
  }
}
