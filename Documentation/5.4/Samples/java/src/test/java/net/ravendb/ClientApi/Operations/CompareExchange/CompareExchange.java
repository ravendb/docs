package net.ravendb.ClientApi.Operations;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.compareExchange.*;

import java.util.Map;

public class CompareExchange {

    private interface IFoo<T> {
        /*
        //region get_0
        GetCompareExchangeValueOperation(Class<T> clazz, String key);
        //endregion

        //region get_list_0
        GetCompareExchangeValuesOperation(Class<T> clazz, String[] keys);
        //endregion

        //region get_list_1
        GetCompareExchangeValuesOperation(Class<T> clazz, String startWith)
        GetCompareExchangeValuesOperation(Class<T> clazz, String startWith, Integer start)
        GetCompareExchangeValuesOperation(Class<T> clazz, String startWith, Integer start, Integer pageSize)
        //endregion

        //region put_0
        public PutCompareExchangeValueOperation(String key, T value, long index)
        //endregion

        //region delete_0
        public DeleteCompareExchangeValueOperation(Class<T> clazz, String key, long index)
        //endregion
        */
    }

    private static class Foo {
        //region compare_exchange_value
        public class CompareExchangeValue<T> {
            private String key;
            private long index;
            private T value;

            public CompareExchangeValue(String key, long index, T value) {
                this.key = key;
                this.index = index;
                this.value = value;
            }

            public String getKey() {
                return key;
            }

            public void setKey(String key) {
                this.key = key;
            }

            public long getIndex() {
                return index;
            }

            public void setIndex(long index) {
                this.index = index;
            }

            public T getValue() {
                return value;
            }

            public void setValue(T value) {
                this.value = value;
            }
        }
        //endregion

        //region compare_exchange_result
        public class CompareExchangeResult<T> {
            private T value;
            private long index;
            private boolean successful;

            public T getValue() {
                return value;
            }

            public void setValue(T value) {
                this.value = value;
            }

            public long getIndex() {
                return index;
            }

            public void setIndex(long index) {
                this.index = index;
            }

            public boolean isSuccessful() {
                return successful;
            }

            public void setSuccessful(boolean successful) {
                this.successful = successful;
            }
        }
        //endregion
    }

    private static class User {
        private int age;

        public int getAge() {
            return age;
        }

        public void setAge(int age) {
            this.age = age;
        }
    }

    public CompareExchange() {
        try (IDocumentStore store = new DocumentStore()) {
            {
                //region get_1
                CompareExchangeValue<Long> readResult
                    = store.operations().send(new GetCompareExchangeValueOperation<>(Long.class, "nextClientId"));

                Long value = readResult.getValue();
                //endregion
            }

            {
                //region get_2
                CompareExchangeValue<User> readResult
                    = store.operations().send(
                        new GetCompareExchangeValueOperation<>(User.class, "AdminUser"));

                User admin = readResult.getValue();
                //endregion
            }

            {
                //region get_list_2
                Map<String, CompareExchangeValue<String>> compareExchangeValues
                    = store.operations().send(
                        new GetCompareExchangeValuesOperation<>(String.class, new String[]{"Key-1", "Key-2"}));
                //endregion
            }

            {
                //region get_list_3
                // Get values for keys that have the common prefix 'users'
                // Retrieve maximum 20 entries
                Map<String, CompareExchangeValue<User>> compareExchangeValues
                    = store.operations().send(
                        new GetCompareExchangeValuesOperation<>(User.class, "users", 0, 20));

                //endregion
            }

            {
                //region put_1
                CompareExchangeResult<String> compareExchangeResult = store.operations().send(
                    new PutCompareExchangeValueOperation<>("Emails/foo@example.org", "users/123", 0));

                boolean successful = compareExchangeResult.isSuccessful();
                // If successful is true: then Key 'foo@example.org' now has the value of "users/123"
                //endregion
            }

            {
                //region put_2
                // Get existing value
                CompareExchangeValue<User> readResult
                    = store.operations().send(
                        new GetCompareExchangeValueOperation<>(User.class, "AdminUser"));

                readResult.getValue().setAge(readResult.getValue().getAge() + 1);

                // Update value
                CompareExchangeResult<User> saveResult
                    = store.operations().send(
                        new PutCompareExchangeValueOperation<>("AdminUser", readResult.getValue(), readResult.getIndex()));

                // The save result is successful only if 'index' wasn't changed between the read and write operations
                boolean saveResultSuccessful = saveResult.isSuccessful();
                //endregion
            }

            {
                //region delete_1
                // First, get existing value
                CompareExchangeValue<User> readResult
                    = store.operations().send(
                        new GetCompareExchangeValueOperation<>(User.class, "AdminUser"));

                // Delete the key - use the index received from the 'Get' operation
                CompareExchangeResult<User> deleteResult
                    = store.operations().send(
                        new DeleteCompareExchangeValueOperation<>(User.class, "AdminUser", readResult.getIndex()));

                // The delete result is successful only if the index has not changed between the read and delete operations
                boolean deleteResultSuccessful = deleteResult.isSuccessful();
                //endregion
            }
        }
    }

}
