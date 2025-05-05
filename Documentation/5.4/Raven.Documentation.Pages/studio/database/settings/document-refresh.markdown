# Document Refresh
---

{NOTE: }

* Use this view to configure the **Document Refresh** feature.  
* Documents can be scheduled for refresh by a `@refresh` property in their `@metadata`.  
* When the document refresh feature is enabled, it routinely checks for documents that 
  include the `@refresh` property and refreshes them on the scheduled time.  
  When a document is refreshed its [change vector](../../server/clustering/replication/change-vector) 
  is incremented, triggering its re-indexation and other features that react to document updates.  
* Documents are refreshed only once: after refreshing a document, the refresh feature removes 
  the `@refresh` property from its `@metadata`.  
* Learn more about this option [here](../../../server/extensions/refresh).  

{NOTE/}

---

{PANEL: Document Refresh View}

![Document Refresh View](images/document-refresh.png "Document Refresh View")

1. **Document Refresh**  
   Click to configure document refresh.

2. **Document Refresh Configuration**  
    * **Enable Document Refresh**  
      Toggle this option to enable or disable the refresh feature for the selected database.  
    * **Set refresh frequency**  
      Set the frequency at which the server scans for documents scheduled for refresh.  
      Default value: _60 seconds_
    * **Set max number of documents to process in a single run**  
      Set the maximal number of documents the feature is allowed to refresh in one run.  
    * **Save**  
      Changes in the configuration will apply only after saving them.  

3. **Set the `@refresh` property**  
   * Add a `@refresh` property to the `@metadata` of each document that you want to schedule refresh for.  
   * Set the refresh time in UTC format, e.g. - `"@refresh": "2025-04-22T08:00:00.0000000Z"`  

{PANEL/}

## Related Articles

- [Server: Documents Refresh](../../../server/extensions/refresh)  
