package net.ravendb.clientapi.session.querying;

import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.commons.lang.time.DateUtils;

import net.ravendb.abstractions.basic.Reference;
import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.client.IDocumentQueryCustomization;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.RavenQueryStatistics;
import net.ravendb.client.document.DocumentQueryCustomizationFactory;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractTransformerCreationTask;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.QEmployee;


public class HowToCustomize {
  private class Employees_NoLastName extends AbstractTransformerCreationTask {
    // ...
  }

  private interface IFoo {
    //region customize_1_0
    public IDocumentQueryCustomization beforeQueryExecution(Action1<IndexQuery> action);
    //endregion

    //region customize_2_0
    public IDocumentQueryCustomization noCaching();
    //endregion

    //region customize_3_0
    public IDocumentQueryCustomization noTracking();
    //endregion

    //region customize_4_0
    public IDocumentQueryCustomization randomOrdering();

    public IDocumentQueryCustomization randomOrdering(String seed);
    //endregion

    //region customize_5_0
    public IDocumentQueryCustomization setAllowMultipleIndexEntriesForSameDocumentToResultTransformer(boolean val);
    //endregion

    //region customize_6_0
    public IDocumentQueryCustomization showTimings();
    //endregion

    //region customize_8_0
    public IDocumentQueryCustomization waitForNonStaleResults();

    public IDocumentQueryCustomization waitForNonStaleResults(long waitTimeout);
    //endregion

    //region customize_9_0
    public IDocumentQueryCustomization waitForNonStaleResultsAsOf(Date cutOff);

    public IDocumentQueryCustomization waitForNonStaleResultsAsOf(Date cutOff, long waitTimeout);

    public IDocumentQueryCustomization waitForNonStaleResultsAsOf(Etag cutOffEtag);

    public IDocumentQueryCustomization waitForNonStaleResultsAsOf(Etag cutOffEtag, long waitTimeout);
    //endregion

    //region customize_10_0
    public IDocumentQueryCustomization waitForNonStaleResultsAsOfLastWrite();

    public IDocumentQueryCustomization waitForNonStaleResultsAsOfLastWrite(long waitTimeout);
    //endregion

    //region customize_11_0
    public IDocumentQueryCustomization waitForNonStaleResultsAsOfNow();

    public IDocumentQueryCustomization waitForNonStaleResultsAsOfNow(long waitTimeout);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToCustomize() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region customize_1_1
        // set 'PageSize' to 10
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().beforeQueryExecution(new Action1<IndexQuery>() {
            @Override
            public void apply(IndexQuery query) {
              query.setPageSize(10);
            }
          }))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_2_1
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().noCaching())
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_3_1
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().noTracking())
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_4_1
        // results will be ordered randomly each time
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().randomOrdering())
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_5_1
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class)
          .customize(
            new DocumentQueryCustomizationFactory()
              .setAllowMultipleIndexEntriesForSameDocumentToResultTransformer(true)
            )
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_6_1
        QEmployee e = QEmployee.employee;
        Reference<RavenQueryStatistics> statsRef = new Reference<>();
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().showTimings())
          .statistics(statsRef)
          .where(e.firstName.eq("Robert"))
          .toList();

        Map<String, Double> timings = statsRef.value.getTimingsInMilliseconds();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_8_1
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().waitForNonStaleResults())
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_9_1
        // results will be considered non-stale
        // if last indexed document modified date
        // will be greater than 1 minute ago
        QEmployee e = QEmployee.employee;
        Date date = DateUtils.addMinutes(new Date(), -1);
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().waitForNonStaleResultsAsOf(date))
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_10_1
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().waitForNonStaleResultsAsOfLastWrite())
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region customize_11_1
        QEmployee e = QEmployee.employee;
        List<Employee> results = session
          .query(Employee.class)
          .customize(new DocumentQueryCustomizationFactory().waitForNonStaleResultsAsOfNow())
          .where(e.firstName.eq("Robert"))
          .toList();
        //endregion
      }
    }
  }
}
