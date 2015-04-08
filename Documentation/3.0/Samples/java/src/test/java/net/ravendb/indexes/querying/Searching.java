package net.ravendb.indexes.querying;

import java.util.List;

import com.mysema.query.annotations.QueryEntity;

import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.abstractions.data.QueryResult;
import net.ravendb.abstractions.indexing.FieldIndexing;
import net.ravendb.client.EscapeQueryOptions;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.SearchOptions;
import net.ravendb.client.SearchOptionsSet;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;


public class Searching {

  //region search_1_3
  public static class Users_ByName extends AbstractIndexCreationTask {
    public Users_ByName() {
      QSearching_User u = QSearching_User.user;
      map =
       " from user in docs.Users " +
       " select new              " +
       " {                       " +
       "     user.Name           " +
       " }; ";

      index(u.name, FieldIndexing.ANALYZED);
    }
  }
  //endregion

  //region search_4_3
  public static class Users_ByHobbies extends AbstractIndexCreationTask {
    public Users_ByHobbies() {
      QSearching_User u = QSearching_User.user;
      map =
       " from user in docs.Users " +
       " select new              " +
       " {                       " +
       "     user.Hobbies        " +
       " }; ";

      index(u.hobbies, FieldIndexing.ANALYZED);
    }
  }
  //endregion

  //region search_5_3
  public static class Users_ByNameAndHobbies extends AbstractIndexCreationTask {
    public Users_ByNameAndHobbies() {
      QSearching_User u = QSearching_User.user;
      map =
       " from user in docs.Users " +
       " select new              " +
       " {                       " +
       "     user.Name,          " +
       "     user.Hobbies        " +
       " }; ";

      index(u.name, FieldIndexing.ANALYZED);
      index(u.hobbies, FieldIndexing.ANALYZED);
    }
  }
  //endregion

  //region search_7_3
  public static class Users_ByNameAgeAndHobbies extends AbstractIndexCreationTask {
    public Users_ByNameAgeAndHobbies() {
      QSearching_User u = QSearching_User.user;
      map =
        " from user in docs.Users " +
        " select new              " +
        " {                       " +
        "     user.Name,          " +
        "     user.Age,           " +
        "     user.Hobbies        " +
        " }; ";

      index(u.name, FieldIndexing.ANALYZED);
      index(u.hobbies, FieldIndexing.ANALYZED);
    }
  }
  //endregion

  //region linq_extensions_search_user_class
  @QueryEntity
  public static class User {
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
  //endregion

  @SuppressWarnings({"unused", "boxing"})
  public Searching() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_1_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByName.class)
          .where(u.name.eq("John"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_1_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByName.class)
          .whereEquals(u.name, "John")
          .toList();
        //endregion
      }

      //region search_1_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByName",
          new IndexQuery("Name:John"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_2_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByName.class)
          .where(u.name.startsWith("Jo"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_2_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByName.class)
          .whereStartsWith(u.name, "Jo")
          .toList();
        //endregion
      }

