# Search Engine: Corax
---

{NOTE: }

* **Corax** is RavenDB's native search engine, introduced in 
  RavenDB version 5.4 as an in-house searching alternative for Lucene.  
  Its main role it to query the database by indexes and user queries.  

* Corax is currently an **experimental feature**, and you must   
  explicitly [enable experimental features](../../indexes/search-engine/corax#enabling-corax) 
  to use it.  

* You can select either Corax or Lucene as your default seaech engine.  
  The default search engine can be selected separately for 
  [auto](../../indexes/creating-and-deploying#auto-indexes) and 
  [static](../../indexes/creating-and-deploying#static-indexes) indexes.  

* The search engine can also be selected per index, overriding the 
  default settings for the specified index.  

* In this page:  
   * [Enabling Corax](../../indexes/search-engine/corax#enabling-corax)  
   * [Selest Search Engine](../../indexes/search-engine/corax#selest-search-engine)  
      * [Select Search Engine Server-Wide](../../indexes/search-engine/corax#select-search-engine-server-wide)  
      * [Select Search Engine Per Database](../../indexes/search-engine/corax#select-search-engine-per-database)  
      * [Select Search Engine Per Index](../../indexes/search-engine/corax#select-search-engine-per-index)  
   * [Supported Features](../../indexes/search-engine/corax#supported-features)  

{NOTE/}

---

{PANEL: Enabling Corax}

Corax is an experimental feature, and is disabled by default.  
To use it, you must explicitly enable RavenDB's experimental features.  

* To enable experimental features **when RavenDB is already installed**:  
  Edit RavenDB's [configuration file](../../server/configuration/configuration-options#settings.json) 
  and Enable [experimental features](../../server/configuration/core-configuration#features.availability).  
  
      E.g. set [settings.json](../../server/configuration/configuration-options#settings.json) to:  
      {CODE-BLOCK: csharp}
      {
        "ServerUrl": "http://127.0.0.1:8080",
        "Setup.Mode": "None",
        
        // enable experimental features
        "Features.Availability": "Experimental"
}
      {CODE-BLOCK/}

      {NOTE: }
        You must restart the server after making these changes, 
        for the new settings to be read and applied.  
      {NOTE/}


* To enable experimental features during [Setup](../../start/installation/setup-wizard):  
  check **Enable the following experimental features** in the **Welcome** page.  

    ![Enable Experimental Features](images/corax-01_setup-wizard.png "Enable Experimental Features")

{PANEL/}

{PANEL: Selest Search Engine}

* You can select your preferred search engine in several scopes:  
   * **Per server** (selecting the search engine for all databases on this server),  
   * **Per-database** (overriding the per-server selection),  
   * or **Per-index** (overriding the per-sever and per-database selections for a specific index).  

* Two configuration options are available:  
   * [Indexing.Auto.SearchEngineType](../../server/configuration/indexing-configuration#indexing.auto.searchenginetype)  
     Use this option to select the search engine (either `Lucene`, `Corax`, or `None`) for **auto** indexes.  
     The search engine can be selected [server-wide](../../indexes/search-engine/corax#select-search-engine-server-wide) 
     or [per database](../../indexes/search-engine/corax#select-search-engine-per-database).  
   * [Indexing.Static.SearchEngineType](../../server/configuration/indexing-configuration#indexing.static.searchenginetype)  
     Use this option to select the search engine (either `Lucene`, `Corax`, or `None`) for **static** indexes.  
     The search engine can be selected [server-wide](../../indexes/search-engine/corax#select-search-engine-server-wide), 
     [per database](../../indexes/search-engine/corax#select-search-engine-per-database), 
     or [per index](../../indexes/search-engine/corax#select-search-engine-per-index).  

---

### Select Search Engine Server-Wide

Select the search engine for all the databases that run on a given server 
by modifying the server's [settings.json](../../server/configuration/configuration-options#settings.json).  
E.g. add `settings.json` these lines:  
{CODE-BLOCK: csharp}
{
    "Indexing.Auto.SearchEngineType": "Corax"
    "Indexing.Static.SearchEngineType": "Corax"
}
{CODE-BLOCK/}

{NOTE: }
You must restart the server after making these changes, 
for the new settings to be read and applied.  
{NOTE/}

---

### Select Search Engine Per Database

To select the search engine that the database would use, modify the 
relevant Database Record settings . You can easily do this via Studio:  

* Open Studio's [Database Settings](../../studio/database/settings/database-settings) 
  page, and enter `SearchEngine` in the search bar to find the search engine settings.  
  Click `Edit` to modify the default search engine.  

     ![Database Settings](images/corax-04_database-settings_01.png "Database Settings")

* Select your preferred search engine for Auto and Static indexes.  

     ![Corax Database Options](images/corax-05_database-settings_02.png "Corax Database Options")

* Restart the database or the server for your changes to take effect.  
  The updated database record will be read and applied upon restart.  

     ![Default Search Engine](images/corax-06_database-settings_03.png "Default Search Engine")

---

### Select Search Engine Per Index

You can also select the search engine that would be used by a specific index, 
overriding any per-database and per-server settings.  
This too can be easily done via Studio:  

Open Studio's [Index List](../../studio/database/indexes/indexes-list-view) 
view and select the index whose search engine you want to set.  

![Index Definition](images/corax-02_index-definition.png "Index Definition")

1. Open the index's **Configuration** tab.  
2. Select the search engine you prefer for this index.  
   ![Per-Index Search Engine](images/corax-03_index-definition_searcher-select.png "Per-Index Search Engine")

{PANEL/}

{PANEL: Supported Features}

Corax is an experimental feature, gradually expanded so support additional features.  
Features that aren't currently supported will be added in time.  

Currently supported:  

* **Auto Indexes**: Fully supported  
* **Static Indexes**: Supported except for [Dynamic Fields](../../indexes/using-dynamic-fields)  
* **Querying**: Supported except for [Facets](../../indexes/querying/faceted-search)  

{PANEL/}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)

### Studio
- [Custom Analyzers](../studio/database/settings/custom-analyzers)  
- [Create Map Index](../studio/database/indexes/create-map-index)  
