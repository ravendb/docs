from ravendb import SessionOptions, TransactionMode
from examples_base import ExampleBase


class HowToQuery(ExampleBase):
    class DNS:
        def __init__(self, ip_address: str = None):
            ip_address = ip_address

    def setUp(self):
        super().setUp()

    def test_sync(self):
        with self.embedded_server.get_document_store("ClusterTransactionCompareExchangeSync") as store:
            # region_cluster_session_sync
            with store.open_session(
                session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
            ) as session:
                # endregion
                # region new_compare_exchange_sync
                # The session must be first opened with cluster-wide mode
                session.advanced.cluster_transaction.create_compare_exchange_value(
                    key="Best NoSQL Transactional Database",
                    item="RavenDB",
                )

                session.save_changes()
                # endregion
                key = "key"
                key1 = "Best NoSQL Transactional Database"
                keys = ["key"]
                index = 0
                value = ""
                item = session.advanced.cluster_transaction.get_compare_exchange_value(key1)

                # region methods_1_sync
                session.advanced.cluster_transaction.get_compare_exchange_value(key)
                # endregion

                # region methods_2_sync
                session.advanced.cluster_transaction.get_compare_exchange_values(keys)
                # endregion

                # region methods_3_sync
                session.advanced.cluster_transaction.create_compare_exchange_value(key, value)
                # endregion

                # region methods_4_sync
                # Delete by key & index
                session.advanced.cluster_transaction.delete_compare_exchange_value(key, index)

                # Delete by compare-exchange item
                session.advanced.cluster_transaction.delete_compare_exchange_value(item)
                # endregion

                # region methods_sync_lazy_1
                # Single value
                session.advanced.cluster_transaction.lazily.get_compare_exchange_value(key)

                # Multiple values
                session.advanced.cluster_transaction.lazily.get_compare_exchange_values(keys)
                # endregion
