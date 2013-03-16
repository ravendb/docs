# Tasks

On the tasks page you can import and export your database:  
![Tasks Fig 1](Images/studio_tasks_1.PNG)

## Import Database

In here you can import from a .ravendump or .raven.dump file.  
Pay attention that existing documents may be overwritten.  

The Imported data will be stored in the current database

##Export Database

With this task you can export the selected database to a .ravendump file.

##Backup database
You need to select a location to store the backup:  
![Tasks Fig 2](Images/studio_tasks_2.PNG)

##Toggle Indexing
In here you can enable or disable indexing (mostly for debugging)
You can see the current state of the indexing in here as well:  
![Tasks Fig 3](Images/studio_tasks_3.PNG)

##Create sample Data 
You can create a sample data for your database here, this option will only work for an empty database 

## CSV Import
Select a CSV file and import it to this database. CSV files containing unicode characters should be encoded in UTF-8.

## Restore Database (will only appear in the system database) 
In order to restore from a previous backup you need to enter the backup location, an optional location for the database and a name for the database.  
![Tasks Fig 4](Images/studio_tasks_4.PNG)
