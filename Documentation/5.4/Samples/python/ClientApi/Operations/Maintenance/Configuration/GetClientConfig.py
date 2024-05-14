from ravendb import GetClientConfigurationOperation, ClientConfiguration
from ravendb.documents.operations.definitions import MaintenanceOperation

from examples_base import ExampleBase


class GetClientConfig(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_client_config(self):
        with self.embedded_server.get_document_store("GetClientConfig") as store:
            # region get_config
            # Define the get client-configuration operation
            get_client_config_op = GetClientConfigurationOperation()

            # Execute the operation by passing it to maintenance.send
            result = store.maintenance.send(get_client_config_op)

            client_configuration = result.configuration
            # endregion


class Foo:
    # region syntax_1
    class GetClientConfigurationOperation(MaintenanceOperation): ...

    # no __init__ (default)
    # endregion
    # region syntax_2
    # Executing the operation returns the following object:
    class Result:
        def __init__(self, etag: int, configuration: ClientConfiguration):
            # The configuration Etag
            self.etag = etag
            # The current client-configuration deployed on the server for the database
            self.configuration = configuration

    # endregion
