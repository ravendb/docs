# Ongoing Tasks Stats: Overview
---

{NOTE: }

* The **Ongoing Tasks Stats** view monitors the following ongoing tasks:  
   * External Replication  
   * RavenDB ETL  
   * Subscription  
* Tasks activities are graphically displayed for selected time frames.  

* In this page:  
   * [Ongoing Tasks Stats View](../../../../studio/database/stats/ongoing-tasks-stats/overview#ongoing-tasks-stats-view)  
   * [The Timeline Ruler](../../../../studio/database/stats/ongoing-tasks-stats/overview#the-timeline-ruler)  
   * [Task Rulers](../../../../studio/database/stats/ongoing-tasks-stats/overview#task-rulers)  
      * [Closed View](../../../../studio/database/stats/ongoing-tasks-stats/overview#task-rulers-closed-view)  
      * [Expanded View](../../../../studio/database/stats/ongoing-tasks-stats/overview#task-rulers-expanded-view)  
      * [Active and Inactive Display](../../../../studio/database/stats/ongoing-tasks-stats/overview#active-and-inactive-task-view)  
      * [Connection Indicators](../../../../studio/database/stats/ongoing-tasks-stats/overview#connection-indicators)  

{NOTE/}

---

{PANEL: Ongoing Tasks Stats View}

![Ongoing Tasks Stats View](images/stats-view-01_1-ongoing-tasks-stats-view.png "Ongoing Tasks Stats View")

1. **Ongoing Tasks Stats**  
   Click to open the Ongoing Tasks Stats View.  

2. **Database**  
   Select the database whose ongoing tasks you want to monitor.  
   Data for this database's ongoing tasks will be collected and presented in the graph.  

---

**Stats Graph**  
The graph is added to the view once data is collected from at least one of the monitored tasks.  
![Ongoing Tasks Stats Graph](images/stats-view-01_2-rulers-and-bars.png "Ongoing Tasks Stats Graph")

{PANEL/}

{PANEL: The Timeline Ruler}

![The Timeline Ruler](images/stats-view-02-the-timeline-ruler.png "The Timeline Ruler")

* The `Timeline Ruler` allows you to select any timeline segment and explore task activities during this period.  
* The timeline ruler `Line` depicts a summary of the activity volume for all tasks presented in the graph.  

---

![Timeline Ruler Actions](images/stats-view-03-selection-and-clear-buttons.png "Timeline Ruler Actions")

1. **Selected Time Frame**  
   Click & drag or resize this frame to select the timeline segment for which the data will be shown in the graph.  
2. **Clear Selection**  
   Click to clear the selected frame.  
3. **Clear Graph**  
   Click to clear all the collected data from the graph.  
   {INFO: }
   Recently collected statistics are kept by RavenDB in memory, allowing you to 
   refresh the stats view without losing its entire history.  
   Using the `Clear Graph` button will clear all historical data.  
   {INFO/}

---

![Tail Stream End](images/stats-view-04-monitor-tail.png "Tail Stream End")

* 4. **Monitoring (tail -f)**  
  When this option is checked, the selected time frame will be continuously reallocated at the end of the data stream.  

{PANEL/}

{PANEL: Task Rulers}

### Task Rulers Closed View

![Task Rulers Closed Views](images/stats-view-05-task-rulers-closed-views.png "Task Rulers Closed Views")

1. **Selected Time Frame**  
2. **Task Ruler: Closed View**  
     * A ruler dedicated to each monitored task displays the task's activities during 
       the time specified by the timeline ruler above.  
     * Information summary is shown in a single bar.  
3. **Task Bar**  
     * Hover over the bar to popup task information.  
     * Click and drag the bar to slide the graph.  
     * Click the bar for an expanded view that details task events in multiple bars.  

---

### Task Rulers Expanded View

![Task Rulers Expanded Views](images/stats-view-06-task-rulers-expanded-views.png "Task Rulers Expanded Views")

1. **Selected Time Frame**  
2. **Task Ruler: Expanded View**  
   Detailed information is shown in multiple bars.  
3. **Task Bars**  
4. **Bar Frame**  
   Bars are split into frames that depict particular statistics related to the task.  
   Hover over bar frames to popup information related to them, e.g. -  
    * **Sending Documents** - The amount of time it took to send documents:  
      ![Sending Documents](images/stats-view-sending-documents.png "Sending Documents")
    * **Waiting For ACK** - The amount of time the task waited for client Acknowledgement:  
      ![Waiting For Ack](images/stats-view-waiting-for-ack.png "WWaiting For Ack")

---

### Active and Inactive Task View

**Active** tasks (e.g. a subscription task that a client is currently connected to) 
and **Inactive** tasks are shown in different styles.  

* An active task:  
  ![Active Task](images/stats-view-active-task.png "Active Task")
* An inactive task:  
  ![Inactive Task](images/stats-view-inactive-task.png "Inactive Task")

---

### Connection Indicators

Connection Indicators signify events related to the initiation, state, or 
termination of clients' connections with tasks.  

* Connection indicators are shown in both the [Closed](../../../../studio/database/stats/ongoing-tasks-stats/overview#task-rulers-closed-view) 
  and the [Expanded](../../../../studio/database/stats/ongoing-tasks-stats/overview#task-rulers-expanded-view) 
  task views.  
* Hovering over a connection indicator will popup information related to it, if available.  

![Connection Indicators](images/stats-view-connection-indicators.png "Connection Indicators")


{PANEL/}


## Related Articles  

### Studio  
[Ongoing Tasks - General Info](../../../../studio/database/tasks/ongoing-tasks/general-info)  
[RavenDB ETL Task](../../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
[External Replication Task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task)  
### Client API  
[Data Subscriptions](../../../../client-api/data-subscriptions/what-are-data-subscriptions)  
[Consuming Data Subscription](../../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)  

### Server  
[Ongoing Tasks: ETL Basics](../../../../server/ongoing-tasks/etl/basics#ongoing-tasks-etl-basics)  
[Ongoing Tasks: External Replication](../../../../server/ongoing-tasks/external-replication)  
