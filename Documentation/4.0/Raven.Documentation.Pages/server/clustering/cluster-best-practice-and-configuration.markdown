# Cluster: Best Practices

{NOTE: }

While technically we consider _one_ node as a cluster, obviously it is not the recommended configuration.  

A _two_ nodes cluster is also not recommended, since the cluster must have a majority of nodes to operate, 
and in this case, if one of the nodes is down or partitioned, no raft command will be committed, although any _database_ on the surviving node will still be responsive to the user.  

So the best practice is to have any **odd number of nodes equal or greater than 3**.  
{NOTE/}
