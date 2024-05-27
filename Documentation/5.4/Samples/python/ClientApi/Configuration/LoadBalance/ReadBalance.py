from ravendb import (
    DocumentStore,
    ReadBalanceBehavior,
    ClientConfiguration,
    PutClientConfigurationOperation,
    PutServerWideClientConfigurationOperation,
)
from ravendb.documents.conventions import DocumentConventions

from examples_base import ExampleBase


class ReadBalance(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_read_balance(self):
        with self.embedded_server.get_document_store("ReadBalance") as store:
            # region ReadBalance_1
            # Initialize 'ReadBalanceBehavior' on the client:
            document_store = DocumentStore(
                urls=["ServerURL_1", "ServerURL_2", "..."],
                database="DefaultDB",
            )
            conventions = DocumentConventions()
            # With ReadBalanceBehavior set to: 'FastestNode':
            # Client READ requests will address the fastest node
            # Client WRITE requests will address the preferred node
            conventions.read_balance_behavior = ReadBalanceBehavior.FASTEST_NODE

            document_store.conventions = conventions
            # endregion

            # region ReadBalance_2
            # Setting 'ReadBalanceBehavior' on the server by sending an operation:
            with document_store:
                # Define the client configuration to put on the server
                client_configuration = ClientConfiguration()
                # Replace 'FastestNode' (from the example above) with 'RoundRobin'
                client_configuration.read_balance_behavior = ReadBalanceBehavior.ROUND_ROBIN

                # Define the put configuration operation for the DEFAULT database
                put_configuration_op = PutClientConfigurationOperation(client_configuration)

                # Execute the operation by passing it to maintenance.send
                document_store.maintenance.send(put_configuration_op)

                # After the operation has executed:
                # All WRITE requests will continue to address the preferred node
                # READ requests, per session, will address a different node based on the RoundRobin logic

            # endregion

            # region ReadBalance_3
            # Setting 'ReadBalanceBehavior' on the server by sending an operation:
            with document_store:
                # Define the client configuration to put on the server
                client_configuration = ClientConfiguration()
                # Replace 'FastestNode' (from the example above) with 'RoundRobin'
                client_configuration.read_balance_behavior = ReadBalanceBehavior.ROUND_ROBIN

                # Define the put configuration operation for the ALL databases
                put_configuration_op = PutServerWideClientConfigurationOperation(client_configuration)

                # Execute the operation by passing it to maintenance.server.send
                document_store.maintenance.server.send(put_configuration_op)

                # After the operation has executed:
                # All WRITE requests will continue to address the preferred node
                # READ requests, per session, will address a different node based on the RoundRobin logic
            # endregion
