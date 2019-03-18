package net.ravendb.Server;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.compareExchange.CompareExchangeResult;
import net.ravendb.client.documents.operations.compareExchange.DeleteCompareExchangeValueOperation;
import net.ravendb.client.documents.operations.compareExchange.PutCompareExchangeValueOperation;
import net.ravendb.client.documents.session.CmpXchg;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.IRawDocumentQuery;

import java.time.Duration;
import java.time.LocalDateTime;
import java.util.Date;
import java.util.List;

public class CompareExchange {
    private DocumentStore store;
    private static class User {
        private String email;
        private String id;
        private int age;

        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public int getAge() {
            return age;
        }

        public void setAge(int age) {
            this.age = age;
        }
    }

    public void sample() {
        try (IDocumentStore store = new DocumentStore()) {
            //region email
            String email = "user@example.com";

            User user = new User();
            user.setEmail(email);

            try (IDocumentSession session = store.openSession()) {
                session.store(user);

                // At this point, the user document has an Id assigned

                // Try to reserve a new user email
                // Note: This operation takes place outside of the session transaction,
                //       It is a cluster-wide reservation
                CompareExchangeResult<String> cmpXchgResult = store
                    .operations().send(
                        new PutCompareExchangeValueOperation<>(
                            "emails/" + email, user.getId(), 0));

                if (!cmpXchgResult.isSuccessful()) {
                    throw new RuntimeException("Email is already in use");
                }

                // At this point we managed to reserve/save the user email -
                // The document can be saved in SaveChanges
                session.saveChanges();
            }
            //endregion

            //region query_cmpxchg
            try (IDocumentSession session = store.openSession()) {
                List<User> query = session.advanced().rawQuery(User.class,
                    "from Users as s where id() == cmpxchg(\"emails/ayende@ayende.com\")")
                    .toList();

                IDocumentQuery<User> q = session.advanced()
                    .documentQuery(User.class)
                    .whereEquals("id", CmpXchg.value("emails/ayende@ayende.com"));
            }
            //endregion
        }
    }

    //region shared_resource
    private class SharedResource {
        private LocalDateTime reservedUntil;

        public LocalDateTime getReservedUntil() {
            return reservedUntil;
        }

        public void setReservedUntil(LocalDateTime reservedUntil) {
            this.reservedUntil = reservedUntil;
        }
    }

    public void printWork() throws InterruptedException {
        // Try to get hold of the printer resource
        long reservationIndex = lockResource(store, "Printer/First-Floor", Duration.ofMinutes(20));

        try {
            // Do some work for the duration that was set.
            // Don't exceed the duration, otherwise resource is available for someone else.
        }
        finally {
            releaseResource(store, "Printer/First-Floor", reservationIndex);
        }
    }

    public long lockResource(IDocumentStore store, String resourceName, Duration duration) throws InterruptedException {
        while (true) {
            LocalDateTime now = LocalDateTime.now();

            SharedResource resource = new SharedResource();
            resource.setReservedUntil(now.plus(duration));

            CompareExchangeResult<SharedResource> saveResult =
                store.operations().send(
                    new PutCompareExchangeValueOperation<SharedResource>(resourceName, resource, 0));

            if (saveResult.isSuccessful()) {
                // resourceName wasn't present - we managed to reserve
                return saveResult.getIndex();
            }

            // At this point, Put operation failed - someone else owns the lock or lock time expired
            if (saveResult.getValue().reservedUntil.isBefore(now)) {
                // Time expired - Update the existing key with the new value
                CompareExchangeResult<SharedResource> takeLockWithTimeoutResult =
                    store.operations().send(
                        new PutCompareExchangeValueOperation<>(resourceName, resource, saveResult.getIndex()));

                if (takeLockWithTimeoutResult.isSuccessful()) {
                    return takeLockWithTimeoutResult.getIndex();
                }
            }

            // Wait a little bit and retry
            Thread.sleep(20);
        }
    }

    public void releaseResource(IDocumentStore store, String resourceName, long index) {
        CompareExchangeResult<SharedResource> deleteResult = store
            .operations().send(
                new DeleteCompareExchangeValueOperation<>(SharedResource.class, resourceName, index));

        // We have 2 options here:
        // deleteResult.Successful is true - we managed to release resource
        // deleteResult.Successful is false - someone else took the lock due to timeout
    }
    //endregion
}
