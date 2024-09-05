package net.ravendb.Start;

import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.FieldIndexing;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.embedded.ServerOptions;
import net.ravendb.test.driver.RavenTestDriver;
import org.junit.Assert;
import org.junit.Test;

import java.util.List;

public class RavenDBTestDriver extends RavenTestDriver {

    //region test_driver_PreInitialize
    //This allows us to modify the conventions of the store we get from 'getDocumentStore'
    @Override
    protected void preInitialize(IDocumentStore documentStore) {
        documentStore.getConventions().setMaxNumberOfRequestsPerSession(50);
    }
    //endregion

    //region test_driver_MyFirstTest
    @Test
    public void myFirstTest() {
        try (IDocumentStore store = getDocumentStore()) {
            store.executeIndex(new TestDocumentByName());

            try (IDocumentSession session = store.openSession()) {
                TestDocument testDocument1 = new TestDocument();
                testDocument1.setName("Hello world!");
                session.store(testDocument1);

                TestDocument testDocument2 = new TestDocument();
                testDocument2.setName("Goodbye...");
                session.store(testDocument2);

                session.saveChanges();
            }

            waitForIndexing(store); //If we want to query documents sometime we need to wait for the indexes to catch up
            waitForUserToContinueTheTest(store); //Sometimes we want to debug the test itself, this redirect us to the studio

            try (IDocumentSession session = store.openSession()) {
                List<TestDocument> query = session.query(TestDocument.class, TestDocumentByName.class)
                    .whereEquals("name", "hello")
                    .toList();

                Assert.assertEquals(1, query.size());
            }
        }
    }
    //endregion

    @Test
    public void testDriverConfigureServer() {
        //region test_driver_ConfigureServer
        ServerOptions testServerOptions = new ServerOptions();

        // specify where ravendb server should be extracted (optional)
        testServerOptions.setTargetServerLocation("PATH_TO_TEMPORARY_SERVER_LOCATION");

        // Specify where ravendb data will be placed/located (optional)
        testServerOptions.setDataDirectory("PATH_TO_RAVENDB_DATADIR");
        //endregion
    }

    public static class TestDocumentByName extends AbstractIndexCreationTask {
        public TestDocumentByName() {
            map = "from doc in docs select new { doc.name }";
            index("name", FieldIndexing.SEARCH);
        }
    }

    public static class TestDocument {
        private String name;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
}
