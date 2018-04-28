package net.ravendb.ClientApi.Session.Querying;

import com.sun.org.apache.xpath.internal.operations.Or;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class HowToQueryWithExactMatch {


    private interface IFoo<T> {
        //region query_1_0
        IDocumentQuery<T> whereEquals(String fieldName, Object value, boolean exact);
        IDocumentQuery<T> whereNotEquals(String fieldName, Object value, boolean exact);

        // ... rest of where methods
        //endregion
    }

    public HowToQueryWithExactMatch() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region query_1_1
                // load all entities from 'Employees' collection
                // where firstName equals 'Robert' (case sensitive match)
                List<Employee> employees = session.query(Employee.class)
                    .whereEquals("firstName", "Robert", true)
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region query_2_1
                // return all entities from 'Orders' collection
                // which contain at least one order line with
                // 'Singaporean Hokkien Fried Mee' product
                // perform a case-sensitive match
                List<Order> orders = session.query(Order.class)
                    .whereEquals("lines[].productName", "Singaporean Hokkien Fried Mee", true)
                    .toList();
                //endregion
            }
        }
    }

    private static class Employee {
    }

    private static class Order {

    }
}
