from typing import List

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractJavaScriptIndexCreationTask

from examples_base import ExampleBase, Product

"""
public class LoadDocumentSyntax
        {
            private interface ILoadDocument
            {
                # region syntax
                T LoadDocument<T>(string relatedDocumentId);
                
                T LoadDocument<T>(string relatedDocumentId, string relatedCollectionName);

                T[] LoadDocument<T>(IEnumerable<string> relatedDocumentIds);

                T[] LoadDocument<T>(IEnumerable<string> relatedDocumentIds, string relatedCollectionName);
                # endregion
            }
        }
"""


# region indexing_related_documents_1
class Products_ByCategoryName(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, category_name: str = None):
            self.category_name = category_name

    def __init__(self):
        super().__init__()
        self.map = (
            "from product in docs.Products "
            'let category = this.LoadDocument(product.Category, "Categories") '
            "select new { category_name = category.Name }"
        )


# endregion
# region indexing_related_documents_1_JS
class Products_ByCategoryName_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            # Call method 'load' to load the related Category document
            # The document ID to load is specified by 'product.Category'
            # The Name field from the related Category document will be indexed
            """
            map('products', function(product) {
                let category = load(product.Category, 'Categories')
                return {
                    category_name: category.Name
                };
            })
            """
            # Since no_tracking was not specified,
            # then any change to either Products or Categories will trigger reindexing
        }


# endregion
# region indexing_related_documents_3
# The referencing document
class Author:
    def __init__(self, Id: str = None, name: str = None, book_ids: List[str] = None):
        self.Id = Id
        self.name = name

        # Referencing a list of related document IDs
        self.book_ids = book_ids


# The related document
class Book:
    def __init__(self, Id: str = None, name: str = None):
        self.Id = Id
        self.name = name


# endregion


# region indexing_related_documents_4
class Authors_ByBooks(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, book_names: List[str] = None):
            self.book_names = book_names

    def __init__(self):
        super().__init__()
        self.map = (
            "from author in docs.Authors "
            "select new "
            "{"
            # For each Book ID, call LoadDocument and index the book's name
            '    book_names = author.book_ids.Select(x => LoadDocument(x, "Books").Name)'
            "}"
        )
        # Since no_tracking was not specified,
        # then any change to either Authors or Books will trigger reindexing


# endregion


# region indexing_related_documents_4_JS
class Authors_ByBooks_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            # For each Book ID, call 'load' and index the book's name
            """
            map('Author', function(author) {
                return {
                    books: author.BooksIds.map(x => load(x, 'Books').Name)
                }
            })
            """
            # Since no_tracking was not specified,
            # then any change to either Authors or Books will trigger reindexing
        }


# endregion
# region indexing_related_documents_6
class Products_ByCategoryName_NoTracking(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, category_name: str = None):
            self.category_name = category_name

    def __init__(self):
        super().__init__()
        self.map = (
            "from product in docs.Products "
            # Call NoTracking.LoadDocument to load the related Category document w/o tracking
            'let category = NoTracking.LoadDocument(product.Category, "Categories") '
            "select new {"
            # Index the name field from the related Category document
            " category_name = category.Name "
            "}"
        )
        # Since NoTracking is used -
        # then only the changes to Products will trigger reindexing


# endregion


# region indexing_related_documents_6_JS
class Products_ByCategoryName_NoTracking_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            # Call 'noTracking.load' to load the related Category document w/o tracking
            """
            map('products', function(product) {
                let category = noTracking.load(product.Category, 'Categories')
                return {
                    category_name: category.Name
                };
            })
            """
        }
        # Since noTracking is used -
        # then only the changes to Products will trigger reindexing


# endregion


class IndexingRelatedDocuments(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_indexing_related_documents(self):
        with self.embedded_server.get_document_store("IndexingRelatedDocuments") as store:
            Products_ByCategoryName().execute(store)
            Products_ByCategoryName_NoTracking_JS().execute(store)
            Authors_ByBooks().execute(store)
            Authors_ByBooks_JS().execute(store)
            Products_ByCategoryName_NoTracking().execute(store)
            Products_ByCategoryName_NoTracking_JS().execute(store)

        with store.open_session() as session:
            # region indexing_related_documents_2
            matching_products = list(
                session.query_index_type(Products_ByCategoryName, Products_ByCategoryName.IndexEntry)
                .where_equals("category_name", "Beverages")
                .of_type(Product)
            )
            # endregion
            # region indexing_related_documents_5
            # Get all authors that have books with title: "The Witcher"
            matching_authors = list(
                session.query_index_type(Authors_ByBooks, Authors_ByBooks.IndexEntry)
                .where_in("book_names", ["The Witcher"])
                .of_type(Author)
            )
            # endregion
            # region indexing_related_documents_7
            matching_products = list(
                session.query_index_type(
                    Products_ByCategoryName_NoTracking, Products_ByCategoryName_NoTracking.IndexEntry
                )
                .where_equals("category_name", "Beverages")
                .of_type(Product)
            )
            # endregion
