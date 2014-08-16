# KB : Server : Long shutdown times

One of the things that is good to be aware of is a possibility of a longer-running shutdown. To cover this subject we will start by describing the shutdown sequence of a RavenDB database.

When RavenDB shuts down, following operations take place:

* Server no longer accepts new connections and is aborting existing ones   
* For each loaded database (including `system` database):   
	* Indexing process is getting stopped   
	* For each index:   
		* Server waits for current indexing batch to complete    
		* Index is getting flushed   
		* Index is closed   
	* Database is closed    

Notice that the number of databases that have to be processed using above algorithm will cause the shutdown time to extend and when document indexation, index optimization or index flushing are in process, then the process will take even longer.

{NOTE When long-running shutdown will take place, the appropriate information will be put into the log. /}

#### Related articles

TODO
