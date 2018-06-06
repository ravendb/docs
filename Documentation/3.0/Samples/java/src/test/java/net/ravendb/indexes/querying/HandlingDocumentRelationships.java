package net.ravendb.indexes.querying;

import static org.junit.Assert.assertEquals;

import java.util.Arrays;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;

import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.MultiLoadResult;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentQueryCustomizationFactory;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.Supplier;

import com.mysema.query.annotations.QueryEntity;


public class HandlingDocumentRelationships {

  //region includes_3_3
  public static class Orders_ByTotalPrice extends AbstractIndexCreationTask {
    public Orders_ByTotalPrice() {
      map =
       " from order in docs.Orders " +
       " select new                " +
       " {                         " +
       "     order.TotalPrice      " +
       " }; ";
    }
  }
  //endregion

  //region includes_8_5
  public class Order2s_ByTotalPrice extends AbstractIndexCreationTask {
    public Order2s_ByTotalPrice() {
      map =
       " from order in docs.Orders " +
       " select new                " +
       " {                         " +
       "     order.TotalPrice      " +
       " };";
    }
  }
  //endregion

  //region order
  @QueryEntity
  public static class Order {
    private String customerId;
    private UUID[] supplierIds;
    private Referral referral;
    private List<LineItem> lineItems;
    private double totalPrice;

