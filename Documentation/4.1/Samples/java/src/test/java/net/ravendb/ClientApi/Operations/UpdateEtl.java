package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.etl.*;

import java.util.Arrays;

public class UpdateEtl {

    private interface IFoo {
        /*
        //region update_etl_operation
        public UpdateEtlOperation(long taskId, EtlConfiguration<T> configuration);
        //endregion
        */
    }

    public UpdateEtl() {
        try (IDocumentStore store = new DocumentStore()) {
            AddEtlOperationResult addEtlResult = new AddEtlOperationResult();
            //region update_etl_example
            //store.maintenance().send(new AddEtlOperation<RavenConnectionString>(...));

            RavenEtlConfiguration etlConfiguration = new RavenEtlConfiguration();
            etlConfiguration.setConnectionStringName("raven-connection-string-name");
            etlConfiguration.setName("Employees ETL");
            Transformation transformation = new Transformation();
            transformation.setName("Script #1");
            transformation.setCollections(Arrays.asList("Employees"));
            transformation.setScript("loadToEmployees ({\n" +
                "                        Name: this.FirstName + ' ' + this.LastName,\n" +
                "                            Title: this.Title\n" +
                "                    });");

            etlConfiguration.setTransforms(Arrays.asList(transformation));

            UpdateEtlOperation<RavenConnectionString> operation = new UpdateEtlOperation<>(
                addEtlResult.getTaskId(), etlConfiguration);
            UpdateEtlOperationResult result = store.maintenance().send(operation);
            //endregion
        }
    }
}
