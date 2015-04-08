package net.ravendb.clientapi.bundles;

import net.ravendb.abstractions.data.FailoverServers;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.abstractions.replication.ReplicationDestination;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.document.FailoverBehavior;
import net.ravendb.client.document.FailoverBehaviorSet;


public class HowClientIntegratesWithReplicationBundle {

  public void Sample() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region client_integration_1
      store.getConventions().setFailoverBehavior(FailoverBehaviorSet.of(FailoverBehavior.FAIL_IMMEDIATELY));
      //endregion

      //region client_integration_4
      store.getConventions().setFailoverBehavior(
        FailoverBehaviorSet.of(
          FailoverBehavior.READ_FROM_ALL_SERVERS,
          FailoverBehavior.ALLOW_READS_FROM_SECONDARIES_AND_WRITES_TO_SECONDARIES));
      //endregion

      //region client_integration_2
      RavenJObject hiloSetup = new RavenJObject();
      hiloSetup.add("ServerPrefix", "NorthServer/");
      store
      .getDatabaseCommands()
      .put("Raven/ServerPrefixForHilo", null, hiloSetup, new RavenJObject());
      //endregion
    }
  }

  public HowClientIntegratesWithReplicationBundle() throws Exception {
    try (DocumentStore store = new DocumentStore()) {
      //region client_integration_3
      store.setFailoverServers(new FailoverServers());
      ReplicationDestination destination1 = new ReplicationDestination();
      destination1.setUrl("http://localhost:8078");
      destination1.setApiKey("apikey");

      ReplicationDestination destination2 = new ReplicationDestination();
      destination2.setUrl("http://localhost:8077");
      destination2.setDatabase("test");
      destination2.setUsername("user");
      destination2.setPassword("secret");
      store.getFailoverServers().addForDefaultDatabase(destination1, destination2);

      ReplicationDestination northwindDestination = new ReplicationDestination();
      northwindDestination.setUrl("http://localhost:8076");

      store.getFailoverServers().addForDatabase("Northwind", northwindDestination);
      //endregion
    }

    /*
    //region client_integration_5
    Url = http://localhost:59233;
        // Primary server url
    Failover = { Url:'http://localhost:8078'};
        // Failover for DefaultDatabase
    Failover = { Url:'http://localhost:8077/', Database:'test'};
        // Failover for DefaultDatabase with non-default database
    Failover = Northwind|{ Url:'http://localhost:8076/'};
        // Failover for 'Northwind' database
    Failover= { Url:'http://localhost:8075', Username:'user', Password:'secret'};
        // Failover for DefaultDatabase with Username and Password
    Failover= { Url:'http://localhost:8074', ApiKey:'d5723e19-92ad-4531-adad-8611e6e05c8a'}
        // Failover for DefaultDatabase with ApiKey
    //endregion
     */
  }
}
