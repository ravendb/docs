# KB: Server: Long shutdown times

It is good to be aware of the possibility of a longer-running shutdown. We will start describing this subject with defining the shutdown sequence of a RavenDB database.

When RavenDB shuts down, the following operations take place:

* Server no longer accepts new connections 
* For each loaded database (including `system` database) and file system:   
    * It waits for existing requests to complete (up to 3 seconds), 
	* Indexing process is getting stopped   
	* For each index:   
		* Server waits for current indexing batch to complete    
		* Index is getting flushed   
		* Index is closed   
	* Database is closed    

Notice that the number of databases that have to be processed using the above algorithm will cause the shutdown time to extend and when document indexation, index optimization or index flushing are in process, the process will take even longer.

{NOTE When long-running shutdown will take place, the appropriate information will be put into the log. /}
