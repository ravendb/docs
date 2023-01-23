# Force License Update

---

{NOTE: }

* Every 24 hours (counted from server startup), RavenDB will check the [License Server](../../start/licensing/licensing-overview#license-server)  
  to see if there are any updates made to your current license that need to be applied.  
  This is done for all license types.  

* This automatic update will be applied provided that:  
  * You have an active connection to RavenDB's License Server.  
  * Both [DisableAutoUpdate](../../server/configuration/license-configuration#license.disableautoupdate) and [DisableAutoUpdateFromApi](../../server/configuration/license-configuration#license.disableautoupdatefromapi) are set to false.  

* If changes were made to your current license and you want to __apply immediately__,    
  then you can __force the update from Studio__.  

* In this page:
    * [Force license update from Studio](../../start/licensing/force-update#force-license-update-from-studio)

{NOTE/}

---

{PANEL: Force license update from Studio}

![Force Update](images/force-update.png "Force-update")

1. __About__  
   Navigate to the _About_ page in Studio.  

2. __Force Update__  
   Click the _FORCE UPDATE_ button.  
   This will update your current license immediately.  

{PANEL/}

## Related Articles

### Licensing
- [Licensing overview](../../start/licensing/licensing-overview)
- [Activate license](../../start/licensing/activate-license)
- [Replace license](../../start/licensing/replace-license)
- [Renew license](../../start/licensing/renew-license)

### Server
- [License configuration options](../../server/configuration/license-configuration)


