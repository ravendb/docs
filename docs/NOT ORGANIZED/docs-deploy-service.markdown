#Raven - Running as a service
Raven supports running as a system service, creating its own HTTP server and processing requests internally.

##Installing the Raven service

* Extract distributed zip
* Go to the Server directory
* Execute the following command on the command line:
    `Raven.Server.exe /install 
    Note: Raven may ask you for administrator privileges while installing the service.`

That's it. Raven is now installed and running.

##Uninstalling the Raven service

* Go to the Server directory
* Execute the following command on the command line:
    `Raven.Server.exe /uninstall`

##Server Configuration
You can set the following configuration options in the appSettings section of Raven.Server.exe.config:

* Raven/DataDir - The physical location for the Raven data directory.
* Raven/AnonymousAccess - What access rights anonymous users have. The default is Get (anonymous users can only read data). The other options are None and All.
* Raven/Port - The port that Raven will listen to., The default is 8080.
* Raven/VirtualDirectory - The virtual directory that Raven will listen to. The default is empty.
* Raven/PluginsDirectory - The plugin directory for extending Raven. The default is a directory named "Plugins" under Raven base directory.
* Raven/MaxPageSize - The maximum number of results a Raven query can return (overrides any page size set by the client). The default is 1024.

Changes to the config file or additions / removal from the Plugins directory will not be picked up automatically by the Raven service, you need to restart the service to recognize your changes. You can do so using:  
`Raven.Server.exe /restart`