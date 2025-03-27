import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function operations() {
    //region operations_ex
    // Define operation, e.g. get all counters info for a document
    const getCountersOp = new GetCountersOperation("products/1-A");

    // Execute the operation by passing the operation to operations.send
    const allCountersResult = await documentStore.operations.send(getCountersOp);

    // Access the operation result
    const numberOfCounters = allCountersResult.counters.length;
    //endregion

    //region maintenance_ex
    // Define operation, e.g. stop an index 
    const stopIndexOp = new StopIndexOperation("Orders/ByCompany");

    // Execute the operation by passing the operation to maintenance.send
    await documentStore.maintenance.send(stopIndexOp);

    // This specific operation returns void
    // You can send another operation to verify the index running status
    const indexStatsOp = new GetIndexStatisticsOperation("Orders/ByCompany");
    const indexStats = await documentStore.maintenance.send(indexStatsOp);
    const status = indexStats.status; // will be "Paused"
    //endregion

    //region server_ex
    // Define operation, e.g. get the server build number
    const getBuildNumberOp = new GetBuildNumberOperation();

    // Execute the operation by passing the operation to maintenance.server.send
    const buildNumberResult = await documentStore.maintenance.server.send(getBuildNumberOp);

    // Access the operation result
    const version = buildNumberResult.buildVersion;
    //endregion

    //region wait_ex
    // Define operation, e.g. delete all discontinued products 
    // Note: This operation implements interface: 'IOperation<OperationIdResult>'
    const deleteByQueryOp = new DeleteByQueryOperation("from Products where Discontinued = true");

    // Execute the operation
    // 'send' returns an object that can be awaited on
    const asyncOperation = await documentStore.operations.send(deleteByQueryOp);

    // Call method 'waitForCompletion' to wait for the operation to complete 
    await asyncOperation.waitForCompletion();
    //endregion

    //region kill_ex
    // Define operation, e.g. delete all discontinued products 
    // Note: This operation implements interface: 'IOperation<OperationIdResult>'
    const deleteByQueryOp = new DeleteByQueryOperation("from Products where Discontinued = true");

    // Execute the operation
    // 'send' returns an object that can be 'killed'
    const asyncOperation = await documentStore.operations.send(deleteByQueryOp);

    // Call method 'kill' to abort operation
    await asyncOperation.kill();
    //endregion
}

{
    //region operations_send
    // Available overloads:
    await send(operation);
    await send(operation, sessionInfo);
    await send(operation, sessionInfo, documentType);

    await send(patchOperaton);
    await send(patchOperation, sessionInfo);
    await send(patchOperation, sessionInfo, resultType);
    //endregion

    //region maintenance_send
    await send(operation);
    //endregion

    //region server_send
    await send(operation);
    //endregion

    //region wait_kill_syntax
    await waitForCompletion();
    await kill();
    //endregion
}
