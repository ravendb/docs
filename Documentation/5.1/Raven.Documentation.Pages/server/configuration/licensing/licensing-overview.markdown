# Licensing Overview
---

{NOTE: }

* Your license key is sent to the email address that you entered when registering your license.  
  It is in .json format.  

* There are a few [licensing options](../../../server/configuration/licensing/licensing-overview#licensing-options) 
  available for you to choose from according to your needs.  
  See the [pricing page](https://ravendb.net/buy) for more info about which features are included in each license.  

* Licenses are activated and updated automatically unless: 
   * You change the [license configurations](../../../server/configuration/licensing/license-configuration) in your server's settings.json.
   * RavenDB's "License Server" cannot connect to check and update any changes in your license.  
     This is usually either caused by a firewall blocking the connection or when servers run offline.  
     * Make sure that RavenDB has access to `api.ravendb.net`.
     * See Studio [License Server](../../../studio/server/license-management#license-information) to check your connection to
      RavenDB's License Server.

* Licenses can also be [managed via Studio](../../../studio/server/license-management).

* In this page:
   * [Licensing Options](../../../server/configuration/licensing/licensing-overview#licensing-options)
   * [Activating Your License](../../../server/configuration/licensing/licensing-overview#activating-your-license)
   * [Changing Your License](../../../server/configuration/licensing/licensing-overview#changing-your-license)
   * [Renewing Your License](../../../server/configuration/licensing/licensing-overview#renewing-your-license)
   * [Force Update for Immediate Update](../../../server/configuration/licensing/licensing-overview#force-update-for-immediate-update)
   * [Offline Activation, Upgrade, and Renewal](../../../server/configuration/licensing/licensing-overview#renewing-your-license)

{NOTE/}

---

{PANEL: Licensing Options}

See the [pricing page](https://ravendb.net/buy) for more info about which features are included in each license.  

* **Development** (fully featured and temporary - must be upgraded before launch)  
   * Developer licenses last for 6 months.
   * The certificate lasts for 3 months, but can be renewed in the [Studio Certificates Management View](../../../studio/server/certificates/server-management-certificates-view#studio-certificates-management-view)
     by navigating in Studio from the **Manage Server** tab --> **Certificates** --> **Server Certificate** section.
* **Community** (basic production-grade)
   * Community licenses last for a year and can be extended every year.
* **Professional** (standard production-grade)
* **Enterprise** (high-performance, fully-featured production-grade)
* [Cloud portal](../../../cloud/cloud-overview) for cloud-based servers

{PANEL/}

{PANEL: Activating Your License}

Because licenses are activated and updated automatically by default, the following sections are only relevant if you 
changed the license configurations in the server's [settings.json](../../../server/configuration/configuration-options#json)
or if your server is running offline.

To activate your license, you must either embed the key directly into the settings.json, 
or provide a path to the license key.  

You can either [activate via Studio](../../../studio/server/license-management#register-license) or use the following 
[configuration](../../../server/configuration/licensing/license-configuration) options in settings.json:

* **License**  
    Embed the full license string for RavenDB. If `License` is specified, it overrides the `License.Path` configuration.  
    E.g. - `"License": paste your license key including curly brackets here`  
    Your license key was sent to the email that is registered with the license.

* **License.Path**  
    Path to the license file (either **full** or **relative to the Server folder**).  
    The file `license.json` is in each node's **...Cluster.Settings.zip** in wizard installs or possibly the **Server** folder if manually installed.  
    E.g. (full) - `"License.Path": "D:\\RavenDB\\Server\\license.json"`  
    E.g. (relative) - `"License.Path": "License\\license.json"`  

{NOTE: }

* Each instance of RavenDB has to be registered with a license.  
* A development license isn't applicable for commercial use and must be upgraded before launching.  
  * When [upgrading](https://ravendb.net/buy), be sure to choose the license that has the features 
    with which your client was developed.  
    For example, if you need Backups, ETL, Encryption, or Hub/Sink replication, be sure that your license includes these features.
  * To prevent unexpected issues upon launching, you can use a temporary [cloud instance](../../../cloud/cloud-overview#instances-provisioning-and-ravendb-products) 
    with machine performance specs similar to your plans for production. This way you can securely test the API and processing of 
    data that mimics your actual data-set and planned infrastructure.  

{NOTE/}

{PANEL/}

{PANEL: Changing Your License}

Upon [changing your license](https://ravendb.net/buy), RavenDB will send the new license key to the email 
registered with the license.  
Copy the entire code block and either:  

* [Update via Studio](../../../studio/server/license-management#replace-license) 
* [Replace the license key](../../../server/configuration/licensing/licensing-overview#activating-your-license) where it is located (either directly in the settings.json or 
  in the file path that settings.json is configured for.)  

If the default connection with RavenDB's [License Server](../../../studio/server/license-management#license-information) is active, 
the license will try to update itself every 24 hours.  
To update it immediately, you can [force the update in Studio](../../../studio/server/license-management#force-update).

{INFO: Maintaining auto-renewal of Let's Encrypt certificate}
If you set up with the [Setup Wizard](../../../start/installation/setup-wizard) and used a Let's Encrypt certificate, 
contact [customer support](https://ravendb.net/contact) when changing your license to maintain auto-renewals 
of certificates.  
Otherwise, changing your license ID will cause a mismatch between the new one and the ID that Let's Encrypt expects when renewing.
{INFO/}

{PANEL/}

{PANEL: Renewing Your License}

By default, RavenDB automatically renews **Professional** and **Enterprise** licenses as long as they aren't canceled
and if your server has an active connection to RavenDB's [License Server](../../../studio/server/license-management#license-information).  

If the automatic renewal feature is turned off in the settings.json, you can renew your license with the same process 
as [activating your license](../../../server/configuration/licensing/licensing-overview#activating-your-license).

{PANEL/}

{PANEL: Force Update for Immediate Update}

Unless the feature is disabled, RavenDB automatically checks for license updates every 24 hours.  

If you've upgraded your license and want to work with the new features today, you can 
[force the update in Studio](../../../studio/server/license-management#force-update).

{PANEL/}

{PANEL: Offline Activation, Upgrade, and Renewal}

If your server is disconnected from the internet, RavenDB's default automatic renewal feature cannot trigger renewal.  
The feature must be disabled and the license must be manually placed, either directly in the settings.json 
or in the license.json file.

1. **Disable automatic renewal.**  
   In the [settings.json](../../../server/configuration/configuration-options#json) 
   (located in the installation package "Server" folder), add the following configuration 
   `"License.DisableAutoUpdateFromApi": "true"`.
2. **Either set the path to the license file in settings.json or embed the license key directly into the settings.json.**  
   * File path option - `"License.Path": "path to your .json license file"`
   * Embed in settings.json - `"License": paste your license key including curly brackets here`.
   * See [examples here](../../../server/configuration/licensing/licensing-overview#activating-your-license).
4. **Other license configuration options**  
   See [Configuration: License Options](../../../server/configuration/licensing/license-configuration)

{PANEL/}

## Related Articles

- [Studio : Licensing Management](../../../studio/server/license-management)
- [Configuration : License Options](../../../server/configuration/licensing/license-configuration)
- [Configuring the settings.json](../../../server/configuration/configuration-options#json)


## Related Links

- [Get License](https://ravendb.net/buy)
- [Support Options](https://ravendb.net/support)

