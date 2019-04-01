# Configuration: Windows Clustering on Windows Server 2012

{INFO Windows Clustering feature is only available in the Enterprise Edition. /}

In order to ensure RavenDB high availability you can run it on a failover cluster.

##Requirements

* Windows Server 2012 R2
* Failover Clustering feature added
* Storage Area Network (SAN) configured

##Configuring RavenDB service

Execute the following steps on every cluster node:

1. If RavenDB isn't installed as a service, follow [this link](../../../server/installation/as-a-service) in order to do it
2. Stop RavenDB service by executing the following command on the command line: Raven.Server.exe /stop
3. In the Raven.Server.exe.config file set up Raven/DataDir to the SAN

*Note*: A failover cluster will take care of working RavenDB. If the failure takes place on one of the cluster nodes, then a failover cluster will start RavenDB service immediately. So it's important that RavenDB instance on every node should use the same data directory placed on the SAN. 

##Creating failover cluster

1. Go to Server Manager -> Tools -> Failover Cluster Manager
2. Select Create a cluster option from context menu  
![Figure 1.1: FCM Configuration](images\01CreateCluster_2012.jpg)
3. Go through the wizard   
![Figure 1.2: FCM Configuration](images\01CreateCluster_Wizard01_2012.jpg)
4. Add cluster nodes  
![Figure 1.3: FCM Configuration](images\01CreateCluster_Wizard02_2012.jpg)
5. Name a cluster and assign IP address for it  
![Figure 1.4: FCM Configuration](images\01CreateCluster_Wizard08_2012.jpg)
6. Confirm and unselect 'Add all eligible storage to the cluster'
![Figure 1.5: FCM Configuration](images\01CreateCluster_Wizard09_2012.jpg)
7. Wait for cluster creation
![Figure 1.6: FCM Configuration](images\01CreateCluster_Wizard10_2012.jpg)
7. Finish operation   
![Figure 1.7: FCM Configuration](images\01CreateCluster_Wizard11_2012.jpg)

##Adding RavenDB as a Generic Service resource

1. Right click on *Roles* of newly created cluster and choose *Configure Role...* option from context menu  
![Figure 2.1: FCM Configuration](images\02ConfigureService_2012.jpg)
2. Go through the High Availability Wizard  
![Figure 2.2: FCM Configuration](images\02ConfigureService_Wizard01_2012.jpg)
3. Choose Generic Service  
![Figure 2.3: FCM Configuration](images\02ConfigureService_Wizard02_2012.jpg)
4. Select RavenDB service  
![Figure 2.4: FCM Configuration](images\02ConfigureService_Wizard03_2012.jpg)
5. Type the cluster service name and assign the IP address under which RavenDB will be available  
![Figure 2.5: FCM Configuration](images\02ConfigureService_Wizard04_2012.jpg)
6. RavenDB has been already configured on every node to keep data in SAN, so no storage is needed here. Click *Next*  
![Figure 2.6: FCM Configuration](images\02ConfigureService_Wizard05_2012.jpg)
7. Click *Next*  
![Figure 2.7: FCM Configuration](images\02ConfigureService_Wizard06_2012.jpg)
8. Summary screen. Click *Finish*  
![Figure 2.8: FCM Configuration](images\02ConfigureService_Wizard07_2012.jpg)

{INFO: Windows Authentication usage}
In a clustered environment a host can have more than one name associated. That can cause problems for Kerberos.
If you are going to use Windows Authentication then use static IP addresses and add a DNS A record for RavenDB.
{INFO/}

##Summary

* After click on *Roles* in a cluster tree you will see RavenDB service information  
![Figure 3: FCM Configuration](images\03ServiceInstalled_2012.jpg)
* Service properties  
![Figure 4.1: FCM Configuration](images\04ServiceProperties_2012.jpg)
	* General options  
![Figure 4.2: FCM Configuration](images\04ServiceProperties_01General_2012.jpg)
	* Failover options  
![Figure 4.3: FCM Configuration](images\04ServiceProperties_02Failover_2012.jpg)
* Failure simulation  
 1. Right click on RavenDB resource and choose *Simulate failure of this resource* 
![Figure 5.1: FCM Configuration](images\05FailureSimulation_2012.jpg)
 2. During failover the resource has been moved to a different node and is in pending mode  
![Figure 5.2: FCM Configuration](images\05FailureSimulation_Pending_2012.jpg)
 3. Failover succeeded  
![Figure 5.3: FCM Configuration](images\05FailureSimulation_FailoverSuccess_2012.jpg)

## Related articles

 - [Windows Clustering on Windows Server 2008](./windows-server-2008)
