# Walkthroughs : Setting up replication

{PANEL}

Below step-by-step guides will demonstrate how to quickly setup master-slave and master-master replication between databases using Studio.

{PANEL/}

{PANEL:**Master-Slave replication**}

To setup master-slave replication, this is to replicate documents from one database to another, we need to have two databases (they can be on the same or different server). For our purpose we will create two separate servers. First one, will be our master server and will run on port 8081, second one, the slave server, will run at 8082 port.

### Configuring master server

- first, we need to have a database that we want to replicate to another server. This database **must** have a replication bundle enabled. Let's call it `Northwind-Master`.

![Figure 1. Studio. Replication. Master. Create database.](images/replication-master-create-database.png)  

- to setup replication, we need to go to our [Replication Settings View](../../studio/overview/settings/replication) and add new replication destination using `Add destination` button. Here we need to provide such information like the URL of the destination server (`http://localhost:8082/`) and name of the database there (`Northwind-Backup`).

![Figure 2. Studio. Replication. Master. Setup Replication.](images/replication-master-replication-settings.png)  

- you will notice that two documents will be created, first one is called `Raven/Replication/Destinations` and you will find your replication configuration there, second document is called `Raven/Replication/Destinations/localhost8082databasesNorthwind-Backup` and it has been created because RavenDB detected a problem with replicating documents to slave, which is expected because we haven't configured it yet.

![Figure 3. Studio. Replication. Master. Documents.](images/replication-master-documents.png)  

<hr />

### Configuring slave server

- there is only one step here, you need to create a database with replication bundle **turned on**.

![Figure 4. Studio. Replication. Slave. Create database.](images/replication-slave-create-database.png)  

<hr />

### Test

- on slave server go to [Documents View](../../studio/overview/documents/documents-view) and if all went well, all documents from master server should be there. Worth noting is that replication process might take some time.

![Figure 5. Studio. Replication. Slave. Documents.](images/replication-slave-documents.png)  

{PANEL/}

{PANEL:**Master-Master replication**}

To setup master-master replication, this is to replicate documents from one database to another and backwards, you need to have two databases (they can be on the same of different server). For our purpose we will create two separate servers. First one, will run on port 8081, second one, at 8082.

### Configuring first master server

- first, we need to have a database that we want to replicate to another server. This database **must** have a replication bundle enabled. Let's call it `Northwind-Master-1` and fill it up with [sample data](../../studio/overview/tasks/create-sample-data).

![Figure 6. Studio. Replication. Master. Create database.](images/replication-master-master-create-database-1.png)  

- to setup replication, we need to go to our [Replication Settings View](../../studio/overview/settings/replication) and add new replication destination using `Add destination` button. Here we need to provide such information like the URL of the destination server (`http://localhost:8082/`) and name of the database there (`Northwind-Master-2`).

![Figure 7. Studio. Replication. Master. Setup Replication.](images/replication-master-master-setup-replication-1.png)  

- you will notice that two documents will be created, first one is called `Raven/Replication/Destinations` and you will find your replication configuration there, second document is called `Raven/Replication/Destinations/localhost8082databasesNorthwind-Master-2` and it has been created because RavenDB detected a problem with replicating documents to slave, which is expected because we haven't configured it yet.

![Figure 8. Studio. Replication. Master. Documents.](images/replication-master-master-documents-1.png)  

### Configuring second master server

- open the Studio on second master server and create new database called `Northwind-Master-2`. Remember to **enable** replication bundle.

![Figure 9. Studio. Replication. Master. Create database.](images/replication-master-master-create-database-2.png)  

- setup a replication to your first master server by going into [Replication Settings View](../../studio/overview/settings/replication) and adding new replication destination. In our example we need to use `http://localhost:8081/` as a server URL and `Northwind-Master-1`.

![Figure 10. Studio. Replication. Master. Setup Replication.](images/replication-master-master-setup-replication-2.png)  

### Test

- on of of the masters (let's say 8082) create [sample data](../../studio/overview/tasks/create-sample-data),
- go to the 8081 server and wait for documents from 8082 (might take some time),
- change any value on 8081 and check corresponding document on 8082 (might take some time)


{PANEL/}