      //region search_2_2
      store
        .getDatabaseCommands()
        .query("Users/ByName",
          new IndexQuery("Name:Jo*"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_3_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByName.class)
          .search(u.name, "John Adam")
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_3_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByName.class)
          .search(u.name, "John Adam")
          .toList();
        //endregion
      }

      //region search_3_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByName",
          new IndexQuery("Name:(John Adam)"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_4_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByHobbies.class)
          .search(u.hobbies, "looking for someone who likes sport books computers")
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_4_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByHobbies.class)
          .search(u.hobbies, "looking for someone who likes sport books computers")
          .toList();
        //endregion
      }

      //region search_4_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByHobbies",
          new IndexQuery("Name:(looking for someone who likes sport books computers)"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_5_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByNameAndHobbies.class)
          .search(u.name, "Adam")
          .search(u.hobbies, "sport")
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_5_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByNameAndHobbies.class)
          .search(u.name, "Adam")
          .search(u.hobbies, "sport")
          .toList();
        //endregion
      }

      //region search_5_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByNameAndHobbies",
          new IndexQuery("Name:(Adam) OR Hobbies:(sport)"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_6_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByHobbies.class)
          .search(u.hobbies, "I love sport", 10)
          .search(u.hobbies, "but also like reading books", 5)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_6_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByHobbies.class)
          .search(u.hobbies, "I love sport")
          .boost(10.0)
          .search(u.hobbies, "but also like reading books")
          .boost(5.0)
          .toList();
        //endregion
      }

      //region search_6_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByHobbies",
          new IndexQuery("Hobbies:(I love sport)^10 OR Hobbies:(but also like reading books)^5"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_7_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByNameAgeAndHobbies.class)
          .search(u.hobbies, "computer")
          .search(u.name, "James")
          .where(u.age.eq((byte)20))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_7_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByNameAgeAndHobbies.class)
          .search(u.hobbies, "computer")
          .search(u.name, "James")
          .whereEquals(u.age, (byte)20)
          .toList();
        //endregion
      }

      //region search_7_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByNameAgeAndHobbies",
          new IndexQuery("(Hobbies:(computers) OR Name:(James)) AND Age:20"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_8_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByNameAndHobbies.class)
          .search(u.name, "Adam")
          .search(u.hobbies, "sport", SearchOptionsSet.of(SearchOptions.AND))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_8_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByNameAndHobbies.class)
          .search(u.name, "Adam")
          .andAlso()
          .search(u.hobbies, "sport")
          .toList();
        //endregion
      }

      //region search_8_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByNameAndHobbies",
          new IndexQuery("Name:(Adam) AND Hobbies:(sport)"));
      //endregion
    }


    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_9_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByName.class)
          .search(u.name, "James", SearchOptionsSet.of(SearchOptions.NOT))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_9_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByName.class)
          .not()
          .search(u.name, "James")
          .toList();
        //endregion
      }

      //region search_9_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByName",
          new IndexQuery("-Name:(James)"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_10_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByNameAndHobbies.class)
          .search(u.name, "Adam")
          .search(u.hobbies, "sport", SearchOptionsSet.of(SearchOptions.NOT, SearchOptions.AND))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_10_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByNameAndHobbies.class)
          .search(u.name, "Adam")
          .andAlso()
          .not()
          .search(u.hobbies, "sport")
          .toList();
        //endregion
      }

      //region search_10_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByName",
          new IndexQuery("Name:(Adam) AND -Hobbies:(sport)"));
      //endregion
    }


    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_11_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByName.class)
          .search(u.name, "Jo* Ad*", EscapeQueryOptions.ALLOW_POSTFIX_WILDCARD)
          .toList();
       //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_11_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByName.class)
          .search(u.name, "Jo* Ad*", EscapeQueryOptions.ALLOW_POSTFIX_WILDCARD)
          .toList();
       //endregion
      }

      //region search_11_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByName",
          new IndexQuery("Name:(Jo* Ad*)"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_12_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByName.class)
          .search(u.name, "*oh* *da*", EscapeQueryOptions.ALLOW_ALL_WILDCARDS)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_12_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByName.class)
          .search(u.name, "*oh* *da*", EscapeQueryOptions.ALLOW_ALL_WILDCARDS)
          .toList();
        //endregion
      }

      //region search_12_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByName",
          new IndexQuery("Name:(*oh* *da*)"));
      //endregion
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_13_0
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .query(User.class, Users_ByName.class)
          .search(u.name, "*J?n*", EscapeQueryOptions.RAW_QUERY)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_13_1
        QSearching_User u = QSearching_User.user;
        List<User> users = session
          .advanced()
          .documentQuery(User.class, Users_ByName.class)
          .search(u.name, "*J?n*", EscapeQueryOptions.RAW_QUERY)
          .toList();
        //endregion
      }

      //region search_13_2
      QueryResult result = store
        .getDatabaseCommands()
        .query("Users/ByName",
          new IndexQuery("Name:(*J?n*)"));
      //endregion
    }

  }
}
