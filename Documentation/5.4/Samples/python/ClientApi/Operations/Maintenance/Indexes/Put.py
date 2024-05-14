from ravendb import IndexDefinition, IndexDeploymentMode, PutIndexesOperation
from ravendb.documents.indexes.abstract_index_creation_tasks import IndexDefinitionBuilder
from ravendb.documents.indexes.definitions import IndexPriority
from ravendb.documents.operations.definitions import MaintenanceOperation

from examples_base import ExampleBase


class Foo:
    # region syntax
    class PutIndexesOperation(MaintenanceOperation):
        def __init__(self, *indexes_to_add: IndexDefinition): ...

    # endregion


class Put(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_put_indexes_operation(self):
        with self.embedded_server.get_document_store("PutIndexes") as store:
            # region put_1
            # Create an index definition
            index_definition = IndexDefinition(
                # Name is mandatory, can use any string
                name="OrdersByTotal",
                # Define the index Map functions, string format
                # A single string for a map-index, multiple strings for a multi-map-index
                maps={
                    """
                      // Define the collection that will be indexed:
                      from order in docs.Orders

                        // Define the index-entry:
                        select new 
                        {
                            // Define the index-fields within each index-entry:
                            Employee = order.Employee,
                            Company = order.Company,
                            Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                        }          
                """
                },
                # reduce = ...
                # Can provide other index definitions available on the IndexDefinition class
                # Override the default values, e.g.:
                deployment_mode=IndexDeploymentMode.ROLLING,
                priority=IndexPriority.HIGH,
                configuration={"Indexing.IndexMissingFieldsAsNull": "true"},
                # See all available properties in syntax below
            )

            # Define the put indexes operation, pass the index definition
            # Note: multiple index definitions can be passed, see syntax below
            put_indexes_op = PutIndexesOperation(index_definition)

            # Execute the operation by passing it to maintenance.send
            store.maintenance.send(put_indexes_op)
            # endregion

            # region put_1_JS
            # Create an index definition
            index_definition = IndexDefinition(
                # Name is mandatory, can use any string
                name="OrdersByTotal",
                # Define the index map functions, string format
                # A single string for a map-index, multiple strings for a multimap index
                maps={
                    """
                    map('Orders', function(order) {
                              return {
                                  Employee: order.Employee,
                                  Company: order.Company,
                                  Total: order.Lines.reduce(function(sum, l) {
                                      return sum + (l.Quantity * l.PricePerUnit) * (1 - l.Discount);
                                  }, 0)
                              };
                        });
                    """
                },
                # reduce = ...,
                # Can provide other index definitions available on the IndexDefinition class
                # Override the default values, e.g.:
                deployment_mode=IndexDeploymentMode.ROLLING,
                priority=IndexPriority.HIGH,
                configuration={"Indexing.IndexMissingFieldsAsNull": "true"},
                # See all available properties in syntax below
            )
            # Define the put indexes operation, pass the index definition
            # Note: multiple index definitions can be passed, see syntax below
            put_indexes_op = PutIndexesOperation(index_definition)

            # Execute the operation by passing it to Maintenance.Send
            store.maintenance.send(put_indexes_op)
            # endregion

            # region put_2
            # Create an index definition builder
            builder = IndexDefinitionBuilder()
            builder.map = """
                      // Define the collection that will be indexed:
                      from order in docs.Orders

                        // Define the index-entry:
                        select new 
                        {
                            // Define the index-fields within each index-entry:
                            Employee = order.Employee,
                            Company = order.Company,
                            Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))
                        }"               
                    """
            # Can provide other properties available on the IndexDefinitionBuilder class, e.g.:
            builder.deployment_mode = IndexDeploymentMode.ROLLING
            builder.priority = IndexPriority.HIGH
            # builder.reduce = ..., etc.

            # Generate index definition from builder
            # Pass the conventions, needed for building the maps property
            builder.to_index_definition(store.conventions)

            # Optionally, set the index name, can use any string
            # If not provided then default name from builder is used, e.g.: "IndexDefinitionBuildersOfOrders"
            index_definition.name = "OrdersByTotal"

            # Define the put indexes operation, pass the index definition
            # Note: multiple index definitions can be passed, see syntax below
            put_indexes_op = PutIndexesOperation(index_definition)

            # Execute the operation by passing it to Maintenance.Send
            store.maintenance.send(put_indexes_op)
            # endregion
