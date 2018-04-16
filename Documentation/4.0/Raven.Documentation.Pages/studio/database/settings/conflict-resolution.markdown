# Conflict Resolution
---

{NOTE: }

* Define the server behaviour upon a conflict between documents.  

* Conflicts that are created can be manually resolved in [Documents Conflicts View](../../../studio/database/documents/conflicts-view).  

* In this page:  
  * [The Conflicts Resolution View](../../../studio/database/settings/conflict-resolution#the-conflicts-resolution-view)  
  * [Script Examples](../../../studio/database/settings/conflict-resolution#script-examples)  
{NOTE/}

---

{PANEL: The Conflicts Resolution View}

![Figure 1. Conflicts Resolution View](images/conflict-resolution-view-1.png "Conflict Resolution")

1. **Save, Add, Modify or Delete** conflict resolution scripts.  

2. **Set default behaviour**  -  Conflicts will be created _only if_:  
   * This option is unchecked and no script is defined  
   * This option is unchecked and the script is defined but returns null  

3. **Resolution Script** (optional)  
   * Supply a javascript to resolve the conflicted documents. 
   * The script returns an object that is written as the new document object.  
   * The sript is defined per collection  
   * Script Variables:  
     * ***docs*** - the conflicted documents objects array  
     * ***hasTombstone*** - true if either of the conflicted documents was deleted  
     * ***resolveToTombstone*** - return this value from script if the resolution wanted is to delete this document  

{PANEL/}

{PANEL: Script Examples}

* Resolve according to field content - return the highest value of the field

{CODE-BLOCK:javascript}
var maxRecord = 0;
for (var i = 0; i < docs.length; i++) {
    maxRecord = Math.max(docs[i].maxRecord, maxRecord);   
}
docs[0].MaxRecord = maxRecord;

return docs[0];  
{CODE-BLOCK/}

* Resolve by deleting the document

{CODE-BLOCK:javascript}
if (hasTombstone) {
    return resolveToTombstone;
}
{CODE-BLOCK/}
{PANEL/}

## Related Articles

- [Documents Conflicts View](../../../studio/database/documents/conflicts-view)  
- [What is a Conflict](../../../server/clustering/replication-conflicts#what-is-a-conflict)  
