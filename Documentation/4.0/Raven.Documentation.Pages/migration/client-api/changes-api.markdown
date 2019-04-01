# Migration: Changes API

There is one known issue related to using Changes API on RavenDB 4.0:

{DANGER:Changes API on Secured Server}

Changes API uses WebSockets under the covers. Due to [the lack of support for client certificates in WebSockets implementation in .NET Core 2.0](https://github.com/dotnet/corefx/issues/5120#issuecomment-348557761)
the Changes API won't work for secured servers accessible over HTTPS.

This issue will be fixed in the final version of .NET Core 2.1 (Q2 2018). The fix is already available in [daily builds of .NET Core 2.1](https://github.com/dotnet/core-setup#daily-builds). 
In order to workaround this you can switch your application to use .NET Core 2.1 (build `2.1.0-preview2-26308-01` or newer). 

The issue affects only the RavenDB client, the server can be running on .NET Core 2.0.
{DANGER/}
