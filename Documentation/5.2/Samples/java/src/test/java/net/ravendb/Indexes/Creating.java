import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.*;
import net.ravendb.client.documents.operations.indexes.PutIndexesOperation;
import net.ravendb.client.documents.session.IDocumentSession;
import java.util.Collections;
import java.util.List;

public class Creating {

    private static class Order {
    }
    
    private static class Employee {
    }

    //region indexes_8
    public static class Orders_Totals extends AbstractIndexCreationTask {
        public static class Result {
            private String employee;
            private String company;
            private double total;

            public String getEmployee() {
                return employee;
            }

            public void setEmployee(String employee) {
                this.employee = employee;
            }

            public String getCompany() {
                return company;
            }

            public void setCompany(String company) {
                this.company = company;
            }

            public double getTotal() {
                return total;
            }

            public void setTotal(double total) {
                this.total = total;
            }
        }

        public Orders_Totals() {
            map = "docs.Orders.Select(order => new { " +
                "    Employee = order.Employee, " +
                "    Company = order.Company, " +
                "    Total = Enumerable.Sum(order.Lines, l => ((decimal)((((decimal) l.Quantity) * l.PricePerUnit) * (1M - l.Discount)))) " +
                "})";
        }

        public static void main(String[] args) {
            try (IDocumentStore store = new DocumentStore(new String[]{ "http://localhost:8080" }, "Northwind")) {
                store.initialize();

                new Orders_Totals().execute(store);

                try (IDocumentSession session = store.openSession()) {
                    List<Order> orders = session
                        .query(Result.class, Orders_Totals.class)
                        .whereGreaterThan("Total", 100)
                        .ofType(Order.class)
                        .toList();
                }
            }
        }
    }
    //endregion

    private class Foo {
        //region indexes_1
        public class Orders_Totals extends AbstractIndexCreationTask {
            /// ...
        }
        //endregion
    }

    public Creating() {
        try (IDocumentStore store = new DocumentStore( new String[]{ "http://127.0.0.1:8080" }, "Northwind")) {
            IndexCreation.createIndexes(Collections.singletonList(new Users_ByName()), store);

            //region indexes_6
            IndexDefinitionBuilder builder = new IndexDefinitionBuilder();
            builder.setMap(
                    "from order in docs.Orders \n" +
                            "select new \n" +
                            " {\n" +
                            "    order.employee,\n" +
                            "    order.company,\n" +
                            "    total = order.lines.Sum(l => (l.quantity * l.pricePerUnit) * (1 - l.discount))\n" +
                            "}");

            store.maintenance()
                    .send(new PutIndexesOperation(builder.toIndexDefinition(store.getConventions())));
           //endregion
        }

        try (IDocumentStore store = new DocumentStore()) {
            //region indexes_2
            // deploy index to database defined in `DocumentStore.getDatabase` method
            // using default DocumentStore `conventions`
            new Orders_Totals().execute(store);
            //endregion

            //region indexes_3
            // deploy index to `Northwind` database
            // using default DocumentStore `conventions`
            new Orders_Totals().execute(store, store.getConventions(), "Northwind");
            //endregion

            //region indexes_5
            IndexDefinition indexDefinition = new IndexDefinition();
            indexDefinition.setName("Orders/Totals");
            indexDefinition.setMaps(Collections.singleton(
                "from order in docs.Orders " +
                " select new " +
                " { " +
                "    order.employee, " +
                "    order.company, " +
                "    total = order.lines.Sum(l => (l.quantity * l.pricePerUnit) * (1 - l.discount)) " +
                "}"
            ));

            store
                .maintenance()
                .send(new PutIndexesOperation(indexDefinition));
            //endregion

            try (IDocumentSession session = store.openSession()) {
                //region indexes_7
                List<Employee> employees = session
                    .query(Employee.class)
                    .whereEquals("firstName", "Robert")
                    .andAlso()
                    .whereEquals("lastName", "King")
                    .toList();
                //endregion
            }




        }
    }

    private class Foo2{
        IndexConfiguration configuration;
        //region indexes_9
        public class Orders_Totals extends AbstractIndexCreationTask {
            public Orders_Totals() {
                // ...
                configuration.put("MapTimeoutInSec","30");
                setConfiguration(configuration);
            }
        }
        //endregion
    }


    private class Users_ByName extends AbstractIndexCreationTask {

    }
}
