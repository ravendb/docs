# Replication walkthrough

Below examples will show you how to quickly setup replication in RavenDB using The Studio.

## Replicating documents from one server to another (master-slave)

To setup a master-slave replication we will need at least two servers. First server will serve as a master (in our example the server at port 8081) and second one will be a slave (server at port 8081).

### Configuring master server

- Open studio on master server, create new database called `ExampleDB1` and enable `Replication bundle` feature.

![Figure 1: Create `ExampleDB1` database with `Replication bundle` enabled](Images/studio_replicationwalkthrough_1.PNG)

- Setup replication destination that will point to your slave server (in our example http://localhost:8082/) and enter `ExampleDB2` in Database Name.

![Figure 2: Setup replication destination](Images/studio_replicationwalkthrough_2.PNG)

- You will notice that two documents will be created, first one is called `Raven/Replication/Destinations` and you will find your replication configuration there, second document is called `Raven/Replication/Destinations/localhost8082databasesExampleDB2` and it has been created because RavenDB detected a problem with replicating documents to slave, which is expected because we haven't configured it yet.

![Figure 3: Replication documents](Images/studio_replicationwalkthrough_3.PNG)

### Configuring slave server

- Open studio on slave server, create new database called `ExampleDB2` and enable `Replication bundle` feature.

![Figure 4: Create `ExampleDB2` database with `Replication bundle` enabled](Images/studio_replicationwalkthrough_4.PNG)

- To setup a slave replication, do not add any replication destinations. Just proceed by clicking `OK`.

![Figure 5: Setup replication destination](Images/studio_replicationwalkthrough_5.PNG)

- Document `Raven/Replication/Destinations` will be created, you will notice that, in a contrast to master server, it will be empty.

![Figure 6: Replication documents](Images/studio_replicationwalkthrough_6.PNG)

### Test

- On a master server (8081) create document `users/1` as seen on a figure below.

![Figure 7: Create `users/1` document on master server](Images/studio_replicationwalkthrough_7.PNG)

- On a slave server (8082), you will notice that document `users/1` has been replicated successfully (this may take few moments).

![Figure 8: Verify that document `users/1` has been replicated to slave](Images/studio_replicationwalkthrough_8.PNG)

## Replicating documents between servers (master-master)

To setup a master-master replication we will need at least two servers. Both of them (in our example 8081 and 8082) will serve as masters.

### Configuring first master server

- Open studio on first master server, create new database called `ExampleDB1` and enable `Replication bundle` feature.

![Figure 9: Create `ExampleDB1` database with `Replication bundle` enabled](Images/studio_replicationwalkthrough_1.PNG)

- Setup replication destination to your second master server (in our example http://localhost:8082/) and enter `ExampleDB2` in Database Name.

![Figure 10: Setup replication destination](Images/studio_replicationwalkthrough_2.PNG)

- You will notice that two documents will be created, first one is called `Raven/Replication/Destinations` and you will find your replication configuration there, second document is called `Raven/Replication/Destinations/localhost8082databasesExampleDB2` and it has been created because RavenDB detected a problem with replication documents to our second server, which is expected because we haven't configured it yet.

![Figure 11: Replication documents](Images/studio_replicationwalkthrough_3.PNG)

### Configuring second master server

- Open studio on second master server, create new database called `ExampleDB2` and enable `Replication bundle` feature.

![Figure 12: Create `ExampleDB2` database with `Replication bundle` enabled](Images/studio_replicationwalkthrough_4.PNG)

- Setup replication destination to your first master server (in our example http://localhost:8081/) and enter `ExampleDB1` in Database Name.

![Figure 13: Setup replication destination](Images/studio_replicationwalkthrough_10.PNG)

### Test

- On one of the masters (let's say 8082) create document `albums/1` as seen on a figure below.

![Figure 14: Create `albums/1` document](Images/studio_replicationwalkthrough_11.PNG)

- On other master (8081), you will notice that document `albums/1` has been replicated successfully (this may take few moments). Change content of this document and save it (changed album name from 'Raven' to 'Cake').

![Figure 15: Verify that document `albums/1` has been replicated to other master, change and save its content](Images/studio_replicationwalkthrough_12.PNG)

- Document replicates successfully to 8082.

![Figure 16: Verify that document `albums/1` has been replicated with new content](Images/studio_replicationwalkthrough_13.PNG)
