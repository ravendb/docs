from ravendb import DocumentStore
from ravendb.documents.conventions import DocumentConventions

from examples_base import ExampleBase, Category


class CustomizeCollectionAssignmentForEntities(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_collection_assignment_for_entities(self):
        store = DocumentStore()

        # region custom_collection_name
        def __find_collection_name(object_type: type) -> str:
            if issubclass(object_type, Category):
                return "ProductGroups"

            return DocumentConventions.default_get_collection_name(object_type)

        store.conventions.find_collection_name = __find_collection_name
        # endregion
