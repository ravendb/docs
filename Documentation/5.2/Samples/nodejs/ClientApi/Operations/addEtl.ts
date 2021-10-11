
import {
    AddEtlOperation,
    DocumentStore,
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

    //change it after all is OK
    //region add_etl_operation
    public AddEtlOperation(configuration: EtlConfiguration<T>);
    //endregion


    //region add_raven_etl
    const etlConfigurationRvn = Object.assign(new RavenEtlConfiguration(), {
        connectionStringName: "raven-connection-string-name",
        disabled: false,
        name: "etlRvn"
    } as Partial<RavenEtlConfiguration>);

    const transformationRvn: Transformation = {
        applyToAllDocuments: true,
        name: "Script #1"
    };

    etlConfigurationRvn.transforms = [transformationRvn];

    const operationRvn = new AddEtlOperation(etlConfigurationRvn);

    const etlResultRvn = await store.maintenance.send(operationRvn);
    //endregion


    //region add_sql_etl
    const transformation: Transformation = {
        applyToAllDocuments: true,
        name: "Script #1"
    }

    const table1: SqlEtlTable = {
        documentIdColumn: "Id",
        insertOnlyMode: false,
        tableName: "Users"
    }

    const etlConfigurationSql = Object.assign(new SqlEtlConfiguration(), {
        connectionStringName: "sql-connection-string-name",
        disabled: false,
        name: "etlSql",
        transforms: [transformation],
        sqlTables: [table1]
    } as Partial<SqlEtlConfiguration>);

    const operationSql = new AddEtlOperation(etlConfigurationSql);
    const etlResult = await store.maintenance.send(operationSql);
    //endregion

    //region add_olap_etl
    const transformationOlap: Transformation = {
        applyToAllDocuments: true,
        name: "Script #1"
    }


    const etlConfigurationOlap = Object.assign(new OlapEtlConfiguration(), {
        connectionStringName: "olap-connection-string-name",
        disabled: false,
        name: "etlOlap",
        transforms: [transformationOlap],
    } as Partial<OlapEtlConfiguration>);

    const operationOlap = new AddEtlOperation(etlConfigurationOlap);
    const etlResultOlap = await store.maintenance.send(operation);
    //endregion
}