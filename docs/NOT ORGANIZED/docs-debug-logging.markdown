#Enabling debug logging for Raven

Raven has extensive support for debug logging, enabling you to figure out exactly what is going on in the server. By default, logging is turned off but you can enable it at any time by creating a file called "NLog.config" in Raven's base directory with the following content:

    <nlog xmlns="http://www.nlog-project.org/schemas/NLog.netfx35.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
        <targets>
            <target 
                xsi:type="AsyncWrapper"
                name="AsyncLog">
                
            <target xsi:type="OutputDebugString">
            <layout xsi:type="SimpleLayout"/>
          </target>        
                
            </target>
            
        </targets>
        <rules>
            <logger name="Raven.*" writeTo="AsyncLog"/>
        </rules>
    </nlog>

You can then use a tool such as [dbgview](http://technet.microsoft.com/en-us/sysinternals/bb896647.aspx) to view the logs.