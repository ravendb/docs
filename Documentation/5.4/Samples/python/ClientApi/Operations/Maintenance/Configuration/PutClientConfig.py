from typing import Optional, Union

from ravendb import (
    DocumentStore,
    ClientConfiguration,
    ReadBalanceBehavior,
    PutClientConfigurationOperation,
    LoadBalanceBehavior,
)
from ravendb.documents.conventions import DocumentConventions
from ravendb.documents.operations.definitions import VoidMaintenanceOperation

from examples_base import ExampleBase


class PutClientConfig(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_put_client_config(self):
        with self.embedded_server.get_document_store("PutClientConfig") as store:
            # region put_config_1
            # You can customize the client-configuration options in the client
            # when creating the Document Store (this is optional):
            # =================================================================
            document_store = DocumentStore(urls=["ServerURL_1", "ServerURL_2", "..."], database="DefaultDB")
            document_store.conventions = DocumentConventions()

            # Initialize some client-configuration options:
            document_store.conventions.max_number_of_requests_per_session = 100
            document_store.conventions.identity_parts_separator = "$"
            # ...

            document_store.initialize()
            # endregion

            # region put_config_2
            # Override the initial client-configuration in the server using the put operation:
            # ================================================================================
            with document_store:
                # Define the client-configuration object
                client_configuration = ClientConfiguration()
                client_configuration.max_number_of_requests_per_session = 200
                client_configuration.read_balance_behavior = ReadBalanceBehavior.FASTEST_NODE
                # ...

            # Define the put client-configuration operation, pass the configuration
            put_client_config_op = PutClientConfigurationOperation(client_configuration)

            # Execute the operation by passing it to maintenance.send
            document_store.maintenance.send(put_client_config_op)
            # endregion


class Foo:
    # region syntax_1
    class PutClientConfigurationOperation(VoidMaintenanceOperation):
        def __init__(self, config: ClientConfiguration): ...

    # endregion
    # region syntax_2
    class ClientConfiguration:
        def __init__(self):
            self.__identity_parts_separator: Union[None, str] = None
            self.etag: int = 0
            self.disabled: bool = False
            self.max_number_of_requests_per_session: Optional[int] = None
            self.read_balance_behavior: Optional[ReadBalanceBehavior] = None
            self.load_balance_behavior: Optional[LoadBalanceBehavior] = None
            self.load_balancer_context_seed: Optional[int] = None

    # endregion
