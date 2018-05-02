package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldStorage;
import net.ravendb.client.documents.session.IDocumentSession;
import org.junit.Assert;

import java.util.List;

public class IndexingLinqExtensions {

    //region indexes_1
    public static class Employees_ByReversedFirstName extends AbstractIndexCreationTask {
        public Employees_ByReversedFirstName() {
            map = "docs.Employees.Select(employee => new { " +
                "    firstName = employee.firstName.Reverse() " +
                "})";
        }
    }
    //endregion

    //region indexes_3
    public static class Item_Parse extends AbstractIndexCreationTask {
        public static class Result {
            private int majorWithDefault;
            private int majorWithCustomDefault;

            public int getMajorWithDefault() {
                return majorWithDefault;
            }

            public void setMajorWithDefault(int majorWithDefault) {
                this.majorWithDefault = majorWithDefault;
            }

            public int getMajorWithCustomDefault() {
                return majorWithCustomDefault;
            }

            public void setMajorWithCustomDefault(int majorWithCustomDefault) {
                this.majorWithCustomDefault = majorWithCustomDefault;
            }
        }

        public Item_Parse() {
            map = "docs.Items.Select(item => new {" +
                "    item = item, " +
                "    parts = item.version.Split('.', System.StringSplitOptions.None) " +
                "}).Select(this0 => new { " +
                "    majorWithDefault = this0.parts[0].ParseInt(), " + // will return default(int) in case of parsing failure
                "    majorWithCustomDefault = this0.parts[0].ParseInt(-1) " + // will return -1 in case of parsing failure
                "})";

            storeAllFields(FieldStorage.YES);
        }
    }
    //endregion

    //region indexes_4
    public static class Item {
        private String version;

        public String getVersion() {
            return version;
        }

        public void setVersion(String version) {
            this.version = version;
        }
    }
    //endregion
    
    private static class Employee {
        
    }

    public IndexingLinqExtensions() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region indexes_2
                List<Employee> results = session
                    .query(Employee.class, Employees_ByReversedFirstName.class)
                    .whereEquals("firstName", "treboR")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region indexes_5
                Item item1 = new Item();
                item1.setVersion("3.0.1");

                Item item2 = new Item();
                item2.setVersion("Unknown");

                session.store(item1);
                session.store(item2);

                session.saveChanges();

                List<Item_Parse.Result> results = session
                    .query(Item_Parse.Result.class, Item_Parse.class)
                    .toList();

                Assert.assertEquals(2, results.size());
                Assert.assertTrue(results.stream().anyMatch(x -> x.getMajorWithDefault() == 3));
                Assert.assertTrue(results.stream().anyMatch(x -> x.getMajorWithCustomDefault() == 3));
                Assert.assertTrue(results.stream().anyMatch(x -> x.getMajorWithDefault() == 0));
                Assert.assertTrue(results.stream().anyMatch(x -> x.getMajorWithCustomDefault() == -1));
                //endregion
            }
        }
    }
}
