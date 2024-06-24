from typing import Dict, List

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractJavaScriptIndexCreationTask

from examples_base import ExampleBase


class Foo:
    """
    # region syntax
     object CreateField(string name, object value);

     object CreateField(string name, object value, bool stored, bool analyzed);

     object CreateField(string name, object value, CreateFieldOptions options);
    # endregion
    """

    # endregion


# region dynamic_fields_1
class Product:
    def __init__(self, Id: str = None, attributes: Dict[str, object] = None):
        self.Id = Id

        # The KEYS under the Attributes object will be dynamically indexed
        # Fields added to this object after index creation time will also get indexed
        self.attributes = attributes


# endregion
P1 = Product


# region dynamic_fields_2
class Products_ByAttributeKey(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = (
            "from p in docs.Products select new {"
            "_ = p.attributes.Select(item => CreateField(item.Key, item.Value))"
            "}"
        )


# endregion
# region dynamic_fields_2_JS
class Products_ByAttributeKey_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Products', function (p) {
                        return {
                            _: Object.keys(p.attributes).map(key => createField(key, p.attributes[key],
                                { indexing: 'Search', storage: true, termVector: null }))
                        };
                    })
            """
        }


# endregion


# region dynamic_fields_4
class Product:
    def __init__(self, Id: str = None, first_name: str = None, last_name: str = None, title: str = None):
        self.Id = Id

        # All KEYS in the document will be dynamically indexes
        # Fields added to the document after index creation time wil also get indexed
        self.first_name = first_name
        self.last_name = last_name
        self.title = title
        # ...


# endregion
P2 = Product


# region dynamic_fields_5_JS
class Products_ByAnyField_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        # This will index EVERY FIELD under the top level of the document
        self.maps = {
            """
            map('Products', function (p) {
                return {
                    _: Object.keys(p).map(key => createField(key, p[key],
                        { indexing: 'Search', storage: true, termVector: null }))
                }
            })
            """
        }


# endregion


# region dynamic_fields_7
class Product:
    def __init__(self, Id: str = None, product_type: str = None, price_per_unit: float = None):
        self.Id = Id

        # The VALUE of ProductType will be dynamically indexed
        self.product_type = product_type
        self.price_per_unit = price_per_unit


# endregion

P3 = Product


# region dynamic_fields_8
class Products_ByProductType(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()

        # Call 'CreateField' to generate the dynamic-index-fields
        # The field name will be the value of document field 'product_type'
        # The field terms will be derived from document field 'price_per_unit'
        self.map = "from p in docs.Products select new { _ = CreateField(p.product_type, p.price_per_unit)}"


# endregion


# region dynamic_fields_8_JS
class Products_ByProductType_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Products', function (p) {
                return {
                    _: createField(p.product_type, p.price_per_unit,
                        { indexing: 'Search', storage: true, termVector: null })
                };
            })
            """
        }


# endregion

# region dynamic_fields_10


class Attribute:
    def __init__(self, prop_name: str = None, prop_value: str = None):
        self.prop_name = prop_name
        self.prop_value = prop_value


class Product:
    def __init__(self, Id: str = None, name: str = None, attributes: List[Attribute] = None):
        self.Id = Id
        self.name = name
        # For each element in this list, the VALUE of property 'prop_name' will be dynamically indexed
        # e.g. color, width, length (in ex. below) will become dynamic-index-field
        self.attributes = attributes


# endregion
P4 = Product


# region dynamic_fields_11
class Attributes_ByName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = (
            "from a in docs.Products select new "
            "{ _ = a.attributes.Select( item => CreateField(item.prop_name, item.prop_value)), name = a.name "
            "}"
        )


# endregion
# region dynamic_fields_11_JS
class Attributes_ByName_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Products', function (p) {
                return {
                    _: p.Attributes.map(item => createField(item.PropName, item.PropValue,
                        { indexing: 'Search', storage: true, termVector: null })),
                   Name: p.Name
                };
            })
            """
        }


# endregion


class DynamicFields(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_create_fields(self):
        with self.embedded_server.get_document_store("CreateFields") as store:
            with store.open_session() as session:
                Product = P1
                Products_ByAttributeKey().execute(store)
                Products_ByAttributeKey_JS().execute(store)
                # region dynamic_fields_3
                matching_documents = list(
                    session.query_index_type(Products_ByAttributeKey, Product)
                    # 'size' is a dynamic-index-field that was indexed from the attributes object
                    .where_equals("size", 42)
                )
                # endregion
                Product = P2
                Products_ByAnyField_JS().execute(store)
                # region dynamic_fields_6
                # 'last_name' is a dynamic-index-field that was indexed from the document
                matching_documents = list(
                    session.query_index_type(Products_ByAnyField_JS, Product).where_equals("last_name", "Doe")
                )
                # endregion
                Product = P3
                Products_ByProductType().execute(store)
                Products_ByProductType_JS().execute(store)
                # region dynamic_fields_9
                # 'electronics' is the dynamic-index-field that was indexed from the document 'product_type'
                matching_documents = list(
                    session.advanced.document_query_from_index_type(Products_ByProductType, Product).where_equals(
                        "electronics", 23
                    )
                )
                # endregion
                Attributes_ByName().execute(store)
                Attributes_ByName_JS().execute(store)
                # region dynamic_fields_12
                matching_documents = list(
                    session.advanced.document_query_from_index_type(Attributes_ByName, Product).where_equals(
                        "width", 10
                    )
                )
                # endregion
