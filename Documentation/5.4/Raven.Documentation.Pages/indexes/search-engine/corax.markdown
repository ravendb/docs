# Search Engine: Corax
---

{NOTE: }

* **Corax** is RavenDB's native search engine, introduced in RavenDB 
  version 5.4 as an in-house searching alternative for Lucene.  
  Lucene remains available as well; you can use either Corax or 
  Lucene as your search engine, as you prefer.  

    {INFO: }
    Corax is still an **experimental feature**. 
    To use it, you must 
    [enable experimental features](../../indexes/search-engine/corax#enabling-corax) first.  
    {INFO/}

* The main role of the database's search engine is to **satisfy incoming queries**.  
  In RavenDB, the search engine achieves this by handling each query via an index.  
  If no relevant index exists, the search engine will create one automatically.  
  
    The search engine is the main "moving part" of the indexing mechanism, 
    that actually processes and indexes documents by index definitions.  

* The search engine can be selected separately for 
  [auto](../../indexes/creating-and-deploying#auto-indexes) and 
  [static](../../indexes/creating-and-deploying#static-indexes) indexes.  

* The search engine can be selected per server, per database, and per index (for static indexes only).  

* In this page:  
   * [Enabling Corax](../../indexes/search-engine/corax#enabling-corax)  
   * [Selest Search Engine](../../indexes/search-engine/corax#selest-search-engine)  
      * [Server-Wide Search Engine](../../indexes/search-engine/corax#server-wide-search-engine)  
      * [Per-Database Search Engine](../../indexes/search-engine/corax#per-database-search-engine)  
      * [Per-index Search Engine](../../indexes/search-engine/corax#per-index-search-engine)  
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
   * [Server-wide](../../indexes/search-engine/corax#server-wide-search-engine), 
     selecting which search engine will be used by all the databases hosted by this server.  
   * [Per database](../../indexes/search-engine/corax#per-database-search-engine), 
     overriding server-wide settings for a specific database.  
   * [Per index](../../indexes/search-engine/corax#per-index-search-engine), 
     overriding sever-wide and per-database settings.  
     Per-index settings are available only for **static** indexes.  

* Two configuration options are available:  
   * [Indexing.Auto.SearchEngineType](../../server/configuration/indexing-configuration#indexing.auto.searchenginetype)  
     Use this option to select the search engine (either `Lucene` or `Corax`) for **auto** indexes.  
     The search engine can be selected [server-wide](../../indexes/search-engine/corax#select-search-engine-server-wide) 
     or [per database](../../indexes/search-engine/corax#select-search-engine-per-database).  
   * [Indexing.Static.SearchEngineType](../../server/configuration/indexing-configuration#indexing.static.searchenginetype)  
     Use this option to select the search engine (either `Lucene` or `Corax`) for **static** indexes.  
     The search engine can be selected [server-wide](../../indexes/search-engine/corax#select-search-engine-server-wide), 
     [per database](../../indexes/search-engine/corax#select-search-engine-per-database), 
     or [per index](../../indexes/search-engine/corax#select-search-engine-per-index).  

---

### Server-Wide Search Engine

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

### Per-Database Search Engine

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

### Per-index Search Engine

You can also select the search engine that would be used by a specific index, 
overriding any per-database and per-server settings.  

* Select the search engine per-index **via Studio**:  
  Open Studio's [Index List](../../studio/database/indexes/indexes-list-view) 
  view and select the index whose search engine you want to set.  

      ![Index Definition](images/corax-02_index-definition.png "Index Definition")

      1. Open the index's **Configuration** tab.  
      2. Select the search engine you prefer for this index.  
         ![Per-Index Search Engine](images/corax-03_index-definition_searcher-select.png "Per-Index Search Engine")

* Select the search engine per-index **using Code**:  

     While defining an index using the API, use the `SearchEngineType` 
     property to select the search engine that would run the index.  
     Available values: `SearchEngineType.Lucene`, `SearchEngineType.Corax`.  

      * You can pass the index `Execute` method the search engine type you prefer:  
        {CODE:csharp index-definition_select-while-creating-index@Indexes/SearchEngines.cs /}  
      * And use the preferred type in the actual index definition:  
        {CODE:csharp index-definition_set-search-engine-type@Indexes/SearchEngines.cs /}  

{PANEL/}

{PANEL: Supported Features}

Corax is still under construction. It is fully operative with [Auto](../../indexes/creating-and-deploying#auto-indexes) 
and [Static](../../indexes/creating-and-deploying#static-indexes) indexes, except for the following cases:  

* While indexing, Corax does not support:  
  [Boosting](../../indexes/boosting)  
  [Facets](../../indexes/querying/faceted-search)  
* While querying, Corax does not support:  
   [MoreLikeThis](../../indexes/querying/morelikethis)
   [Facets](../../indexes/querying/faceted-search)
   [Fuzzy Search](../../client-api/session/querying/how-to-use-fuzzy)
* Corax doesn't support [Dynamic Fields](../../indexes/using-dynamic-fields) yet.  
  As a result, many Javascript indexes are not supported since they use dynamic fields.  

{PANEL/}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)

### Studio
- [Custom Analyzers](../studio/database/settings/custom-analyzers)  
- [Create Map Index](../studio/database/indexes/create-map-index)  
