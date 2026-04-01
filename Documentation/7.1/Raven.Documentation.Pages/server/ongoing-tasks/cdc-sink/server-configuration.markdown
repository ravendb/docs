# CDC Sink: Server Configuration
---

{NOTE: }

* This page documents the RavenDB server configuration keys that control CDC Sink
  task behavior.

* In this page:
  * [Configuration Keys](../../../server/ongoing-tasks/cdc-sink/server-configuration#configuration-keys)

{NOTE/}

---

{PANEL: Configuration Keys}

These keys can be set in `settings.json` or passed as environment variables.
See [Configuration Overview](../../../server/configuration/configuration-options).

---

#### `CdcSink.MaxBatchSize`

**Default:** `1024`

The maximum number of change events processed in a single batch. Larger values
increase throughput but also increase memory usage per batch.

---

#### `CdcSink.MaxFallbackTimeInSec`

**Default:** `900` (15 minutes)

How long the task will remain in fallback mode (continuously retrying) after losing
connection to the source database before reporting an error.

Set to `0` to disable fallback mode entirely — the task will move to error state
immediately on connection failure.

---

#### `CdcSink.PollIntervalInSec`

**Default:** `1`

How frequently CDC Sink polls the source database for new change events when the
stream is idle. A shorter interval reduces latency but increases polling load on
the source.

{PANEL/}

---

## Related Articles

### CDC Sink

- [Overview](../../../server/ongoing-tasks/cdc-sink/overview)
- [Monitoring](../../../server/ongoing-tasks/cdc-sink/monitoring)
- [Failover and Consistency](../../../server/ongoing-tasks/cdc-sink/failover-and-consistency)

### Server Configuration

- [Configuration Overview](../../../server/configuration/configuration-options)
