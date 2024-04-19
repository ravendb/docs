from ravendb import CommandData, PatchRequest
from ravendb.documents.commands.batches import (
    PutCommandDataBase,
    PatchCommandData,
    ForceRevisionCommandData,
    DeleteCommandData,
)

from examples_base import ExampleBase


class DeferExample(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_defer(self):
        with self.embedded_server.get_document_store("Defer") as store:
            with store.open_session() as session:
                # region defer_1

                # Defer is available in the session's advanced methods
                session.advanced.defer(
                    # Define commands to be executed:
                    # i.e. Put a new document
                    PutCommandDataBase(
                        "products/999-A",
                        None,
                        {"Name": "My Product", "Supplier": "suppliers/1-A", "@metadata": {"@collection": "Products"}},
                    ),
                    # Patch document
                    PatchCommandData("products/999-A", None, PatchRequest("this.Supplier = 'suppliers/2-A'"), None),
                    # Force a revision to be created
                    ForceRevisionCommandData("products/999-A"),
                    # Delete a document
                    DeleteCommandData("products/1-A", None),
                )

                # All deferred commands will be sent to the server upon calling save_changes
                session.save_changes()

                # endregion


class Foo:
    # region syntax
    def defer(self, *commands: CommandData) -> None: ...

    # endregion
