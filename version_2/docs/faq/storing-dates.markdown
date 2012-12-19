#Storing dates in RavenDB

##How should I store dates in RavenDB? Using UTC? Using Local Time?
When you store a date to RavenDB It will save whether its UTC or Local.  
However if you have people from round the world using the same database and you use local time the offset is not stored. If you want to deal with this senario you need to store the date with its offset

The decision whatever to use UTC or Local Time is an application decision, not an infrastructure decision.
