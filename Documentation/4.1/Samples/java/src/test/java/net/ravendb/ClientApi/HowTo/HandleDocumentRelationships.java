package net.ravendb.ClientApi.HowTo;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;
import org.junit.Assert;

import java.util.*;
import java.util.function.Supplier;

public class HandleDocumentRelationships {

    //region order
    public class Order {
        private String customerId;
        private String[] supplierIds;
        private Referral referral;
        private LineItem[] lineItems;
        private double totalPrice;

        public String getCustomerId() {
            return customerId;
        }

        public void setCustomerId(String customerId) {
            this.customerId = customerId;
        }

        public String[] getSupplierIds() {
            return supplierIds;
        }

        public void setSupplierIds(String[] supplierIds) {
            this.supplierIds = supplierIds;
        }

        public Referral getReferral() {
            return referral;
        }

        public void setReferral(Referral referral) {
            this.referral = referral;
        }

        public LineItem[] getLineItems() {
            return lineItems;
        }

        public void setLineItems(LineItem[] lineItems) {
            this.lineItems = lineItems;
        }

        public double getTotalPrice() {
            return totalPrice;
        }

        public void setTotalPrice(double totalPrice) {
            this.totalPrice = totalPrice;
        }
    }
    //endregion

    //region customer
    public class Customer {
        private String id;
        private String name;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
    //endregion

    //region denormalized_customer
    public class DenormalizedCustomer {
        private String id;
        private String name;
        private String address;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getAddress() {
            return address;
        }

        public void setAddress(String address) {
            this.address = address;
        }
    }
    //endregion


    //region referral
    public class Referral {
        private String customerId;
        private double commissionPercentage;

        public String getCustomerId() {
            return customerId;
        }

        public void setCustomerId(String customerId) {
            this.customerId = customerId;
        }

        public double getCommissionPercentage() {
            return commissionPercentage;
        }

        public void setCommissionPercentage(double commissionPercentage) {
            this.commissionPercentage = commissionPercentage;
        }
    }
    //endregion

    //region line_item
    public class LineItem {
        private String productId;
        private String name;
        private int quantity;

        public String getProductId() {
            return productId;
        }

        public void setProductId(String productId) {
            this.productId = productId;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public int getQuantity() {
            return quantity;
        }

        public void setQuantity(int quantity) {
            this.quantity = quantity;
        }
    }
    //endregion

    //region order_2
    public class Order2 {
        private int customerId;
        private UUID supplierIds;
        private Referral referral;
        private LineItem[] lineItems;
        private double totalPrice;

        public int getCustomerId() {
            return customerId;
        }

        public void setCustomerId(int customerId) {
            this.customerId = customerId;
        }

        public UUID getSupplierIds() {
            return supplierIds;
        }

        public void setSupplierIds(UUID supplierIds) {
            this.supplierIds = supplierIds;
        }

        public Referral getReferral() {
            return referral;
        }

        public void setReferral(Referral referral) {
            this.referral = referral;
        }

        public LineItem[] getLineItems() {
            return lineItems;
        }

        public void setLineItems(LineItem[] lineItems) {
            this.lineItems = lineItems;
        }

        public double getTotalPrice() {
            return totalPrice;
        }

        public void setTotalPrice(double totalPrice) {
            this.totalPrice = totalPrice;
        }
    }
    //endregion

    //region customer_2
    public class Customer2 {
        private int id;
        private String name;

        public int getId() {
            return id;
        }

