import net.ravendb.client.documents.CloseableIterator;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.StreamResult;
import net.ravendb.client.documents.session.*;
import net.ravendb.client.documents.session.loaders.ILoaderWithInclude;
import net.ravendb.client.primitives.Reference;

import java.io.ByteArrayOutputStream;
import java.util.Collection;
import java.util.Map;

public class LoadingEntities {
    private interface IFoo {
        //region loading_entities_1_0
        <T> T load(Class<T> clazz, String id);
        //endregion

        //region loading_entities_2_0
        ILoaderWithInclude include(String path);

        <TResult> Map<String, TResult> load(Class<TResult> clazz, String... ids);

        <TResult> Map<String, TResult> load(Class<TResult> clazz, Collection<String> ids);

        <TResult> TResult load(Class<TResult> clazz, String id);
        //endregion

        //region loading_entities_3_0
        <TResult> Map<String, TResult> load(Class<TResult> clazz, String... ids);

        <TResult> Map<String, TResult> load(Class<TResult> clazz, Collection<String> ids);
        //endregion

        //region loading_entities_4_0
        <T> T[] loadStartingWith(Class<T> clazz, String idPrefix);

        <T> T[] loadStartingWith(Class<T> clazz, String idPrefix, String matches);

        <T> T[] loadStartingWith(Class<T> clazz, String idPrefix, String matches, int start);

        <T> T[] loadStartingWith(Class<T> clazz, String idPrefix, String matches, int start, int pageSize);

        <T> T[] loadStartingWith(Class<T> clazz, String idPrefix, String matches, int start, int pageSize, String exclude);

        <T> T[] loadStartingWith(Class<T> clazz, String idPrefix, String matches, int start, int pageSize, String exclude, String startAfter);
        //endregion

        //region loading_entities_5_0
        <T> CloseableIterator<StreamResult<T>> stream(IDocumentQuery<T> query);

        <T> CloseableIterator<StreamResult<T>> stream(IDocumentQuery<T> query, Reference<StreamQueryStatistics> streamQueryStats);

        <T> CloseableIterator<StreamResult<T>> stream(IRawDocumentQuery<T> query);

        <T> CloseableIterator<StreamResult<T>> stream(IRawDocumentQuery<T> query, Reference<StreamQueryStatistics> streamQueryStats);

        <T> CloseableIterator<StreamResult<T>> stream(Class<T> clazz, String startsWith);

        <T> CloseableIterator<StreamResult<T>> stream(Class<T> clazz, String startsWith, String matches);

        <T> CloseableIterator<StreamResult<T>> stream(Class<T> clazz, String startsWith, String matches, int start);

        <T> CloseableIterator<StreamResult<T>> stream(Class<T> clazz, String startsWith, String matches, int start, int pageSize);

        <T> CloseableIterator<StreamResult<T>> stream(Class<T> clazz, String startsWith, String matches, int start, int pageSize, String startAfter);
        //endregion

        //region loading_entities_6_0
        boolean isLoaded(String id);
        //endregion

        //region loading_entities_7_0
        <T> ConditionalLoadResult<T> conditionalLoad(Class<T> clazz, String id, String changeVector)
        //endregion
    }

    private static class Employee {

    }
    private static class Supplier {

    }
    private static class Product {
        private String supplier;

        public String getSupplier() {
            return supplier;
        }

        public void setSupplier(String supplier) {
            this.supplier = supplier;
        }
    }

    public LoadingEntities() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_1_1
                Employee employee = session.load(Employee.class, "employees/1");
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_2_1
                // loading 'products/1'
                // including document found in 'supplier' property
                Product product = session
                    .include("Supplier")
                    .load(Product.class, "products/1");

                Supplier supplier = session.load(Supplier.class, product.getSupplier()); // this will not make server call
                //endregion
                Product product1 = session.load()


            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_2_2
                // loading 'products/1'
                // including document found in 'supplier' property
                Product product = session
                    .include("Supplier")
                    .load(Product.class, "products/1");

                Supplier supplier = session.load(Supplier.class, product.getSupplier()); // this will not make server call
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_3_1
                Map<String, Employee> employees
                    = session.load(Employee.class,
                    "employees/1", "employees/2", "employees/3");
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_4_1
                // return up to 128 entities with Id that starts with 'employees'
                Employee[] result = session
                    .advanced()
                    .loadStartingWith(Employee.class, "employees/", null, 0, 128);
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_4_2
                // return up to 128 entities with Id that starts with 'employees/'
                // and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
                Employee[] result = session
                    .advanced()
                    .loadStartingWith(Employee.class, "employees/", "1*|2*", 0, 128);
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_5_1
                try (CloseableIterator<StreamResult<Employee>> iterator =
                         session.advanced().stream(Employee.class, "employees/")) {
                    while (iterator.hasNext()) {
                        StreamResult<Employee> employee = iterator.next();
                    }
                }
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_5_2
                ByteArrayOutputStream baos = new ByteArrayOutputStream();
                session
                    .advanced()
                    .loadStartingWithIntoStream("employees/", baos);
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_6_1
                boolean isLoaded = session.advanced().isLoaded("employees/1");//false
                Employee employee = session.load(Employee.class, "employees/1");
                isLoaded = session.advanced().isLoaded("employees/1"); // true
                //endregion
            }

            User user;
            string changeVector;
            //region loading_entities_7_1
            try (IDocumentSession session = store.openSession()) {

                string changeVector;
                User user = new User("Bob");

                session.store(User, "users/1");
                session.saveChanges();

                changeVector = session.advanced().getChangeVectorFor(User user);
            }

            try (IDocumentSession session = store.openSession()) {
                // New session which does not track our User entity

                    // The given change vector matches
                    // the server-side change vector
                    // Does not load the document
                    ConditionalLoadResult<T> result1 = session.advanced()
                     .conditionalLoad(User.class,"users/1", changeVector);

                    // Modify the document
                    user.setName = "Bob Smith";
                    session.store(user);
                    session.saveChanges();

                    // Change vectors do not match
                    // Loads the document
                    ConditionalLoadResult<T> result2 = session.advanced().conditionalLoad(
                        User.class, "users/1", changeVector);
                    }

                //endregion
            }
        }
    }
}
