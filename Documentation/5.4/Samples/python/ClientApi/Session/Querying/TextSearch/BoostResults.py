from typing import TypeVar

from ravendb import DocumentQuery
from ravendb.primitives import constants

from examples_base import ExampleBase, Employee, Company


_T = TypeVar("_T")


class BoostResults(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_examples(self):
        with self.embedded_server.get_document_store("Boosting") as store:
            with store.open_session() as session:
                # region boost_1
                employees = list(
                    session
                    # Make a dynamic full-text search Query on 'Employees' collection
                    .query(object_type=Employee)
                    # This search predicate will use the default boost value of 1
                    .search("Notes", "English")
                    # This search predicate will use a boost value of 10
                    .search("Notes", "Italian")
                    # Call 'boost' to set the boost value to previous 'search' call
                    .boost(10)
                )

                # * Results will contain all Employee documents that have
                #   EITHER 'English' OR 'Italian' in their 'Notes' field.
                #
                # * Matching documents with 'Italian' will be listed FIRST in the results,
                #   before those with 'English'.
                #
                # * Search is case-insensitive.
                # endregion

            with store.open_session() as session:
                # region boost_4
                companies = list(
                    session.advanced
                    # Make a dynamic DocumentQuery on 'Companies' collection
                    .document_query(object_type=Company)
                    # Define a 'where' condition
                    .where_starts_with("Name", "O")
                    # Call 'boost' to set the boost value of the previous 'where' predicate
                    .boost(10)
                    # Call 'or_else' so that OR operator will be used between statements
                    .or_else()
                    .where_starts_with("Name", "P")
                    .boost(50)
                    .or_else()
                    .where_ends_with("Name", "OP")
                    .boost(90)
                )

                # * Results will contain all Company documents that either
                #   (start-with 'O') OR (start-with 'P') OR (end-with 'OP') in their 'Name' field.
                #
                # * Matching documents the end-with 'OP' will be listed FIRST.
                #   Matching documents that start-with 'P' will then be listed.
                #   Matching documents that start-with 'O' will be listed LAST.
                #
                # * Search is case-insensitive.

                # endregion

        with store.open_session() as session:
            # region boost_6
            # Make a query:
            # =============

            employees = list(
                session.query(object_type=Employee).search("Notes", "English").search("Notes", "Italian").boost(10)
            )

            # Get the store:
            # =============

            # Call 'get_metadata_for', pass an entity from the resultng employees list
            metadata = session.advanced.get_metadata_for(employees[0])

            # Score is available in the '@index-score' metadata property
            score = metadata[constants.Documents.Metadata.INDEX_SCORE]
            # endregion

    class Foo:
        # region syntax
        def boost(self, boost: float) -> DocumentQuery[_T]: ...

        # endregion
