# Monitoring: Disk I/O Performance Monitor

It is possible to track I/O rates in RavenDB using the Monitor tool.

To get the Monitor tool you need to download RavenDB's ZIP package from the [download page](https://ravendb.net/downloads/builds)
The tool is located in the extracted ZIP package in the folder: \Raven.Monitor\bin\Debug\Raven.Monitor.exe

First, make sure there is a server instance running locally.   
Then open cmd as administrator and run Raven.Monitor.exe without specifying options to see the help menu.   

![Figure 1. Monitoring : View Raven.Monitor help](images/monitoring-Disk_IO_test-1.PNG)   

To start monitoring run: 
{CODE-BLOCK:plain}
    Raven.Monitor.exe --disk-io
{CODE-BLOCK/}
![Figure 2. Monitoring : Run disk I/O test](images/monitoring-Disk_IO_test-2.PNG)

You can also specify a different process or server URL.   
The default monitoring time is 60 seconds.   

If you'd like to control the duration or do it remotely you can run the monitor as a server:
{CODE-BLOCK:plain}
    Raven.Monitor.exe --run-as-server
{CODE-BLOCK/}
![Figure 3. Monitoring : Run as server](images/monitoring-Disk_IO_test-3.PNG)   
Now the monitor is listening to port 9091 and can accept the following REST calls:
{CODE-BLOCK:plain}
http://localhost:9091/monitor/start-monitoring
http://localhost:9091/monitor/stop-monitoring
{CODE-BLOCK/}

While the monitor is running you can run your database scenario. 
When you're done recording, you can [view the report](../../../studio/management/disk-io-viewer) in the studio.

## Related articles

- [Studio: Disk I/O Viewer](../../../studio/management/disk-io-viewer)
