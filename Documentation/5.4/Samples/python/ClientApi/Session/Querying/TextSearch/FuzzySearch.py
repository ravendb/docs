from typing import TypeVar

from ravendb import DocumentQuery

from examples_base import ExampleBase, Company

_T = TypeVar("_T")


class FuzzySearch(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_fuzzy(self):
        with self.embedded_server.get_document_store("FuzzySearch") as store:
            with store.open_session() as session:
                # region fuzzy_1
                companies = list(
                    session.advanced.document_query(object_type=Company)
                    # Query with a term that is misspelled
                    .where_equals("Name", "Ernts Hhandel")
                    # Call 'fuzzy'
                    # Pass the required similarity, a decimal param between 0.0 and 1.0
                    .fuzzy(0.5)
                )
                # Running the above query on the Northwind sample data returns document: companies/20-A
                # which contains "Ernst Handel" in its Name field.
                # endregion

    class Foo:
        # region syntax
        def fuzzy(self, fuzzy: float) -> DocumentQuery[_T]: ...

        # endregion
