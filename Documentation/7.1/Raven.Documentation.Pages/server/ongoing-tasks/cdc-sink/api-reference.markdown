# CDC Sink: API Reference
---

{NOTE: }

* CDC Sink tasks can be created, updated, and managed programmatically using
  the RavenDB Client API.

* In this page:
  * [Add a CDC Sink Task](../../../server/ongoing-tasks/cdc-sink/api-reference#add-a-cdc-sink-task)
  * [Update a CDC Sink Task](../../../server/ongoing-tasks/cdc-sink/api-reference#update-a-cdc-sink-task)
  * [Get Task Info](../../../server/ongoing-tasks/cdc-sink/api-reference#get-task-info)
  * [Toggle Task State](../../../server/ongoing-tasks/cdc-sink/api-reference#toggle-task-state)
  * [Delete a Task](../../../server/ongoing-tasks/cdc-sink/api-reference#delete-a-task)

{NOTE/}

---

{PANEL: Add a CDC Sink Task}

Use `AddCdcSinkOperation` to create a new CDC Sink task:

    var config = new CdcSinkConfiguration
    {
        Name = "OrdersSync",
        ConnectionStringName = "MyPostgresConnection",
        Tables = new List<CdcSinkTableConfig>
        {
            new CdcSinkTableConfig
            {
                Name = "Orders",
                SourceTableName = "orders",
                PrimaryKeyColumns = new List<string> { "id" },
                ColumnsMapping = new Dictionary<string, string>
                {
                    { "id",            "Id" },
                    { "customer_name", "CustomerName" },
                    { "total",         "Total" }
                }
            }
        }
    };

    var result = await store.Maintenance.SendAsync(
        new AddCdcSinkOperation(config));

    long taskId = result.TaskId;

`AddCdcSinkOperationResult`:

| Property | Type | Description |
|----------|------|-------------|
| `TaskId` | `long` | Assigned task ID |
| `RaftCommandIndex` | `long` | Raft index of the command |

{PANEL/}

---

{PANEL: Update a CDC Sink Task}

Use `UpdateCdcSinkOperation` to modify an existing task.
Pass the full updated configuration including the `TaskId`:

    config.TaskId = taskId;        // Must be set
    config.Tables.Add(new CdcSinkTableConfig
    {
        Name = "Customers",
        SourceTableName = "customers",
        PrimaryKeyColumns = new List<string> { "id" },
        ColumnsMapping = new Dictionary<string, string>
        {
            { "id",    "Id" },
            { "name",  "Name" },
            { "email", "Email" }
        }
    });

    await store.Maintenance.SendAsync(
        new UpdateCdcSinkOperation(taskId, config));

{WARNING: }
Adding or removing tables, or changing table names, changes the hash used to generate
the replication slot and publication names. This results in a new slot and publication
being created and the old ones becoming orphaned. The database administrator must
drop the old slot and publication manually.
See [Cleanup and Maintenance](../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance).
{WARNING/}

{PANEL/}

---

{PANEL: Get Task Info}

Use `GetOngoingTaskInfoOperation` to retrieve the current state of a CDC Sink task:

    var taskInfo = await store.Maintenance.SendAsync(
        new GetOngoingTaskInfoOperation(taskId, OngoingTaskType.CdcSink));

{PANEL/}

---

{PANEL: Toggle Task State}

Pause or resume a CDC Sink task using `ToggleOngoingTaskStateOperation`:

    // Pause the task
    await store.Maintenance.SendAsync(
        new ToggleOngoingTaskStateOperation(taskId, OngoingTaskType.CdcSink, disable: true));

    // Resume the task
    await store.Maintenance.SendAsync(
        new ToggleOngoingTaskStateOperation(taskId, OngoingTaskType.CdcSink, disable: false));

{PANEL/}

---

{PANEL: Delete a Task}

Use `DeleteOngoingTaskOperation` to delete a CDC Sink task:

    await store.Maintenance.SendAsync(
        new DeleteOngoingTaskOperation(taskId, OngoingTaskType.CdcSink));

{WARNING: }
Deleting the task in RavenDB does **not** drop the replication slot or publication
in the source database. These must be cleaned up manually by the database administrator.
See [Cleanup and Maintenance](../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance).
{WARNING/}

{PANEL/}

---

## Related Articles

### CDC Sink

- [Configuration Reference](../../../server/ongoing-tasks/cdc-sink/configuration-reference)
- [Overview](../../../server/ongoing-tasks/cdc-sink/overview)

### Client API

- [Operations: How to Send Operations](../../../client-api/operations/how-to/send-multiple-operations)
- [Ongoing Task Operations](../../../client-api/operations/maintenance/ongoing-tasks/ongoing-task-operations)
