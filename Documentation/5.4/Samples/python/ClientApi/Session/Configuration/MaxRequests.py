from examples_base import ExampleBase


class MaxRequests(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_max_requests(self):
        with self.embedded_server.get_document_store("MaxRequests") as store:
            with store.open_session() as session:
                # region max_requests_1
                session._max_number_of_requests_per_session = 50
                # endregion

        # region max_requests_2
        store.conventions.max_number_of_requests_per_session = 100
        # endregion
