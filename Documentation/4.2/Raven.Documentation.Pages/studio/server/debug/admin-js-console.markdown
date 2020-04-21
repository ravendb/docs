# Admin JS Console

---

{NOTE: }

* The Admin Console lets you run javascript code to execute advanced operations on the server.  

{DANGER: Danger}
Do not use the console unless you are sure about what you're doing. Running a script in the Admin Console could cause your server to crash, cause loss of 
data, or other irreversible harm.  
{DANGER/}

* This page contains a partial list of operations that can be executed with the console.  

* In this page:
    * [Console view](../../../studio/server/debug/admin-js-console#console-view)
    * [Operations](../../../studio/server/debug/admin-js-console#operations)

{NOTE/}

---

{PANEL: Console view}

![](images/AdminJSConsole.png)  
<br/>
1. Select the target for the script you want to run. The options are `Server` and `Database`.  
2. If you selected `Database` as your target, use this dropdown menu to select which database to run the script against.  
3. Write your javascript code here. The server or database you have chosen as your target is represented by the variable `server` or `database` 
respectively.  
4. The output of the script.  

{PANEL/}

{PANEL: Operations}

This is a partial list of operations that can be used from the console. The operations are sorted into endpoints with a common parent path.  
<br/>
###Paths from `server`  

{CODE-BLOCK: javascript}
server.ServerStore.Engine.*
{CODE-BLOCK/}

####Methods
| Method | Parameters | Description |
| - | - | - |
| `HardResetToPassive()` | Cluster Topology ID | Force this server to leave its cluster and change state to `passive`. The server will not be able to perform [ongoing tasks](../../database/tasks/ongoing-tasks/general-info) while it is in passive state. <br/>This method takes a [cluster topology](../../../server/clustering/rachis/cluster-topology) ID. If `null` is passed, the node will retain its current cluster topology ID. If you want to later add this server to an existing cluster, its cluster topology ID needs to match that cluster's ID. |
| `HardResetToNewCluster()` | Cluster node tag | Force this server to leave its cluster and bootstrap a new cluster (in which it is the only node and is in state `leader`). <br/>This method takes a [node tag](../../../glossary/node-tag). If `null` is passed, the server's node tag will be `A`. |
| `FoundAboutHigherTerm()` | `long`; `string` | Set the term number of this server to the first parameter - a number of type `long`. A cluster's term number is incremented each time an election occurs. If you pass a number greater than the current term, the server's term number will update, then propagate this value to the rest of the cluster. _*This does not trigger an election, the leader node remains leader*_. <br/>If you pass a number smaller than the current term, the server and cluster retain their current term number - this can be used to break an election that doesn't end on its own, i.e. when the cluster is stuck in "voting in progress". <br/>The second parameter is a `string` that will be printed to the log of the term update: it records the reason for the change in term. It can be set to `null`. |

####Variables
| Endpoint | Type | Description |
| - | - | - |
| `RequestSnapshot` | boolean | Set this value to true to make this server request the raft logs from the leader node of its cluster. This will allow the server to resynchronize. |
{PANEL/}
