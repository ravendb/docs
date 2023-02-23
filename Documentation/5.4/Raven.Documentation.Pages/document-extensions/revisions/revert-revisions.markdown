# Revert Revisions

---

{NOTE: }

* Use Revert Revisions to __revert the database to its state at a specified point in time__.   

* Being able to restore the database to a previous state can simplify auditing,  
  enhance understanding of changes made over time,  
  and facilitate instant recovery without the need to search and retrieve a stored backup.

* You can select whether to revert specific collections or revert all collections.
 
* In this page:  
   * [The document revisions view](../../document-extensions/revisions/revert-revisions#the-document-revisions-view)
   * [Revert](../../document-extensions/revisions/revert-revisions#revert)
     * [Point in time](../../document-extensions/revisions/revert-revisions#point-in-time)
     * [Time Window](../../document-extensions/revisions/revert-revisions#time-window)
     * [Content reverted](../../document-extensions/revisions/revert-revisions#content-reverted)

{NOTE/}

---

{PANEL: The Document Revisions view}

![Document Revisions View](images/revert-revisions-1.png "Document Revisions View")

1. __Document Revisions view__  
   In the Studio, go to _Settings > Document Revisions_. From this view you can:
   * [Set](../../studio/database/settings/document-revisions#revisions-configuration) revision configurations
   * [Enforce](../../studio/database/settings/document-revisions#enforce-configuration) revision configurations
   * [Revert](../../document-extensions/revisions/revert-revisions#revert) revisions

2. __Revision Configurations__  
   * The ability to revert documents to their past revisions depends on revisions being created.  
   * When no default configuration or collection-specific configurations are defined and enabled,  
     no revisions will be created for any document.  
   * Make sure that a configuration that suits your needs is defined.  

3. __Revert Revisions__  
   Click to revert revisions. See more [below](../../document-extensions/revisions/revert-revisions#revert).

{PANEL/}

{PANEL: Revert}

![Revert Revisions](images/revert-revisions-2.png "Revert Revisions")

1. Enter the __Point in Time__ to which you would like to revert the documents. Learn more [below](../../document-extensions/revisions/revert-revisions#point-in-time).  

2. Enter the __Time Window__. Learn more [below](../../document-extensions/revisions/revert-revisions#time-window).  

3. Revert __All collections__ in the database, or toggle off to select __Specific collections__ to revert.

{PANEL/}

{PANEL: Point in time}

![Set point in time](images/set-point-in-time.png "Set point in time")

* Select or enter the point in time (LOCAL) to revert documents to.   
  The information text on the right will display the expected behavior in [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time).

---

* When the revert process is executed:

    {NOTE: }
    
    Documents created __AFTER__ the specified Point in Time will be __moved to the Revision Bin__.
    
    {NOTE/}  

    {NOTE: }

    Documents created __BEFORE__ this time and that were modified after this time:
    
     * Any of these documents that own revisions will be __reverted__  
       to the revision created at the specified Point in Time or to the latest revision preceding this time.  
    
     * When [setting a limit](../../studio/database/settings/document-revisions#limit-revisions) on the number of revisions that are kept (by number or by age)  
       then only the most recent revisions are kept for the document.  
       If all of these revisions were created AFTER the Point in Time then the oldest revision will be  
       the one we revert to, even though that revision is newer than the specified time.  
       By doing so we make sure that all the documents that existed before the specified Point in Time  
       still exist after the revert process.

    {NOTE/}

    {NOTE: }

    Documents created __BEFORE__ this time that were Not modified after this Time are __not reverted__.

    {NOTE/}

{PANEL/}

{PANEL: Time window}

* Revisions are ordered in the _revisions storage_ by their change-vector, and Not by creation time.

* When reverting the database (or selected collections) to some previous date,  
  RavenDB will iterate through the revisions table, starting from the most recent revision,  
  and search for revisions that should be restored.

* To avoid conducting unnecessarily long searches or revert to revisions that are too old  
  RavenDB sets a limit to this search.  
  The search will stop once we hit a revision that was created prior to: `Point in Time - Time Window`

* The default Time Window value is 96 hours (4 days).

{NOTE: }

__Example__:  

* Point in Time to revert to: `15.2.2023 02:00`  
  The latest revisions prior to this Point in Time should be restored.

* Time Window: `4 days`  
  We will stop the search in the revision table once we hit a revision with creation time prior to:  
  `11.2.2023 02:00`  

* A sample revisions table:  
   * The table contains revisions of all documents, it is not just revisions of a single document.
   * The revisions are Not ordered by creation time, the order is set by their change-vector.

| Revision | Creation time | |
| - | - | - |
| 1)  `Users/1` | 20.2.2023 01:00 | |
| 2)  `Users/2` | 19.2.2023 01:00 | |
| 3)  `Users/5` | 19.2.2023 01:00 | |
| 4)  `Users/3` | 14.2.2023 01:00 | => Document Users/3 stays with this revision content |
| 5)  `Users/4` | 17.2.2023 01:00 | => Will be moved to Revisions Bin |
| 6)  `Users/1` | 18.2.2023 01:00 | |
| 7)  `Users/2` | 14.2.2023 01:00 | => Will be restored to the current document |
| 8)  `Users/1` | 13.2.2023 01:00 | => Will be restored to the current document |
| 9)  `Users/5` | 11.2.2023 01:00 | => Will be restored to the current document + <br>STOP the search for more Users/5 revisions |
| 10) `Users/5` | 11.2.2023 03:00 | |
| 11) `Users/9` | 10.2.2023 01:00 | => Not restored + <br>STOP the search in this table |
| 12) `Users/6` | 11.2.2023 01:00 | |

