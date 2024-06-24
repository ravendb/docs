# region more_like_this_4
from ravendb import AbstractIndexCreationTask, MoreLikeThisOptions
from ravendb.documents.indexes.definitions import FieldStorage
from ravendb.documents.queries.more_like_this import MoreLikeThisStopWords

from examples_base import ExampleBase


class Article:
    def __init__(self, Id: str = None, name: str = None, article_type: str = None):
        self.Id = Id
        self.name = name
        self.article_type = article_type


class Articles_ByArticleBody(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from doc in docs.Articles select { doc.article_body }"
        self._store("article_body", FieldStorage.YES)
        self._analyze("article_body", "StandardAnalyzer")


# endregion


class MoreLikeThisExample(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_more_like_this(self):
        with self.embedded_server.get_document_store("MoreLikeThisIndex") as store:
            with store.open_session() as session:
                # region more_like_this_1
                articles = list(
                    session.query_index_type(Articles_ByArticleBody, Article).more_like_this(
                        lambda builder: builder.using_document(lambda x: x.where_equals("id()", "articles/1"))
                    )
                )
                # endregion

                # region more_like_this_2
                options = MoreLikeThisOptions(fields=["article_body"])
                articles = list(
                    session.query_index_type(Articles_ByArticleBody, Article).more_like_this(
                        lambda builder: builder.using_document(
                            lambda x: x.where_equals("id()", "articles/1")
                        ).with_options(options)
                    )
                )

                # endregion
                # region more_like_this_3
                stop_words = MoreLikeThisStopWords(stop_words=["I", "A", "Be"])
                session.store(stop_words, "Config/Stopwords")
                # endregion
