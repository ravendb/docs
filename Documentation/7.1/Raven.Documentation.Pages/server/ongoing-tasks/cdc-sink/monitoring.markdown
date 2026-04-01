# CDC Sink: Monitoring
---

{NOTE: }

* This page explains how to monitor a CDC Sink task — its running state, progress,
  fallback behavior, and statistics available through the Management Studio and API.

* In this page:
  * [Task State](../../../server/ongoing-tasks/cdc-sink/monitoring#task-state)
  * [Fallback Mode](../../../server/ongoing-tasks/cdc-sink/monitoring#fallback-mode)
  * [Statistics](../../../server/ongoing-tasks/cdc-sink/monitoring#statistics)
  * [Notifications](../../../server/ongoing-tasks/cdc-sink/monitoring#notifications)

{NOTE/}

---

{PANEL: Task State}

A CDC Sink task can be in one of the following states:

| State | Description |
|-------|-------------|
| `Active` | Running normally — streaming changes or waiting for new changes |
| `ActiveByAnotherNode` | Another cluster node is the mentor; this node is a replica |
| `Disabled` | Manually disabled via Studio or API |
| `Error` | The task encountered an error and stopped |
| `FallbackMode` | Connection to the source database was lost; retrying |
| `NotOnThisNode` | This node does not hold the task |

The current state is visible in the **Ongoing Tasks** view in the Management Studio.

{PANEL/}

---

{PANEL: Fallback Mode}

When CDC Sink cannot reach the source database, it enters **fallback mode** rather
than failing immediately.

In fallback mode:
* The task continues retrying the connection at regular intervals
* No changes are applied while the connection is down
* The task automatically resumes streaming once the source is reachable again

The maximum time the task will remain in fallback mode before reporting an error
is controlled by the `CdcSink.MaxFallbackTimeInSec` configuration key.
See [Server Configuration](../../../server/ongoing-tasks/cdc-sink/server-configuration).

{PANEL/}

---

{PANEL: Statistics}

CDC Sink exposes runtime statistics through the `GetOngoingTaskInfoOperation`:

    var taskInfo = await store.Maintenance.SendAsync(
        new GetOngoingTaskInfoOperation(taskId, OngoingTaskType.CdcSink));

The returned object includes:

| Field | Description |
|-------|-------------|
| `TaskState` | Current state of the task |
| `MentorNode` | Configured preferred node |
| `ResponsibleNode` | Node currently running the task |
| `Error` | Error message if the task is in error state |

Detailed per-table statistics — including row counts and last processed position —
are available through the Management Studio's ongoing tasks detail view.

{PANEL/}

---

{PANEL: Notifications}

CDC Sink participates in RavenDB's standard alert system. If the task enters an
error state or fallback mode, an alert is raised and visible in:

* The **Notification Center** in the Management Studio (bell icon)
* The cluster's alert log

Alerts include the error message and which table or operation triggered the failure,
making it straightforward to diagnose the root cause.

{PANEL/}

---

## Related Articles

### CDC Sink

- [How It Works](../../../server/ongoing-tasks/cdc-sink/how-it-works)
- [Failover and Consistency](../../../server/ongoing-tasks/cdc-sink/failover-and-consistency)
- [Server Configuration](../../../server/ongoing-tasks/cdc-sink/server-configuration)
- [Troubleshooting](../../../server/ongoing-tasks/cdc-sink/troubleshooting)
