package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.queries.moreLikeThis.IMoreLikeThisBuilderForDocumentQuery;
import net.ravendb.client.documents.queries.moreLikeThis.IMoreLikeThisOperations;
import net.ravendb.client.documents.queries.moreLikeThis.MoreLikeThisBase;
import net.ravendb.client.documents.queries.moreLikeThis.MoreLikeThisOptions;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.IFilterDocumentQueryBase;

import java.util.List;
import java.util.function.Consumer;

public class MoreLikeThis {

    private class Foo {
        //region more_like_this_2
        private Integer minimumTermFrequency = 2;
        private Integer maximumQueryTerms = 25;
        private Integer maximumNumberOfTokensParsed = 5000;
        private Integer minimumWordLength = 0;
        private Integer maximumWordLength = 0;
        private Integer minimumDocumentFrequency = 5;
        private Integer maximumDocumentFrequency = Integer.MAX_VALUE;
        private Integer maximumDocumentFrequencyPercentage;
        private Boolean boost = false;
        private Float boostFactor = 1f;
        private String stopWordsDocumentId;
        private String[] fields;

        // getters and setters
        //endregion
    }

    private interface IFoo2<T> {
        //region more_like_this_1
        IDocumentQuery<T> moreLikeThis(MoreLikeThisBase moreLikeThis);

        IDocumentQuery<T> moreLikeThis(Consumer<IMoreLikeThisBuilderForDocumentQuery<T>> builder);
        //endregion

        //region more_like_this_3
        IMoreLikeThisOperations<T> usingAnyDocument();

        IMoreLikeThisOperations<T> usingDocument(String documentJson);

        IMoreLikeThisOperations<T> usingDocument(Consumer<IFilterDocumentQueryBase<T, IDocumentQuery<T>>> builder);

        IMoreLikeThisOperations<T> withOptions(MoreLikeThisOptions options);
        //endregion
    }

    private class Article {
        private String id;
        private String category;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getCategory() {
            return category;
        }

        public void setCategory(String category) {
            this.category = category;
        }
    }

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region more_like_this_4
                // Search for similar articles to 'articles/1'
                // using 'Articles/MoreLikeThis' index and search only field 'body'
                MoreLikeThisOptions options = new MoreLikeThisOptions();
                options.setFields(new String[]{ "body" });

                List<Article> articles = session
                    .query(Article.class, Query.index("Articles/MoreLikeThis"))
                    .moreLikeThis(builder -> builder
                        .usingDocument(x -> x.whereEquals("id()", "articles/1"))
                        .withOptions(options))
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region more_like_this_6
                // Search for similar articles to 'articles/1'
                // using 'Articles/MoreLikeThis' index and search only field 'body'
                // where article category is 'IT'
                MoreLikeThisOptions options = new MoreLikeThisOptions();
                options.setFields(new String[]{ "body" });
                List<Article> articles = session
                    .query(Article.class, Query.index("Articles/MoreLikeThis"))
                    .moreLikeThis(builder -> builder
                        .usingDocument(x -> x.whereEquals("id()", "articles/1"))
                        .withOptions(options))
                    .whereEquals("category", "IT")
                    .toList();
                //endregion
            }
        }
    }
}
