from typing import TypeVar, Union, Optional

from ravendb import DocumentQuery, MethodCall
from ravendb.infrastructure.orders import Employee

from examples_base import ExampleBase, Employee

_T = TypeVar("_T")


class Foo:
    # region whereExists_syntax
    def where_exists(self, field_name: str) -> DocumentQuery[_T]: ...

    # endregion


class HowToFilterByField(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_examples(self):
        with self.embedded_server.get_document_store("FilterByField") as store:
            with store.open_session() as session:
                # region whereExists_1
                # Only documents that contain field 'first_name' will be returned
                results = list(session.advanced.document_query(object_type=Employee).where_exists("first_name"))
                # endregion

            with store.open_session() as session:
                # region whereExists_2
                results = list(
                    session.advanced.document_query(object_type=Employee).where_exists("address.location.latitude")
                )
                # endregion
