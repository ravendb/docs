# License Management
---

{NOTE: }

* Your [license](../../server/configuration/licensing/licensing-overview) affects performance and which features are available to you. 

* This article describes how to manage your license via Studio. 

* You can also manage your license by [configuring the server's settings.json](../../server/configuration/licensing/licensing-overview#activating-your-license)

In this page: 

* [About Version, License, and Support View](../../studio/server/license-management#about-version,-license,-and-support-view)
* [Register License](../../studio/server/license-management#register-license)
* [Replace License](../../studio/server/license-management#replace-license)
* [Force Update](../../studio/server/license-management#force-update)
* [Renew License](../../studio/server/license-management#renew-license)

{NOTE/}

{PANEL: About Version, License, and Support View}

![About Version, License, and Support View](images/manage-license-full-view.png "About Version, License, and Support View")

#### 1. **About**  
   Select to see the version, license, and support view.  

#### 2. **Version**  
Specifies which RavenDB version this server is using.  
Knowing the version is important because documentation, feature-sets, and patches are version sensitive.

   * **Send Feedback**  
     Click here to send RavenDB feedback about any issues you've encountered since updating your server version.
   * **Update Available**  
     Click to go to our downloads page.  
     See [instructions about minor upgrades](../../start/installation/upgrading-to-new-version).  
     See [instructions about major version upgrades (AKA migration)](../../migration/server/data-migration).

#### 3. **License Information**  

   * **Type**  
     Specifies which license you're using, its specs and which features are available.
   * **Expires**  
     Specifies the date at which the license will expire.  
     * Production licenses renew automatically unless they're canceled or the auto-renewal feature is disabled in the 
       server's settings.json.
     * Developer licenses are fully featured but last for 3 months.  
       They can be extended for another 3 months by [getting a new license](https://ravendb.net/buy#developer) 
       and [renewing](../../studio/server/license-management#renew-license).
       * When you are ready to go into production, you must first [upgrade to a production license](https://ravendb.net/buy)
         and then [replace it](../../studio/server/license-management#replace-license).
   * **License Server**  
     Notifies you if there is an active connection with RavenDB's automatic update License Server.
     If you aren't connected, it is usually either because a firewall is blocking the connection or your server is running offline.
     ![Automatic Update License Server Connection](images/license-server.png "Automatic Update License Server Connection")
   * [Replace License](../../studio/server/license-management#replace-license) - Copy the new license code from the email and paste it here. 
   * [Force Update](../../studio/server/license-management#force-update) - Force immediate update of license to use new features today. 
   * [Renew License](../../studio/server/license-management#renew-license) - Copy the new license code from the email and paste it here. 

#### 4. **Support Plan**  
Specifies which support options you have with your current support plan.  

   * **Access**  
     Click to select Github community forum or Google Group archives. 
     **Upgrade**
     Click to go to our support options page.  
   

{PANEL/}

{PANEL: Register License}

In order to register an instance of RavenDB with a license, you can: 

![Register License](images/manage-license-1.png "Register License")

1. **About**  
   Navigate to the _About_ page in Studio 
2. **Register**  
   Click the _Register_ button, copy/paste your license code from the email and click submit.

* Use the following [configuration](../../server/configuration/license-configuration) options:
   * **License**  
     The full license string for RavenDB. If **License** is specified, it overrides the `License.Path` configuration.
   * **License.Path**  
     Path (either **full** or **relative to the server folder**) to the license file.  
     Default: `license.json` in the **server** folder.  
     E.g. - "License.Path": "D:\\RavenDB\\Server\\license.json"  
     E.g. - "License.Path": "License\\license.json"  

{NOTE: }

* Each instance of RavenDB has to be registered with a license.  
* A development license isn't applicable for commercial use.  

{NOTE/}

{PANEL/}

{PANEL: Replace License} 

To replace license, copy the new license code block from the email, click _REPLACE LICENSE_, and submit a new one.

![Replace License](images/manage-license-2.png "Replace License")

{PANEL/}

{PANEL: Force Update }

After [changing your license](https://ravendb.net/buy), in order to update the license instantly, click _FORCE UPDATE_.

![Force Update](images/manage-license-3.png "Force Update")

{NOTE License information is automatically updated nightly.  _FORCE UPDATE_ does not stop this operation. /}

{PANEL/}

{PANEL: Renew License}

If your server is connected to the automatic update [License Server](../../studio/server/license-management#license-information),
your license will automatically update as long as it hasn't been canceled.

If you aren't connected to the License Server, copy the entire license code block from the license renewal email and paste it into 
the **Renew License** interface. 

![Renew License](images/manage-license-4.png "Renew License")

{PANEL/}

## Related Articles

- [Licensing Overview](../../server/configuration/licensing/license-configuration)
- [Configuration : License Options](../../server/configuration/license-configuration)
- [Configuring the settings.json](../../../server/configuration/configuration-options#settings.json)


## Related Links

- [Get License](https://ravendb.net/buy)
- [Support Options](https://ravendb.net/support)

