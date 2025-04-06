import datetime

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.definitions import FieldIndexing

from examples_base import ExampleBase, Product

# region index_1
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
                # region query_1
                results = list(
                    session.query_index_type(Products_WithMetadata, Products_WithMetadata.Result)
                    .order_by_descending("LastModified")
                    .of_type(Product)
                )
                # endregion
