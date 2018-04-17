# Best Practice Clusters

While technically we consider one node as cluster, obviously it is not the recommended configuration. 
Two nodes cluster is also not recommended, since the cluster must have a majority of nodes to operate, and in case one of the nodes is down or partitioned, no raft command will be committed, although any _database_ on the surviving node will be still responsive to the user.
So the best practice is to have any odd number of nodes larger than 3. 
