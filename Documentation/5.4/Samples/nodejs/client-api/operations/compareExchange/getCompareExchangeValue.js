import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function getCompareExchangeValue() {
    {
        //region get_1
        // Put a new compare-exchange item,
        // e.g. save the number of sales made by an employee as the value + some metadata info        
        const putCmpXchgOp = new PutCompareExchangeValueOperation("employees/1-A", 12345, 0,{
            "Department": "Sales",
            "Role": "Salesperson",
        });
        const result = await documentStore.operations.send(putCmpXchgOp);

        // Get the compare-exchange item:
        // ==============================
        
        // Define the get compare-exchange operation, pass the unique item key
        const getCmpXchgOp = new GetCompareExchangeValueOperation("employees/1-A");
        
        // Execute the operation by passing it to operations.send
        const item = await documentStore.operations.send(getCmpXchgOp);
        
        // Access the value and metadata of the retrieved item
        const numberOfSales = item.value;
        const employeeRole = item.metadata["Role"];
        
        // Access the version number of the retrieved item
        const version = item.index;
        //endregion
    }
    {
        //region get_2
        // Put a new compare-exchange item,
        // e.g. save an object as the value
        const employee = new Employee();
        employee.role = "Salesperson"
        employee.department = "Sales";
        employee.numberOfSales = 12345;

        const putCmpXchgOp = new PutCompareExchangeValueOperation("employees/1-A", employee, 0);
        const result = await documentStore.operations.send(putCmpXchgOp);

        // Get the compare-exchange item:
        // ==============================

        // Define the get compare-exchange operation, pass the unique item key & the class type
        const getCmpXchgOp = new GetCompareExchangeValueOperation("employees/1-A", Employee);

        // Execute the operation by passing it to operations.send
        const item = await documentStore.operations.send(getCmpXchgOp);

        // Access the value of the retrieved item
        const employeeResult = item.value;
        const employeeClass = employeeResult.constructor;   // Employee

        const employeeRole = employeeResult.role;           // Salesperson
        const employeeDep = employeeResult.department;      // Sales
        const employeeSales = employeeResult.numberOfSales; // 12345

        // Access the version number of the retrieved item
        const version = item.index;
        //endregion
    }
}

//region syntax

//region syntax_1 
const getCmpXchgOp = new GetCompareExchangeValueOperation(key, clazz, materializeMetadata);
//endregion

//region syntax_2 
// Return value of store.operations.send(getCmpXchgOp)
// ===================================================
class CompareExchangeValue {
    key;
    value;
    metadata;
    index;
}
//endregion

//region employee_class
class Employee {
    constructor(
        id = null,
        department = "",
        role = "",
        numberOfSales = 0

    ) {
        Object.assign(this, {
            id,
            department,
            role,
            numberOfSales
        });
    }
}
//endregion

//endregion
