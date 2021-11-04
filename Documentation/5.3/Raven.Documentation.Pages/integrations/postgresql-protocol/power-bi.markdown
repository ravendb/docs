# PostgreSQL Protocol: Power BI

* The [Power BI](https://en.wikipedia.org/wiki/Microsoft_Power_BI) Desktop and 
  Online services Can use RavenDB as a PostgreSQL server and retrieve data from it.  

* See below how to use Power BI Desktop to -  
   * Easily select RavenDB collections and retrieve chosen data.  
   * Query RavenDB using [RQL](../../indexes/querying/what-is-rql).  

---

{NOTE: }

* In this page:  
  * [Using RavenDB From Power BI Desktop](../../integrations/postgresql-protocol/power-bi#using-ravendb-from-power-bi-desktop)  
     * [Connect RavenDB](../../integrations/postgresql-protocol/power-bi#connect-ravendb)  
     * [Retrieve Collections Data](../../integrations/postgresql-protocol/power-bi#retrieve-collections-data)  
     * [Query RavenDB Using RQL](../../integrations/postgresql-protocol/power-bi#query-ravendb-using-rql)  
{NOTE/}

---

{PANEL: Using RavenDB From Power BI Desktop}

---

### Connect RavenDB

!["Get Data"](images/get-data-button.png "Get Data")

* Click "Get Data" from Power BI Desktop's startup wizard or menu option.  

---

!["Select PostgreSQL database"](images/select-postgresql-database.png "Select PostgreSQL database")

* Select the **PostgreSQL database** option and click **Connect**.  

---

!["Connection Details"](images/connection-details.png "Connection Details")

* Provide RavenDB's **URL**, its PostgreSQL port number, and the name of the database you 
  want to retrieve data from.  
   * Provide the URL without its "https://" prefix.  
   * RavenDB's PostgreSQL port number is by default 5433, and is [configurable](../../integrations/postgresql-protocol/overview#postgresql-port).  
   * The URL and port number should be provided in the form **URL:Port**,  
     E.g. - **`a.ravenpostgresql.development.run:5433`**  

---

!["Credentials"](images/credentials.png "Credentials")

* Provide the [credentials](../../studio/database/settings/integrations) (user name & password) 
  required by RavenDB to authenticate your Power BI client, and click **Connect**.  

---

### Retrieve Collections Data

When you connect the database, A list of your database's collection will appear.  

!["Collections"](images/collections.png "Collections")

* Select the collection/s whose data you want to retrieve, and click **Load** or **Transform**.  

---

!["Retrieved Collection Data"](images/retrieved-collection-data.png "Retrieved Collection Data")

* Your data is loaded, and you can play with it as you will.  
  One notable field is the rightmost "json()" field; we placed 
  it there for irregular data items, should there be ones, that 
  don't fit into one of the otherwise regular json arrays.  

---

### Query RavenDB Using RQL

Instead of loading collections in their entirety, you can run [RQL](../../indexes/querying/what-is-rql) queries 
to import into Power BI just the data you're looking for.  

!["RQL Query"](images/rql-query.png "RQL Query")

* After providing the URL, port number, and database name,  
  open **Advanced options**, enter your query into the **SQL Statement** field, 
  and click **OK**.  

---

!["RQL Query Results"](images/rql-query-results.png "RQL Query Results")


{PANEL/}

## Related articles

**Studio**  
[Integrations & Credentials](../../studio/database/settings/integrations)  

**Integrations**  
[PostgreSQL](../../integrations/postgresql-protocol/overview)  
[PostgreSQL Port Configuration](../../integrations/postgresql-protocol/overview#postgresql-port)  

**Queries**  
[RQL](../../indexes/querying/what-is-rql)  

**Additional Links**  
[Power BI](https://en.wikipedia.org/wiki/Microsoft_Power_BI)  



