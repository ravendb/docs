package net.ravendb.start;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.test.driver.RavenServerLocator;
import net.ravendb.client.test.driver.RavenTestDriver;
import org.junit.Assert;
import org.junit.Test;

import java.util.List;

public class RavenDBTestDriverFull {
    //region test_driver_1

    public class MyRavenDBTestDriver extends RavenTestDriver {
        public MyRavenDBTestDriver(RavenServerLocator locator, RavenServerLocator securedLocator) {
            super(new MyRavenDBLocator(), null);
        }

        //This allows us to modify the conventions of the store we get from 'getDocumentStore'
        @Override
        protected void customizeStore(DocumentStore store) {
            store.getConventions().setMaxNumberOfRequestsPerSession(50);
        }

        @Test
        public void myFirstTest() throws Exception {
            try (IDocumentStore store = getDocumentStore()) {
                store.executeIndex(new RavenDBTestDriver.TestDocumentByName());

                try (IDocumentSession session = store.openSession()) {
                    TestDocument document1 = new TestDocument();
                    document1.setName("Hello world!");

                    TestDocument document2 = new TestDocument();
                    document2.setName("Goodbye...");

                    session.store(document1);
                    session.store(document2);
                    session.saveChanges();
                }

                waitForIndexing(store);  //If we want to query documents sometime we need to wait for the indexes to catch up
                waitForUserToContinueTheTest(store); //Sometimes we want to debug the test itself, this redirect us to the studio

                try (IDocumentSession session = store.openSession()) {
                    List<RavenDBTestDriver.TestDocument> query = session
                            .query(
                                RavenDBTestDriver.TestDocument.class,
                                RavenDBTestDriver.TestDocumentByName.class
                            )
                        .whereEquals("name", "hello")
                        .toList();

                    Assert.assertEquals(1, query.size());
                }
            }
        }
    }

    public class MyRavenDBLocator extends RavenServerLocator {
        @Override
        public String getServerPath() {
            return "/opt/RavenDB/Server/Raven.Server";
        }

        @Override
        public String getCommand() {
            return getServerPath();
        }
    }

    public static class TestDocumentByName extends AbstractIndexCreationTask {
        public TestDocumentByName() {
            map = "from doc in docs select new { doc.name }";
            index("name", FieldIndexing.SEARCH);
        }
    }

    public class TestDocument {
        private String name;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
    //endregion
}
