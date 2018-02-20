#The Azure Router Timeout

##Symptoms

- An exception is thrown in RavenDB Client when accessing RavenDB Server. The exception was because of a socket timeout or general connection failure.
- Problem happens mainly on Azure hosted VMs

##Cause

Azure's Load Balancer closes connections that are idle for more than 60 seconds.

##Resolution

We have to make sure that there are no connections that are idle for more than 60 seconds.

There are two possible ways to do that:

- Make sure that RavenDB is accessed at least once a minute (consider having a keep-alive message)
- Change service point manager's [MaxIdleTime](https://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=EN-US&k=k(System.Net.ServicePointManager.MaxServicePointIdleTime);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5);k(DevLang-csharp)&rd=true) value in your client application 

{NOTE RavenDB is using heartbeats in 3-10 second intervals, on all its internal communication channels so they will be kept open./}

##Further Reading

- [Azure SQL Server Connection Management](http://social.technet.microsoft.com/wiki/contents/articles/1541.windows-azure-sql-database-connection-management.aspx)
