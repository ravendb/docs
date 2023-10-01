# Replace License

---

{NOTE: }

* Upon [upgrading your license](https://ravendb.net/buy), RavenDB will send 
  your new license key to the email provided when obtaining the license.  

* Replace the existing license with the new one using Studio, as shown below.  

* If you upgrade to RavenDB 6.0, you need to upgrade your current license key 
  before applying it to the new version. Use our online license upgrade tool 
  as explained [below](../../start/licensing/replace-license#upgrade-a-license-key-for-ravendb-6.x) 
  to upgrade your key.  

* In this page:
    * [Replace license from Studio](../../start/licensing/replace-license#replace-license-from-studio)  
    * [Maintain auto-renewal of Let's Encrypt certificates](../../start/licensing/replace-license#maintain-auto-renewal-of-let)  
    * [Upgrade a License Key For RavenDB 6.x](../../start/licensing/replace-license#upgrade-a-license-key-for-ravendb-6.x)  

{NOTE/}

---

{PANEL: Replace license from Studio}

Replace the existing license with the new one from the Studio:

![Replace License](images/replace-license_button.png "Replace License")

1. **About**  
   Click to open the Studio _About_ view  

2. **Replace**  
   Click the _Replace License_ button

![Paste and Submit](images/replace-license_paste-and-submit.png "Paste and Submit")

{PANEL/}

{PANEL: Maintain auto-renewal of Let's Encrypt certificates }

* If you set RavenDB up using the [Setup Wizard](../../start/installation/setup-wizard) 
  and used a Let's Encrypt certificate,  contact [customer support](https://ravendb.net/contact) 
  when changing your license to maintain auto-renewals of certificates.  
* Otherwise, changing your license ID will cause a mismatch between the new 
  license ID and the ID that Let's Encrypt expects when renewing the certificate.  

{PANEL/}

{PANEL: Upgrade a License Key For RavenDB 6.x}

If you have a license for a RavenDB version older than `6.x`, upgrading to version `6.0` 
will require you to upgrade your license key. Upgrading your key can be done in a few seconds 
using our online [License upgrade tool](https://ravendb.net/l/8O2YU1).  

![Replace License Tool](images/replace-license_replace-license-tool.png "Replace License Tool")

A new key will be sent to the email address you registered your current license with.  
[Replace](../../start/licensing/replace-license#replace-license-from-studio) your current key with the new one.  

{PANEL/}


## Related Articles

### Licensing
- [Licensing overview](../../start/licensing/licensing-overview)
- [Activate license](../../start/licensing/activate-license)
- [Renew license](../../start/licensing/renew-license)
- [Force update license](../../start/licensing/force-update)

### Server
- [License configuration options](../../server/configuration/license-configuration)
