# Ongoing Tasks Stats View
---

{NOTE: }

* The **Ongoing Tasks Stats** view monitors ongoing _Replication_, _ETL_, and _Data Subscription_ tasks.  
* The monitored tasks' overall performance is shown as a graph in a continuous timeline ruler.  
* The timeline selection tool allows you to choose any timeline segment and explore task activities 
  during this period.  
* Activities related to each monitored task are displayed over a ruler dedicated to this task.  
  Task rulers show task activities during the time selected by the timeline select tool.  

* In this page:  
  * [Ongoing Tasks Stats View](../../../studio/database/stats/ongoing-tasks-stats#ongoing-tasks-stats-view)  
  * [The Timeline Ruler](../../../studio/database/stats/ongoing-tasks-stats#the-timeline-ruler)  
  * [Specific Tasks View](../../../studio/database/stats/ongoing-tasks-stats#specific-tasks-view)  
     * [Task Rulers](../../../studio/database/stats/ongoing-tasks-stats#task-rulers)  
     * [Basic Task View](../../../studio/database/stats/ongoing-tasks-stats#basic-task-view)  
     * [Expanded Task View](../../../studio/database/stats/ongoing-tasks-stats#expanded-task-view)  
     * [Additional Information](../../../studio/database/stats/ongoing-tasks-stats#additional-information)  



{NOTE/}

---

{PANEL: Ongoing Tasks Stats View}

![Ongoing Tasks Stats View](images/ongoing-tasks-stats-view.png "Ongoing Tasks Stats View")

* **1.** Ongoing Tasks Stats  
  Click to open the Ongoing Tasks Stats View.  

* **2.** Database  
  Click to choose the database whose ongoing tasks you want to monitor.  

{PANEL/}

{PANEL: The Timeline Ruler}

![The Timeline Ruler](images/the-timeline-ruler.png "The Timeline Ruler")

* The timeline ruler will appear after data is collected from ongoing tasks.  

---

![Clear Graph and Selection](images/selection-and-clear-buttons.png "Clear Graph and Selection")

* **1.** Selection Frame  
  Click and Drag to select a segment of the timeline.  
* **2.** Clear Selection  
  Click to clear your current selection frame.  
* **3.** Clear Graph  
  Click to clear the collected data.  

---

![Tail Stream End](images/monitor-tail.png "Tail Stream End")

* **Monitoring (tail -f)**  
  When this option is checked Studio continuously relocates the selection frame at the end of the data stream.  

{PANEL/}

{PANEL: Specific Tasks View}

---

### Task Rulers

A ruler dedicated to each monitored task displays the task's activities during the 
time specified by the timeline selection box.  

![Task Rulers](images/task-rulers.png "Task Rulers")  

* **1.** ETL Task Ruler  
* **2,3,4.** Data Subscription Tasks Rulers  

---

### Basic Task View

Basic task information is shown in a single bar that can be expanded for a detailed view.  

![Basic Task View](images/basic-task-view.png "Basic Task View")

* **1.** Chosen time segment  
* **2.** ETL task basic view  
    ![ETL Basic View](images/etl-basic-view.png "ETL Basic View")  
* **3.** Data Subscription task basic view  

---

### Expanded Task View

The expanded view breaks task activities into several bars. You can hover your cursor over each bar 
to display additional information related to it.  

![Data Subscription Task Expanded View](images/data-subscription-expanded-view.png "Data Subscription Task Expanded View")

* **Client Connection**  
  Details of the client whose connection is monitored:  
   * Connection duration  
   * Client URL  
   * Client [Strategy](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription#worker-interplay)  
   * Number of acknowledged batches  
   * Batches size  
* **Documents Batch**  
  Details related to the monitored batch:  
   * Batch duration  
   * Number of documents sent in this batch  
   * Transferred documents size  
   * Number of [included](../../../client-api/data-subscriptions/creation/examples#create-subscription-with-include-statement) 
     documents, counters, and time series  
* **Sending Documents**  
  Documents delivery duration  
* **Waiting for ACK**  
  How long did it take for the delivery to be acknowledged?  

---

### Additional Information

Additional information that can be learned from task views includes:  

* **Active** and **Inactive** tasks are represented in different styles.  
   * An active task:  
    ![Active Task](images/active-task.png "Active Task")  
   * An inactive task:  
    ![Inactive Task](images/inactive-task.png "Inactive Task")  

* Rejected and forcibly-aborted connections are marked as such, and 
  hovering over the mark provides an explanation for this event.  
   * A forcibly-aborted connection:  
    ![Forcibly-Aborted Connnection](images/forcibly-aborted-connection.png "Forcibly-Aborted Connnection")  
   * A rejected connection:  
    ![Rejected Connnection](images/rejected-connection.png "Rejected Connnection")  
      * **Rejected Connection**  
        In this case, a client was rejected for trying to connect a subscription that 
        was already in use by another client.  
        Hovering over the rejection mark pops up a message explaining that the subscription 
        was occupied.  

{PANEL/}


## Related Articles  

### Studio  
[Ongoing Tasks - General Info](../../../studio/database/tasks/ongoing-tasks/general-info)  
[RavenDB ETL Task](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
[External Replication Task](../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
### Client API  
[Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)  
[Consuming Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)  

### Server  
[Ongoing Tasks: ETL Basics](../../../server/ongoing-tasks/etl/basics#ongoing-tasks-etl-basics)  
[Ongoing Tasks: External Replication](../../../server/ongoing-tasks/external-replication)  
