#Conventions Related to Cluster

###ReadBalanceBehavior

This convention gets or sets the default load balancing behavior of a RavenDB Cluster.

{CODE ReadBalanceBehavior@ClientApi\Configuration\Cluster.cs /}

There are three available settings which govern how RequestExecuter selects which cluster nodes to send requests to.

 * `None` - The RequestExecuter will select the _preferred node_ 
 * `RoundRobin` - The RequestExecuter will select a different node at each request, using the Round-robin principle
 * `FastestNode` - The RequestExecutor will select the fastest nodes according to periodic speed tests. More about the speed test and how it works can be read [here](../../client-api/cluster/speed-test)

{INFO: Preferred node}

If there is at least one node that has not had any communication or other errors, it will be chosen. If communication to all nodes had failed at least once, 
we will choose the first one as preferred so the user would get an error or recovers if the errors were transient.

{INFO/}

{INFO:Session usage}

When using `RoundRobin` or `FastestNode` it might happen that the next [session](../../client-api/session/opening-a-session) you open will access a different node. If you need to ensure that the next request will be able to read
what you just wrote, you need to use [write assurance](../../client-api/session/saving-changes#waiting-for-replication---write-assurance).

In many cases a short delay in replicating changes to other nodes is acceptable. There is no need for confirmation across the entire cluster then.

{INFO/}