    public String getCustomerId() {
      return customerId;
    }
    public void setCustomerId(String customerId) {
      this.customerId = customerId;
    }
    public UUID[] getSupplierIds() {
      return supplierIds;
    }
    public void setSupplierIds(UUID[] supplierIds) {
      this.supplierIds = supplierIds;
    }
    public Referral getReferral() {
      return referral;
    }
    public void setReferral(Referral referral) {
      this.referral = referral;
    }
    public List<LineItem> getLineItems() {
      return lineItems;
    }
    public void setLineItems(List<LineItem> lineItems) {
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
  @QueryEntity
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
  @QueryEntity
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
  @QueryEntity
  public class LineItem {
    private UUID productId;
    private String name;
    private int quantity;

    public UUID getProductId() {
      return productId;
    }
    public void setProductId(UUID productId) {
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
  @QueryEntity
  public class Order2 {
    private int customerId;
    private UUID[] supplierIds;
    private Referral referral;
    private List<LineItem> lineItems;
    private double totalPrice;

    public int getCustomerId() {
      return customerId;
    }
    public void setCustomerId(int customerId) {
      this.customerId = customerId;
    }
    public UUID[] getSupplierIds() {
      return supplierIds;
    }
    public void setSupplierIds(UUID[] supplierIds) {
      this.supplierIds = supplierIds;
    }
    public Referral getReferral() {
      return referral;
    }
    public void setReferral(Referral referral) {
      this.referral = referral;
    }
    public List<LineItem> getLineItems() {
      return lineItems;
    }
    public void setLineItems(List<LineItem> lineItems) {
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
  @QueryEntity
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
  @QueryEntity
  public static class Person {
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
  @QueryEntity
  public static class PersonWithAttribute {
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

  @QueryEntity
  public static class Attribute {
    private String ref;

    public String getRef() {
      return ref;
    }
    public void setRef(String ref) {
      this.ref = ref;
    }
  }
  //endregion


  @SuppressWarnings({"unused", "boxing"})
  public void Includes() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_1_0
        QHandlingDocumentRelationships_Order o = QHandlingDocumentRelationships_Order.order;
        Order order = session
          .include(o.customerId)
          .load(Order.class, "orders/1234");

        // this will not require querying the server!
        Customer customer = session.load(Customer.class, order.getCustomerId());
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region includes_1_1
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "orders/1234" } , new String[] { "CustomerId" });

      RavenJObject order = result.getResults().get(0);
      RavenJObject customer = result.getIncludes().get(0);
      //endregion
    }


    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_2_0
        QHandlingDocumentRelationships_Order o = QHandlingDocumentRelationships_Order.order;
        Order[] orders = session
          .include(o.customerId)
          .load(Order.class, Arrays.asList("orders/1234", "orders/4321"));

        for (Order order : orders) {
          // this will not require querying the server!
          Customer customer = session.load(Customer.class, order.getCustomerId());
        }
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_2_1
        MultiLoadResult result = store
          .getDatabaseCommands()
          .get(new String[] { "orders/1234", "orders/4321" }, new String[] { "CustomerId" });

        List<RavenJObject> orders = result.getResults();
        List<RavenJObject> customers = result.getIncludes();
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_3_0
        QHandlingDocumentRelationships_Order o = QHandlingDocumentRelationships_Order.order;
        List<Order> orders = session
          .query(Order.class, Orders_ByTotalPrice.class)
          .customize(new DocumentQueryCustomizationFactory().include(o.customerId))
          .where(o.totalPrice.gt(100))
          .toList();

        for (Order order : orders) {
          // this will not require querying the server!
          Customer customer = session.load(Customer.class, order.getCustomerId());
        }
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region includes_3_1
        QHandlingDocumentRelationships_Order o = QHandlingDocumentRelationships_Order.order;
        List<Order> orders = session
          .advanced()
          .documentQuery(Order.class, Orders_ByTotalPrice.class)
          .include(o.customerId)
          .whereGreaterThan(o.totalPrice, 100.0)
          .toList();

        for (Order order : orders) {
          // this will not require querying the server!
          Customer customer = session
            .load(Customer.class, order.getCustomerId());
        }
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region includes_3_2
        IndexQuery query = new IndexQuery("TotalPrice_Range:{Ix100 TO NULL}");
        QueryResult result = store
          .getDatabaseCommands()
          .query("Orders/ByTotalPrice", query, new String[] { "CustomerId" });

        List<RavenJObject> orders = result.getResults();
        Collection<RavenJObject> customers = result.getIncludes();
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_4_0
        QHandlingDocumentRelationships_Order o = QHandlingDocumentRelationships_Order.order;
        Order order = session
          .include(o.supplierIds)
          .load(Order.class, "orders/1234");

        for (UUID supplierId : order.getSupplierIds()) {
          // this will not require querying the server!
          Supplier supplier = session.load(Supplier.class, supplierId);
        }
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region includes_4_1

        MultiLoadResult result = store
          .getDatabaseCommands()
          .get(new String[] { "orders/1234" }, new String[] { "SupplierIds" });

        RavenJObject order = result.getResults().get(0);
        RavenJObject customer = result.getIncludes().get(0);
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_5_0
        QHandlingDocumentRelationships_Order o = QHandlingDocumentRelationships_Order.order;
        Order[] orders = session
          .include(o.supplierIds)
          .load(Order.class, "orders/1234", "orders/4321");

        for (Order order : orders) {
          for (UUID supplierId : order.getSupplierIds()) {
            // this will not require querying the server!
            Supplier supplier = session.load(Supplier.class, supplierId);
          }
        }
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region includes_5_1
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "orders/1234", "orders/4321" }, new String[] { "SupplierIds" });

      List<RavenJObject> orders = result.getResults();
      List<RavenJObject> customers = result.getIncludes();
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_6_0
        QHandlingDocumentRelationships_Order o = QHandlingDocumentRelationships_Order.order;
        QHandlingDocumentRelationships_Referral r = QHandlingDocumentRelationships_Referral.referral;
        Order order = session
          .include(o.referral.customerId)
          .load(Order.class, "orders/1234");

        // this will not require querying the server!
        Customer customer = session.load(Customer.class, order.getReferral().getCustomerId());
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_6_2
        Order order = session
          .include("Referral.CustomerId")
          .load(Order.class, "orders/1234");

        // this will not require querying the server!
        Customer customer = session.load(Customer.class, order.getReferral().getCustomerId());
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region includes_6_1
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "orders/1234" }, new String[] { "Referral.CustomerId" });

      RavenJObject order = result.getResults().get(0);
      RavenJObject customer = result.getIncludes().get(0);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_7_0
        QHandlingDocumentRelationships_Order o = QHandlingDocumentRelationships_Order.order;
        Order order = session
          .include(o.lineItems.select().productId)
          .load(Order.class, "orders/1234");

