import pathlib
from typing import List

from ravendb.documents.operations.server_misc import ToggleDatabasesStateOperation, DisableDatabaseToggleResult
from ravendb.serverwide.operations.common import ServerOperation

from examples_base import ExampleBase


class ToggleDatabasesState(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_toggle_databases_state(self):
        with self.embedded_server.get_document_store("ToggleDatabasesState") as store:
            # region enable
            # Define the toggle state operation
            # specify the database name & pass 'False' to enable
            enable_database_op = ToggleDatabasesStateOperation("Northwind", disable=False)

            # To enable multiple databases use:
            # enable_database_op = ToggleDatabasesStateOperation.from_multiple_names(["DB1", "DB2", ...], disable=False)

            # Execute the operation by passing it to maintenance.server.send
            toggle_result = store.maintenance.server.send(enable_database_op)
            # endregion

            # region disable
            # Define the toggle state operation
            # specify the database name(s) & pass 'True' to disable
            disable_database_op = ToggleDatabasesStateOperation("Northwind", disable=True)

            # To disable multiple databases use:
            # enable_database_op = ToggleDatabasesStateOperation.from_multiple_names(["DB1", "DB2", ...], disable=True)

            # Execute the operation by passing it to maintenance.server.send
            toggle_result = store.maintenance.server.send(disable_database_op)
            # endregion

            database_path = "db_path"
            # region disable-database-via-file-system
            # Prevent database access by creating disable.marker in its path
            disable_marker_path = pathlib.Path(database_path, "disable.marker")
            disable_marker_path.touch()
            # endregion

    class Foo:
        class ToggleDatabasesStateOperation:
            # region syntax_1
            class ToggleDatabasesStateOperation(ServerOperation[DisableDatabaseToggleResult]):
                def __init__(self, database_name: str, disable: bool): ...
                @classmethod
                def from_multiple_names(cls, database_names: List[str], disable: bool): ...

            # endregion
            # region syntax_2
            class DisableDatabaseToggleResult:
                def __init__(
                    self, disabled: bool = None, name: str = None, success: bool = None, reason: str = None
                ) -> None:
                    self.disabled = disabled  # Is database disabled
                    self.name = name  # Name of the database
                    self.success = success  # Has request succeeded
                    self.reason = reason  # Reason for success or failure

            # endregion
