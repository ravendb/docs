import datetime
from typing import TypeVar, Optional, Union, Generic

from ravendb import (
    PatchOperation,
    PatchStatus,
    DocumentStore,
    StopIndexOperation,
    GetIndexStatisticsOperation,
    DeleteByQueryOperation,
    RavenCommand,
)
from ravendb.documents.conventions import DocumentConventions
from ravendb.documents.operations.counters import GetCountersOperation
from ravendb.documents.operations.definitions import (
    IOperation,
    OperationIdResult,
    _T,
    VoidMaintenanceOperation,
    MaintenanceOperation,
)
from ravendb.documents.operations.operation import Operation
from ravendb.documents.session.misc import SessionInfo
from ravendb.http.http_cache import HttpCache
from ravendb.serverwide.operations.common import ServerOperation, GetBuildNumberOperation

from examples_base import ExampleBase

_Operation_T = TypeVar("_Operation_T")
_T_OperationResult = TypeVar("_T_OperationResult")


class WhatAreOperations(ExampleBase):
    class SendSyntax:
        # region operations_send
        # Available overloads:
        def send(self, operation: IOperation[_Operation_T], session_info: SessionInfo = None) -> _Operation_T: ...

        def send_async(self, operation: IOperation[OperationIdResult]) -> Operation: ...

        def send_patch_operation(self, operation: PatchOperation, session_info: SessionInfo) -> PatchStatus: ...

        def send_patch_operation_with_entity_class(
            self, entity_class: _T, operation: PatchOperation, session_info: Optional[SessionInfo] = None
        ) -> PatchOperation.Result[_T]: ...

        # endregion

    class MaintenanceSyntax:
        # region maintenance_send
        def send(
            self, operation: Union[VoidMaintenanceOperation, MaintenanceOperation[_Operation_T]]
        ) -> Optional[_Operation_T]: ...

        def send_async(self, operation: MaintenanceOperation[OperationIdResult]) -> Operation: ...

        # endregion

        # region ioperation

        # (It's beginning with capital I to mirror the different clients API - and is similar to what is an interface)
        class IOperation(Generic[_Operation_T]):
            def get_command(
                self, store: "DocumentStore", conventions: "DocumentConventions", cache: HttpCache
            ) -> "RavenCommand[_Operation_T]":
                pass

        # endregion

    class ServerSend:
        # region server_send
        def send(self, operation: ServerOperation[_T_OperationResult]) -> Optional[_T_OperationResult]: ...

        def send_async(self, operation: ServerOperation[OperationIdResult]) -> Operation: ...

    def test_examples(self):
        with self.embedded_server.get_document_store("WhatAreOperations") as store:
            # region operations_ex
            # Define operation, e.g. get all counters info for a document
            get_counters_op = GetCountersOperation("products/1-A")

            # Execute the operation by passing the operation to operations.send
            all_counters_result = store.operations.send(get_counters_op)

            # Access the operation result
            number_of_counters = len(all_counters_result.counters)
            # endregion

            # region maintenance_ex
            # Define operation, e.g. stop an index
            stop_index_op = StopIndexOperation("Orders/ByCompany")

            # Execute the operation by passing the operation to maintenance.send
            store.maintenance.send(stop_index_op)

            # This specific operation returns void
            # You can send another operation to verify the index running status
            index_stats_op = GetIndexStatisticsOperation("Orders/ByCompany")
            index_stats = store.maintenance.send(index_stats_op)
            status = index_stats.status  # will be "Paused"
            # endregion

            # region server_ex
            # Define operation, e.g. get the server build number
            get_build_number_op = GetBuildNumberOperation()

            # Execute the operation by passing to maintenance.server.send
            build_number_result = store.maintenance.server.send(get_build_number_op)

            # Access the operation result
            version = build_number_result.build_version
            # endregion

            # region kill_ex
            # Define operation, e.g. delete all discontinued products
            # Note: This operation implements class: IOperation[OperationIdResult]

            delete_by_query_op = DeleteByQueryOperation("from Products where Discontinued = true")

            # Execute the operation
            # Send_async returns an 'Operation' object that can be 'killed'
            operation = store.operations.send_async(delete_by_query_op)

            # Call 'kill' to abort operation
            # todo: skip it - wait for the merge of the ticket linked in the checklist
            operation.kill()
            # endregion

        # region wait_timeout_ex

    def wait_for_completion_with_timeout(timeout: datetime.timedelta, document_store: DocumentStore):
        # Define operation, e.g. delete all discontinued products
        # Note: This operation implements:'IOperation[OperationIdResult]'
        delete_by_query_op = DeleteByQueryOperation("from Products where Discontinued = true")

        # Execute the operation
        # Send returns an 'Operation' object that can be awaited on
        operation = document_store.operations.send_async(delete_by_query_op)

        try:
            # Call method 'WaitForCompletion' to wait for the operation to complete.
            # If a timeout is specified, the method will only wait for the specified time frame.
            result = operation.wait_for_completion_with_timeout(timeout)  # todo: skip, wait for the merge

            # The operation has finished within the specified timeframe
            number_of_items_deleted = result.total
        except TimeoutError:
            # The operation did not finish within the specified timeframe
            ...

    # endregion
