from ravendb import AbstractIndexCreationTask
from ravendb.infrastructure.orders import Order

from examples_base import ExampleBase


# region the_index
# Define a static index on the 'Orders' collection
# ================================================


class Orders_ByFreight(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, freight: int = None, Id: str = None):
            self.freight = freight
            self.Id = Id

    def __init__(self):
        # Call super().__init__() to initialize your index class
        super().__init__()
        # Define the index Map function
        self.map = "from o in docs.Orders select new { freight = o.freight, Id = o.Id }"


# endregion


class FilterByNonExistingField(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("FilterByNonExistingField") as store:
            with store.open_session() as session:
                Orders_ByFreight().execute(store)
                self.add_orders(session)

    def test_filter_by_non_existing_field(self):
        with self.embedded_server.get_document_store("FilterByNonExistingField") as store:
            with store.open_session() as session:
                # region whereNotExists_1
                orders_without_freight_field = list(
                    session
                    # Define a DocumentQuery on 'Orders' collection
                    .document_query(object_type=Order)
                    # Search for documents that do not contain field 'freight'
                    .not_().where_exists("freight")
                )

                # Results will be only the documents that do not contain the 'freight' field in 'Orders' collection
                # endregion

            with store.open_session() as session:
                # region whereNotExists_2
                # Query the index
                # ===============
                fields = list(session.query_index_type(Orders_ByFreight, Orders_ByFreight.IndexEntry))
                orders_without_freight_field = list(
                    session
                    # Define a DocumentQuery on the index
                    .query_index_type(Orders_ByFreight, Orders_ByFreight.IndexEntry)
                    # Verify the index is not stale (optional)
                    .wait_for_non_stale_results()
                    # Search for documents that do not contain field 'freight'
                    .not_().where_exists("freight")
                )

                # Results will be only the documents that do not contain the 'freight' field in 'Orders' collection
                # endregion
