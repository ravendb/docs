package net.ravendb.start;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.test.driver.RavenServerLocator;
import net.ravendb.client.test.driver.RavenTestDriver;
import org.junit.Assert;
import org.junit.Test;

import java.util.List;

public class RavenDBTestDriver {

    private class Foo {
        //region RavenServerLocator
        public abstract class RavenServerLocator {
            /**
             * Allows to fetch the server path
             * @return server path
             */
            public abstract String getServerPath();

            /**
             * Allows to fetch the command used to invoke the server.
             * @return command
             */
            public abstract String getCommand();

            /**
             * Allows to fetch the command arguments.
             * @return command line arguments
             */
            public String[] getCommandArguments() {
                return new String[0];
            }
        }
        //endregion
    }

    public class MyRavenDBTestDriver extends RavenTestDriver {
        public MyRavenDBTestDriver(RavenServerLocator locator, RavenServerLocator securedLocator) {
            super(new MyRavenDBLocator(), null);
        }

        //region test_driver_3
        //This allows us to modify the conventions of the store we get from 'getDocumentStore'
        @Override
        protected void customizeStore(DocumentStore store) {
            store.getConventions().setMaxNumberOfRequestsPerSession(50);
        }
        //endregion

        //region test_driver_4
        @Test
        public void myFirstTest() throws Exception {
            try (IDocumentStore store = getDocumentStore()) {
                store.executeIndex(new TestDocumentByName());

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
                    List<TestDocument> query = session.query(TestDocument.class, TestDocumentByName.class)
                        .whereEquals("name", "hello")
                        .toList();

                    Assert.assertEquals(1, query.size());
                }
            }
        }
        //endregion

    }

    //region test_driver_5
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
    //endregion

    public static class TestDocumentByName extends AbstractIndexCreationTask {

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
}
