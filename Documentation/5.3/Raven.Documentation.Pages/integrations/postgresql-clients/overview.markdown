# PostgreSQL Clients: Overview

* RavenDB integrates a PostgreSQL connector, allowing PostgreSQL clients like 
  [Power BI](../../integrations/postgresql-clients/power-bi) to retrieve data 
  from the database.  

* To use RavenDB as a PostgreSQL server you need -  
   * a [license](../../studio/server/license-management) that enables PostgreSQL.  
   * To explicitly enable PostgreSQL in your [settings](../../server/configuration/configuration-options).  

* [Installing](../../start/installation/setup-wizard) RavenDB as 
  a [secure](../../server/security/overview) server allows you to authenticate 
  PostgreSQL clients, granting access only to clients that provide the proper credentials.  

---

{NOTE: }

* In this page:  
  * [Enabling PostgreSQL, and Additional Settings](../../integrations/postgresql-clients/overview#enabling-postgresql,-and-additional-settings)  
     * [License](../../integrations/postgresql-clients/overview#license)  
     * [Settings](../../integrations/postgresql-clients/overview#settings)  
     * [PostgreSQL Port](../../integrations/postgresql-clients/overview#postgresql-port)  
  * [Security](../../integrations/postgresql-clients/overview#security)  
{NOTE/}

---

{PANEL: Enabling PostgreSQL, and Additional Settings}

---

### License

* Your RavenDB license determines which features are active.  
* Check whether your license activates PostgreSQL, in Studio's **About** Page.  
  !["About - License"](images/about-license.png "About - License")
* If your current license doesn't allow you to use PostgreSQL, acquire [one that does](../../studio/server/license-management).  

---

### Settings

* PostgreSQL must be explicitly enabled in your [settings](../../server/configuration/configuration-options#json).  
* Add this line to your server's `settings.json` file to enable PostgreSQL:  
  **`"Integrations.PostgreSQL.Enabled": true`**

---

### PostgreSQL Port

* To connect RavenDB, youur lients need not only its URL but also its PostgreSQL **Port** number.  
  By default, the port number is *5433*.  
* To use another port, add the following line to your settings.json file, with a port number 
  of your choice:  
  **`"Integrations.PostgreSQL.Port": 5433`**

{PANEL/}

{PANEL: Security}

Allowing just any client to connect your database (via PostgreSQL or otherwise) 
without authentication is risky, and should in general be avoided. To allow access 
only for authorized clients -  

* Set RavenDB as a [Secure Server](../../server/security/overview).  
  This will allow RavenDB to authenticate PostgreSQL clients, in addition 
  to many other security measures this setup provides.  
* Create [PostgreSQL Credentials](../../studio/database/settings/integrations) using RavenDB Studio.  
  PostgreSQL credentials are a **user name** and a **password**, that a client 
  would have to provide in order to access the database.  

{PANEL/}

## Related articles

**Integrations**  
[Integrations: Power BI](../../integrations/postgresql-clients/power-bi)  

**Studio**  
[Studio: Integrations and Credentials](../../studio/database/settings/integrations)  

**Security**  
[Setup Wizard](../../start/installation/setup-wizard)  
[Security Overview](../../server/security/overview)  

**Settings**  
[settings.json](../../server/configuration/configuration-options#json)  

**Additional Links**  
[Microsoft Power BI Download Page](https://powerbi.microsoft.com/en-us/downloads)  



