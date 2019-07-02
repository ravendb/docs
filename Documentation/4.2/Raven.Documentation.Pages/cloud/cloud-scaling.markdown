#RavenDB on the Cloud: Scaling Instances

---

{NOTE: }

* RavenDB instances in the **development** and **production** tiers can be **upscaled** or **downscaled** into an 
instance of the *same tier.*  

* An instance can't be converted to a different tier, but databases can be [migrated](cloud-migration) between any 
two RavenDB instances.  
{NOTE/}

---

{PANEL: }

To scale a RavenDB cloud instance, go to the [Products tab](../cloud/cloud-control-panel#the-products-tab) 
in the cloud Control Panel. Click on the `Manage` button for the product you want to scale. In the `General` 
tab you will see buttons to `change instance type` and `change storage`:

![Figure 1: Scaling Buttons](images/CloudScaling_1.png)  
  
Clicking on the `change instance type` button will open a menu with options for `CPU Priority` and `Cluster Size`:  

![](images/CloudScaling_instance.png)  
  
Clicking on the `change storage` button will open a menu with options for the instance's storage capacity:  

![](images/CloudScaling_storage.png)  
  
For production tier instances, there are two types of storage: standard and premium. In this menu, premium 
storage gives you the additional option of selecting the amount of IOPS (in/out operations per second) that the 
instance can handle.  

{PANEL/}

