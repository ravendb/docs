package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.queries.moreLikeThis.MoreLikeThisOptions;
import net.ravendb.client.documents.queries.moreLikeThis.MoreLikeThisStopWords;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Arrays;
import java.util.List;

public class MoreLikeThis {

    //region more_like_this_4
    public class Article {
        private String id;
        private String name;
        private String articleBody;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getArticleBody() {
            return articleBody;
        }

        public void setArticleBody(String articleBody) {
            this.articleBody = articleBody;
        }
    }

    public class Articles_ByArticleBody extends AbstractIndexCreationTask {
        public Articles_ByArticleBody() {
            map = "from doc in docs.articles " +
                "select new {" +
                "   doc.articleBody " +
                "}";

            store("articleBody", FieldStorage.YES);
            analyze("articleBody", "StandardAnalyzer");
        }
    }
    //endregion

    public MoreLikeThis() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                {
                    //region more_like_this_1
                    List<Article> articles = session
                        .query(Article.class, Articles_ByArticleBody.class)
                        .moreLikeThis(builder -> builder.usingDocument(x -> x.whereEquals("id()", "articles/1")))
                        .toList();
                    //endregion
                }

                {
                    //region more_like_this_2
                    MoreLikeThisOptions options = new MoreLikeThisOptions();
                    options.setFields(new String[]{ "articleBody" });
                    List<Article> articles = session
                        .query(Article.class, Articles_ByArticleBody.class)
                        .moreLikeThis(builder -> builder
                            .usingDocument(x -> x.whereEquals("id()", "articles/1"))
                            .withOptions(options))
                        .toList();
                    //endregion
                }

                {
                    //region more_like_this_3
                    MoreLikeThisStopWords stopWords = new MoreLikeThisStopWords();
                    stopWords.setStopWords(Arrays.asList("I", "A", "Be"));
                    session.store(stopWords, "Config/Stopwords");
                    //endregion
                }

            }
        }
    }
}
