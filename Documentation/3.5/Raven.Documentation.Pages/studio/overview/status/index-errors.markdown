# Status: Index Errors

Here we can view all indexing errors that occurred during indexation. Alerts are listed by date of occurrence in a system, starting with the most recent ones.

Each entry contains the following information:

- name of an index in which an error occurred,
- action type (e.g. Map or Reduce),
- timestamp,
- error's details,
- document which caused an error's occurrence

To illustrate this mechanism, we will change the definition of `Orders/Totals` index and introduce invalid casting, changing `decimal` into `string`.

![Figure 1. Studio. Status. Index Errors. Index definition change.](images/status-index_errors-1.png)

When our index is saved, we can see errors that occurred in `Index Errors` view.

![Figure 2. Studio. Status. Index Errors. List of errors.](images/status-index_errors-2.png)

{SAFE:Invalid indexes}

Note that server will mark index as `Invalid` if a considerable amount of errors connected to a single index is detected. The server will not process such index any longer.


To check `Invalid` indexes you can go directly to [Indexes View](../../overview/indexes/indexes-view).

![Figure 3. Studio. Status. Index Errors. Invalid indexes.](images/status-index_errors-3.png)

![Figure 4. Studio. Status. Index Errors. Invalid indexes.](images/status-index_errors-4.png)

{SAFE/}
