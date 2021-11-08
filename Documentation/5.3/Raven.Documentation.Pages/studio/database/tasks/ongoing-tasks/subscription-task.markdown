# Subscription Task
---

{NOTE: }

* Data subscriptions are task that automatically send certain documents to a subscribed 
client, called the "subscription worker".  

* This page explains how to create a subscription on the server side using the Studio.  

* In this page:
  * [Subscription Task Definition](../../../../studio/database/tasks/ongoing-tasks/subscription-task#subscription-task-definition)
  * [Testing Subscriptions](../../../../studio/database/tasks/ongoing-tasks/subscription-task#testing)
  * [Details in Task List View](../../../../studio/database/tasks/ongoing-tasks/subscription-task#details-in-task-list-view)

{NOTE/}

---

{PANEL: Subscription Task Definition}

![Figure 1. Subscription Task Definition](images/subscriptions-1.png "Subscription Task Definition")

1. The RQL query which selects which documents the subscription sends 
   to the client.  

2. Limit the age of the documents that will be retrieved. Default: "the 
   beginning of time" - meaning all documents are retrieved regardless of 
   age. The other options are:  
   * Latest document - of the documents that existed before the subscription 
     task began, retrieve only the newest one.  
   * Change vector - specify a starting point for the subscription by a 
     document change vector.  

3. Choose which node will perform this task by default.  

4. Test the subscription query.  

{PANEL/}

{PANEL: Testing}

![Figure 2. Testing Subscription](images/subscriptions-2.png "Testing Subscription")

Testing the subscription shows which documents are retrieved by the 
specified query.  

1. Limit the number of results to retrieve for this test.  

2. Limit how long - in seconds - the test should continue before 
stopping automatically.  

3. Run the test.  

{PANEL/}

{PANEL: Details in Task List View}

![Figure 3. Task List View](images/subscriptions-3.png "Task List View")

Click the info button on a task in the task list view, to see 
more detailed information.  

1. Task Details Panel  
   * Task status -  
     **Active**: status is 'Active' while a subscription worker is connected to the task.  
     **Not active**: status is 'Not-active' when no workers are connected.  
   * Mode -  
     **Single**: mode is 'Single' if the workers' strategy allows them to connect one by one.  
     **Concurrent**: mode is 'Concurrent' for workers with Concurrent strategy  
   * Client URI - the identifier of the subscription worker 
     subscribed to this task.  
   * Connection strategy - determines the workers connection strategy.  
   * Change vector for next batch - the change vector of the last 
     document in the batch that will be sent next.  
   * Last batch acknowledgement time - the last time a worker 
     responded that it has recieved a batch.  
   * Last client connection time - the last time a worker communicated 
     with or pinged the server.  
2. Database Group Topology

---

### Concurrent Subscriptions

When [Concurrent Subscribers](../../../../client-api/data-subscriptions/concurrent-subscriptions) 
are connected to a subscription task, they are all listed on the task details panel.  

![Figure 4. Concurrent Subscribers](images/subscriptions-4.png "Concurrent Subscribers")

1. Each concurrent subscriber can be disconnected individually.  
2. Refresh the view to see changes in workers connection states.  

{PANEL/}

## Related Articles

### Data Subscriptions

- [What are Data Subscriptions](../../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
