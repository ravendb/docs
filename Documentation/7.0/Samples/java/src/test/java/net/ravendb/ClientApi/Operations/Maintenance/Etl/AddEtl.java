package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.etl.*;
import net.ravendb.client.documents.operations.etl.olap.OlapConnectionString;
import net.ravendb.client.documents.operations.etl.olap.OlapEtlConfiguration;
import net.ravendb.client.documents.operations.etl.sql.SqlConnectionString;
import net.ravendb.client.documents.operations.etl.sql.SqlEtlConfiguration;
import net.ravendb.client.documents.operations.etl.sql.SqlEtlTable;

import java.util.Arrays;

public class AddEtl {

    private interface IFoo<T> {
        /*
        //region add_etl_operation
        public AddEtlOperation(EtlConfiguration<T> configuration);
        //endregion
        */
    }

    public AddEtl() {
        try (IDocumentStore store = new DocumentStore()) {
            //region add_raven_etl
            RavenEtlConfiguration configuration = new RavenEtlConfiguration();
            configuration.setName("Employees ETL");
            Transformation transformation = new Transformation();
            transformation.setName("Script #1");
            transformation.setScript("loadToEmployees ({\n" +
                "  Name: this.FirstName + ' ' + this.LastName,\n" +
                "  Title: this.Title\n" +
                "});");

            configuration.setTransforms(Arrays.asList(transformation));
            AddEtlOperation<RavenConnectionString> operation = new AddEtlOperation<>(configuration);
            AddEtlOperationResult result = store.maintenance().send(operation);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region add_sql_etl
            SqlEtlConfiguration configuration = new SqlEtlConfiguration();
            SqlEtlTable table1 = new SqlEtlTable();
            table1.setTableName("Orders");
            table1.setDocumentIdColumn("Id");
            table1.setInsertOnlyMode(false);

            SqlEtlTable table2 = new SqlEtlTable();
            table2.setTableName("OrderLines");
            table2.setDocumentIdColumn("OrderId");
            table2.setInsertOnlyMode(false);

            configuration.setSqlTables(Arrays.asList(table1, table2));
            configuration.setName("Order to SQL");
            configuration.setConnectionStringName("sql-connection-string-name");

            Transformation transformation = new Transformation();
            transformation.setName("Script #1");
            transformation.setCollections(Arrays.asList("Orders"));
            transformation.setScript("var orderData = {\n" +
                "    Id: id(this),\n" +
                "    OrderLinesCount: this.Lines.length,\n" +
                "    TotalCost: 0\n" +
                "};\n" +
                "\n" +
                "    for (var i = 0; i < this.Lines.length; i++) {\n" +
                "        var line = this.Lines[i];\n" +
                "        orderData.TotalCost += line.PricePerUnit;\n" +
                "\n" +
                "        // Load to SQL table 'OrderLines'\n" +
                "        loadToOrderLines({\n" +
                "            OrderId: id(this),\n" +
                "            Qty: line.Quantity,\n" +
                "            Product: line.Product,\n" +
                "            Cost: line.PricePerUnit\n" +
                "        });\n" +
                "    }\n" +
                "    orderData.TotalCost = Math.round(orderData.TotalCost  * 100) / 100;\n" +
                "\n" +
                "    // Load to SQL table 'Orders'\n" +
                "    loadToOrders(orderData)");

            configuration.setTransforms(Arrays.asList(transformation));

            AddEtlOperation<SqlConnectionString> operation = new AddEtlOperation<>(configuration);

            AddEtlOperationResult result = store.maintenance().send(operation);
            //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region add_olap_etl
            OlapEtlConfiguration configuration = new OlapEtlConfiguration();

            configuration.setName("Orders ETL");
            configuration.setConnectionStringName("olap-connection-string-name");

            Transformation transformation = new Transformation();
            transformation.setName("Script #1");
            transformation.setCollections(Arrays.asList("Orders"));
            transformation.setScript("var orderDate = new Date(this.OrderedAt);\n"+
                "var year = orderDate.getFullYear();\n"+
                "var month = orderDate.getMonth();\n"+
                "var key = new Date(year, month);\n"+
                "loadToOrders(key, {\n"+
                "    Company : this.Company,\n"+
                "    ShipVia : this.ShipVia\n"+
                "})"
            );

            configuration.setTransforms(Arrays.asList(transformation));

            AddEtlOperation<OlapConnectionString> operation = new AddEtlOperation<OlapConnectionString>(configuration);

            AddEtlOperationResult result = store.maintenance().send(operation);
            //endregion
        }

    }
}
