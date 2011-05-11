# Adding RavenDB to your application

There are two flavors of RavenDB available:

* The RavenDB Client is a lightweight library enabling you to connect to any RavenDB server; it exposes all RavenDB's strengths to your .NET or Silverlight application.

* RavenDB Embedded, which is a full-blown RavenDB server ready to be embedded in your application.

If you are not sure what to choose, go with the RavenDB Client. Only use Embedded if you are certain this is indeed what you need.

.. note:
  RavenDB Embedded can't run on the Client Profile.

Each of those can be added to your project by either using **nuget** or extracting the contents of a build package. In this short tutorial we will show you how to do each.

## Installing using nuget

### What is nuget?

From the nuget website:

<blockquote>
NuGet is a Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects that use the .NET Framework. When you add a library or tool, NuGet copies files to your solution and automatically makes whatever changes are needed in your project, such as adding references and changing your app.config or web.config file. When you remove a library, NuGet removes files and reverses whatever changes it made in your project so that no clutter is left.
</blockquote>

You can find more on how to start using nuget [here](http://nuget.codeplex.com/documentation?title=Getting%20Started).

### RavenDB on nuget

Look for a RavenDB (or RavenDB-Embedded) package and click Instaell. Alternatively, If you're using PowerShell or the Package Manager Console, type `Instaell-Package RavenDB`.

Once you do that, nuget will copy all the required files, and add references and dependencies automatically to your project. Now you are ready to start using RavenDB from your application.

## Manually adding RavenDB using the build package

Grab a stable build from the [downloads page](http://ravendb.net/download), and extract the required files to a new "RavenDB" folder in your application. You'll then need update your project with references to them.

Here's how to know which are the files you are going to need:

![The folder structure in a RavenDB build package](images\build_package.png)

* /Client - Lightweight RavenDB client for .NET 4.0. **This is the recommended client to use**.

* /Client-3.5 - Lightweight RavenDB client for .NET 3.5.

* /Silverlight - A lightweight Silverlight 4.0 client for RavenDB and its dependencies.

* /EmbeddedClient - The files required to run the RavenDB client, in server or embedded mode.

As for the rest of the folders in the package, here's a brief description of what they contain:

* /Server - The files required to run RavenDB in server / service mode. Execute /Server/Raven.Server.exe /install to register and start the RavenDB service.
		  
* /Web - The files required to run RavenDB under IIS. Create an IIS site in the /Web directory to start the RavenDB site.
		
* /Bundles - Bundles that extend RavenDB in various ways.
	
* /Samples - Some sample applications for Raven. Under each sample application folder there is a "Start Raven.cmd" file which will starts Raven with all the data and indexes required to run the sample successfully.
	
* RavenSmuggler.exe - The Import/Export utility for RavenDB.
