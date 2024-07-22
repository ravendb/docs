
import {
    AddEtlOperation,
    DocumentStore, OlapEtlConfiguration,
    RavenEtlConfiguration,
    SqlEtlConfiguration,
    SqlEtlTable,
    Transformation
} from 'ravendb';
import { EtlConfiguration } from 'ravendb';

    let urls, database, authOptions;

    class T {
    }

{
    //document_store_creation
    const store = new DocumentStore(["http://localhost:8080"], "Northwind2");
    store.initialize();
    const session = store.openSession();
    let configuration;
    let etlConfiguration;


    //region add_etl_operation
    const operation = new AddEtlOperation(etlConfiguration);
    //endregion


    //region add_raven_etl
    const etlConfigurationRvn = Object.assign(new RavenEtlConfiguration(), {
        connectionStringName: "raven-connection-string-name",
        disabled: false,
        name: "etlRvn"
    });

    const transformationRvn = {
        applyToAllDocuments: true,
        name: "Script #1"
    };

    etlConfigurationRvn.transforms = [transformationRvn];

    const operationRvn = new AddEtlOperation(etlConfigurationRvn);
    const etlResultRvn = await store.maintenance.send(operationRvn);
    //endregion


    //region add_sql_etl
    const transformation = {
        applyToAllDocuments: true,
        name: "Script #1"
    };

    const table1 = {
        documentIdColumn: "Id",
        insertOnlyMode: false,
        tableName: "Users"
    };

    const etlConfigurationSql = Object.assign(new SqlEtlConfiguration(), {
        connectionStringName: "sql-connection-string-name",
        disabled: false,
        name: "etlSql",
        transforms: [transformation],
        sqlTables: [table1]
    });

    const operationSql = new AddEtlOperation(etlConfigurationSql);
    const etlResult = await store.maintenance.send(operationSql);
    //endregion

    //region add_olap_etl
    const transformationOlap = {
        applyToAllDocuments: true,
        name: "Script #1"
    };

    const etlConfigurationOlap = Object.assign(new OlapEtlConfiguration(), {
        connectionStringName: "olap-connection-string-name",
        disabled: false,
        name: "etlOlap",
        transforms: [transformationOlap],
    });

    const operationOlap = new AddEtlOperation(etlConfigurationOlap);
    const etlResultOlap = await store.maintenance.send(operationOlap);
    //endregion
}