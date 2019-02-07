# Working Asynchronously

The RavenDB client API supports executing the common operations asynchronously, so lengthy operations don't have to be blocking. This is also how the RavenDB Silverlight client is working.

The TPL support is using `System.Threading.Tasks`, a good intro to which can be found [here](https://www.simplethread.com/net-40-and-systemthreadingtasks/).

{CODE async1@ClientApi\WorkingAsynchronously.cs /}