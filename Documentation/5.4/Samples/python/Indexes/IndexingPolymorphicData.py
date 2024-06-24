from abc import ABC
from typing import Type

from ravendb import DocumentStore
from ravendb.documents.conventions import DocumentConventions
from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractMultiMapIndexCreationTask

from examples_base import ExampleBase


class Animal(ABC):
    def __init__(self, name: str = None):
        self.name = name


class Dog(Animal): ...


class Cat(Animal): ...


# region multi_map_1
class Animals_ByName(AbstractMultiMapIndexCreationTask):
    def __init__(self):
        super().__init__()
        self._add_map("from c in docs.Cats select new { c.name }")
        self._add_map("from d in docs.Dogs select new { d.name }")


# endregion


class MultiMapIndexes(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_multi_map_indexes(self):
        with self.embedded_server.get_document_store("MultiMapIndexes") as store:
            with store.open_session() as session:
                Animals_ByName().execute(store)
                session.store(Cat("Mitzy"))
                session.store(Dog("Mitzy"))
                session.save_changes()
                # region multi_map_2
                results = list(session.query_index_type(Animals_ByName, Animal).where_equals("name", "Mitzy"))
                # endregion

        # region other_ways_1
        store = DocumentStore()

        def _custom_find_collection_name(object_type: Type) -> str:
            if issubclass(object_type, Animal):
                return "Animals"
            return DocumentConventions.default_get_collection_name(object_type)

        store.conventions.find_collection_name = _custom_find_collection_name
        # endregion