        public void setId(int id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
    //endregion

    //region referral_2
    public class Referral2 {
        private int customerId;
        private double commissionPercentage;

        public int getCustomerId() {
            return customerId;
        }

        public void setCustomerId(int customerId) {
            this.customerId = customerId;
        }

        public double getCommissionPercentage() {
            return commissionPercentage;
        }

        public void setCommissionPercentage(double commissionPercentage) {
            this.commissionPercentage = commissionPercentage;
        }
    }
    //endregion

    //region order_3
    public class Order3 {
        private DenormalizedCustomer customer;
        private String[] supplierIds;
        private Referral referral;
        private LineItem[] lineItems;
        private double totalPrice;

        public DenormalizedCustomer getCustomer() {
            return customer;
        }

        public void setCustomer(DenormalizedCustomer customer) {
            this.customer = customer;
        }

        public String[] getSupplierIds() {
            return supplierIds;
        }

        public void setSupplierIds(String[] supplierIds) {
            this.supplierIds = supplierIds;
        }

        public Referral getReferral() {
            return referral;
        }

        public void setReferral(Referral referral) {
            this.referral = referral;
        }

        public LineItem[] getLineItems() {
            return lineItems;
        }

        public void setLineItems(LineItem[] lineItems) {
            this.lineItems = lineItems;
        }

        public double getTotalPrice() {
            return totalPrice;
        }

        public void setTotalPrice(double totalPrice) {
            this.totalPrice = totalPrice;
        }
    }
    //endregion

    //region person_1
    public class Person {
        private String id;
        private String name;
        private Map<String, String> attributes;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public Map<String, String> getAttributes() {
            return attributes;
        }

        public void setAttributes(Map<String, String> attributes) {
            this.attributes = attributes;
        }
    }
    //endregion

    //region person_2

    public class PersonWithAttribute {
        private String id;
        private String name;
        private Map<String, Attribute> attributes;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public Map<String, Attribute> getAttributes() {
            return attributes;
        }

        public void setAttributes(Map<String, Attribute> attributes) {
            this.attributes = attributes;
        }
    }

    public class Attribute {
        private String ref;

        public Attribute() {
        }

        public Attribute(String ref) {
            this.ref = ref;
        }

        public String getRef() {
            return ref;
        }

        public void setRef(String ref) {
            this.ref = ref;
        }
    }
    //endregion

    public static class Product {

    }

    public void includes() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_1_0
                Order order = session
                    .include("CustomerId")
                    .load(Order.class, "orders/1-A");

                // this will not require querying the server!
                Customer customer = session.load(Customer.class, order.getCustomerId());
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_2_0
                Map<String, Order> orders = session
                    .include("CustomerId")
                    .load(Order.class, "orders/1-A", "orders/2-A");

                for (Order order : orders.values()) {
                    Customer customer = session.load(Customer.class, order.getCustomerId());
                }
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_3_0
                List<Order> orders = session
                    .query(Order.class)
                    .include("CustomerId")
                    .whereGreaterThan("TotalPrice", 100)
                    .toList();

                for (Order order : orders) {
                    // this will not require querying the server!
                    Customer customer = session
                        .load(Customer.class, order.getCustomerId());
                }
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region includes_3_0_builder
                List<Order> orders = session
                    .query(Order.class)
                    .include(i -> i.
                        includeDocuments("CustomerId").
                        includeCounter("OrderUpdateCount"))
                    .whereGreaterThan("TotalPrice", 100)
                    .toList();

                for (Order order : orders) {
                    // this will not require querying the server!
                    Customer customer = session
                        .load(Customer.class, order.getCustomerId());
                }
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_4_0
                Order order = session
                    .include("SupplierIds")
                    .load(Order.class, "orders/1-A");

                for (String supplierId : order.getSupplierIds()) {
                    // this will not require querying the server!
                    Supplier supplier = session.load(Supplier.class, supplierId);
                }
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_4_0_builder
                Order order = session.load(Order.class, "orders/1-A",
                    i -> i.includeDocuments("SupplierIds"));

                for (String supplierId : order.getSupplierIds()) {
                    // this will not require querying the server!
                    Supplier supplier = session.load(Supplier.class, supplierId);
                }
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_5_0
                Map<String, Order> orders = session
                    .include("SupplierIds")
                    .load(Order.class, "orders/1-A", "orders/2-A");

                for (Order order : orders.values()) {
                    for (String supplierId : order.getSupplierIds()) {
                        // this will not require querying the server!

                        Supplier supplier = session.load(Supplier.class, supplierId);
                    }
                }
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_6_0
                Order order = session
                    .include("Referral.CustomerId")
                    .load(Order.class, "orders/1-A");

                // this will not require querying the server!
                Customer customer = session.load(Customer.class, order.getReferral().getCustomerId());
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region includes_6_0_builder
                Order order = session
                    .load(Order.class, "orders/1-A",
                        i -> i.includeDocuments("Referral.CustomerId"));

                // this will not require querying the server!
                Customer customer = session.load(Customer.class, order.getReferral().getCustomerId());
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_7_0
                Order order = session
                    .include("LineItems[].ProductId")
                    .load(Order.class, "orders/1-A");

                for (LineItem lineItem : order.getLineItems()) {
                    // this will not require querying the server!
                    Product product = session.load(Product.class, lineItem.getProductId());
                }
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region includes_7_0_builder
                Order order = session.load(Order.class, "orders/1-A",
                    i -> i.includeDocuments("LineItems[].ProductId"));

                for (LineItem lineItem : order.getLineItems()) {
                    // this will not require querying the server!
                    Product product = session.load(Product.class, lineItem.getProductId());
                }
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_9_0
                Order3 order = session
                    .include("Customer.Id")
                    .load(Order3.class, "orders/1-A");

                // this will not require querying the server!
                Customer customer = session.load(Customer.class, order.getCustomer().getId());
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_10_0
                HashMap<String, String> attributes1 = new HashMap<>();
                attributes1.put("Mother", "people/2");
                attributes1.put("Father", "people/3");

                Person person1 = new Person();
                person1.setId("people/1-A");
                person1.setName("John Doe");
                person1.setAttributes(attributes1);

                session.store(person1);

                Person person2 = new Person();
                person2.setId("people/2");
                person2.setName("Helen Doe");
                person2.setAttributes(Collections.emptyMap());

                session.store(person2);

                Person person3 = new Person();
                person3.setId("people/3");
                person3.setName("George Doe");
                person3.setAttributes(Collections.emptyMap());

                session.store(person3);
                //endregion

                //region includes_10_1
                Person person = session.include("Attributes.Values")
                    .load(Person.class, "people/1-A");

                Person mother = session
                    .load(Person.class, person.getAttributes().get("Mother"));

                Person father = session
                    .load(Person.class, person.getAttributes().get("Father"));

                Assert.assertEquals(1, session.advanced().getNumberOfRequests());
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region includes_10_1_builder
                Person person = session.load(Person.class, "people/1-A",
                    i -> i.includeDocuments("Attributes.Values"));

                Person mother = session
                    .load(Person.class, person.getAttributes().get("Mother"));

                Person father = session
                    .load(Person.class, person.getAttributes().get("Father"));

                Assert.assertEquals(1, session.advanced().getNumberOfRequests());
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_10_3
                Person person = session
                    .include("Attributes.Keys")
                    .load(Person.class, "people/1-A");
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region includes_10_3_builder
                Person person = session
                    .load(Person.class, "people/1-A",
                        i -> i.includeDocuments("Attributes.Keys"));
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_11_0
                HashMap<String, Attribute> attributes = new HashMap<>();
                attributes.put("Mother", new Attribute("people/2"));
                attributes.put("Father", new Attribute("people/3"));

                PersonWithAttribute person1 = new PersonWithAttribute();
                person1.setId("people/1-A");
                person1.setName("John Doe");
                person1.setAttributes(attributes);

                session.store(person1);

                Person person2 = new Person();
                person2.setId("people/2");
                person2.setName("Helen Doe");
                person2.setAttributes(Collections.emptyMap());

                session.store(person2);

                Person person3 = new Person();
                person3.setId("people/3");
                person3.setName("George Doe");
                person3.setAttributes(Collections.emptyMap());

                session.store(person3);
                //endregion
            }
        }

        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region includes_11_1
                PersonWithAttribute person = session
                    .include("Attributes[].Ref")
                    .load(PersonWithAttribute.class, "people/1-A");

                Person mother = session
                    .load(Person.class, person.getAttributes().get("Mother").getRef());

                Person father = session
                    .load(Person.class, person.getAttributes().get("Father").getRef());

                Assert.assertEquals(1, session.advanced().getNumberOfRequests());
                //endregion
            }
        }
    }

}
