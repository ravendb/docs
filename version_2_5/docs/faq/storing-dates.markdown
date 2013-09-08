#Storing dates in RavenDB

##How should I store dates in RavenDB? Using UTC? Using Local Time?
When you store a date to RavenDB, it will save whether it's UTC or not.  When it's not UTC, a local date is treated as "Unspecified".
  
However if you have people from around the world using the same database, and you use unspecified local times, the offset is not stored. If you want to deal with this scenario you need to store the date using a `DateTimeOffset` that will store the date and time, and its time zone offset.

The decision of whether to use UTC, or Local Time, or `DateTimeOffset` is an application decision, not an infrastructure decision.  There are valid reasons for using any one of these.

##More Information
For detailed information about this topic, please refer to the [Working with Date and Time in RavenDB](http://ravendb.net/kb/61/working-with-date-and-time-in-ravendb) knowledge base article.