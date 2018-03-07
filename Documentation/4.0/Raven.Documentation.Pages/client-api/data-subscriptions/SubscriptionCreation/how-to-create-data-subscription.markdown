# How to create a data subscription?
---

{NOTE: }

Subscriptions are created by performing a request to the server with certain subscription creations parameters, see [Creation API summary](ApiOverview#create-and-createasync-overloads-summary).  
Once created, it's definition and progress will be stored on the cluster, and not in a single server.  
Upon subscription creation, the cluster will choose a prefered node that will run the subscription (unless client has stated a mentor node).  
From that point and on, clients that will connect to a server in order to consume the subscription will be redirected to the node mentioned above.  

In this page:  

[Subscription creation prerequisites](#subscription-creation-prerequisetes)   
[Subscription name](#subscription-name)  
[Responsible node](#responsible-node)  

{NOTE/}



{PANEL:Subscription creation prerequisetes}

Data subscription is a batch processing mechanism, that send the clients documents that answer a specific criteria.  
In order to create a data subscription, we first need to define the criteria, it's minimum requirement is the collection to which the data subscription belongs, and it can be a complex 
RQL-like expression defining javascript functions for the filtering and the projections. See a simple example:

{CODE create_whole_collection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

Fore more complex usage examples see [examples](Examples)  

{PANEL/}

{PANEL:Subscription name}

Another important, but not mandatory subscription preperty is it's name: 

{CODE create_whole_collection_generic_with_name@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

In order to consume a data subscription, a subscription name is required to identify it.  
Be default, the server can generate a subscription name by it's own, but it's also possible to pass a custom name. 
A dedicated name can be usefull for use cases like dedicated, long running batch processing mechanisms, where it'll be more comfortable to use a human readable
name in the code and even use the same name between different environments (as long as subscription creation is taken care of up fornt).

{INFO:Uniqueness}
Note that subscription name is unique and it will not be possible to create two subscriptions with the same name in the same database.
{INFO/}

{PANEL/}

{PANEL:Responsible node}

As stated above, upon creation, the cluster will choose a node that will be responsible for the data subscription server side processing.  Once chosen, that node
will be the node that will the only node to process the subscription. There is an enterprise license level feature that support subscription (and any other task) 
failover between nodes, but eventually, as long as the originally assigned node is online, it will be the one to process the data subscription.  
Nevertheless, there is an option to manually decide the node that will be responsible for the subscription processing, it's called the `MentoNode`:

{CODE create_whole_collection_generic_with_mentor_node@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

The mentor node receives the responsible node tag, as can be seen in the subscription topology.  
Setting that node manually can help manually choosing a more fitting server from resources, client proximity or any other point of view.

{PANEL/}

## Related articles

- [What are data subscriptions?](../what-are-data-subscriptions)
- [How to **consume** a data subscription?](../SubscriptionConsumption/how-to-consume-data-subscription)
