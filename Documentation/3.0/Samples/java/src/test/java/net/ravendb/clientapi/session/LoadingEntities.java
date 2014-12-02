package net.ravendb.clientapi.session;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.UUID;

import com.mysema.query.types.Path;

import net.ravendb.abstractions.basic.CloseableIterator;
import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.data.StreamResult;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.ILoadConfiguration;
import net.ravendb.client.LoadConfigurationFactory;
import net.ravendb.client.RavenPagingInformation;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.document.ILoaderWithInclude;
import net.ravendb.client.indexes.AbstractTransformerCreationTask;
import net.ravendb.samples.northwind.Address;
import net.ravendb.samples.northwind.Employee;
import net.ravendb.samples.northwind.Product;
import net.ravendb.samples.northwind.QProduct;


public class LoadingEntities {

  public static class Employees_NoLastName extends AbstractTransformerCreationTask {
    // ...
  }

  @SuppressWarnings("unused")
  private interface IFoo {
    //region loading_entities_1_0
    public <T> T load(Class<T> clazz, String id);

    public <T> T load(Class<T> clazz, Number id);

    public <T> T load(Class<T> clazz, UUID id);

    public <TResult> TResult load(Class<TResult> clazz, String transformer, String id);

    public <TResult> TResult load(Class<TResult> clazz, String transformer, String id, LoadConfigurationFactory configure);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult load(Class<TTransformer> tranformerClass,
      Class<TResult> clazz, String id);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult load(Class<TTransformer> tranformerClass,
      Class<TResult> clazz, String id, LoadConfigurationFactory configure);
    //endregion

    //region loading_entities_2_0
    public ILoaderWithInclude include(String path);

    public ILoaderWithInclude include(Path<?> path);

    public ILoaderWithInclude include(Class<?> targetEntityClass, Path<?> path);
    //endregion

    //region loading_entities_3_0
    public <T> T[] load(Class<T> clazz, String...ids);

    public <T> T[] load(Class<T> clazz, Collection<String> ids);

    public <T> T[] load(Class<T> clazz, Number... ids);

    public <T> T[] load(Class<T> clazz, UUID... ids);

    public <TResult> TResult[] load(Class<TResult> clazz, String transformer, Collection<String> ids);

