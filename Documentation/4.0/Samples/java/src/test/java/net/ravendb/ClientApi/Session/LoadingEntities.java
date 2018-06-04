package net.ravendb.ClientApi.Session;

import net.ravendb.client.documents.CloseableIterator;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.commands.StreamResult;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.IRawDocumentQuery;
import net.ravendb.client.documents.session.StreamQueryStatistics;
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
                    .include("supplier")
                    .load(Product.class, "products/1");

                Supplier supplier = session.load(Supplier.class, product.getSupplier()); // this will not make server call
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region loading_entities_2_2
                // loading 'products/1'
                // including document found in 'supplier' property
                Product product = session
                    .include("supplier")
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
        }
    }
}
