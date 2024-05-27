from ravendb.serverwide.operations.common import DatabasePutResult, ServerOperation

from examples_base import ExampleBase


class Foo:
    # region syntax
    class AddDatabaseNodeOperation(ServerOperation[DatabasePutResult]):
        def __init__(self, database_name: str, node_tag: str = None): ...

    # endregion


class AddDatabaseNode(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_add_database_node(self):
        with self.embedded_server.get_document_store("AddDatabaseNode") as store:
            # region add_1
            # Create the AddDatabaseNodeOperation
            # Add a random node to 'Northwind' database-group
            add_database_node_op = AddDatabaseNodeOperation("Northwind")

            # Execute the operation by passing it to maintenance.server.send
            result = store.maintenance.server.send(add_database_node_op)

            # Can access the new topology
            number_of_replicas = len(result.topology.all_nodes)
            # endregion

            # region add_2
            # Create the AddDatabaseNodeOperation
            # Add node C to 'Northwind
            add_database_node_op = AddDatabaseNodeOperation("Northwind", "C")

            # Execute the operation by passing it to maintenance.server.send
            result = store.maintenance.server.send(add_database_node_op)
            # endregion
