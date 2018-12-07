import * as assert from "assert";
import { DocumentStore, PutCommandDataWithJson, DeleteCommandData } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

let idConvention, clazz;

const conventions = store.conventions;

class Employee {}

{
    //region employees_custom_async_convention
    store.conventions.registerIdConvention(Employee,
        (dbName, entity) => Promise.resolve(`employees/${entity.lastName}/${entity.firstName}`)); 

    // or using async keyword
    store.conventions.registerIdConvention(Employee,
        async (dbName, entity) => `employees/${entity.lastName}/${entity.firstName}`); 

    //endregion

    //region employees_custom_async_convention_typedescriptor
    // for object literal based entities you can pass type descriptor object
    const typeDescriptor = {
        name: "Employee",
        isType(entity) {
            // if it quacks like a duck... ekhm employee
            return entity 
                && "firstName" in entity 
                && "lastName" in entity 
                && "boss" in entity;
        }
    };

    store.conventions.registerIdConvention(typeDescriptor, 
        async (dbName, entity) => `employees/${entity.lastName}/${entity.firstName}`);
    //endregion
}

async function example() {
    {
        //region employees_custom_convention_example
        const session = store.openSession();
        const employee = new Employee("James", "Bond");

        await session.store(employee);
        await session.saveChanges();
        //endregion
    }

    {
        //region register_id_convention_method_async
        conventions.registerIdConvention(clazz, idConvention);
        //endregion
    }
}
