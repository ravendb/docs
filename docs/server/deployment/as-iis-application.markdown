# Deploying as an IIS application

RavenDB can be run as an IIS application.

## Creating a RavenDB application

1. [Download the distribution zip](http://ravendb.net/download), and extract the "Web" folder.
2. In IIS Manager, create a new website and point it's physical path to the "/Web" folder you extracted.
3. Set the Application Pool to "ASP.Net v4.0" (or create a new Application Pool set to .NET 4.0 Integrated Pipeline)
4. Set port and host if needed.
5. Make sure that the user you set for the website has write access to the physical database location.

## Setting up with IIS 7.5

* Remove the WebDAVModulse from the installed Modules in your server
* If you wish to run with authentication:
    * Ensure that "Windows Authentication" is installed support to IIS 7.5 (by default it is not)
    * Enable "Windows Authentication"  for the RavenDB website.
    * In the web.config file, set the app settings value `Raven/AnonymousUserAccessMode` to Get or None
* If you with to run with no authentication:
    * In the web.config file, set the app settings value `Raven/AnonymousUserAccessMode` to All

## Setting up with IIS 7

You may get a "405 Method Not Allowed" error when trying to create an index from the Client API when RavenDB is running in IIS 7.

This usually happens when you are running RavenDB from a virtual directory. The problem is a conflict that occurs with the WebDAV module. To resolve that, you need to edit the web.config file in the parent directory and add:

{CODE-START:xml/}
<system.webServer>
   <modules runAllManagedModulesForAllRequests="true">
      <remove name="WebDAVModule" />
   </modules>
 </system.webServer>
{CODE-END/}
 
That will remove the WebDAV module and resolve the conflict.

{NOTE Important: This modification is not to the RavenDB's web.config file, it is to the web.config of the parent application. /}

## Setting up with IIS 6

When using IIS 6, you need to modify the Web.config file and remove the `system.webServer` element and add the following system.web element:

{CODE-START:xml/}
<system.web>
   <httpHandlers>
      <add path="*" verb="*" 
         type="Raven.Web.ForwardToRavenRespondersFactory, Raven.Web"/>
   </httpHandlers>
</system.web>
{CODE-END/}

## Web Configuration

Many configuration options are available for tuning RavenDB and fitting it to your needs. See the [Configuration options](configuration) page for complete info.

## Recommended IIS Configuration

RavenDB isn't a typical web site because it needs to be running at all times. In IIS 7.5, you can set this using the following configuration settings:

* If you created a dedicated application pool for RavenDB, change the application pool configuration in the application host file (C:\Windows\System32\inetsrv\config\applicationHost.config) to:

{CODE-START:xml/}
       <add name="RavenApplicationPool" managedRuntimeVersion="v4.0" startMode="AlwaysRunning" />
{CODE-END/}

* If Raven runs in an application pool with other sites, modify the application host file (C:\Windows\System32\inetsrv\config\applicationHost.config) to: 

{CODE-START:xml/}
       <application path="/Raven" serviceAutoStartEnabled="true" />
{CODE-END/}

## HTTP Error 503

You may hit an HTTP Error 503 - "The service is unavailable" - when deplying to IIS, with nothing written to the Event Log.

This problem usually occurs when you are trying to run the RavenDB's IIS site on port 8080 after you have run RavenDB in executable mode. The problem stems from RavenDB reserving the `http://+:8080/` namespace for your user, and when IIS attempts to start a site on the same endpoint, it errors.

You can resolve this problem by executing the following on the command line:

    netsh http delete urlacl http://+:8080/

You can also see all existing registrations with the following command:

    ntsh http show urlacl