# Hub/Sink Replication: Sink Task
---

{NOTE: }

A Replication Sink is an ongoing task, designed to replicate documents to and/or 
from Hub tasks to maintain a live replica of a database or chosen documents of it.  
The Sink is always the one to initiate the connection with the Hub, but data can 
be replicated in either or both directions.  
Hub/Sink connections are secure, and require [certification](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#accesses-and-certificates).  

While defining a Sink task using the Studio, you can -  
* Import certificates issued by the Hubs it uses  
* Choose replication direction/s (`Hub->Sink` / `Hub<-Sink` / `Hub<->Sink`)  
* Use [Filtered Replication](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#filtered-replication) to choose which documents are to be replicated  

* In this page:
   * [Create an Ongoing Replication Sink Task](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task#create-an-ongoing-replication-sink-task)  
   * [Import Configuration From Hub](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task#import-configuration-from-hub)  
   * [Sink Configuration](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task#sink-configuration)  
   * [Access Configuration](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task#access-configuration)  
   * [Definining Filtered Replication](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task#defining-filtered-replication)  
   * [Ongoing Tasks View](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task#ongoing-tasks-view)  

{NOTE/}

---

{PANEL: Create an Ongoing Replication Sink Task}

![Figure 1. Create Database Task](images/sink/sink_create-database-task.png "Create Database Task")  

![Figure 2. Create a Replication Sink Task](images/sink/sink_choose-sink-task.png "Create a Replication Sink Task")  

{PANEL/}

{PANEL: Import Configuration From Hub}

![Figure 3. Import Configuration From Hub](images/sink/sink_import-configuration-from-hub.png "Import Configuration From Hub")  

{PANEL/}

1. **Import configuration from Hub**  
   * Click this button to browse the file system for a configuration 
     file you have previously exported from the Hub task you want this 
     Sink to connect.  
     Importing a configuration file would fill **Sink**, **Access** 
     and **Filtering** fields you've filled while defining the Hub task, 
     including the Hub task's certificate, and save you the effort 
     of typing or defining them here again.  

{PANEL: Sink Configuration}

![Figure 4. New Replication Sink](images/sink/sink_new-replication-sink.png "New Replication Sink")  

1. **Choose Database**  
   * Choose the database that the task would replicate data for,  
     from a list of databases whose database-group includes this node.  

2. **Save / Cancel**  
   * **Save** to create or update the task  
   * **Cancel** to dismiss any changes or cancel task creation  

3. **Sink Task Name**  
   * Choose a name for your Sink task  

4. **Hub Task Name**  
   * Enter the name of the Hub task this Sink is to connect with.  

5. **Choose preferred mentor node manually**  
   * Choose which node would run the Sink task by default, 
     from a list of nodes running the database you chose.  

6. **Allow replication from Hub to Sink**  
   * Enable or Disable data replication from Hub to Sink.  

7. **Allow replication from Sink to Hub**  
   * Enable or Disable data replication from Sink to Hub.  

8. **Create a new RavenDB connection string**  
   The connection string defines the path to the Hub task this Sink 
   is to use.  
   * **Disable** to select an existing connection string  
   ![Figure 5. Select a Connection String](images/sink/sink_select-connection-string.png "Select a Connection String")  
     **`a`**. Click to select from a list of existing connection strings  
   * **Enable** to create a new connection string  
   ![Figure 6. New Connection String](images/sink/sink_new-connection-string.png "New Connection String")  
     **`a`**. Enter a connection string name that hasn't been used yet  
     **`b`**. Enter the name of the database running the Hub task  
     **`c`**. Enter URLs of nodes that may run the Hub task  
     **`d`**. Click to add the URL to the list  
     **`e`**. Remove URL  
     **`f`**. Verify that the URL is reachable  
     **`g`**. Reachable URLs will produce this affirmation

{PANEL/}

{PANEL: Access Configuration}

![Figure 7. New Replication Access](images/sink/sink_new-replication-access.png "New Replication Access")

1. **Access Name**  
   * Provide a name for this Sink access  

2. **Certificate Source**  
   Choose where the Sink certificate (with its own private key and 
   the Hub's public key) is from. 
   ([Import Hub configuration](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task#import-configuration-from-hub) 
   to save yourself the need to redefine these details.)  
   * **Provide your own certificate**  
     Use this option to import a certificate of your own, e.g. one 
     you've issued using the Hub task.  
     **`a`**. **Certificate File**  
        Use the Browse button to locate the exported certificate file.  
     **`b`**. **Certificate Passphrase**  
        If you need to provide a passphrase for your certificate, enter it here.  
   * **Use the server certificate**  
     ![Figure 8. New Replication Access](images/sink/sink_use-server-certificate.png "New Replication Access")
      Choose this option if you prefer to import and reuse the certificate 
      that is already used by the server to validate client access.  
      The Studio will attempt to import it from the server. Use the 
      **Download** button (**`a`**) if you need to locate it manually.  
{PANEL/}

{PANEL: Defining Filtered Replication}

![Figure 9. Defining Filtered Replication](images/sink/sink_filtered-replication.png "Defining Filtered Replication")

1. **From Hub to Sink**  
   * Enter each path you want the Hub to replicate to the Sink, in 
     the `Enter documents prefix` text box.  
     Click the `Add Prefix` button to add it to the prefix list.  
      * You can define a prefix that uses a wildcard (`*`).  
        Place the wildcard in the prefix' suffix, after `/` or `-`.  
        e.g. `products/*`  
      * You can provide exact document IDs.  

2. **Use above prefixes (Hub to Sink) for both directions**
   * **Enable** this option to allow replication of the same prefixes 
     in both directions, Hub to Sink and Sink to Hub.  
     **Disable** it to provide an independent list of prefixes for 
     Sink to Hub replication.  
     
         {INFO: }
         Note that only prefixes that are defined in **both** lists 
         will be replicated.  
         {INFO/}

{PANEL/}

{PANEL: Ongoing Tasks View}

![Figure 10. Ongoing Tasks View Info](images/sink/sink_ongoing_tasks_view_info.png "Ongoing Tasks View Info")

1. **Task Status**  
   Task status may be -  
    * *Active*  
      Sink tasks turn active when replication actually takes place, after 
      a document they replicate has changed.  
    * *Not Active*  
    * *Not on Node*  
      The task is not defined for this node.  
      To check this task's status, open the Ongoing Tasks view 
      from a node the task belongs to.  

2. **Responsible Node**  
   The node responsible for this task.  

3. **Topology View**  
   Show the task and its responsible node in the database group topology.  

---

![Figure 11. Ongoing Tasks View Buttons](images/sink/sink_ongoing_tasks_view_buttons.png "Ongoing Tasks View Buttons")

1. **Enable/Disable task**  
2. **Edit task**  
3. **Delete task**  
4. **Info button**  
   Click the Info button to show the following details:  
   ![Figure 12. Extended Info View](images/sink/sink_extended-info.png "Extended Info View")  

{PANEL/}

## Related Articles

**Studio Articles**:   
[Hub/Sink Replication: Overview](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  
[Replication Hub Task](../../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task)  

**Server Articles**:  
[External Replication](../../../../../server/ongoing-tasks/external-replication)  
[Client Certificate Usage](../../../../../server/security/authentication/client-certificate-usage)  

**Client Articles**:  
[Adding a Connection String](../../../../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string)  
