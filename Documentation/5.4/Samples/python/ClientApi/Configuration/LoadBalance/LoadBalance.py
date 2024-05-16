from ravendb import (
    DocumentStore,
    LoadBalanceBehavior,
    ClientConfiguration,
    PutClientConfigurationOperation,
    PutServerWideClientConfigurationOperation,
)
from ravendb.documents.conventions import DocumentConventions

from examples_base import ExampleBase, Employee


class LoadBalance(ExampleBase):
    def setUp(self):
        super().setUp()

    # region LoadBalance_6
    # A customized method for getting a default context string
    def get_default_context(self, db_name: str) -> str:
        # Method is invoked by RavenDB with the database name
        # Use that name - or return any string of your choice
        return "DefaultContextString"

    # endregion
    def test_load_balance(self):
        get_default_context = self.get_default_context
        with self.embedded_server.get_document_store("LoadBalance") as store:
            # region LoadBalance_1
            # Initialize 'LoadBalanceBehavior' on the client:
            document_store = DocumentStore(
                urls=["ServerURL_1", "ServerURL_2", "..."],
                database="DefaultDB",
            )
            conventions = DocumentConventions()

            # Enable the session-context feature
            # If this is not enabled then a context string set in a session will be ignored
            conventions.load_balance_behavior = LoadBalanceBehavior.USE_SESSION_CONTEXT

            # Assign a method that sets the default context string
            # This string will be used for sessions that do Not provide a context string
            # A sample GetDefaultContext method is defined below
            conventions.load_balancer_per_session_context_selector = get_default_context

            # Set a seed
            # The seed is 0 by default, provide any number to override
            conventions.load_balancer_context_seed = 5

            document_store.conventions = conventions
            document_store.initialize()

        # endregion

        # region LoadBalance_2
        # Open a session that will use the DEFAULT store values:
        with document_store.open_session() as session:
            # For all Read & Write requests made in this session
            # node to access is calculated from string & seed values defined on the store
            employee = session.load("employees/1-A", Employee)
        # endregion

        # region LoadBalance_3
        # Open a session that will use a UNIQUE context string:
        with document_store.open_session() as session:
            # Call context, pass a unique context string for this session
            session.advanced.session_info.context = "SomeOtherContext"

            # For all Read & Write requests made in this session,
            # node to access is calculated from the unique string & the seed defined on the store
            employee = session.load("employees/1-A", Employee)
        # endregion

        # region LoadBalance_4
        # Setting 'LoadBalanceBehavior' on the server by sending an operation:
        with document_store:
            # Define the client configuration to put on the server
            configuration_to_save = ClientConfiguration()
            # Enable the session-context feature
            # If this is not enabled then a context string set in a session will be ignored
            configuration_to_save.load_balance_behavior = LoadBalanceBehavior.USE_SESSION_CONTEXT

            # Set a seed
            # The seed is 0 by default, provide any number to override
            load_balancer_context_seed = 10

            # NOTE:
            # The session's context string is Not set on the server
            # You still need to set it on the client:
            # * either as a convention on the document store
            # * or pass it to 'SetContext' method on the session

            # Configuration will be in effect when Disabled is set to false
            configuration_to_save.disabled = False

            # Define the put configuration operation for the DEFAULT database
            put_configuration_op = PutClientConfigurationOperation(configuration_to_save)

            # Execute the operation by passing it to maintenance.send
            document_store.maintenance.send(put_configuration_op)

            #  After the operation has executed:
            #  all Read & Write requests, per session, will address the node calculated from:
            #    * the seed set on the server &
            #    * the session's context string set on the client
        # endregion

        # region LoadBalance_5
        with document_store:
            # Define the client configuration to put on the server
            configuration_to_save = ClientConfiguration()
            # Enable the session-context feature
            # If this is not enabled then a context string set in a session will be ignored
            configuration_to_save.load_balance_behavior = LoadBalanceBehavior.USE_SESSION_CONTEXT

            # Set a seed
            # The seed is 0 by default, provide any number to override
            load_balancer_context_seed = 10

            # NOTE:
            # The session's context string is Not set on the server
            # You still need to set it on the client:
            # * either as a convention on the document store
            # * or pass it to 'SetContext' method on the session

            # Configuration will be in effect when Disabled is set to false
            configuration_to_save.disabled = False

            # Define the put configuration operation for ALL databases
            put_configuration_op = PutServerWideClientConfigurationOperation(configuration_to_save)

            # Execute the operation by passing it to maintenance.server.send
            document_store.maintenance.server.send(put_configuration_op)

            #  After the operation has executed:
            #  all Read & Write requests, per session, will address the node calculated from:
            #    * the seed set on the server &
            #    * the session's context string set on the client
        # endregion
