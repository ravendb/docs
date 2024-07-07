package net.ravendb.ClientApi.Session.Revisions;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.json.MetadataAsDictionary;

import java.util.List;
import java.util.stream.Collectors;

public class CounterRevisions {

    public void samples() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region example
                List<MetadataAsDictionary> orderRevisionsMetadata = session.advanced()
                    .revisions()
                    .getMetadataFor("orders/1-A", 0, 10);

                List<MetadataAsDictionary> orderCountersSnapshots = orderRevisionsMetadata
                    .stream()
                    .filter(x -> x.containsKey("@counters-snapshot"))
                    .map(x -> (MetadataAsDictionary) x.get("@counters-snapshot"))
                    .collect(Collectors.toList());
                //endregion
            }
        }
    }
}
