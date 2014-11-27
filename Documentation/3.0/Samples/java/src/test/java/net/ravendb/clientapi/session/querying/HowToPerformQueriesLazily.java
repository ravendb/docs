package net.ravendb.clientapi.session.querying;

import java.util.List;
import java.util.Map;

import net.ravendb.abstractions.basic.Lazy;
import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.Facet;
import net.ravendb.abstractions.data.FacetResult;
import net.ravendb.abstractions.data.FacetResults;
import net.ravendb.abstractions.data.SuggestionQuery;
import net.ravendb.abstractions.data.SuggestionQueryResult;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.Camera;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.QEmployee;


public class HowToPerformQueriesLazily {

  @SuppressWarnings("unused")
  private interface IFoo<T> {
    //region lazy_1
    public Lazy<List<T>> lazily();

    public Lazy<List<T>> lazily(Action1<List<T>> onEval);
    //endregion

    //region lazy_4
    public Lazy<Integer> countLazily();
    //endregion

    //region lazy_6
    public Lazy<SuggestionQueryResult> suggestLazy();

    public Lazy<SuggestionQueryResult> suggestLazy(SuggestionQuery query);
    //endregion

    //region lazy_8
    public Lazy<FacetResults> toFacetsLazy(List<Facet> facets);

    public Lazy<FacetResults> toFacetsLazy(List<Facet> facets, int start);

    public Lazy<FacetResults> toFacetsLazy(List<Facet> facets, int start, Integer pageSize);

    public Lazy<FacetResults> toFacetsLazy(String facetSetupDoc);

    public Lazy<FacetResults> toFacetsLazy(String facetSetupDoc, int start);

    public Lazy<FacetResults> toFacetsLazy(String facetSetupDoc, int start, Integer pageSize);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToPerformQueriesLazily() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region lazy_2
        QEmployee e = QEmployee.employee;
        Lazy<List<Employee>> employeesLazy = session
          .query(Employee.class)
          .where(e.firstName.eq("Robert"))
          .lazily();

        List<Employee> employees = employeesLazy.getValue(); // query will be executed here
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region lazy_5
        QEmployee e = QEmployee.employee;
        Lazy<Integer> countLazy = session
          .query(Employee.class)
          .where(e.firstName.eq("Robert"))
          .countLazily();

        Integer count = countLazy.getValue(); // query will be executed here
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region lazy_7
        Lazy<SuggestionQueryResult> suggestLazy = session
          .query(Employee.class)
          .suggestLazy();

        SuggestionQueryResult suggest = suggestLazy.getValue(); // query will be executed here
        String[] suggestions = suggest.getSuggestions();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region lazy_9
        QEmployee e = QEmployee.employee;
        Lazy<FacetResults> facetsLazy = session
          .query(Camera.class, "Camera/Costs")
          .toFacetsLazy("facets/CameraFacets");

        FacetResults facets = facetsLazy.getValue(); // query will be executed here
        Map<String, FacetResult> results = facets.getResults();
        //endregion
      }
    }
  }
}
