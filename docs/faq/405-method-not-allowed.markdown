#Resolving (405) Method Not Allowed when running on IIS 7

You may get a 405 Method Not Allowed when trying to create an index from the Client API when RavenDB is running  in IIS 7.

This usually happen when you are running RavenDB inside a virtual directory inside IIS. The problem is a conflict that occurs with the WebDAV module. To resolve that, you need to edit the web.config file in the parent directory and add:

    <system.webServer>
       <modules runAllManagedModulesForAllRequests="true">
         <remove name="WebDAVModule" />
       </modules>
     </system.webServer>

That will remove the WebDAV module and resolve the conflict.

**Important**: This modification is not to the RavenDB's web.config file, it is to the web.config of the parent application.
