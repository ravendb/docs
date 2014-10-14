package net.ravendb.clientapi.session;

import static org.junit.Assert.assertSame;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Company;


public class WhatIsSession {
  public WhatIsSession() throws Exception {

    try (IDocumentStore store = new DocumentStore()) {
      //region session_usage_1
      String companyId;
      try (IDocumentSession session = store.openSession()) {
        Company entity = new Company();
        entity.setName("Company");
        session.store(entity);
        session.saveChanges();
        companyId = entity.getId();
      }

      try (IDocumentSession session = store.openSession()) {
        Company entity = session.load(Company.class, companyId);
        System.out.println(entity.getName());
      }
      //endregion

      //region session_usage_2
      try (IDocumentSession session = store.openSession()) {
        Company entity = session.load(Company.class, companyId);
        entity.setName("Another Company");
        session.saveChanges(); // will send the change to the database
      }
      //endregion

      try (IDocumentSession session = store.openSession()) {
        //region session_usage_3
        assertSame(session.load(Company.class, companyId), session.load(Company.class, companyId));
        //endregion
      }
    }
  }
}
