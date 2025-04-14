# Renew License

---

{NOTE: }

* When your license **expires** Studio is blocked.  
  Client API operations and other RavenDB features will continue to work.  
  However, any usage of expired RavenDB licenses is outside the license agreement  
  and doesn't comply with the [EULA terms](https://ravendb.net/terms).  

* **Renew your license** as described below.  

* In this page:
    * [Renew commercial licenses](../../start/licensing/renew-license#renew-commercial-licenses)
    * [Renew Developer & Community licenses](../../start/licensing/renew-license#renew-developer-&-community-licenses)

{NOTE/}

---

{PANEL: Renew commercial licenses}

* This section relates to all commercial licenses: **Professional**, **Enterprise**, and all **ISV licenses**.  

{NOTE: }
**Automatic renewal**:  

---
If your server has an active connection to RavenDB's [License Server](../../start/licensing/licensing-overview#license-server),  
and if the [DisableAutoUpdate](../../server/configuration/license-configuration#license.disableautoupdate) configuration is Not set to true,  
then these commercial licenses will be automatically renewed.  
{NOTE/}

{NOTE: }
**Manual (offline) renewal**:  

---
If the connection to the License Server is unavailable, contact [customer service](https://ravendb.net/contact) to extend your license.  
A license renewal email will be sent to you (the license ID will stay the **same**).  
Copy the new license key from the mail and proceed with one of the following options.  

**Either**:  

* Replace the existing license key with the new one from Studio, as described [here](../../start/licensing/replace-license#replace-license-from-studio).

**Or**:  

* Set [DisableAutoUpdateFromApi](../../server/configuration/license-configuration#license.disableautoupdatefromapi) to true.

* Activate the new license key using the [configuration keys](../../start/licensing/activate-license#activate-license-with-configuration-keys).

* Restart your server (needed only if any configuration key was changed).  
  {NOTE/}

{PANEL/}

{PANEL: Renew Developer & Community licenses}

* The **Developer** and **Community** licenses are renewed from Studio,  
  the 'Renew' button is available only for those license types.  
    {NOTE: }
  
     - Servers using a Community license are **required to run the latest major version**.  
       E.g., if RavenDB `7.0` is released and your server runs RavenDB `6.2` with a Community license, 
       you will be required to upgrade RavenDB to version `7.0`.  
     - Running a major version older than the latest with a Community license will **block your server's access to Studio**.  
     - This requirement does not apply to licenses other than _Community_.  
  {NOTE/}

* They can be renewed when there are less than 30 days remaining on your current license.

![Renew License](images/renew-1.png "Renew License")

1. **About**  
   Navigate to the _About_ page in Studio  

2. **Renew**  
   Click the _Renew License_ button  

   * ![Renew License](images/renew-2.png "Renew and Submit")  

3. **Renew**  
  Click to renew the current license.  
  The renewed license will be sent to you by mail.  

4. **Submit**  
   Paste the renewed license key and click Submit.

{PANEL/}

## Related Articles

### Licensing
- [Licensing overview](../../start/licensing/licensing-overview)
- [Activate license](../../start/licensing/activate-license)
- [Replace license](../../start/licensing/replace-license)
- [Force update license](../../start/licensing/force-update)

### Server
- [License configuration options](../../server/configuration/license-configuration)




