#Default database
When you start the Raven server for the first time, when it needs to create a new database, Raven will look for a file called default.raven in the current directory. The default.raven is the output of the Smuggler utility, which export a full database to a file.

This allows you to package a database (indexes and documents) along with Raven and create the default database the first time that it is created.

**Note:** When starting Raven for the first time in service mode or web mode, this feature will not be available. It is only active when running in interactive mode (When executing RavendDB.exe directly).

