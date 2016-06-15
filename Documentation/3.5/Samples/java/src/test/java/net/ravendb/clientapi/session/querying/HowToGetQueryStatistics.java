package net.ravendb.clientapi.session.querying;

import java.util.List;

import net.ravendb.abstractions.basic.Reference;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.RavenQueryStatistics;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.linq.IRavenQueryable;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.QEmployee;


public class HowToGetQueryStatistics {
  @SuppressWarnings("unused")
  private interface IFoo<TResult> {
    //region stats_1
    IRavenQueryable<TResult> statistics(Reference<RavenQueryStatistics> stats);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToGetQueryStatistics() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region stats_2
        Reference<RavenQueryStatistics> statsRef = new Reference<>();
        QEmployee e = QEmployee.employee;
        List<Employee> employees = session
          .query(Employee.class)
          .where(e.firstName.eq("Robert"))
          .statistics(statsRef)
          .toList();

        int totalResults = statsRef.value.getTotalResults();
        long durationMiliseconds = statsRef.value.getDurationMiliseconds();
        //endregion
      }
    }
  }
}
