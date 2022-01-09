import net.ravendb.client.documents.BulkInsertOperation;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.ISessionDocumentCounters;
import net.ravendb.client.documents.smuggler.DatabaseItemType;
import net.ravendb.client.documents.smuggler.DatabaseSmugglerExportOptions;
import net.ravendb.client.documents.BulkInsertOperation.*;

import java.util.*;


public class Counters {


    void test() {
        DocumentStore docStore = new DocumentStore();
        //region counters_region_CountersFor_with_document_load
        // Use countersFor by passing it a document object

        // 1. Open a session
        try (IDocumentSession session = docStore.openSession()) {
            // 2. Use the session to load a document.
            Product document = session.load(Product.class, "products/1-C");

            // 3. Create an instance of `countersFor`
            //   Pass the document object returned from session.load as a param.
            ISessionDocumentCounters documentCounters = session.countersFor(document);

            // 4. Use `countersFor` methods to manage the product document's Counters
            documentCounters.delete("productLikes"); // Delete the "productLikes" Counter
            documentCounters.increment("productModified", 15); // Add 15 to Counter "productModified"
            Long counter = documentCounters.get("daysLeftForSale");// Get value of "daysLeftForSale"

            // 5. Save the changes to the session
            session.saveChanges();
        }
        //endregion

        //region counters_region_CountersFor_without_document_load
        // Use CountersFor without loading a document

        // 1. Open a session
        try (IDocumentSession session = docStore.openSession()) {
            // 2. pass an explicit document ID to the countersFor constructor
            ISessionDocumentCounters documentCounters = session.countersFor("products/1-C");

            // 3. Use `countersFor` methods to manage the product document's Counters
            documentCounters.delete("productLikes");  // Delete the "productLikes" Counter
            documentCounters.increment("productModified", 15); // Add 15 to Counter "productModified"
            Long counter = documentCounters.get("daysLeftForSale");// Get "daysLeftForSale"'s value

            // 4. Save changes to the session
            session.saveChanges();
        }
        //endregion

        //region counters_region_Delete
        // 1. Open a session
        try (IDocumentSession session = docStore.openSession()) {
            // 2. pass CountersFor's constructor a document ID
            ISessionDocumentCounters documentCounters = session.countersFor("products/1-C");

            // 3. Delete the "productLikes" Counter
            documentCounters.delete("productLikes");

            // 4. Save changes to the session
            session.saveChanges();
        }
        //endregion

        //region counters_region_Increment
        // 1. Open a session
        try (IDocumentSession session = docStore.openSession()) {
            // 2. pass CountersFor's constructor a document ID
            ISessionDocumentCounters documentCounters = session.countersFor("products/1-C");

            // 3. Use `countersFor.increment`
            documentCounters.increment("productLikes");  // Increase "productLikes" by 1, or create it with a value of 1
            documentCounters.increment("productDislikes", 1); // Increase "productDislikes" by 1, or create it with a value of 1
            documentCounters.increment("productPageViews", 15); // Increase "productPageViews" by 15, or create it with a value of 15
            documentCounters.increment("daysLeftForSale", -10); // Decrease "daysLeftForSale" by 10, or create it with a value of -10

            // 4. Save changes to the session
            session.saveChanges();
        }
        //endregion

        //region counters_region_Get
        // 1. Open a session
        try (IDocumentSession session = docStore.openSession()) {
            // 2. pass CountersFor's constructor a document ID
            ISessionDocumentCounters documentCounters = session.countersFor("products/1-C");

            // 3. Use `countersFor.get` to retrieve a Counter's value
            Long daysLeft = documentCounters.get("daysLeftForSale");
            System.out.println("Days Left For Sale: " + daysLeft);
        }
        //endregion

        //region counters_region_GetAll
        // 1. Open a session
        try (IDocumentSession session = docStore.openSession()) {
            // 2. pass countersFor's constructor a document ID
            ISessionDocumentCounters documentCounters = session.countersFor("products/1-C");

            // 3. Use GetAll to retrieve all of the document's Counters' names and values.
            Map<String, Long> counters = documentCounters.getAll();

            // list counters' names and values
            for (Map.Entry<String, Long> kvp : counters.entrySet()) {
                System.out.println("counter name: " + kvp.getKey() + ", counter value: " + kvp.getValue());
            }
        }
        //endregion


        try (IDocumentSession session = docStore.openSession()) {
            //region counters_region_load_include1
            //include single Counters
            Product productPage = session
                .load(Product.class, "products/1-C", includeBuilder -> {
                    includeBuilder.includeCounter("productLikes")
                        .includeCounter("productDislikes")
                        .includeCounter("productDownloads");
                });
            //endregion
        }

        try (IDocumentSession session = docStore.openSession()) {
            //region counters_region_load_include2
            //include multiple Counters
            //note that you can combine the inclusion of Counters and documents.

            Product productPage = session.load(Product.class, "orders/1-A", includeBuilder -> {
                includeBuilder.includeDocuments("products/1-C")
                    .includeCounters(new String[]{"productLikes", "productDislikes"});
            });
            //endregion
        }

        try (IDocumentSession session = docStore.openSession()) {
            //region counters_region_query_include_single_Counter
            //include a single Counter
            IDocumentQuery<Product> query = session.query(Product.class)
                .include(includeBuilder -> {
                    includeBuilder.includeCounter("productLikes");
                });
            //endregion
        }

        try (IDocumentSession session = docStore.openSession()) {
            //region counters_region_query_include_multiple_Counters
            //include multiple Counters
            IDocumentQuery<Product> query = session.query(Product.class)
                .include(includeBuilder -> {
                    includeBuilder.includeCounters(new String[]{"productLikes", "productDownloads"});
                });
            //endregion
        }

        try (IDocumentSession session = docStore.openSession()) {
            //region counters_region_rawqueries_counter
            //Various RQL expressions sent to the server using counter()
            //Returned Counter value is accumulated
            List<CounterResult> rawQuery1 = session
                .advanced()
                .rawQuery(CounterResult.class, "from products as p select counter(p, \"productLikes\")")
                .toList();

            List<CounterResult> rawQuery2 = session.advanced().rawQuery(CounterResult.class,
                "from products select counter(\"productLikes\") as productLikesCount")
                .toList();

            List<CounterResult> rawQuery3 = session.advanced()
                .rawQuery(CounterResult.class,
                    "from products where PricePerUnit > 50 select Name, counter(\"productLikes\")")
                .toList();
            //endregion

            //region counters_region_rawqueries_counterRaw
            //An RQL expression sent to the server using counterRaw()
            //Returned Counter value is distributed
            List<CounterResultRaw> query = session
                .advanced().rawQuery(CounterResultRaw.class,
                    "from users as u select counterRaw(u, \"downloads\")")
                .toList();
            //endregion
        }
        List<User> result = new ArrayList<>();
        //region bulk-insert-counters
        try (IDocumentSession session = docStore.openSession()) {
            IDocumentQuery<User> query = session.query(User.class)
                .whereLessThan("age", 30);

            result = query.toList();
        }

        try (BulkInsertOperation bulkInsert = docStore.bulkInsert()) {
            for (User user : result) {
                String userId = user.getId();
                CountersBulkInsert countersFor = bulkInsert.countersFor(userId);
                bulkInsert.countersFor(userId).increment("downloaded", 100);
            }
        }

        //endregion
    }


