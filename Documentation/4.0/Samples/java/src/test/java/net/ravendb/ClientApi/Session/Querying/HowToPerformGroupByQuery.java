package net.ravendb.ClientApi.Session.Querying;

import com.sun.org.apache.xpath.internal.operations.Or;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.queries.GroupBy;
import net.ravendb.client.documents.session.GroupByField;
import net.ravendb.client.documents.session.IDocumentSession;
import org.omg.CORBA.ORB;

import java.util.List;

//region group_by_using
import static net.ravendb.client.documents.queries.GroupBy.array;
import static net.ravendb.client.documents.queries.GroupBy.field;
//endregion

public class HowToPerformGroupByQuery {



    public HowToPerformGroupByQuery() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region group_by_1

                List<CountryAndQuantity> orders = session.query(Order.class)
                    .groupBy("ShipTo.Country")
                    .selectKey("ShipTo.Country", "Country")
                    .selectSum(new GroupByField("Lines[].Quantity", "OrderedQuantity"))
                    .ofType(CountryAndQuantity.class)
                    .toList();

                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region group_by_2
                List<CountByCompanyAndEmployee> results = session.query(Order.class)
                    .groupBy("Employee", "Company")
                    .selectKey("Employee", "EmployeeIdentifier")
                    .selectKey("Company")
                    .selectCount()
                    .ofType(CountByCompanyAndEmployee.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region group_by_3
                List<CountOfEmployeeAndCompanyPairs> orders = session.query(Order.class)
                    .groupBy("Employee", "Company")
                    .selectKey("key()", "EmployeeCompanyPair")
                    .selectCount("Count")
                    .ofType(CountOfEmployeeAndCompanyPairs.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region group_by_4
                List<ProductsInfo> products = session.query(Order.class)
                    .groupBy(array("Lines[].Product"))
                    .selectKey("key()", "Products")
                    .selectCount()
                    .ofType(ProductsInfo.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region group_by_5
                List<ProductInfo> results = session.advanced().documentQuery(Order.class)
                    .groupBy("Lines[].Product", "ShipTo.Country")
                    .selectKey("Lines[].Product", "Product")
                    .selectKey("ShipTo.Country", "Country")
                    .selectCount()
                    .ofType(ProductInfo.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region group_by_6
                List<ProductInfo> results = session.query(Order.class)
                    .groupBy(array("Lines[].Product"), array("Lines[].Quantity"))
                    .selectKey("Lines[].Product", "Product")
                    .selectKey("Lines[].Quantity", "Quantity")
                    .selectCount()
                    .ofType(ProductInfo.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region group_by_7
                List<ProductsInfo> results = session.query(Order.class)
                    .groupBy(array("Lines[].Product"))
                    .selectKey("key()", "Products")
                    .selectCount()
                    .ofType(ProductsInfo.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region group_by_8
                List<ProductsInfo> results = session.query(Order.class)
                    .groupBy(array("Lines[].Product"), field("ShipTo.Country"))
                    .selectKey("Lines[].Product", "Products")
                    .selectKey("ShipTo.Country", "Country")
                    .selectCount()
                    .ofType(ProductsInfo.class)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region group_by_9
                List<ProductsInfo> results = session.query(Order.class)
                    .groupBy(array("Lines[].Product"), array("Lines[].Quantity"))
                    .selectKey("Lines[].Product", "Products")
                    .selectKey("Lines[].Quantity", "Quantities")
                    .selectCount()
                    .ofType(ProductsInfo.class)
                    .toList();
                //endregion

            }
        }
    }

    public static class CountByCompanyAndEmployee {
        private int count;
        private String company;
        private String employeeIdentifier;

        public int getCount() {
            return count;
        }

        public void setCount(int count) {
            this.count = count;
        }

        public String getCompany() {
            return company;
        }

        public void setCompany(String company) {
            this.company = company;
        }

        public String getEmployeeIdentifier() {
            return employeeIdentifier;
        }

        public void setEmployeeIdentifier(String employeeIdentifier) {
            this.employeeIdentifier = employeeIdentifier;
        }
    }

    public static class Order {

    }

    public static class CountryAndQuantity {
        private String country;
        private int orderedQuantity;

        public String getCountry() {
            return country;
        }

        public void setCountry(String country) {
            this.country = country;
        }

        public int getOrderedQuantity() {
            return orderedQuantity;
        }

        public void setOrderedQuantity(int orderedQuantity) {
            this.orderedQuantity = orderedQuantity;
        }
    }

    public static class EmployeeAndCompany {
        private String employee;
        private String company;

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
    }

    public static class CountOfEmployeeAndCompanyPairs {
        private EmployeeAndCompany employeeCompanyPair;
        private int count;

        public EmployeeAndCompany getEmployeeCompanyPair() {
            return employeeCompanyPair;
        }

        public void setEmployeeCompanyPair(EmployeeAndCompany employeeCompanyPair) {
            this.employeeCompanyPair = employeeCompanyPair;
        }

        public int getCount() {
            return count;
        }

        public void setCount(int count) {
            this.count = count;
        }
    }

    public static class ProductInfo {
        private int count;
        private String product;
        private int quantity;

        public int getCount() {
            return count;
        }

        public void setCount(int count) {
            this.count = count;
        }

        public String getProduct() {
            return product;
        }

        public void setProduct(String product) {
            this.product = product;
        }

        public int getQuantity() {
            return quantity;
        }

        public void setQuantity(int quantity) {
            this.quantity = quantity;
        }
    }

    public static class ProductsInfo {
        private int count;
        private List<String> products;
        private int quantity;

        public int getCount() {
            return count;
        }

        public void setCount(int count) {
            this.count = count;
        }

        public List<String> getProducts() {
            return products;
        }

        public void setProducts(List<String> products) {
            this.products = products;
        }

        public int getQuantity() {
            return quantity;
        }

        public void setQuantity(int quantity) {
            this.quantity = quantity;
        }
    }

}