    public <TResult> TResult[] load(Class<TResult> clazz, String transformer, Collection<String> ids, LoadConfigurationFactory configure);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] load(Class<TTransformer> tranformerClass,
      Class<TResult> clazz, String... ids);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] load(Class<TTransformer> tranformerClass,
      Class<TResult> clazz, List<String> ids, LoadConfigurationFactory configure);
    //endregion

    //region loading_entities_4_0
    public <T> T[] loadStartingWith(Class<T> clazz, String keyPrefix);

    public <T> T[] loadStartingWith(Class<T> clazz, String keyPrefix, String matches);

    public <T> T[] loadStartingWith(Class<T> clazz, String keyPrefix, String matches, int start);

    public <T> T[] loadStartingWith(Class<T> clazz, String keyPrefix, String matches, int start, int pageSize);

    public <T> T[] loadStartingWith(Class<T> clazz, String keyPrefix, String matches, int start, int pageSize, String exclude);

    public <T> T[] loadStartingWith(Class<T> clazz, String keyPrefix, String matches, int start, int pageSize, String exclude, RavenPagingInformation pagingInformation);

    public <T> T[] loadStartingWith(Class<T> clazz, String keyPrefix, String matches, int start, int pageSize, String exclude, RavenPagingInformation pagingInformation, String skipAfter);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] loadStartingWith(Class<TResult> clazz, Class<TTransformer> transformerClass,
      String keyPrefix);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] loadStartingWith(Class<TResult> clazz, Class<TTransformer> transformerClass,
      String keyPrefix, String matches);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] loadStartingWith(Class<TResult> clazz, Class<TTransformer> transformerClass,
      String keyPrefix, String matches, int start);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] loadStartingWith(Class<TResult> clazz, Class<TTransformer> transformerClass,
      String keyPrefix, String matches, int start, int pageSize);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] loadStartingWith(Class<TResult> clazz, Class<TTransformer> transformerClass,
      String keyPrefix, String matches, int start, int pageSize, String exclude);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] loadStartingWith(Class<TResult> clazz, Class<TTransformer> transformerClass,
      String keyPrefix, String matches, int start, int pageSize, String exclude,
      RavenPagingInformation pagingInformation);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] loadStartingWith(Class<TResult> clazz, Class<TTransformer> transformerClass,
      String keyPrefix, String matches, int start, int pageSize, String exclude,
      RavenPagingInformation pagingInformation, LoadConfigurationFactory configure);

    public <TResult, TTransformer extends AbstractTransformerCreationTask> TResult[] loadStartingWith(Class<TResult> clazz, Class<TTransformer> transformerClass,
      String keyPrefix, String matches, int start, int pageSize, String exclude,
      RavenPagingInformation pagingInformation, LoadConfigurationFactory configure, String skipAfter);
    //endregion

    //region loading_entities_5_0
    public <T> CloseableIterator<StreamResult<T>> stream(Class<T> entityClass);

    public <T> CloseableIterator<StreamResult<T>> stream(Class<T> entityClass, Etag fromEtag);

    public <T> CloseableIterator<StreamResult<T>> stream(Class<T> entityClass, Etag fromEtag, String startsWith);

    public <T> CloseableIterator<StreamResult<T>> stream(Class<T> entityClass, Etag fromEtag, String startsWith, String matches);

    public <T> CloseableIterator<StreamResult<T>> stream(Class<T> entityClass, Etag fromEtag, String startsWith, String matches, int start);

    public <T> CloseableIterator<StreamResult<T>> stream(Class<T> entityClass, Etag fromEtag, String startsWith, String matches, int start, int pageSize);

    public <T> CloseableIterator<StreamResult<T>> stream(Class<T> entityClass, Etag fromEtag, String startsWith, String matches, int start, int pageSize, RavenPagingInformation pagingInformation);

    public <T> CloseableIterator<StreamResult<T>> stream(Class<T> entityClass, Etag fromEtag, String startsWith, String matches, int start, int pageSize, RavenPagingInformation pagingInformation, String skipAfter);
    //endregion

    //region loading_entities_6_0
    public boolean isLoaded(String id);
    //endregion
  }

  @SuppressWarnings("unused")
  public LoadingEntities() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_1_1
        Employee employee = session.load(Employee.class, "employees/1");
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_1_2
        // loading 'employees/1'
        // and transforming result using 'Employees_NoLastName' transformer
        // which returns 'lastName' as 'null'
        Employee employee = session.load(Employees_NoLastName.class, Employee.class, "employees/1");
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_2_1
        // loading 'products/1'
        // including document found in 'supplier' field
        QProduct p = QProduct.product;
        Product product = session
          .include(p.supplier)
          .load(Product.class, "products/1");

        Address supplier = session.load(Address.class, product.getSupplier()); //this will not make server call
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region Loading_entities_2_2
        // loading 'products/1'
        // including document found in 'supplier' field
        Product product = session
          .include("Supplier")
          .load(Product.class, "products/1");

        Address supplier = session.load(Address.class, product.getSupplier()); //this will not make server call
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_3_1
        Employee[] employees = session.load(Employee.class, Arrays.asList("employees/1", "employees/2"));
        Employee employee1 = employees[0];
        Employee employee2 = employees[1];
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_3_2
        // loading 'employees/1' and 'employees/2'
        // and transforming results using 'Employees_NoLastName' transformer
        // which returns 'lastName' as 'null'
        Employee[] employees = session.load(Employees_NoLastName.class, Employee.class, "employees/1", "employees/2");
        Employee employee1 = employees[0];
        Employee employee2 = employees[1];
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_4_1
        // return up to 128 entities with id that starts with 'employees'
        session.advanced().loadStartingWith(Employee.class, "employees", null, 0, 128);
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_4_2
        // return up to 128 entities with id that starts with 'employees/'
        // and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
        Employee[] result = session.advanced().loadStartingWith(Employee.class, "employees/", "1*|2*", 0, 128);
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_4_3
        // return up to 128 entities with id that starts with 'employees/'
        // and rest of the id have length of 3, begins and ends with "1"
        // and contains any character at 2nd position e.g. employees/101, employees/1B1
        // and transform results using 'Employees_NoLastName' transformer
        session.advanced().loadStartingWith(Employee.class, Employees_NoLastName.class, "employees/", "1?1", 0, 128);
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_5_1
        CloseableIterator<StreamResult<Employee>> iterator = session.advanced().stream(Employee.class, null, "employees/");
        while (iterator.hasNext()) {
          StreamResult<Employee> employee = iterator.next();
        }
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region loading_entities_6_1
        boolean isLoaded = session.advanced().isLoaded("employees/1"); //false
        Employee employee = session.load(Employee.class, "employees/1");
        isLoaded = session.advanced().isLoaded("employees/1"); //true
        //endregion
      }
    }
  }
}
