from ravendb.infrastructure.orders import Order

from examples_base import ExampleBase


class CountQueryResults(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_can_use_count(self):
        with self.embedded_server.get_document_store("CountQueryResult") as store:
            with store.open_session() as session:
                # region count_3
                number_of_orders = (
                    session.advanced.document_query(object_type=Order).where_equals("ship_to.country", "UK").count()
                )
                # The query returns the NUMBER of orders shipped to UK (int)
                # endregion
