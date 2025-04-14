# Force License Update

---

{NOTE: }

* Every 24 hours (counted from server startup), RavenDB will check the [License Server](../../start/licensing/licensing-overview#license-server)  
  to see if there are any updates made to your current license that need to be applied.  
  This is done for all license types.  

* This automatic update will be applied provided that:  
  * You have an active connection to RavenDB's License Server (api.ravendb.net).  
  * [DisableAutoUpdate](../../server/configuration/license-configuration#license.disableautoupdate) is set to false.  

* If changes were made to your current license and you want to **apply immediately**,    
  then you can **force the update from Studio**.  
  This action also requires an active connection to the [License Server](../../start/licensing/licensing-overview#license-server).

* In this page:
    * [Force license update from Studio](../../start/licensing/force-update#force-license-update-from-studio)

{NOTE/}

---

{PANEL: Force license update from Studio}

![Force Update](images/force-update.png "Force-update")

1. **About**  
   Click to open the Studio _About_ view.  

2. **Force Update**  

   * Click the _Force Update_ button.  
     This will update your current license immediately.  
   
   * If [DisableAutoUpdateFromApi](../../server/configuration/license-configuration#license.disableautoupdatefromapi) is set to _true_,
     the license will be updated from the [configuration keys](../../start/licensing/activate-license#activate-license-with-configuration-keys).  
     If set to _false_, the license will be updated from the [License Server](../../start/licensing/licensing-overview#license-server), provided you have an active connection.  

{PANEL/}

## Related Articles

### Licensing
- [Licensing overview](../../start/licensing/licensing-overview)
- [Activate license](../../start/licensing/activate-license)
- [Replace license](../../start/licensing/replace-license)
- [Renew license](../../start/licensing/renew-license)

### Server
- [License configuration options](../../server/configuration/license-configuration)


