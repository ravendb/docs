# Status: Replication Stats

Here you can find information about replication status and detailed information concerning each node to which data is replicated or from which it is received.

![Figure 1. Studio. Status. Replication Stats.](images/status-replication_stats-1.png)

## Replication Topology

An additional functionality allows requesting the server for a replication topology. 
When the answer is received, a graph presenting nodes and their connection status will be displayed.

![Figure 2. Studio. Status. Replication Stats. Replication Topology.](images/status-replication_stats-2.png)
![Figure 3. Studio. Status. Replication Stats. Fetch Replication Topology.](images/status-replication_stats-3.png)

{NOTE A red link indicates there is no connection between the nodes and replication is disabled. /}

If you click on one of the links, the arrow will be displayed as a broken line, and some useful replication information will be displayed.
![Figure 4. Studio. Status. Replication Stats. Click on link between nodes.](images/status-replication_stats-4.png)

In our example, the "Rep1" node was taken down before it got a chance to receive the replicated documents from the "Northwind" node.
In that case, you might want to know what's the status of the replication task and which documents weren't replicated yet. 
Just click calculate for a count of not-replicated documents, or export a CSV to see a detailed list.   
![Figure 5. Studio. Status. Replication Stats. Detailed Replication Info.](images/status-replication_stats-6.png)
![Figure 6. Studio. Status. Replication Stats. Exported CSV.](images/status-replication_stats-7.png)

{NOTE If there are more than 25,000 documents waiting to be replicated, the number displayed will only be an approximation. /}

## Replication Performance Statistics

Here you can find information about replication performance - when and how many documents were replicated, and to which node.

![Figure 7. Studio. Status. Replication Stats. Replication Performance Statistics.](images/status-replication_stats-8.png)
