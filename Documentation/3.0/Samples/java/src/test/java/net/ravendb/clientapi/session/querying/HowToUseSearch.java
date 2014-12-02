package net.ravendb.clientapi.session.querying;

import java.util.List;

import net.ravendb.client.EscapeQueryOptions;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.SearchOptionsSet;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.linq.IRavenQueryable;

import com.mysema.query.annotations.QueryEntity;
import com.mysema.query.types.Path;


public class HowToUseSearch {
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

  @SuppressWarnings("unused")
  private interface IFoo<T> {
    //region search_1
    public IRavenQueryable<T> search(Path<?> fieldSelector, String searchTerms);

    public IRavenQueryable<T> search(Path<?> fieldSelector, String searchTerms, double boost);

    public IRavenQueryable<T> search(Path<?> fieldSelector, String searchTerms, double boost, SearchOptionsSet searchOptions);

    public IRavenQueryable<T> search(Path<?> fieldSelector, String searchTerms, double boost, SearchOptionsSet searchOptions, EscapeQueryOptions escapeQueryOptions);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToUseSearch() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region search_2
        QHowToUseSearch_User u = QHowToUseSearch_User.user;
        List<User> users = session
          .query(User.class, "Users/ByNameAndHobbies")
          .search(u.name, "Adam")
          .search(u.hobbies, "sport")
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region search_3
        QHowToUseSearch_User u = QHowToUseSearch_User.user;
        List<User> users = session
          .query(User.class, "Users/ByNameAndHobbies")
          .search(u.hobbies, "I love sport", 10)
          .search(u.hobbies, "but also like reading books", 5)
          .toList();
        //endregion
      }
    }
  }
}
