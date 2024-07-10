# Data Subscriptions: How to Create a Data Subscription

---

{NOTE: }

Subscription tasks are created by performing a request to the server with certain subscription creations parameters, see [Creation API summary](../../../client-api/data-subscriptions/creation/api-overview#subscription-creation).  
Once created, its definition and progress will be stored on the cluster, and not in a single server.  
Upon subscription creation, the cluster will choose a preferred node that will run the subscription (unless the client has stated a responsible node).  
From that point and on, clients that will connect to a server in order to consume the subscription will be redirected to the node mentioned above.  

* In this page:  
   * [Subscription creation prerequisites](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription#subscription-creation-prerequisites)   
   * [Subscription name](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription#subscription-name)  
   * [Responsible node](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription#responsible-node)  

{NOTE/}

---

{PANEL:Subscription creation prerequisites}

Data subscription is a batch processing mechanism that sends documents that answer specific criteria to connected clients.  
In order to create a data subscription, we first need to define the criteria. The minimum is to provide the collection to which the data subscription belongs.
However, the criteria can be a complex RQL-like expression defining JavaScript functions for the filtering and the projections. See a simple example:

{CODE create_whole_collection_generic1@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

Fore more complex usage examples see [examples](../../../client-api/data-subscriptions/creation/examples)  

{PANEL/}

{PANEL:Subscription name}

Another important, but not mandatory subscription property is its name: 

{CODE create_whole_collection_generic_with_name@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

In order to consume a data subscription, a subscription name is required to identify it.  
By default, the server can generate a subscription name on its own, but it's also possible to pass a custom name. 
A dedicated name can be useful for use cases like dedicated, long-running batch processing mechanisms, where it'll be more comfortable to use a human-readable
name in the code and even use the same name between different environments (as long as subscription creation is taken care of upfront).

{INFO:Uniqueness}
Note that the subscription name is unique and it will not be possible to create two subscriptions with the same name in the same database.
{INFO/}

{PANEL/}

{PANEL:Responsible node}

As stated above, upon creation, the cluster will choose a node that will be responsible for the data subscription server-side processing.  
Once chosen, that node will be the only node to process the subscription. There is an enterprise license level feature 
that supports subscription (and any other ongoing task) 
failover between nodes, but eventually, as long as the originally assigned node is online, it will be the one to process the data subscription.  
Nevertheless, there is an option to manually decide the node that will be responsible for the subscription processing, it's called the `MentorNode`:

{CODE create_whole_collection_generic_with_mentor_node@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

The mentor node receives the responsible node tag, as can be seen in the subscription topology.  
Setting that node manually can help manually choosing a more fitting server from resources, client proximity, or any other point of view.

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

**Knowledge Base**:

- [JavaScript Engine](../../../server/kb/javascript-engine)
