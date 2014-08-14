# Troubleshooting : Enabling logging

RavenDB has extensive support for logging, enabling you to figure out exactly what is going on in the server.   
By default, **logging is turned off**, but it can be easily enabled by creating a file called `NLog.config` in base directory of a server (remember to restart your server).

## Sample log file

{CODE-START:xml/}
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.netfx35.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<targets>
		<target 
			xsi:type="AsyncWrapper"
			name="AsyncLog">

			<target xsi:type="SplitGroup">
				<!-- create log files with a max size of 256 MB -->
				<target 
					name="File" 
					xsi:type="File"    
				    archiveAboveSize="268435456"
				    fileName="${basedir}\Logs\${shortdate}.log">
					<layout xsi:type="CsvLayout">
						<column name="time" layout="${longdate}" />
						<column name="logger" layout="${logger}"/>
						<column name="level" layout="${level}"/>
						<column name="database" layout="${mdc:item=database}"/>
						<column name="message" layout="${message}" />
						<column name="exception" layout="${exception:format=tostring}" />
					</layout>
				</target>
			</target>
		</target>
	</targets>
	<rules>
        <!-- supported levels: Off, Fatal, Error, Warn, Info, Debug, Trace -->
   		<logger name="Raven.*" writeTo="AsyncLog" minlevel="Warn" />
	</rules>
<</nlog>
{CODE-END/}

## Remarks

{INFO To make it even easier for you, we have included `NLog.Ignored.config` file in Server directory inside distribution package that just need to be renamed. /}

#### Related articles

TODO