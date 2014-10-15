package net.ravendb.clientapi.listeners;

import java.util.List;

import net.ravendb.abstractions.basic.Reference;
import net.ravendb.abstractions.data.JsonDocument;


public class Conflict {
  //region document_conflict_listener
  public interface IDocumentConflictListener {

    public boolean tryResolveConflict(String key, List<JsonDocument> results, Reference<JsonDocument> resolvedDocument);
  }
  //endregion

  //region document_conflict_example
  public class ResolveInFavourOfNewest implements IDocumentConflictListener {
    @Override
    public boolean tryResolveConflict(String key, List<JsonDocument> conflictedDocs, Reference<JsonDocument> resolvedDocument) {
      long maxDate = 0;
      for (JsonDocument doc : conflictedDocs) {
        if (doc.getLastModified().getTime() > maxDate) {
          maxDate = doc.getLastModified().getTime();
          resolvedDocument.value = doc;
        }
      }

      return resolvedDocument.value != null;
    }
  }
  //endregion
}
