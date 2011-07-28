# Deploying as an IIS application

RavenDB can be run as an IIS application.

## Creating a RavenDB application

1. Extract the distribution zip.
2. In IIS Manager, create a new Web Site and point its physical path to the "/Web" folder in the extracted folder.
3. Set the Application Pool to "ASP.Net v4.0" (or create a new Application Pool set to .NET 4.0 Integrated Pipeline)
4. Set port and host, if you need them.
5. Make sure that the user you set for the site has write access to the physical database location.

## Setting up with IIS 7.5

* Remove the WebDAVModule from the installed Modules in your server
* If you wish to run with authentication:
    * Ensure that "Windows Authentication" is installed support to IIS 7.5 (by default it is not)
    * Enable "Windows Authentication"  for the RavenDB website.
    * In the web.config file, set the app settings value `Raven/AnonymousUserAccessMode` to Get or None
* If you with to run with no authentication:
    * In the web.config file, set the app settings value `Raven/AnonymousUserAccessMode` to All

## Setting up with IIS 6

When using IIS 6, you need to modify the Web.config file and remove the `system.webServer` element and add the following system.web element:

       <system.web>
               <httpHandlers>
                       <add path="*" verb="*" 
                                type="Raven.Web.ForwardToRavenRespondersFactory, Raven.Web"/>
               </httpHandlers>
       </system.web>

## Web Configuration

Many configuration options are available for tuning RavenDB and fitting it to your needs. See the [Configuration options](configuration.markdown) page for complete info.

## Recommended IIS Configuration

RavenDB isn't a typical web site because it needs to be running at all times. In IIS 7.5, you can set this using the following configuration settings:

* If you created a dedicated application pool for RavenDB, change the application pool configuration in the application host file (C:\Windows\System32\inetsrv\config\applicationHost.config) to:

       <add name="RavenApplicationPool" managedRuntimeVersion="v4.0" startMode="AlwaysRunning" />

* If Raven runs in an application pool with other sites, modify the application host file (C:\Windows\System32\inetsrv\config\applicationHost.config) to: 

       <application path="/Raven" serviceAutoStartEnabled="true" />
