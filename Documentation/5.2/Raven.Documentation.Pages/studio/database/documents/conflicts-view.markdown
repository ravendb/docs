# Conflicts View
---

{NOTE: }

* Manually resolve a conflict between two documents in this view.  

* For a conflict overview see: [What is a Conflict](../../../server/clustering/replication/replication-conflicts)  

* Note:  
  By default, conflicts are resolved using the latest document version when no script is defined.  
  Conflicts will be created _only if_ this option is unchecked in [Conflict Resolution](../../../studio/database/settings/conflict-resolution).  
{NOTE/}

---

{PANEL: The Conflicts View}

![Figure 1. Conflicts View](images/conflict-view-1.png "Resolve the Conflict")

1. **The conflicted document** - Click to open the resolve editor

2. **The conflicted document versions** - Resolve to either version by clicking _'Use this'_  

3. **The conflicted document body** - Resolve by modifying  the document body directly  

4. **Resolve Conflict By:**  
   * ***Resolve and Save*** - Click to save the resolved document version, or -  
   * ***Delete*** - Click to delete this document  
{PANEL/}


## Related Articles

- [Conflict Resolution Definiton](../../../studio/database/settings/conflict-resolution)
- [What is a Conflict](../../../server/clustering/replication/replication-conflicts)  
