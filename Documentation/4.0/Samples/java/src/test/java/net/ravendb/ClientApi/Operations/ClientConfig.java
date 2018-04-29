package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.configuration.ClientConfiguration;
import net.ravendb.client.documents.operations.configuration.GetClientConfigurationOperation;
import net.ravendb.client.documents.operations.configuration.PutClientConfigurationOperation;
import net.ravendb.client.http.ReadBalanceBehavior;

public class ClientConfig {

    private interface IFoo {
        /*
        //region config_1_0
        GetClientConfigurationOperation()
        //endregion


        //region config_2_0
        PutClientConfigurationCommand(ClientConfiguration configuration)
        //endregion
        */
    }

    private static class Foo {
        //region config_1_1
        public static class Result {
            private long etag;
            private ClientConfiguration configuration;

            public long getEtag() {
                return etag;
            }

            public void setEtag(long etag) {
                this.etag = etag;
            }

            public ClientConfiguration getConfiguration() {
                return configuration;
            }

            public void setConfiguration(ClientConfiguration configuration) {
                this.configuration = configuration;
            }
        }
        //endregion
    }

    public ClientConfig() {
        try (IDocumentStore store = new DocumentStore()) {
            {
                //region config_1_2
                GetClientConfigurationOperation.Result config
                    = store.maintenance().send(new GetClientConfigurationOperation());
                ClientConfiguration clientConfiguration = config.getConfiguration();
                //endregion
            }

            {
                //region config_2_2
                ClientConfiguration clientConfiguration = new ClientConfiguration();
                clientConfiguration.setMaxNumberOfRequestsPerSession(100);
                clientConfiguration.setReadBalanceBehavior(ReadBalanceBehavior.FASTEST_NODE);

                store.maintenance().send(
                    new PutClientConfigurationOperation(clientConfiguration));
                //endregion
            }
        }
    }
}
