from typing import TypeVar

from ravendb import DocumentQuery

from examples_base import ExampleBase, Product

_T = TypeVar("_T")


class UsingRegex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_example(self):
        with self.embedded_server.get_document_store("Regex") as store:
            with store.open_session() as session:
                # region regex_1
                # loads all products, which name
                # starts with 'N' or 'A'
                products = list(session.query(object_type=Product).where_regex("Name", "^[NA]"))
                # endregion


class Foo:
    # region syntax
    def where_regex(self, field_name: str, pattern: str) -> DocumentQuery[_T]: ...

    # endregion
