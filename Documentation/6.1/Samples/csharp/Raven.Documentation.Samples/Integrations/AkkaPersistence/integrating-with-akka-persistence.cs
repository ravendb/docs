using System;
#region basic_hosting_config
// Add the following using statements:
using Microsoft.Extensions.Hosting;
using Akka.Hosting;
using Akka.Persistence.Hosting;
using Akka.Persistence.RavenDb.Hosting;

namespace Raven.Documentation.Samples.Integrations.AkkaPersistence
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder().ConfigureServices((context, services) =>
            {
                services.AddAkka("my-actor-system-name", (builder, provider) =>
                {
                    // Call 'WithRavenDbPersistence' to configure RavenDB as the persistence storage
                    builder.WithRavenDbPersistence(
                        
                        // URL of the RavenDB server
                        urls: new[] { "http://localhost:8080" },
                        
                        // The database where the journal events and the snapshots will be persisted
                        databaseName: "MyAkkaStorageDB",
                        
                        // Configuration will apply to both the journal and the snapshot stores
                        mode: PersistenceMode.Both);
                });
            });
            
            var app = host.Build();
            app.Run();
        }
    }
}
#endregion

namespace Raven.Documentation.Samples.Integrations.AkkaPersistence.Syntax
{
    public static class AkkaPersistenceRavenDbHostingExtensions
    {
        #region syntax_1
        // A simple overload providing basic configuration
        // ===============================================
        
        public static AkkaConfigurationBuilder WithRavenDbPersistence(
            this AkkaConfigurationBuilder builder,
            // An array of server URLs where the RavenDB database is stored.
            string[] urls,
            // The name of the database where the persistence data should be stored.
            // It is recommended to create a separate database for Akka storage,
            // distinct from your other work databases.
            string databaseName,
            // Location of a client certificate to access a secure RavenDB database.
            // If a password is required, it should be stored in the RAVEN_CERTIFICATE_PASSWORD env var.
            string? certificatePath = null,
            // Create the database if it doesn't exist.
            bool autoInitialize = true,
            // Determines whether this configuration will be applied to the Journal store,
            // the Snapshot store, or both stores.
            PersistenceMode mode = PersistenceMode.Both,
            string pluginIdentifier = "ravendb",
            bool isDefaultPlugin = true,
            Action<AkkaPersistenceJournalBuilder>? journalBuilder = null)
        #endregion
        {
            throw new NotImplementedException();
        }
    
        #region syntax_2
        public enum PersistenceMode
        {
            // Sets both the akka.persistence.journal and the akka.persistence.snapshot-store to use this plugin.
            Both,
            // Sets ONLY the akka.persistence.journal to use this plugin.
            Journal,
            // Sets ONLY the akka.persistence.snapshot-store to use this plugin.
            SnapshotStore,
        }
        #endregion

        #region syntax_3
        // These overloads allow for applying separate configurations to the Journal and Snapshot stores
        // =============================================================================================
        
        public static AkkaConfigurationBuilder WithRavenDbPersistence(
            this AkkaConfigurationBuilder builder,
            Action<RavenDbJournalOptions>? journalOptionConfigurator = null,
            Action<RavenDbSnapshotOptions>? snapshotOptionConfigurator = null,
            bool isDefaultPlugin = true)
        #endregion
        {
            throw new NotImplementedException();
        }

        #region syntax_4
        public static AkkaConfigurationBuilder WithRavenDbPersistence(
            this AkkaConfigurationBuilder builder,
            RavenDbJournalOptions? journalOptions = null,
            RavenDbSnapshotOptions? snapshotOptions = null)
        #endregion
        {
            throw new NotImplementedException();
        }
    }
    
    /*
    #region syntax_5
    // Use this class to define the Journal store configuration
    public class RavenDbJournalOptions
    {
        public string? Name { get; set; }
        public string[] Urls { get; set; }
        public string? CertificatePath { get; set; }
        
        // Http version for the RavenDB client to use in communication with the server
        public Version? HttpVersion { get; set; }
        // Determines whether to compress the data sent in the client-server TCP communication 
        public bool? DisableTcpCompression { get; set; }
        // Timeout for 'save' requests sent to RavenDB, such as writing or deleting
        // as opposed to stream operations which may take longer and have a different timeout (12h).
        // Client will fail requests that take longer than this.
        public TimeSpan? SaveChangesTimeout { get; set; }
    }
    #endregion
    */
    
    /*
    #region syntax_6
    // Use this class to define the Snapshot store configuration
    public class RavenDbSnapshotOptions
    {
        public string? Name { get; set; }
        public string[] Urls { get; set; }
        public string? CertificatePath { get; set; }
        public Version? HttpVersion { get; set; }
        public bool? DisableTcpCompression { get; set; }
        public TimeSpan? SaveChangesTimeout { get; set; }
    }
    #endregion
    */
}
