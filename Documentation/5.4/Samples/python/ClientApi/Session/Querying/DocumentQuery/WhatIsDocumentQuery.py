from ravendb.infrastructure.orders import Employee, Product, Order, Company

from examples_base import ExampleBase


class TestHowToQuery(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_how_to_query(self):
        with self.embedded_server.get_document_store("HowToQuery") as store:
            with store.open_session() as session:
                # region documentQuery_1
                all_employees = list(session.advanced.document_query(object_type=Employee))
                # endregion

            with store.open_session() as session:
                try:
                    # region documentQuery_2
                    # Query collection by document ID
                    employee = (
                        session.advanced.document_query(object_type=Employee)
                        .where_equals("Id", "employees/1-A")
                        .first()
                    )
                    # endregion
                except Exception as e:
                    pass

            with store.open_session() as session:
                # region documentQuery_3
                # Query collection - filter by document field
                employees = list(
                    session.advanced.document_query(object_type=Employee).where_equals("first_name", "Robert")
                )
                # endregion

            with store.open_session() as session:
                # region documentQuery_4
                # Query collection - page results
                products = list(session.advanced.document_query(object_type=Product).skip(5).take(10))
                # endregion

            with store.open_session() as session:
                # region documentQuery_5
                # Define a document_query
                doc_query = session.advanced.document_query(object_type=Order)  # 'document_query' instance

                query_results = list(doc_query.where_greater_than("freight", 25))
                # endregion