    private class CounterResult {
        private Long productPrice;
        private Long productLikes;
        private String productSection;
        private String name;

        public Long getProductPrice() {
            return productPrice;
        }

        public void setProductPrice(Long productPrice) {
            this.productPrice = productPrice;
        }

        public Long getProductLikes() {
            return productLikes;
        }

        public String getName() {
            return name;
        }

        public void setProductLikes(Long productLikes) {
            this.productLikes = productLikes;
        }

        public String getProductSection() {
            return productSection;
        }

        public void setProductSection(String productSection) {
            this.productSection = productSection;
        }

        public void setName(String name) {
            this.name = name;
        }
    }

    private static class CounterResultRaw {
        private Map<String, Long> downloads;

        public Map<String, Long> getDownloads() {
            return downloads;
        }

        public void setDownloads(Map<String, Long> downloads) {
            this.downloads = downloads;
        }
    }


    private static class Product {
        private String id;
        private String customerId;
        private Date started;
        private Date ended;
        private String issue;
        private int votes;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getCustomerId() {
            return customerId;
        }

        public void setCustomerId(String customerId) {
            this.customerId = customerId;
        }

        public Date getStarted() {
            return started;
        }

        public void setStarted(Date started) {
            this.started = started;
        }

        public Date getEnded() {
            return ended;
        }

        public void setEnded(Date ended) {
            this.ended = ended;
        }

        public String getIssue() {
            return issue;
        }

        public void setIssue(String issue) {
            this.issue = issue;
        }

        public int getVotes() {
            return votes;
        }

        public void setVotes(int votes) {
            this.votes = votes;
        }
    }

    //region counters_region_CounterItem
    public class CounterItem {
        private String name;
        private String docId;
        private String changeVector;
        private long value;

        // getters and setters
    }
    //endregion

    private interface IFoo {
        //region Increment-definition
        void increment(String counterName);

        void increment(String id, String name, long delta);
        //endregion

        //region Delete-definition
        void delete(String counterName);
        //endregion

        //region Get-definition
        long get(String counterName);
        //endregion

        //region GetAll-definition
        Map<String, Long> getAll();
        //endregion
    }

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            DatabaseSmugglerExportOptions exportOptions = new DatabaseSmugglerExportOptions();
            //region smuggler_options
            exportOptions.setOperateOnTypes(EnumSet.of(
                DatabaseItemType.INDEXES,
                DatabaseItemType.DOCUMENTS,
                DatabaseItemType.COUNTERS));
            //endregion
        }
    }

    private interface IFoo3 {
        //region CountersFor-definition
        public CountersBulkInsert countersFor(String id);
        //endregion

        //region Increment-definition
        public void increment(String id, String name, long delta);
        //endregion
    }
}

class User {
    String id;
    String name;

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


