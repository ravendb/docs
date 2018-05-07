package net.ravendb.ClientApi.Session.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.queries.Query;
import net.ravendb.client.documents.queries.SearchOperator;
import net.ravendb.client.documents.session.IDocumentQuery;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

import static net.ravendb.client.documents.queries.Query.index;

public class HowToUseSearch {

    public interface IFoo<T> {
        //region search_1
        IDocumentQuery<T> search(String fieldName, String searchTerms);
        IDocumentQuery<T> search(String fieldName, String searchTerms, SearchOperator operator);
        IDocumentQuery<T> boost(double boost);
        //endregion
    }

    private static class User {
        private String id;
        private String name;
        private byte age;
        private List<String> hobbies;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public byte getAge() {
            return age;
        }

        public void setAge(byte age) {
            this.age = age;
        }

        public List<String> getHobbies() {
            return hobbies;
        }

        public void setHobbies(List<String> hobbies) {
            this.hobbies = hobbies;
        }
    }

    public void examples() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region search_4
                List<User> users = session
                    .query(User.class)
                    .search("name", "a*")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_2
                List<User> users = session.query(User.class, Users_ByNameAndHobbies.class)
                    .search("name", "Adam")
                    .search("hobbies", "sport")
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region search_3
                List<User> users = session
                    .query(User.class, index("Users/ByHobbies"))
                    .search("hobbies", "I love sport")
                    .boost(10)
                    .search("hobbies", "but also like reading books")
                    .boost(5)
                    .toList();
                //endregion
            }
        }
    }

    private static class Users_ByNameAndHobbies extends AbstractIndexCreationTask {

    }
}
