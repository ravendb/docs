from typing import TypeVar

from ravendb import DocumentQuery

from examples_base import ExampleBase, Employee

_T = TypeVar("_T")


class ProximitySearch(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_examples(self):
        with self.embedded_server.get_document_store("Proximity") as store:
            with store.open_session() as session:
                # region proximity_1
                employees = list(
                    session.advanced.document_query(object_type=Employee)
                    # Make a full-text search with search terms
                    .search("Notes", "fluent french")
                    # Call 'proximity' with 0 distance
                    .proximity(0)
                )
                # Running the above query on the Northwind sample data returns the following Employee documents:
                # * employees/2-A
                # * employees/5-A
                # * employees/9-A
                #
                # Each resulting document has the text 'fluent in French' in its 'Notes' field.
                #
                # The word "in" is not taken into account as it is Not part of the terms list generated
                # by the analyzer. (Search is case-insensitive in this case).
                #
                # Note:
                # A document containing text with the search terms appearing with no words in between them
                # (e.g. "fluent french") would have also been returned.
                # endregion

            with store.open_session() as session:
                # region proximity_3
                employees = list(
                    session.advanced.document_query(object_type=Employee)
                    # Make a full-text search with search terms
                    .search("Notes", "fluent french")
                    # Call 'proximity' with 0 distance
                    .proximity(4)
                )
                # Running the above query on the Northwind sample data returns the following Employee documents:
                # * employees/2-A
                # * employees/5-A
                # * employees/6-A
                # * employees/9-A
                #
                # This time document 'employees/6-A' was added to the previous results since it contains the phrase:
                # "fluent in Japanese and can read and write French"
                # where the search terms are separated by a count of 4 terms.
                #
                # "in" & "and" are not taken into account as they are not part of the terms list generated
                # by the analyzer.(Search is case-insensitive in this case).
                # endregion

    class Foo:
        # region syntax
        def proximity(self, proximity: int) -> DocumentQuery[_T]: ...

        # endregion
