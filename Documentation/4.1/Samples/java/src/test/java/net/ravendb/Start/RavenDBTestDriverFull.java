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

//region test_full_example
public class RavenDBTestDriverFull extends RavenTestDriver {

    //This allows us to modify the conventions of the store we get from 'getDocumentStore'
    @Override
    protected void preInitialize(IDocumentStore documentStore) {
        documentStore.getConventions().setMaxNumberOfRequestsPerSession(50);
    }

    @Test
    public void myFirstTest() {
        ServerOptions serverOptions = new ServerOptions();
        serverOptions.setDataDirectory("C:\\RavenDBTestDir");
        configureServer(serverOptions);

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
//endregion
