package net.ravendb.ClientApi.Configuration.IdentifierGeneration;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.conventions.DocumentConventions;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.function.BiFunction;

public class TypeSpecific {

    private static class Employee {
        private String lastName;
        private String firstName;

        public String getLastName() {
            return lastName;
        }

        public void setLastName(String lastName) {
            this.lastName = lastName;
        }

        public String getFirstName() {
            return firstName;
        }

        public void setFirstName(String firstName) {
            this.firstName = firstName;
        }
    }

    private static class EmployeeManager extends Employee {

    }

    public TypeSpecific() {
        DocumentStore store = new DocumentStore();

        //region employees_custom_async_convention
        store.getConventions().registerIdConvention(Employee.class,
            (dbName, employee) ->
                String.format("employees/%s/%s", employee.getLastName(), employee.getFirstName()));
        //endregion

        //region employees_custom_convention_example
        try (IDocumentSession session = store.openSession()) {
            Employee employee = new Employee();
            employee.setFirstName("James");
            employee.setLastName("Bond");

            session.store(employee);
            session.saveChanges();
        }
        //endregion

        //region employees_custom_convention_inheritance
        try (IDocumentSession session = store.openSession()) {
            Employee adam = new Employee();
            adam.setFirstName("Adam");
            adam.setLastName("Smith");
            session.store(adam); // employees/Smith/Adam

            EmployeeManager david = new EmployeeManager();
            david.setFirstName("David");
            david.setLastName("Jones");
            session.store(david);  // employees/Jones/David

            session.saveChanges();
        }
        //endregion

        //region custom_convention_inheritance_2
        store.getConventions().registerIdConvention(Employee.class,
            (dbName, employee) ->
                String.format("employees/%s/%s", employee.getLastName(), employee.getFirstName())
        );

        store.getConventions().registerIdConvention(EmployeeManager.class,
            (dbName, employee) ->
                String.format("managers/%s/%s", employee.getLastName(), employee.getFirstName())
        );

        try (IDocumentSession session = store.openSession()) {
            Employee adam = new Employee();
            adam.setFirstName("Adam");
            adam.setLastName("Smith");
            session.store(adam); // employees/Smith/AdamReadBalanceBehavior

            EmployeeManager david = new EmployeeManager();
            david.setFirstName("David");
            david.setLastName("Jones");
            session.store(david);  // managers/Jones/David

            session.saveChanges();
        }
        //endregion
    }

    public interface IFoo {
        //region register_id_convention_method_async
        public <TEntity> DocumentConventions registerIdConvention(Class<TEntity> clazz, BiFunction<String, TEntity, String> function);
        //endregion
    }
}
