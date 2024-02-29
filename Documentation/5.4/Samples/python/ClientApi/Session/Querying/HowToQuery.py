from typing import Type, Optional, TypeVar

from ravendb import DocumentQuery, RawDocumentQuery
from ravendb.documents.queries.misc import Query
from ravendb.infrastructure.orders import Employee, Product
from examples_base import ExamplesBase

_T = TypeVar("_T")
_TIndex = TypeVar("_TIndex")


class HowToQuery(ExamplesBase):
    def setUp(self):
        super().setUp()

    def test_examples(self):
        with self.embedded_server.get_document_store("HowToQuery") as store:
            with store.open_session() as session:
                # region query_1_1
                # This is a Full Collection Query
                # No auto-index is created since no filtering is applied

                all_employees = list(  # Execute the query
                    session.query(object_type=Employee)  # Query for all documents from 'Employees' collection
                )
                # endregion

            with store.open_session() as session:
                # region query_1_3
                # This is a Full Collection Query
                # No auto-index is created since no filtering is applied

                # Query for all documents from 'Employees' collection
                query = session.query(object_type=Employee)

                # Execute the query
                all_employees = list(query)

                # All 'Employee' entities are loaded and will be tracked by the session
                # endregion

            with store.open_session() as session:
                # region query_2_1
                # Query collection by document ID
                # No auto-index is created when querying only by ID￥

                employee = (
                    session.query(object_type=Employee)
                    .where_equals("Id", "employees/1-A")  # Query for specific document from 'Employees' collection
                    .first()  # Execute the query
                )

                # The resulting 'Employee' entity is loaded and will be tracked by the session
                # endregion

            with store.open_session() as session:
                # region query_2_3
                # Query collection by document ID
                # No auto-index is created when querying only by ID

                # Query for specific document from 'Employees' collection
                query = session.query(object_type=Employee).where_equals("Id", "employees/1-A")

                # Execute the query
                employee_result = query.first()

                # The resulting 'Employee' entity is loaded and will be tracked by the session
                # endregion

            with store.open_session() as session:
                # region query_3_1
                # Query collection - filter by document field

                # An auto-index will be created if there isn't already an existing auto-index
                # that indexes this document field

                employees = list(  # Execute the query
                    session.query(object_type=Employee).where_equals(
                        "first_name", "Robert"
                    )  # Query for all 'Employee' documents that match this predicate
                )

                # The resulting 'Employee' entities are loaded and will be tracked by the session
                # endregion

            with store.open_session() as session:
                # region query_3_3
                # Query collection - filter by document field

                # An auto-index will be created if there isn't already an existing auto-index
                # that indexes this document field

                # Query for all 'Employee' documents that match this predicate
                query = session.query(object_type=Employee).where_equals("first_name", "Robert")

                # Execute the query
                employees = list(query)

                # The resulting 'Employee' entities are loaded and will be tracked by the session
                # endregion

            with store.open_session() as session:
                # region query_4_1
                # Query collection - page results
                # No auto-index is created since no filtering is applied

                products = list()  # Execute the query

                # The resulting 'Product' entities are loaded and will be tracked by the session
                # endregion

            with store.open_session() as session:
                # region query_4_3
                # Query collection - page results
                # No auto-index is created since no filtering is applied

                query = (
                    session.query(object_type=Product).skip(5).take(10)  # Skip first 5 results
                )  # Load up to 10 entities from 'Products' collection

                # Execute the query
                products = list(query)

                # The resulting 'Product' entities are loaded and will be tracked by the session
                # endregion

            with store.open_session() as session:
                # region query_5_1
                # Query with DocumentQuery - filter by document field

                # An auto-index will be created if there isn't already an existing auto-index
                # that indexes this document field

                employees = list(  # Execute the query
                    session.advanced.document_query(object_type=Employee).where_equals(  # Use DocumentQuery
                        "first_name", "Robert"
                    )  # Query for all 'Employee' documents that match this predicate
                )

                # The resulting 'Employee' entities are loaded and will be tracked by the session
                # endregion

            with store.open_session() as session:
                # region query_6_1
                # Query with RawQuery - filter by document field

                # An auto-index will be created if there isn't already an existing auto-index
                # that indexes this document field

                employees = list(  # Execute the query
                    session.advanced.raw_query(
                        "from 'Employees' where first_name = 'Robert'", object_type=Employee
                    )  # Provide RQL to RawQuery
                )
                # The resulting 'Employee' entities are loaded and will be tracked by the session
                # endregion

        class Foo:
            # region syntax
            # Overloads for querying a collection OR an index:
            # ================================================

            def query(
                self, source: Optional[Query] = None, object_type: Optional[Type[_T]] = None
            ) -> DocumentQuery[_T]:
                ...

            def query_collection(
                self, collection_name: str, object_type: Optional[Type[_T]] = None
            ) -> DocumentQuery[_T]:
                ...

            def query_index(self, index_name: str, object_type: Optional[Type[_T]] = None) -> DocumentQuery[_T]:
                ...

            def document_query(
                self,
                index_name: str = None,
                collection_name: str = None,
                object_type: Type[_T] = None,
                is_map_reduce: bool = False,
            ) -> DocumentQuery[_T]:
                ...

            # Overloads for querying an index:
            # ================================
            def query_index_type(
                self, index_type: Type[_TIndex], object_type: Optional[Type[_T]] = None
            ) -> DocumentQuery[_T]:
                ...

            def document_query_from_index_type(
                self, index_type: Type[_TIndex], object_type: Type[_T]
            ) -> DocumentQuery[_T]:
                ...

            # RawQuery
            # ================================
            def raw_query(self, query: str, object_type: Optional[Type[_T]] = None) -> RawDocumentQuery[_T]:
                ...

            # endregion
