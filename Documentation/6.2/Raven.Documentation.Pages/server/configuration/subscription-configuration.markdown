# Configuration: Subscriptions
---

{NOTE: }

* The following configuration keys control various aspects of subscription behavior in RavenDB.  
  Learn more about subscriptions in [Data subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions).

* In this article:
  * [Subscriptions.ArchivedDataProcessingBehavior](../../server/configuration/subscription-configuration#subscriptions.archiveddataprocessingbehavior)
  * [Subscriptions.MaxNumberOfConcurrentConnections](../../server/configuration/subscription-configuration#subscriptions.maxnumberofconcurrentconnections)

{NOTE/}
 
{PANEL: Subscriptions.ArchivedDataProcessingBehavior}

The default processing behavior for archived documents in a subscription query.

- **Type**: `enum ArchivedDataProcessingBehavior`:
    * `ExcludeArchived`: only non-archived documents are processed by the subscription query.
    * `IncludeArchived`: both archived and non-archived documents are processed by the subscription query.
    * `ArchivedOnly`: only archived documents are processed by the subscription query.
- **Default**: `ExcludeArchived`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Subscriptions.MaxNumberOfConcurrentConnections}

The maximum number of concurrent subscription connections allowed per database.

- **Type**: `int`
- **Default**: `1000`
- **Scope**: Server-wide or per database

{PANEL/}
