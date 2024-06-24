from abc import ABC
from typing import List

from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractMultiMapIndexCreationTask
from ravendb.documents.indexes.definitions import FieldIndexing, FieldStorage

from examples_base import ExampleBase


# region multi_map_3
class Animal(ABC):
    def __init__(self, name: str = None):
        self.name = name


# endregion


# region multi_map_1
class Dog(Animal): ...


# endregion


# region multi_map_2
class Cat(Animal): ...


# endregion


# region multi_map_4
class Animals_ByName(AbstractMultiMapIndexCreationTask):
    def __init__(self):
        super().__init__()
        self._add_map("from c in docs.Cats select new { c.name }")
        self._add_map("from d in docs.Dogs select new { d.name }")


# endregion


# region multi_map_1_0
class Smart_Search(AbstractMultiMapIndexCreationTask):
    class Result:
        def __init__(
            self, Id: str = None, display_name: str = None, collection: object = None, content: List[str] = None
        ):
            self.Id = Id
            self.display_name = display_name
            self.collection = collection
            self.content = content

    class Projection:
        def __init__(self, Id: str = None, display_name: str = None, collection: str = None):
            self.Id = Id
            self.display_name = display_name
            self.collection = collection

    def __init__(self):
        super().__init__()
        self._add_map(
            "from c in docs.Companies select new {"
            "Id = c.Id,"
            "content = new[]"
            "{"
            "    c.name"
            "},"
            "display_name=  c.name, "
            'collection = MetadataFor(c)["@collection"]'
            "}"
        )

        self._add_map(
            "from p in docs.Products select new {"
            "Id = p.Id,"
            "content = new[]"
            "{"
            "    p.name"
            "},"
            "display_name = p.name,"
            'collection = MetadataFor(p)["@collection"]'
            "}"
        )

        self._add_map(
            "from e in docs.Employees select new {"
            "Id = e.Id,"
            "content = new[]"
            "{"
            "    e.first_name,"
            "    e.last_name"
            "},"
            'display_name = e.first_name + " " + e.last_name,'
            'collection = MetadataFor(e)["@collection"]'
            "}"
        )

        # mark 'content' field as analyzed which enables full text search operations
        self._index("content", FieldIndexing.SEARCH)

        # storing fields so when projection (e.g. ProjectInto) requests only those fields,
        # data will come from index only, not from storage
        self._store("Id", FieldStorage.YES)
        self._store("display_name", FieldStorage.YES)
        self._store("collection", FieldStorage.YES)


# endregion


class MultiMap(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_multi_map_indexes(self):
        with self.embedded_server.get_document_store("MultiMapIndexes") as store:
            with store.open_session() as session:
                # region multi_map_7
                results = list(session.query_index_type(Animals_ByName, Animal).where_equals("name", "Mitzy"))
                # endregion
                # region multi_map_1_1
                results = list(
                    session.query_index_type(Smart_Search, Smart_Search.Result)
                    .search("content", "Lau*")
                    .select_fields(Smart_Search.Projection)
                )

                for result in results:
                    print(f"{result.collection}: {result.display_name}")
                    # Companies: Laughing Bacchus Wine Cellars
                    # Products: Laughing Lumberjack Lager
                    # Employees: Laura Callahan
                # endregion
