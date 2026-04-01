# CDC Sink for PostgreSQL: Studio UI
---

{NOTE: }

* CDC Sink tasks are created and managed through the **Ongoing Tasks** section of the
  Management Studio.

* In this page:
  * [Creating a CDC Sink Task](../../../../server/ongoing-tasks/cdc-sink/postgres/studio-ui#creating-a-cdc-sink-task)
  * [Monitoring Task State](../../../../server/ongoing-tasks/cdc-sink/postgres/studio-ui#monitoring-task-state)
  * [Editing a Task](../../../../server/ongoing-tasks/cdc-sink/postgres/studio-ui#editing-a-task)

{NOTE/}

---

{PANEL: Creating a CDC Sink Task}

{TODO: Screenshot needed — "Ongoing Tasks" list view showing the CDC Sink task type in the "Add Task" dropdown.
Reference ETL documentation screenshots for style (e.g., server/ongoing-tasks/etl/raven/defining-raven-etl-task.markdown). }

To create a CDC Sink task in the Studio:

1. Navigate to **Databases** → your database → **Ongoing Tasks**
2. Click **Add Task** and select **CDC Sink**
3. Configure the connection string pointing to the PostgreSQL source
4. Add one or more root tables with their column mappings
5. (Optional) Add embedded tables and linked tables for each root table
6. (Optional) Configure patches for INSERT/UPDATE/DELETE events
7. Click **Save**

{TODO: Screenshot needed — CDC Sink task creation form, showing the connection string field, table configuration panel, and column mapping editor. }

{PANEL/}

---

{PANEL: Monitoring Task State}

{TODO: Screenshot needed — Ongoing Tasks list view with a running CDC Sink task, showing state badge ("Active"), responsible node, and the expand button for details. }

The task list shows:

* **Task name** and **connection string name**
* **State** — Active, Disabled, Error, or FallbackMode
* **Responsible node** — the cluster node currently running the task
* **Progress** — during initial load, shows which tables have been scanned

Clicking on a task opens the detail view with per-table statistics.

{TODO: Screenshot needed — CDC Sink task detail view, showing per-table row counts, last processed position, and the initial load progress per table. }

{PANEL/}

---

{PANEL: Editing a Task}

To edit a task, click its name in the Ongoing Tasks list. The same configuration
form used for creation opens in edit mode.

{WARNING: }
Adding or removing tables from the task configuration changes the replication slot
and publication names. The old slot and publication will become orphaned and must
be dropped manually by a database administrator.
See [Cleanup and Maintenance](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance).
{WARNING/}

{PANEL/}

---

## Related Articles

### CDC Sink for PostgreSQL

- [Initial Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup)
- [Monitoring](../../../../server/ongoing-tasks/cdc-sink/monitoring)
- [Cleanup and Maintenance](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance)

### CDC Sink

- [API Reference](../../../../server/ongoing-tasks/cdc-sink/api-reference)
- [Configuration Reference](../../../../server/ongoing-tasks/cdc-sink/configuration-reference)
