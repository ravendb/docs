package net.ravendb.clientapi.configuration.conventions.identifierGeneration;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.connection.IDatabaseCommands;
import net.ravendb.client.delegates.IdConvention;
import net.ravendb.client.document.DocumentConvention;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Employee;


public class TypeSpecific {

  public static interface IFoo {
    //region register_id_convention_method
    public <TEntity> DocumentConvention registerIdConvention(Class<TEntity> type, IdConvention func);
    //endregion
  }

  public static class EmployeeManager extends Employee {
    // empty by design
  }

  //region class_with_interger_id
  public static class EntityWithIntegerId {
    private String id;

    /*
     * ....
     */

    public String getId() {
      return id;
    }

    public void setId(String id) {
      this.id = id;
    }
  }
  //endregion

  public TypeSpecific() throws Exception {
    DocumentStore store = new DocumentStore();
    //region eployees_custom_convention
    store.getConventions().registerIdConvention(Employee.class, new IdConvention() {
      @Override
      public String findIdentifier(String dbName, IDatabaseCommands databaseCommands, Object employee) {
        Employee e = (Employee) employee;
        return String.format("employees/%s/%s", e.getLastName(), e.getFirstName());
      }
    });
    //endregion

    //region eployees_custom_convention_example
    try (IDocumentSession session = store.openSession()) {
      Employee employee = new Employee();
      employee.setFirstName("James");
      employee.setLastName("Bond");
      session.store(employee);
      session.saveChanges();
    }
    //endregion

    //region eployees_custom_convention_inheritance
    try (IDocumentSession session = store.openSession()) {
      Employee employee1 = new Employee();
      employee1.setFirstName("Adam");
      employee1.setLastName("Smith");
      session.store(employee1);  // employees/Smith/Adam

      EmployeeManager employeeManager = new EmployeeManager();
      employeeManager.setFirstName("David");
      employeeManager.setLastName("Jones");
      session.store(employeeManager); // employees/Jones/David

      session.saveChanges();
    }
    //endregion

    //region custom_convention_inheritance_2
    store.getConventions().registerIdConvention(Employee.class, new IdConvention() {
      @Override
      public String findIdentifier(String dbName, IDatabaseCommands databaseCommands, Object employee) {
        Employee e = (Employee) employee;
        return String.format("employees/%s/%s", e.getLastName(), e.getFirstName());
      }
    });

    store.getConventions().registerIdConvention(EmployeeManager.class, new IdConvention() {
      @Override
      public String findIdentifier(String dbName, IDatabaseCommands databaseCommands, Object employee) {
        EmployeeManager e = (EmployeeManager) employee;
        return String.format("managers/%s/%s", e.getLastName(), e.getFirstName());
      }
    });

    try (IDocumentSession session = store.openSession()) {
      Employee employee1 = new Employee();
      employee1.setFirstName("Adam");
      employee1.setLastName("Smith");
      session.store(employee1);  // employees/Smith/Adam

      EmployeeManager employeeManager = new EmployeeManager();
      employeeManager.setFirstName("David");
      employeeManager.setLastName("Jones");
      session.store(employeeManager); // managers/Jones/David

      session.saveChanges();
    }
    //endregion

    //region id_generation_on_load_1
    store.getConventions().registerIdConvention(EntityWithIntegerId.class, new IdConvention() {
      @Override
      public String findIdentifier(String dbName, IDatabaseCommands databaseCommands, Object entity) {
        EntityWithIntegerId e = (EntityWithIntegerId) entity;
        return "ewi/" + e.getId();
      }
    });
    //endregion

  }
}
