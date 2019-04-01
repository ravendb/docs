# Documents: Conflicts View

If [Replication Bundle](../../../server/scaling-out/replication/how-replication-works) is enabled then an additional view is available in the studio - Conflicts. In order navigate there expand the `Documents` option from the main bar.

![Figure 1. Studio. Conflicts View. Accessing the view.](images/conflicts_view_1.png)  

This view presents all [replication conflicts](../../../server/scaling-out/replication/replication-conflicts) created by replication process.

![Figure 2. Studio. Conflicts View. Conflicts view.](images/conflicts_view_2.png)  

Each of the conflicts contains the following information:

* conflicted document identifier,
* conflict detection time,
* links to conflict items which are different versions of a conflicted document.

After accessing the conflicted document you will see the which parts of its content are conflicted:

![Figure 3. Studio. Donflicted document view.](images/conflicts_view_3.png)  

You can resolve the conflict in the studio by editing the conflicted parts and saving it.

![Figure 4. Studio. Conflict solved.](images/conflicts_view_4.png)  

Then the conflict will disappear:

![Figure 5. Studio. No conflicts.](images/conflicts_view_5.png)

Another way to solve all the issues in this page is to press the Resolve all conflicts button and chose how to resolve them:

<span class="float-left">![Figure 6. Studio. resolve_conflicts_button.](images/resolve_conflicts_button.png)

You can resolve all the conflicts in the studio by three options:

<span class="float-left">![Figure 7. Studio. resolve_conflicts_options.](images/resolve_conflicts_options.png)

* `Resolve with Local` if you want to resolve the conflicts in favor of local version,
* `Resolve to newest remote` if you want to resolve the conflicts in favor of newest remote version,
* `Resolve to latest` if you want to resolve the conflicts in favor of latest document that was created

## Related articles

- [Server: Replication Bundle](../../../server/scaling-out/replication/how-replication-works)
- [File System Studio: Conflicts View](../../../file-system/studio/synchronization/conflicts-view)
