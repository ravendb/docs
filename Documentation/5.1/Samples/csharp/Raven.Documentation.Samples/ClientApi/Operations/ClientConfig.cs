using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Http;
using Raven.Client.ServerWide.Operations;
using Xunit;
using Raven.Client.ServerWide.Operations.Configuration;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class ClientConfig
    {
        private interface IFoo
        {
            /*
            #region config_1_0
            public GetClientConfigurationOperation()
            #endregion
            */

            /*
            #region config_2_0
            public PutClientConfigurationOperation(ClientConfiguration configuration)
            #endregion
            */
        }
        private class Foo
        {
            #region config_1_1
            public class Result
            {
                public long Etag { get; set; }

                public ClientConfiguration Configuration { get; set; }
            }
            #endregion
        }



        public ClientConfig()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region config_1_2
                    GetClientConfigurationOperation.Result config = store.Maintenance.Send(new GetClientConfigurationOperation());
                    ClientConfiguration clientConfiguration = config.Configuration;
                    #endregion
                }

                {
                    #region config_2_2
                    ClientConfiguration clientConfiguration = new ClientConfiguration
                    {
                        MaxNumberOfRequestsPerSession = 100,
                        ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                    };
                    
                    store.Maintenance.Send(new PutClientConfigurationOperation(clientConfiguration));
                    #endregion
                }


                {
                    #region ApplyDatabaseSettings-PutDatabaseSettingsOperation
                    // Configure database settings
                    static void PutConfigurationSettings(DocumentStore store, Dictionary<string, string> settings)
                    {
                        // Save the new settings with PutDatabaseSettingsOperation
                        store.Maintenance.Send(new PutDatabaseSettingsOperation(store.Database, settings));
                        // The following two lines are needed to disable/enable the database so that the changes take effect
                        store.Maintenance.Server.Send(new ToggleDatabasesStateOperation(store.Database, disable: true));
                        store.Maintenance.Server.Send(new ToggleDatabasesStateOperation(store.Database, disable: false));
                    }
                    #endregion
                }

                {
                    #region SeeNewDatabaseSettings-GetDatabaseSettingsOperation
                    static DatabaseSettings GetConfigurationSettings(DocumentStore store)
                    {
                        var settings = store.Maintenance.Send(new GetDatabaseSettingsOperation(store.Database));
                        Assert.NotNull(settings);
                        return settings;
                    }
                    #endregion
                }

                {
                    #region signature-PutConfigurationSettings
                    static void PutConfigurationSettings(DocumentStore store, Dictionary<string, string> settings)
                    #endregion
                    {

                    }

                    #region signature-ToggleDatabasesStateOperation
                    store.Maintenance.Server.Send(new ToggleDatabasesStateOperation(store.Database, disable: true));
                    #endregion
                    {

                    }

                    #region signature-GetDatabaseSettingsOperation
                    var settings = store.Maintenance.Send(new GetDatabaseSettingsOperation(store.Database));
                    #endregion
                    {

                    }
                }

            }
        }
    }
}
