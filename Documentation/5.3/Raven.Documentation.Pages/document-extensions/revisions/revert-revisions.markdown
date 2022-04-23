# Revisions: Revert Revisions

---

{NOTE: }

* The **Revert Revisions** option attempts to bring your database back 
  to its state at a specified point of time.  
  To achieve it:  
   * Documents are given new revisions that copy their historic 
     revisions, effectively replacing their live versions with the 
     historic ones.  
   * Documents that did not exist at the specified time are deleted.  

* The accuracy in which the database can be reverted to any of its 
  historic states depends upon the frequency in which documents were 
  revisioned.  

* Being able to bring the database back to any of its historic states 
  can, for example, ease database auditing, help understand document 
  changes in their historical context, and instantly restore the 
  database without requiring restoration from backup (so a DB admin 
  can experiment with potentially damaging operations like patching 
  without fearing the consequences).  
  
* The operation can be reversed by repeating it while specifying 
  a point of time that preceded the "Revert Revisions" operation.  

* In this page:  
   * [Revert Revisions](../../document-extensions/revisions/revert-revisions#revert-revisions)  

{NOTE/}

---

{PANEL: Revert Revisions}

Allow to revert the database to a specific point in time.

{INFO: }

This feature is available only if [Revisions](../../server/extensions/revisions) are enabled.

{INFO/}

You can find this option under the `Document Revisions` in the studio:

![Figure 1.](images/revert-revisions.png)  

<br>

Clicking on `Revert Revision` will open the the following window:

![Figure 2.](images/revert-revisions-2.png)

| Name   |      Description      |  Value Type |
|----------|-------------|------|
| `Point in Time` |  Roll back all of the documents to the version at the specified time. | `DateTime` |
| `Window` |    Window parameter is used for performance optimization: since revisions are not sorted by date, we stop the revert process when hitting a versioned document outside the window.  |   `long` (default: 96 * 3,600) |



For example when reverting to the point in time of `16/03/2019 09:55 UTC` the following rules are applied:

* Documents modified after `16/03/2019 09:55 UTC` will be reverted (by creating new revision) to latest version before `16/03/2019 09:55 UTC`.
* If collection has maximum revisions limit and all of them were created after `16/03/2019 09:55 UTC` the oldest revision will be used.
* Documents created after `16/03/2019 09:55 UTC` will be deleted and moved to Revisions Bin.
* Remaining documents will not be modified.

{PANEL/}

## Related Articles
[Revisions](../../server/extensions/revisions)
