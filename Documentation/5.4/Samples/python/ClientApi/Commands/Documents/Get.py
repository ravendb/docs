from typing import List

from ravendb.documents.commands.crud import GetDocumentsCommand

from examples_base import ExampleBase


class Foo:
    class GetDocumentsCommand:
        # region get_interface_single
        # GetDocumentsCommand.from_single_id(...)
        @classmethod
        def from_single_id(
            cls, key: str, includes: List[str] = None, metadata_only: bool = None
        ) -> GetDocumentsCommand: ...

        # endregion

        # region get_interface_multiple
        # GetDocumentsCommand.from_multiple_ids(...)
        @classmethod
        def from_multiple_ids(
            cls,
            keys: List[str],
            includes: List[str] = None,
            counter_includes: List[str] = None,
            time_series_includes: List[str] = None,
            compare_exchange_value_includes: List[str] = None,
            metadata_only: bool = False,
        ) -> GetDocumentsCommand: ...

        # endregion

        # region get_interface_paged
        # GetDocumentsCommand.from_paging(...)
        @classmethod
        def from_paging(cls, start: int, page_size: int) -> GetDocumentsCommand: ...

        # endregion

        # region get_interface_startswith
        # GetDocumentsCommand.from_starts_with(...)
        @classmethod
        def from_starts_with(
            cls,
            start_with: str,
            start_after: str = None,
            matches: str = None,
            exclude: str = None,
            start: int = None,
            page_size: int = None,
            metadata_only: bool = None,
        ) -> GetDocumentsCommand: ...

        # endregion


class GetSamples(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_command(self):
        with self.embedded_server.get_document_store("GetCommand") as store:
            with store.open_session() as session:
                # region get_sample_single
                command = GetDocumentsCommand.from_single_id("orders/1-A", None, False)
                session.advanced.request_executor.execute_command(command)
                order = command.result.results[0]
                # endregion

                # region get_sample_multiple
                command = GetDocumentsCommand.from_multiple_ids(["orders/1-A", "employees/3-A"])
                session.advanced.request_executor.execute_command(command)
                order = command.result.results[0]
                employee = command.result.results[1]
                # endregion

                # region get_sample_includes
                # Fetch employees/5-A and his boss.
                command = GetDocumentsCommand.from_single_id("employees/5-A", ["ReportsTo"], False)
                session.advanced.request_executor.execute_command(command)
                employee = command.result.results[0]
                boss = command.result.includes.get(employee.get("ReportsTo", None), None)
                # endregion

                # region get_sample_missing
                # Assuming that products/9999-A doesn't exist
                command = GetDocumentsCommand.from_multiple_ids(["products/1-A", "products/9999-A", "products/3-A"])
                session.advanced.request_executor.execute_command(command)
                products = command.result.results  # products/1-A, products/3-A
                # endregion

                # region get_sample_paged
                command = GetDocumentsCommand.from_paging(0, 128)
                session.advanced.request_executor.execute_command(command)
                first_10_docs = command.result.results
                # endregion

                # region get_sample_startswith
                # return up to 128 documents with key that starts with 'products'
                command = GetDocumentsCommand.from_starts_with("products", start=0, page_size=128)
                session.advanced.request_executor.execute_command(command)
                products = command.result.results
                # endregion

                # region get_sample_startswith_matches
                # return up to 128 documents with key that starts with 'products/'
                # and rest of the key begins with "1" or "2" e.g. products/10, products/25
                commands = GetDocumentsCommand.from_starts_with("products", matches="1*2|2*", start=0, page_size=128)
                session.advanced.request_executor.execute_command(command)
                products = command.result.results
                # endregion

                # region get_sample_startswith_matches_end
                # return up to 128 documents with key that starts with 'products/'
                # and rest of the key have length of 3, begins and ends with "1"
                # and contains any character at 2nd position e.g. products/101, products/1B1
                commands = GetDocumentsCommand.from_starts_with("products", matches="1?1", start=0, page_size=128)
                session.advanced.request_executor.execute_command(command)
                products = command.result.results
                # endregion