        for (LineItem lineItem : order.getLineItems()) {
          // this will not require querying the server!
          Product product = session.load(Product.class, lineItem.getProductId());
        }
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region includes_7_1
        MultiLoadResult result = store
          .getDatabaseCommands()
          .get(new String [] { "orders/1234" }, new String[] { "LineItems.,ProductId" });

        RavenJObject order = result.getResults().get(0);
        RavenJObject product = result.getIncludes().get(0);
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_8_0
        QHandlingDocumentRelationships_Order2 o = QHandlingDocumentRelationships_Order2.order2;
        Order2 order = session
          .include(Customer2.class, o.customerId)
          .load(Order2.class, "order2s/1234");

        // this will not require querying the server!
        Customer2 customer = session.load(Customer2.class, order.getCustomerId());
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region includes_8_1
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "order2s/1234" }, new String[] { "CustomerId" });

      RavenJObject order = result.getResults().get(0);
      RavenJObject customer = result.getIncludes().get(0);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_8_2
        QHandlingDocumentRelationships_Order2 o = QHandlingDocumentRelationships_Order2.order2;
        List<Order2> orders = session
          .query(Order2.class, Order2s_ByTotalPrice.class)
          .customize(new DocumentQueryCustomizationFactory().include(Customer2.class, o.customerId))
          .where(o.totalPrice.gt(100))
          .toList();

        for (Order2 order : orders) {
          // this will not require querying the server!
          Customer2 customer = session.load(Customer2.class, order.getCustomerId());
        }
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region includes_8_3
        QHandlingDocumentRelationships_Order2 o = QHandlingDocumentRelationships_Order2.order2;
        List<Order2> orders = session
          .advanced()
          .documentQuery(Order2.class, Order2s_ByTotalPrice.class)
          .include("CustomerId")
          .whereGreaterThan(o.totalPrice, 100.0)
          .toList();

        for (Order2 order : orders) {
          // this will not require querying the server!
          Customer2 customer = session.load(Customer2.class, order.getCustomerId());
        }

        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region includes_8_4
      QueryResult result = store
        .getDatabaseCommands()
        .query("Order2s/ByTotalPrice",
          new IndexQuery("TotalPrice_Range:{Ix100 TO NULL}"),
          new String[] { "CustomerId" });

      List<RavenJObject> orders = result.getResults();
      Collection<RavenJObject> customers = result.getIncludes();
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_8_6
        QHandlingDocumentRelationships_Order2 o = QHandlingDocumentRelationships_Order2.order2;
        Order2 order = session
          .include(Supplier.class, o.supplierIds)
          .load(Order2.class, "order2s/1234");

        for (UUID supplierId : order.getSupplierIds()) {
          // this will not require querying the server!
          Supplier supplier = session.load(Supplier.class, supplierId);
        }
        //endregion
      }
    }


    try (IDocumentStore store = new DocumentStore()) {
      //region includes_8_7
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "order2s/1234" }, new String[] { "SupplierIds" });

      RavenJObject order = result.getResults().get(0);
      List<RavenJObject> suppliers = result.getIncludes();
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_8_8
        QHandlingDocumentRelationships_Order2 o = QHandlingDocumentRelationships_Order2.order2;
        Order2 order = session
          .include(Customer2.class, o.referral.customerId)
          .load(Order2.class, "order2s/1234");

        // this will not require querying the server!
        Customer2 customer = session.load(Customer2.class, order.getReferral().getCustomerId());
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region includes_8_9
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "order2s/1234" }, new String[] { "Referral.CustomerId" });

      RavenJObject order = result.getResults().get(0);
      RavenJObject customer = result.getIncludes().get(0);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_8_10
        QHandlingDocumentRelationships_Order2 o = QHandlingDocumentRelationships_Order2.order2;
        Order2 order = session
          .include(Product.class, o.lineItems.select().productId)
          .load(Order2.class, "orders/1234");

        for (LineItem lineItem : order.getLineItems()) {
          // this will not require querying the server!
          Product product = session.load(Product.class, lineItem.getProductId());
        }
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region includes_8_11
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "order2s/1234" }, new String[] { "LineItems.,ProductId" });

      RavenJObject order = result.getResults().get(0);
      List<RavenJObject> products = result.getIncludes();
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_9_0
        QHandlingDocumentRelationships_Order3 o = QHandlingDocumentRelationships_Order3.order3;
        Order3 order = session
          .include(Customer.class, o.customer.id)
          .load(Order3.class, "orders/1234");

        // this will not require querying the server!
        Customer customer = session.load(Customer.class, order.getCustomer().getId());
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region includes_9_1
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "orders/1234" }, new String[] { "Customer.Id" });

      RavenJObject order = result.getResults().get(0);
      RavenJObject customer = result.getIncludes().get(0);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_10_0
        Map<String, String> attributes = new HashMap<String, String>();
        Person child = new Person();
        child.setId("people/1");
        child.setName("John Doe");
        attributes.put("Mother", "people/2");
        attributes.put("Father", "people/3");
        child.setAttributes(attributes);
        session.store(child);

        Person mother = new Person();
        mother.setId("people/2");
        mother.setName("Helen Doe");
        mother.setAttributes(new HashMap<String, String>());
        session.store(mother);

        Person father = new Person();
        father.setId("people/3");
        father.setName("George Doe");
        father.setAttributes(new HashMap<String, String>());
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region includes_10_1
        QHandlingDocumentRelationships_Person p = QHandlingDocumentRelationships_Person.person;
        Person person = session
          .include(p.attributes.values())
          .load(Person.class, "people/1");

        Person mother = session
          .load(Person.class, person.getAttributes().get("Mother"));

        Person father = session
          .load(Person.class, person.getAttributes().get("Father"));

        assertEquals(1, session.advanced().getNumberOfRequests());
        //endregion
      }

      //region includes_10_2
      MultiLoadResult result = store
        .getDatabaseCommands()
        .get(new String[] { "people/1"}, new String[] { "Attributes.$Values"});

      RavenJObject include1 = result.getIncludes().get(0);
      RavenJObject include2 = result.getIncludes().get(1);
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_10_3
        QHandlingDocumentRelationships_Person p = QHandlingDocumentRelationships_Person.person;
        Person person = session
          .include(p.attributes.keys())
          .load(Person.class, "people/1");
        //endregion
      }

      //region includes_10_4
      store.getDatabaseCommands()
        .get(new String[] { "people/1"}, new String[] { "Attributes.$Keys" });
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region includes_11_0
        Attribute motherAttr = new Attribute();
        motherAttr.setRef("people/2");

        Attribute fatherAttr = new Attribute();
        fatherAttr.setRef("people/3");

        Map<String, Attribute> attributes = new HashMap<String, HandlingDocumentRelationships.Attribute>();
        attributes.put("Mother", motherAttr);
        attributes.put("Father", fatherAttr);

        PersonWithAttribute person = new PersonWithAttribute();
        person.setId("people/1");
        person.setName("John Doe");
        person.setAttributes(attributes);
        session.store(person);

        Person mother = new Person();
        mother.setId("people/2");
        mother.setName("Helen Doe");
        mother.setAttributes(new HashMap<String, String>());
        session.store(mother);

        Person father = new Person();
        father.setId("people/3");
        father.setName("George Doe");
        father.setAttributes(new HashMap<String, String>());
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region includes_11_1
        QHandlingDocumentRelationships_PersonWithAttribute p =
          QHandlingDocumentRelationships_PersonWithAttribute.personWithAttribute;
        PersonWithAttribute person = session
          .include(p.attributes.values().select().ref)
          .load(PersonWithAttribute.class, "people/1");

        Person mother = session
          .load(Person.class, person.getAttributes().get("Mother").getRef());

        Person father = session
          .load(Person.class, person.getAttributes().get("Father").getRef());

        assertEquals(1, session.advanced().getNumberOfRequests());
        //endregion
      }

      //region includes_11_2
      MultiLoadResult result = store.getDatabaseCommands()
        .get(new String[] { "people/1"}, new String[] { "Attributes.$Values,Ref" });

      RavenJObject include1 = result.getIncludes().get(0);
      RavenJObject include2 = result.getIncludes().get(1);
      //endregion
    }
  }
}
