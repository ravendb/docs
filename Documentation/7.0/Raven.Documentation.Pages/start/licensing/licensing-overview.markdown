# Licensing Overview
---

{NOTE: }

* The license key is in JSON format.  
  It is sent to the email address that was provided when the license was obtained.  

* A few **License Types** are available, learn more about them below.  
  {INFO: }
  Visit the [Pricing page](https://ravendb.net/buy) to see **which features are included with each license type**.
  {INFO/}

* Each license has a specific expiration date.  
  To renew your license see [renew license](../../start/licensing/renew-license).  

* In this page:
    * [License types](../../start/licensing/licensing-overview#license-types)
        * [Developer](../../start/licensing/licensing-overview#developer)
        * [Community](../../start/licensing/licensing-overview#community)
        * [Professional](../../start/licensing/licensing-overview#professional)
        * [Enterprise](../../start/licensing/licensing-overview#enterprise)
        * [ISV Licenses](../../start/licensing/licensing-overview#isv-licenses)
    * [Manage license view](../../start/licensing/licensing-overview#manage-license-view)

{NOTE/}

---

{PANEL: License types}

The following licences are available.

---

### Developer

* The developer license is for development use only and isn't applicable for commercial use.  
  You must upgrade this license before going into production.   
* It is fully featured but temporary - license lasts 6 months and can be renewed.  
* Certificates exceeding 4 months expiration period are Not allowed to be used with this license.  
  Automatic renewal of Let’s Encrypt certificates is disabled.  
* When you are ready to go into production:  
  * Be sure to choose a license that has all features with which your client was developed.  
  * [Upgrade to a production license](https://ravendb.net/buy) and then [replace](../../start/licensing/replace-license) the developer license.  
  
* The developer license can be obtained [here](https://ravendb.net/buy#developer).

---

### Community

* A basic production-grade license.
* Community licenses last for one year and can be renewed every year.  

---

### Professional

* A standard production-grade license.  
* Extended automatically if the server has access to RavenDB's License Server.  

---

### Enterprise

* A high-performance, fully-featured production-grade license.  
* Extended automatically if the server has access to RavenDB's License Server.  

---

### ISV Licenses

* ISV licenses are commercial licenses that allow you to redistribute RavenDB with your software  
  for on-premise installation and use by your customers.  
* Available ISV license types are: Essential, Professional, and Enterprise.  
* [Contact RavenDB for more details](https://ravendb.net/contact).

{PANEL/}

{PANEL: Manage license view}

![About page](images/about-page.png "About page")

1. **About**  
   Click to open the About page and view RavenDB version, license, support info, and more.  

2. **License**  

      ![License Info](images/license_info.png "License Info")

      * A. Informs you of your current license type  
      * B. Informs you of your license expiration date  
      * C. Informs you whether your server is currently connected to RavenDB's license server.  
        When connected, you can:  
        ![License Options](images/license_options.png "License Options")
        * **Renew License** to extend its expiration date
        * **Replace** your license
        * **Force update** to sync with the license server
      
        {INFO: }
        Connectivity issues are often caused by a firewall blocking the connection, 
        or the server running offline.  

        * Make sure that RavenDB has access to `api.ravendb.net`.  
        * Click the refresh button to check the connection.  
        {INFO/}

3. **Software Version**  
   
      ![Software Version](images/software_version.png "Software Version")
   
      * **Server version**  
        Shows your current server version.  
        Click `Changelog` to see logged version changes. 
      * **Updates**  
        Informs you whether you are using the latest server version.  
        Click Check for updates to see if an update is available.  

4. **Support & Community**  

      ![Support & Community](images/support_and_ommunity.png "Support & Community")

      * **Support type**  
        Informs you of the support type you are entitled to,  
        and allows you to join our community GitHub discussions.  
      * **Let's solve it together**  
        Allows you to join our developers Discord community.  

5. **License details tab**  
   Details the features that are enabled for your server with each license type.  
   Use the scrollbar to slide through the full features list.  

      ![License Details tab](images/license_details_tab.png "License Details tab")

6. **Support plan tab**  
   Specifies your current support plan and allows you to upgrade it.  

      ![Support plan tab](images/support_plan_tab.png "Support plan tab")

{PANEL/}

## Related Articles

### Licensing
- [Activate license](../../start/licensing/activate-license)
- [Replace license](../../start/licensing/replace-license)
- [Renew license](../../start/licensing/renew-license)
- [Force update license](../../start/licensing/force-update)

### Server
- [License configuration options](../../server/configuration/license-configuration)

## Related Links
- [Get License](https://ravendb.net/buy)
- [Support Options](https://ravendb.net/support)

