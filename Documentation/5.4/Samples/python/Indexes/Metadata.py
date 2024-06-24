import datetime

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.definitions import FieldIndexing

from examples_base import ExampleBase, Product


# region indexes_1
class Products_AllProperties(AbstractIndexCreationTask):
    class Result:
        def __init__(self, query: str = None):
            self.query = query

    def __init__(self):
        super().__init__()
        self.map = (
            "docs.Products.Select(product => new { "
            # convert product to JSON and select all properties from it
            "    query = this.AsJson(product).Select(x => x.Value) "
            "})"
        )

        # mark 'query' field as analyzed which enables full text search operations
        self._index("query", FieldIndexing.SEARCH)


# endregion


# region indexes_3
class Products_WithMetadata(AbstractIndexCreationTask):
    class Result:
        def __init__(self, last_modified: datetime.datetime = None):
            self.last_modified = last_modified

    def __init__(self):
        super().__init__()
        self.map = """
                   docs.Products.Select(product => new { 
                       Product = Product, 
                       Metadata = this.MetadataFor(product) 
                   }).Select(this0 => new { 
                       last_modified = this0.metadata.Value<DateTime>("Last-Modified") 
                   })"""


# endregion


class Metadata(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_metadata(self):
        with self.embedded_server.get_document_store("MetadataJSON") as store:
            with store.open_session() as session:
                # region indexes_2
                results = list(
                    session.query_index_type(Products_AllProperties, Products_AllProperties.Result)
                    .where_equals("query", "Chocolade")
                    .of_type(Product)
                )
                # endregion

                # region indexes_4
                results = list(
                    session.query_index_type(Products_WithMetadata, Products_WithMetadata.Result)
                    .order_by_descending("LastModified")
                    .of_type(Product)
                )
                # endregion
