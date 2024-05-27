from typing import List, Optional

from ravendb import GetTermsOperation
from ravendb.documents.operations.definitions import MaintenanceOperation

from examples_base import ExampleBase


class GetIndexTerms(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_index_terms(self):
        with self.embedded_server.get_document_store("GetIndexTerms") as store:
            # region get_index_terms
            # Define the get terms operation
            # Pass the requested index-name, index-filed, start value & page size
            get_terms_op = GetTermsOperation("Orders/Totals", "Employee", "employees/5-A", 10)

            # Execute the operation by passing it to maintenance.send
            field_terms = store.maintenance.send(get_terms_op)

            # field_terms will contain alle the terms that come after term 'employees/5-A' for index-field 'Employee'
            # endregion

    class Foo:
        # region get_index_terms_syntax
        class GetTermsOperation(MaintenanceOperation[List[str]]):
            def __init__(self, index_name: str, field: str, from_value: Optional[str], page_size: int = None): ...

        # endregion