* (line 1)  
  We iterate on the table starting from `Users/1` revision created on 20.2.2023 01:00.  
  We search for a relevant revision by iterating on all `Users/1` revisions.  
  The revision that will be restored for `Users/1` is the one from 13.2.2023 01:00 (line 8)   
  since it is the latest one prior to 15.2.2023 02:00.

* (line 2)  
  Next, we search for a relevant revision for `Users/2` by iterating on all `Users/2` revisions.  
  The revision that will be restored is the one from 14.2.2023 01:00 (line 7),  
  since it is the latest one prior to 15.2.2023 02:00.  

* (line 3)  
  Next, we search for a relevant revision by iterating on all `Users/5` revisions.  
  The revision that will be restored is the one from 11.2.2023 01:00 (line 9)  
  since it is the latest one prior to 15.2.2023 02:00.  
  The search for `Users/5` revisions will now STOP,  
  since we found a revision that was created prior to 11.2.2023 02:00.  
  The following revision for `Users/5` from 11.2.2023 03:00 (line 10) is Not restored.  

* (line 4)  
  Next, document `Users/3` is Not modified, since it wasn't modified after 15.2.2023 02:00.

* (line 5)   
  Next, `Users/4` has NO revisions prior to 15.2.2023 02:00,  
  which means it was created AFTER this Point in Time,  
  so this document is moved to the Revisions Bin.

* (line 11)   
  Next, we reach `Users/9` revision created on 10.2.2023 01:00, which is PRIOR to 11.2.2023 02:00.  
  The search on this table will now STOP.  
  No further revisions will be taken into account, not even `Users/6` revision created on 11.2.2023 01:00.

{NOTE/}

{PANEL/}

{PANEL: Content reverted}

{INFO: }

* When reverting a document to one of its revisions, RavenDB actually creates a new revision for the document.  
  The content of this new revision is a copy of the historical revision content, and it becomes the current version of the document.

* Database items other than documents, such as ongoing tasks, indexes, and compare-exchange,  
  are Not reverted by this process.

* Document extensions:  
  * __Time series__  
    Time series data is Not reverted. Learn more [here](../../document-extensions/revisions/revisions-and-other-features#reverted-data-1).  
  * __Attachments__   
    When a document is reverted to a revision that owns attachments,  
    the attachments are restored to their state when the revision was created.  
  * __Counters__  
    When a document is reverted to a revision that owns counters,  
    the counters are restored to functionality along with their values.

{INFO/}

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../document-extensions/revisions/overview)  
* [Revisions and Other Features](../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: API Overview](../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../document-extensions/revisions/client-api/session/loading)  

### Studio

* [Settings: Document Revisions](../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../studio/database/document-extensions/revisions)  
