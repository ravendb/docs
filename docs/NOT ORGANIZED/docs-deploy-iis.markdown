#Raven - Deploying to IIS
Raven can be run as an IIS application.

#Installing Raven in IIS
* Extract the distribution zip.
* In IIS Manager, create a new Web Site and point its physical path to the "/Web" folder in the extracted folder.
* Set the Application Pool to "ASP.Net v4.0" (or create a new Application Pool set to .NET 4.0 Integrated Pipeline)
* Set port and host, if you need them.
* Make sure that the user you set for the site has write access to the physical database location.

##Setting up on IIS 7.5
* Remove the WebDAVModule from the installed Modules in your server
* If you wish to run with authentication
* * Ensure that "Windows Authentication" is installed support to IIS 7.5 (by default it is not)
* * Enable "Windows Authentication"  for the RavenDB website.
* * In the web.config file, set the app settings value  "Raven/AnonymousAccess" to Get or None
* If you with to run with no authentication
* * In the web.config file, set the app settings value  "Raven/AnonymousAccess" to All

##Setting up on IIS 6
On IIS 6, you need to modify the Web.config and remove the system.webServer element and add the following system.web element:

       <system.web>
               <httpHandlers>
                       <add path="*" verb="*" 
                                type="Raven.Web.ForwardToRavenRespondersFactory, Raven.Web"/>
               </httpHandlers>
       </system.web>

##Web Configuration
You can set the following configuration options in the Web.config appSettings:

* Raven/DataDir - The physical location for the Raven data directory.
* Raven/AnonymousAccess - What access rights anonymous users have. The default is Get (anonymous users can only read data). The other options are None and All.
* Raven/Port - The port that Raven will listen to. The default is 8080.
* Raven/VirtualDirectory - The virtual directory that Raven will listen to. The default is empty.
* Raven/PluginsDirectory - The plugin directory for extending Raven. The default is a directory named "Plugins" under Raven base directory.
* Raven/MaxPageSize - The maximum number of results a Raven query can return (overrides any page size set by the client). The default is 1024.

##Recommended IIS Configuration
Raven isn't a typical web site because it needs to be running at all times. In IIS 7.5, you can set this using the following configuration settings:

* If you created a dedicated application pool for Raven, change the application pool configuration in the application host file (C:\Windows\System32\inetsrv\config\applicationHost.config) to:

    `<add name="RavenApplicationPool" managedRuntimeVersion="v4.0" startMode="AlwaysRunning" />`

* If Raven runs in an application pool with other sites, modify the application host file (C:\Windows\System32\inetsrv\config\applicationHost.config) to:

    `<application path="/Raven" serviceAutoStartEnabled="true" />`
