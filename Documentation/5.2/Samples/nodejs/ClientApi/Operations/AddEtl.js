
import { DocumentStore } from 'ravendb';
import { EtlConfiguration } from 'ravendb';

let urls, database, authOptions;

{
    //document_store_creation
    const store = new DocumentStore(["http://localhost:8080"], "Northwind2");
    store.initialize();
    const session = store.openSession();

    //region add_etl_operation
    public AddEtlOperation(configuration: EtlConfiguration<T>);
    //endregion



    //region add_raven_etl

    const etlConfiguration = Object.assign(new RavenEtlConfiguration(), {
        connectionStringName: "raven-connection-string-name",
        disabled: false,
        name: "Employees ETL"
    } as Partial<RavenEtlConfiguration>);

    const transformation: Transformation = {
        name: "Script #1",
        collections?: "Employees",
        Script= "loadToEmployees ({ Name: this.FirstName + ' ' + this.LastName, Title: this.Title}); "
    };

    etlConfiguration.transforms = [transformation];

    const operation = new AddEtlOperation(etlConfiguration);

    const etlResult = await store.maintenance.send(operation);

    //endregion



    //region add_sql_etl
    const transformation = {
        name: "Script#1",
        collections?: "Orders",
        Script= "var orderData = {Id: id(this), OrderLinesCount: this.Lines.length, TotalCost: 0};
        for (var i = 0; i< this.Lines.length; i++) {
        var line = this.Lines[i];
        orderData.TotalCost += line.PricePerUnit;
        // Load to SQL table 'OrderLines'
        loadToOrderLines({
            OrderId: id(this),
            Qty: line.Quantity,
            Product: line.Product,
            Cost: line.PricePerUnit
        });
    }
    orderData.TotalCost = Math.round(orderData.TotalCost * 100) / 100;
    loadToOrders(orderData)"
    }
//end of transformation

    const table1 = {
        documentIdColumn: "Id",
        insertOnlyMode: false,
        tableName: "Orders"
    }

    const table2 = {
        documentIdColumn: "OrderId",
        insertOnlyMode: false,
        tableName: "OrderLines"
    }

    const etlConfiguration = Object.assign(new SqlEtlConfiguration(), {
        connectionStringName: "sql-connection-string-name",
        disabled: false,
        name: "Orders to SQL",
        transforms: [transformation],
        sqlTables: [table1, table2]
    }

    const operation = new AddEtlOperation(etlConfiguration);
    const etlResult = await store.maintenance.send(operation);

    //endregion
