# Create Raven Test with RavenTestBase
In case you see an issue with RavenDB and you can to report it, the bast way is to create a failing test and send it to us so we could look into it and fix it as fast as possible.

## How to Create a Failing Test
1) Create a new Console Application  
2) Add the NuGet Package "RavenDB Test Helpers"  
PM> Install-Package RavenDB.Tests.Helpers

![Add RavenDB Test Helpers NuGet Package](Images/tests_1.PNG)  
3) Create a test like this:  
{CODE RavenTestSample1@Samples\RavenTests\RavenTestSample.cs /}

When you inherit from the class "RavenTestBase" you get the following methods to help you create your test:  
{CODE RavenTestBaseMethods@Samples\RavenTests\RavenTestBaseMethods.cs /}

In addtion you have several virtual method you could use:  
{CODE RavenTestBaseViruals@Samples\RavenTests\RavenTestBaseMethods.cs /}

Here is an example of a full test:  
{CODE RavenTestSample2@Samples\RavenTests\RavenTestSample.cs /}

After you have the test finished, send the "cs" file with the failing test to [RavenDB Support](mailto:support@ravendb.net)
