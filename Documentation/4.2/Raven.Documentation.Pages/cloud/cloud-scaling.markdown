#Cloud: Scaling
---

{NOTE: }

After a RavenDB cloud instance has been set up, you can **scale** it, or modify its [configuration](cloud-instances) 
to change the workload it can handle.  

* Instances in the **development** and **production** tiers can be **upscaled** or **downscaled** into an 
instance of the *same tier.*  
* An instance can't be converted to a different tier, but data can be [migrated](cloud-migration) between any 
two RavenDB instances.  
{NOTE/}

---

{PANEL: }

To scale a RavenDB cloud instance, go to the [products tab](../cloud/cloud-portal/cloud-portal#the-products-tab) 
in the cloud portal and click on the `Manage` button for the product you want to scale:  

!["Manage Product"](images/scaling-001-manage.png "Manage Product")  
  
In the `General` tab you will see buttons to `change instance type` and `change storage`:  

!["Scaling Buttons"](images/scaling-002-buttons.png "Scaling Buttons")  
  
Clicking on the `change instance type` button will open a menu with options for `CPU Priority` and `Cluster Size`:  

!["Scaling Instance Type"](images/scaling-003-instance.png "Scaling Instance Type")
  
Clicking on the `change storage` button will open a menu with options for the instance's storage capacity:  

!["Scaling Storage"](images/scaling-004-storage.png "Scaling Storage")
  
There are two types of storage: standard and premium. Selecting premium storage gives you the additional option 
of configuring the number of IOPS (input/output operations per second) that the instance can handle:  

!["Premium IOPS"](images/scaling-005-premium.png "Premium IOPS")  

{NOTE: }
When you scale a development tier instance, it will go down temporarily while the configuration is updated. 
Production tier instances are scaled in a **rolling update**, one cluster node at a time, so your cluster will 
not experience any downtime.  
{NOTE/}

{PANEL/}

##Related Articles

**Cloud**  
[Overview](cloud-overview)  
[Tiers and Instances](cloud-instances)  
[Pricing, Payment and Billing](cloud-pricing-payment-billing)  
[Migration](cloud-migration)
