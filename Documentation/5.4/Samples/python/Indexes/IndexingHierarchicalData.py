from __future__ import annotations
from typing import List

from ravendb import AbstractIndexCreationTask, PutIndexesOperation, IndexDefinition

from examples_base import ExampleBase


# region indexes_1
class BlogPost:
    def __init__(self, author: str = None, title: str = None, text: str = None, comments: List[BlogPostComment] = None):
        self.author = author
        self.title = title
        self.text = text
        self.comments = comments


class BlogPostComment:
    def __init__(self, author: str = None, text: str = None, comments: List[BlogPostComment] = None):
        self.author = author
        self.text = text

        # Comments can be left recursively
        self.comments = comments


# endregion


# region indexes_2
class BlogPosts_ByCommentAuthor(AbstractIndexCreationTask):
    class Result:
        def __init__(self, authors: List[str] = None):
            self.authors = authors

    def __init__(self):
        super().__init__()
        self.map = "from blogpost in docs.Blogposts let authors = Recurse(blogpost, x => x.comments) select new { authors = authors.Select(x => x.author) }"


# endregion


class HierarchicalDataExample(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_hierarchical_data(self):
        with self.embedded_server.get_document_store("HierarchicalDataExample") as store:
            BlogPosts_ByCommentAuthor().execute(store)
            # region indexes_3
            store.maintenance.send(
                PutIndexesOperation(
                    IndexDefinition(
                        name="BlogPosts/ByCommentAuthor",
                        maps={
                            """from blogpost in docs.BlogPosts
from comment in Recurse(blogpost, (Func<dynamic, dynamic>)(x => x.comments))
select new
{
    author = comment.author
}"""
                        },
                    )
                )
            )
            # endregion

            with store.open_session() as session:
                # region indexes_4
                results = list(
                    session.query_index_type(BlogPosts_ByCommentAuthor, BlogPosts_ByCommentAuthor.Result).where_equals(
                        "authors", "John"
                    )
                )
                # endregion
