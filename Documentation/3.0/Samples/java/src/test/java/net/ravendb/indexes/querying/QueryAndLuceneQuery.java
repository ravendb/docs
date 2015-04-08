package net.ravendb.indexes.querying;

import com.mysema.query.annotations.QueryEntity;

import net.ravendb.abstractions.data.QueryOperator;
import net.ravendb.client.IDocumentQuery;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.linq.IRavenQueryable;


public class QueryAndLuceneQuery {
  @QueryEntity
  public static class User {
    private String id;
    private String name;
    private String eyeColor;
    private byte age;

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
    public String getEyeColor() {
      return eyeColor;
    }
    public void setEyeColor(String eyeColor) {
      this.eyeColor = eyeColor;
    }
    public byte getAge() {
      return age;
    }
    public void setAge(byte age) {
      this.age = age;
    }
  }

  @SuppressWarnings({"unused", "boxing"})
  public QueryAndLuceneQuery() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region immutable_query
        QQueryAndLuceneQuery_User u = QQueryAndLuceneQuery_User.user;
        IRavenQueryable<User> query = session
          .query(User.class)
          .where(u.name.startsWith("A"));

        IRavenQueryable<User> ageQuery = query.where(u.age.gt(21));

        IRavenQueryable<User> eyeColor = query.where(u.eyeColor.eq("blue"));
        //endregion

        //region mutable_lucene_query
        IDocumentQuery<User> documentQuery = session
          .advanced()
          .documentQuery(User.class)
          .whereStartsWith(u.name, "A");

        IDocumentQuery<User> ageDocumentQuery = documentQuery.whereGreaterThan(u.age, (byte)21);

        IDocumentQuery<User> eyeDocumentQuery = documentQuery.whereEquals(u.eyeColor, "blue");

        // here all of the DocumentQuery variables are the same references
        //endregion

        //region default_operator
        session
          .advanced()
          .documentQuery(User.class)
          .usingDefaultOperator(QueryOperator.AND);
        //endregion
      }
    }
  }
}
