import datetime

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.definitions import FieldIndexing

from examples_base import ExampleBase, Product

# region index_1
class Products_ByMetadata_AccessViaValue(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, last_modified: datetime.datetime = None, has_counters: bool = None):
            self.last_modified = last_modified
            self.has_counters = has_counters

    def __init__(self):
        super().__init__()
        self.map = """
                   from product in docs.Products
                   let metadata = MetadataFor(product)
                   
                   select new {
                       last_modified = metadata.Value<DateTime>("@last-modified"),
                       has_counters = metadata.Value<object>("@counters") != null
                   }
                   """
# endregion

# region index_2
class Products_ByMetadata_AccessViaIndexer(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, last_modified: datetime.datetime = None, has_counters: bool = None):
            self.last_modified = last_modified
            self.has_counters = has_counters

    def __init__(self):
        super().__init__()
        self.map = """
                   from product in docs.Products
                   let metadata = MetadataFor(product)
                    
                   select new 
                   {
                       last_modified = (DateTime)metadata["@last-modified"],
                       has_counters = metadata["@counters"] != null 
                   } 
                   """
# endregion

class Metadata(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_metadata(self):
        with self.embedded_server.get_document_store("MetadataJSON") as store:
            with store.open_session() as session:
                # region query_1
                productsWithCounters = list(
                    session.query_index_type(Products_ByMetadata_AccessViaValue,
                         Products_ByMetadata_AccessViaValue.IndexEntry)
                    .where_equals("has_counters", True)
                    .order_by_descending("last_modified")
                    .of_type(Product)
                )
                # endregion
