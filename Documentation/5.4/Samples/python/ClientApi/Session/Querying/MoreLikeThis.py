from typing import List, Union, Callable, TypeVar

from ravendb import (
    DocumentQuery,
    MoreLikeThisBase,
    MoreLikeThisBuilder,
    MoreLikeThisOperations,
    MoreLikeThisOptions,
    AbstractIndexCreationTask,
)

from examples_base import ExampleBase

_T = TypeVar("_T")


class Article:
    def __init__(self, Id: str = None, category: str = None):
        self.Id = Id
        self.category = category


class Articles_MoreLikeThis(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = 'from o in docs.Articles select new { Id = o.Id, category = o.category, body = "aaaa" }'


class MoreLikeThisExample(ExampleBase):
    def setUp(self):
        super().setUp()

    class Foo:
        # region more_like_this_2
        def __init__(
            self,
            minimum_term_frequency: int = None,
            maximum_query_terms: int = None,
            maximum_number_of_tokens_parsed: int = None,
            minimum_word_length: int = None,
            maximum_word_length: int = None,
            minimum_document_frequency: int = None,
            maximum_document_frequency: int = None,
            maximum_document_frequency_percentage: int = None,
            boost: bool = None,
            boost_factor: float = None,
            stop_words_document_id: str = None,
            fields: List[str] = None,
        ): ...

        # endregion

    class Foo2:
        # region more_like_this_1
        def more_like_this(
            self, more_like_this_or_builder: Union[MoreLikeThisBase, Callable[[MoreLikeThisBuilder[_T]], None]]
        ) -> DocumentQuery[_T]: ...

        # endregion
        # region more_like_this_3
        def using_any_document(self) -> MoreLikeThisOperations[_T]: ...

        def using_document(
            self, document_json_or_builder: Union[str, Callable[[DocumentQuery[_T]], None]]
        ) -> MoreLikeThisOperations[_T]: ...

        def with_options(self, options: MoreLikeThisOptions) -> MoreLikeThisOperations[_T]: ...

        # endregion

    def test_more_like_this(self):
        with self.embedded_server.get_document_store("MoreLikeThisExample") as store:
            Articles_MoreLikeThis().execute(store)
            with store.open_session() as session:
                session.store(Article("articles/1", "IT"))
                session.store(Article("articles/12", "ITl"))
                session.store(Article("articles/111", "trITl"))
                session.save_changes()
                # region more_like_this_4
                # Search for similar articles to 'articles/1'
                # using 'Articles/MoreLikeThis' index and search only field 'body'
                articles = list(
                    session.query_index_type(Articles_MoreLikeThis, Article).more_like_this(
                        lambda builder: builder.using_document(
                            lambda x: x.where_equals("Id", "articles/1")
                        ).with_options(MoreLikeThisOptions(fields=["body"]))
                    )
                )
                # endregion

            with store.open_session() as session:
                # region more_like_this_6
                # Search for similar articles to 'articles/1'
                # using 'Articles/MoreLikeThis' index and search only field 'body'
                # where article category is 'IT'
                articles = list(
                    session.query_index_type(Articles_MoreLikeThis, Article)
                    .more_like_this(
                        lambda builder: builder.using_document(
                            lambda x: x.where_equals("Id", "articles/1")
                        ).with_options(MoreLikeThisOptions(fields=["body"]))
                    )
                    .where_equals("category", "IT")
                )
                # endregion
