package net.ravendb.clientapi.configuration.conventions;

import java.util.EnumSet;

import net.ravendb.client.connection.IDocumentStoreReplicationInformer;
import net.ravendb.client.connection.ReplicationInformer;
import net.ravendb.client.connection.implementation.HttpJsonRequestFactory;
import net.ravendb.client.delegates.ReplicationInformerFactory;
import net.ravendb.client.document.DocumentConvention;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.document.FailoverBehavior;
import net.ravendb.client.document.FailoverBehaviorSet;
import net.ravendb.client.document.IndexAndTransformerReplicationMode;


public class Replication {

  public Replication() {
    final DocumentStore store = new DocumentStore();
    final DocumentConvention conventions = store.getConventions();

    //region failover_behavior
    conventions.setFailoverBehavior(FailoverBehaviorSet.of(FailoverBehavior.ALLOW_READS_FROM_SECONDARIES));
    //endregion

    //region replication_informer
    conventions.setReplicationInformerFactory(new ReplicationInformerFactory() {
      @Override
      public IDocumentStoreReplicationInformer create(String url, HttpJsonRequestFactory jsonRequestFactory) {
        return new ReplicationInformer(conventions, jsonRequestFactory);
      }
    });
    //endregion

    //region index_and_transformer_replication_mode
    conventions.setIndexAndTransformerReplicationMode(EnumSet.of(IndexAndTransformerReplicationMode.INDEXES,
            IndexAndTransformerReplicationMode.TRANSFORMERS));
    //endregion
  }
}
