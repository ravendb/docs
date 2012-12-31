#Windows Clustering
###Note that this feature is only available in the Enterprise Edition of RavenDB.

In order to ensure RavenDB high availability you can run it on a failover cluster.

##Requirements
* Windows Server 2008
* Failover Cluster Manager snap-in installed
* Storage Area Network (SAN) configured

##Configuring RavenDB service
Execute the following steps on every cluster node:

1. If RavenDB isn't installed as a service follow this link in order to do it
2. Stop RavenDB service by executing the following command on the command line: Raven.Server.exe /stop
3. In the Raven.Server.exe.config file set up Raven/DataDir to the SAN

*Note*: A failover cluster will take care of working RavenDB. If the failure take place on one of the cluster nodes then a failover cluster will start RavenDB service immediately. So it's important that RavenDB instance on every node should use the same data directory placed on the SAN. 

##Creating failover cluster

1. Go to Start -> Administrative Tool -> Failover Cluster Manager
2. Select Create a cluster option from context menu  
![Figure 1.1: FMC Configuration](images\01CreateCluster.jpg)
3. Go through the wizard   
![Figure 1.2: FMC Configuration](images\01CreateCluster_Wizard01.jpg)
4. Add cluster nodes  
![Figure 1.3: FMC Configuration](images\01CreateCluster_Wizard02.jpg)
5. Validate a cluster configuration  
![Figure 1.4: FMC Configuration](images\01CreateCluster_Wizard03.jpg)
6. Validation Wizard will be shown  
![Figure 1.5: FMC Configuration](images\01CreateCluster_Wizard04.jpg)
7. Run all test to make sure that nodes are configured properly  
![Figure 1.6: FMC Configuration](images\01CreateCluster_Wizard05.jpg)
8. Click *Next*  
![Figure 1.7: FMC Configuration](images\01CreateCluster_Wizard06.jpg)
9. Wait for validation finish  
![Figure 1.8: FMC Configuration](images\01CreateCluster_Wizard07.jpg)
10. Name a cluster and assign IP address for it  
![Figure 1.9: FMC Configuration](images\01CreateCluster_Wizard08.jpg)
11. Confirm and finish next steps  
![Figure 1.10: FMC Configuration](images\01CreateCluster_Wizard09.jpg)

##Adding RavenDB as a Generic Service resource

1. Right click on *Services and applications* of newly created cluster and choose *Configure a Service or Application...* option from context menu  
![Figure 2.1: FMC Configuration](images\02ConfigureService.jpg)
2. Go through the High Availability Wizard  
![Figure 2.2: FMC Configuration](images\02ConfigureService_Wizard01.jpg)
3. Choose Generic Service  
![Figure 2.3: FMC Configuration](images\02ConfigureService_Wizard02.jpg)
4. Select RavenDB service  
![Figure 2.4: FMC Configuration](images\02ConfigureService_Wizard03.jpg)
5. Type the cluster service name and assign the IP address under which RavenDB will be available  
![Figure 2.5: FMC Configuration](images\02ConfigureService_Wizard04.jpg)
6. RavenDB has been already configured on every node to keep data in SAN, so no storage is needed here. Click *Next*  
![Figure 2.6: FMC Configuration](images\02ConfigureService_Wizard05.jpg)
7. Click *Next*  
![Figure 2.7: FMC Configuration](images\02ConfigureService_Wizard06.jpg)
8. Summary screen. Click *Finish*  
![Figure 2.8: FMC Configuration](images\02ConfigureService_Wizard07.jpg)

##Summary

* After click on a service name in a cluster tree you will see RavenDB service information  
![Figure 3: FMC Configuration](images\03ServiceInstalled.jpg)
* Service properties  
![Figure 4.1: FMC Configuration](images\04ServiceProperties.jpg)
* * General options  
![Figure 4.2: FMC Configuration](images\04ServiceProperties_01General.jpg)
* * Failover options  
![Figure 4.3: FMC Configuration](images\04ServiceProperties_02Failover.jpg)
* Failure simulation  
 1. Right click on RavenDB resource and choose *Simulate failure of this resource* 
![Figure 5.1: FMC Configuration](images\05FailureSimulation.jpg)
 2. During failover the resource has been moved to a different node and is in pending mode  
![Figure 5.2: FMC Configuration](images\05FailureSimulation_Pending.jpg)
 3. Failover succeed  
![Figure 5.3: FMC Configuration](images\05FailureSimulation_FailoverSuccess.jpg)