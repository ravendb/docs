# Cluster Observer

The primary goal of the `Cluster Observer` is to maintain the `Replication Factor` of each database.

Every newly elected [Leader](../../../server/clustering/rahis/cluster-topology#leader) starts measuring the health of each of nodes.    
It is done by creating dedicated maintenance TCP connections to the other nodes.  
Each **cluster** node will report the current status of _all_  his databases in intervals of [250 milliseconds](../../../server/configuration/cluster-configuration#cluster.workersampleperiodinms) (by default). The `Cluster Observer` will consume those reports every [500 milliseconds](../../../server/configuration/cluster-configuration#cluster.supervisorsampleperiodinms) (by default).

You can interact with the `Cluster Observer` via the [Studio](../../../studio/server/cluster/cluster-observer) or with the following REST API:

| URL | Method | Query Params. | Description |
| - | - | - | - |
| `/admin/cluster/observer/suspend` | POST | value=[`bool`] | Setting `false` will suspend the operation of the `Cluster Observer` for the current term of the `Leader`. |
| `/admin/cluster/observer/decisions` | GET | | Fetch the log of the recent decisions made by the cluster observer. |
| `/admin/cluster/maintenance-stats` | GET | | Fetch the latest reports that the `Cluster Observer` |

The `Cluster Observer` stores his information _in memory_, so when the `Leader` lose his leadership the collected reports and the decisions log are lost.

{INFO: Node Failure}
Upon node failure the [Dynamic Database Distribution](../../../server/clustering/distribution/distributed-database#dynamic-database-distribution) sequence will take place to ensure the `Replication Factor`.
{INFO/}