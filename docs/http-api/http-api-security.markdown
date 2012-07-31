#HTTP API - Security for Anonymous Access
With any document accessible by a URL on the server, some measure of security is necessary to secure access to these documents.

Any method that would normally be used to authenticate and secure access to a given URL can be used, but RavenDB also provides security options to control anonymous access.

By default such anonymous users can GET any documents they want, but not modify or add any documents or indexes. This option is called "Get". The other options are to grant anonymous users all privileges on the database ("All") or no access at all ("None"). You can set which option you prefer in the app.config as follows: 

    <appSettings>  
         <add key="Raven/AnonymousAccess" value="All"/>  
    </appSettings>

By default, while running the server in debug mode, anonymous users are granted Get access.

    Raven.Server.exe /debug