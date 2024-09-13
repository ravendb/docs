/*
#region hocon_config 
<akka>
  <hocon>
  <![CDATA[
    akka.persistence {
    
      # Setup the RavenDB journal store:
      journal {
        plugin = "akka.persistence.journal.ravendb"
        ravendb {
            class = "Akka.Persistence.RavenDb.Journal.RavenDbJournal, Akka.Persistence.RavenDb"
            plugin-dispatcher = "akka.actor.default-dispatcher"
            urls = ["http://localhost:8080"]
            name = "MyAkkaStorageDB"
            auto-initialize = false
            #certificate-path = "\\path\\to\\cert.pfx"
            #save-changes-timeout = 30s
            #http-version = "2.0"
            #disable-tcp-compression = false
        }
      }
       
      # Setup the RavenDB snapshot store:
      snapshot-store {
          plugin = "akka.persistence.snapshot-store.ravendb"
          ravendb {
              class = "Akka.Persistence.RavenDb.Snapshot.RavenDbSnapshotStore, Akka.Persistence.RavenDb"
              plugin-dispatcher = "akka.actor.default-dispatcher"
              urls = ["http://localhost:8080"]
              name = "MyAkkaStorageDB"
              auto-initialize = false
              #certificate-path = "\\path\\to\\cert.pfx"
              #save-changes-timeout = 30s
              #http-version = "2.0"
              #disable-tcp-compression = false
          }
      }
      
      query {
        # Configure RavenDB as the underlying storage engine for querying:
        ravendb {
            class = "Akka.Persistence.RavenDb.Query.RavenDbReadJournalProvider, Akka.Persistence.RavenDb"
            #refresh-interval = 3s
            #max-buffer-size = 65536
        }
      }
    }
  ]]>
  </hocon>
</akka>
#endregion
*/
