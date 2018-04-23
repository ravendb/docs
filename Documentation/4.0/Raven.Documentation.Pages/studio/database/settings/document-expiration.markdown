# Document Expiration
---

{NOTE: }

* Documents can be deleted by setting the document expiration time in this view.  

{NOTE/}

---

{PANEL: Document Expiration View}

![Figure 1. Document Expiration View](images/document-expiration.png "Set Document Expiration.")

1. **Enable Document Expiration**  
   * Check this option to enable the documents expiration feature for the selected database.  
   * Set the frequency at which the server checks for documents that need to be deleted.  
     The default value is 60 secs.

2. **Set the @expires property inside the document that should be deleted**  
   * For each document that needs to be deleted, set the deletion time in the `@expires` property inside the document `@metadata`.  
   * The time must be in UTC format, i.e. ***"@expires": "2018-04-22T08:00:00.0000000Z"***.  
   * Deletion will automatically occur at the specified time, but only if the above 'Enable Document Expiration' option is set.  

{PANEL/}

## Related Articles

- [Documents Expiration](../../../server/extensions/expiration)  
