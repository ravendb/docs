package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.queries.QueryData;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Projections {

    private static class Employee {

    }

    private static class EmployeeProjection {

    }

    private static class FirstAndLastName {

    }

    private static class ShipToAndProducts {

    }

    private static class OrderProjection {

    }

    private static class Order {

    }

    private static class Total {

    }

    private static class FullName {
        private String fullName;

        public String getFullName() {
            return fullName;
        }

        public void setFullName(String fullName) {
            this.fullName = fullName;
        }
    }

    //region indexes_1
    public static class Employees_ByFirstAndLastName extends AbstractIndexCreationTask {
        public Employees_ByFirstAndLastName() {
            map = "docs.Employees.Select(employee => new {" +
                "    FirstName = employee.FirstName," +
                "    LastName = employee.LastName" +
                "})";
        }
    }
    //endregion

    //region indexes_1_stored
    public static class Employees_ByFirstAndLastNameWithStoredFields extends AbstractIndexCreationTask {
        public Employees_ByFirstAndLastNameWithStoredFields() {
            map = "docs.Employees.Select(employee => new {" +
                "    FirstName = employee.FirstName," +
                "    LastName = employee.LastName" +
                "})";

            storeAllFields(FieldStorage.YES); // firstName and lastName fields can be retrieved directly from index
        }
    }
    //endregion

    //region indexes_2
    public static class Employees_ByFirstNameAndBirthday extends AbstractIndexCreationTask {
        public Employees_ByFirstNameAndBirthday() {
            map = "docs.Employees.Select(employee => new {" +
                "    FirstName = employee.FirstName," +
                "    Birthday = employee.Birthday" +
                "})";
        }
    }
    //endregion

    //region indexes_3
    public static class Orders_ByShipToAndLines extends AbstractIndexCreationTask {
        public Orders_ByShipToAndLines() {
            map = "docs.Orders.Select(order => new {" +
                "    ShipTo = order.ShipTo," +
                "    Lines = order.Lines" +
                "})";
        }
    }
    //endregion

    //region indexes_4
    public static class Orders_ByShippedAtAndCompany extends AbstractIndexCreationTask {
        public Orders_ByShippedAtAndCompany() {
            map = "docs.Orders.Select(order => new {" +
                "    ShippedAt = order.ShippedAt," +
                "    Company = order.Company" +
                "})";
        }
    }
    //endregion


    public Projections() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region projections_1
                List<FirstAndLastName> results = session
                    .query(Employee.class, Employees_ByFirstAndLastName.class)
                    .selectFields(FirstAndLastName.class, "FirstName", "LastName")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_1_stored
                List<FirstAndLastName> results = session
                    .query(Employee.class, Employees_ByFirstAndLastNameWithStoredFields.class)
                    .selectFields(FirstAndLastName.class, "FirstName", "LastName")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_2
                QueryData queryData = new QueryData(new String[]{"ShipTo", "Lines[].ProductName"},
                    new String[]{"ShipTo", "Products"});

                List<ShipToAndProducts> results = session.query(Order.class)
                    .selectFields(ShipToAndProducts.class, queryData)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_3
                List<FullName> results = session.advanced().rawQuery(FullName.class, "from Employees as e " +
                    "select {" +
                    "    FullName : e.FirstName + \" \" + e.LastName " +
                    "}").toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_4
                List<Employee> results = session.advanced().rawQuery(Employee.class, "declare function output(e) { " +
                    "    var format = function(p){ return p.FirstName + \" \" + p.LastName; }; " +
                    "    return { FullName : format(e) }; " +
                    "} " +
                    "from Employees as e select output(e)").toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_5
                List<OrderProjection> results = session.advanced().rawQuery(OrderProjection.class, "from Orders as o " +
                    "load o.company as c " +
                    "select { " +
                    "    CompanyName: c.Name," +
                    "    ShippedAt: o.ShippedAt" +
                    "}").toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_6
                List<EmployeeProjection> results = session.advanced().rawQuery(EmployeeProjection.class, "from Employees as e " +
                    "select { " +
                    "    DayOfBirth : new Date(Date.parse(e.Birthday)).getDate(), " +
                    "    MonthOfBirth : new Date(Date.parse(e.Birthday)).getMonth() + 1, " +
                    "    Age : new Date().getFullYear() - new Date(Date.parse(e.Birthday)).getFullYear() " +
                    "}").toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_7
                List<EmployeeProjection> results = session.advanced().rawQuery(EmployeeProjection.class, "from Employees as e " +
                    "select { " +
                    "    Date : new Date(Date.parse(e.Birthday)), " +
                    "    Name : e.FirstName.substr(0,3) " +
                    "}").toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_8
                List<Employee> results = session.advanced().rawQuery(Employee.class, "from Employees as e " +
                    "select {" +
                    "     Name : e.FirstName, " +
                    "     Metadata : getMetadata(e)" +
                    "}").toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region projections_9
                List<Total> results = session.advanced().rawQuery(Total.class, "from Orders as o " +
                    "select { " +
                    "    Total : o.Lines.reduce( " +
                    "        (acc , l) => acc += l.PricePerUnit * l.Quantity, 0) " +
                    "}").toList();
                //endregion
            }

        }
    }
}
