#Conventions Related to Cluster

###ReadBalanceBehavior

This convention gets or sets the default load balancing behavior of RavenDB Cluster.

{CODE ReadBalanceBehavior@ClientApi\Configuration\Cluster.cs /}

There are three available settings which govern how RequestExecuter selects which cluster nodes to send requests to.

 * None - The RequestExecuter will select the _preferred node_ .
 * RoundRobin - The RequestExecuter will select different node at each request, using the Round-robin principle.
 * FastestNode - The RequestExecutor will select the fastest nodes, according to periodic speed test. More about the speed test and how it works can be read [here](../../../client-api/cluster/speed-test).

{NOTE Preferred node: If there is at least one node that had not had any communication or other errors, it will be chosen. If communiction to all nodes had failed at least once, then we choose the first one as preferred, so the user would get an error or recovers if the errors were transient./}
