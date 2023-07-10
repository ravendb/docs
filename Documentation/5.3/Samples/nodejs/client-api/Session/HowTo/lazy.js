import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function lazyExamples() {
    {
        //region lazy_load
        const lazyEmployee = session
             // Add a call to lazily 
            .advanced.lazily
             // Document will Not be loaded from the database here, no server call is made
            .load("employees/1-A");
        
        const employee = await lazyEmployee.getValue(); // 'load' operation is executed here
        // The employee entity is now loaded & tracked by the session
        //endregion
    }
    {        
        //region lazy_loadWithInclude
        const lazyProduct = session
             // Add a call to lazily 
            .advanced.lazily
             // Request to include the related Supplier document
             // Documents will Not be loaded from the database here, no server call is made
            .include("supplierId")
            .load("products/1-A");

        // 'load with include' operation will be executed here
        // Both documents will be retrieved from the database
        const product = await lazyProduct.getValue();
        // The product entity is now loaded & tracked by the session

        // Access the related document, no additional server call is made
        const supplier = await session.load(product.supplierId)
        // The supplier entity is now also loaded & tracked by the session
        //endregion
    }
    {
        //region lazy_loadStartingWith
        const lazyEmployees = session
             // Add a call to lazily 
            .advanced.lazily
             // Request to load entities whose ID starts with 'employees/'
             // Documents will Not be loaded from the database here, no server call is made
            .loadStartingWith("employees/");

        const employees = await lazyEmployees.getValue(); // 'load' operation is executed here
        // The employee entities are now loaded & tracked by the session
        //endregion
    }
    {
        //region lazy_conditionalLoad
        // Create document and get its change-vector:
        {
            const session1 = documentStore.openSession();

            const employee = new Employee();
            await session1.store(employee, "employees/1-A");
            await session.saveChanges();
            
            // Get the tracked entity change-vector
            const changeVector = session.advanced.getChangeVectorFor(employee);
        }
        
        // Conditionally lazy-load the document:
        {
            const session2 = documentStore.openSession();

            const lazyEmployee = session2
                 // Add a call to lazily 
                .advanced.lazily
                 // Document will Not be loaded from the database here, no server call is made
                .conditionalLoad("employees/1-A", changeVector, Employee);

            const loadedItem = await lazyEmployee.getValue(); // 'conditionalLoad' operation is executed here
            const employee = loadeditem.entity;
            
            // If conditionalLoad has actually fetched the document from the server (logic described above)
            // then the employee entity is now loaded & tracked by the session
        }
        //endregion
    }
    {
        //region lazy_query
        // Define a lazy query:
        const lazyEmployees = session
            .query({ collection: "employees" })
            .whereEquals("FirstName", "John")
             // Add a call to lazily, the query will Not be executed here 
            .lazily();
        
        const employees = await lazyEmployees.getValue(); // Query is executed here
        
        // Note: Since query results are not projected,
        // then the resulting employee entities will be tracked by the session.
        //endregion
    }
    {
        //region lazy_revisions
        var lazyRevisions = session
             // Add a call to lazily 
            .advanced.revisions.lazily
             // Revisions will Not be fetched here, no server call is made
            .getFor("employees/1-A");

             // Usage is the same for the other get revisions methods:
             // .get()
             // .getMetadataFor()

        const revisions = lazyRevisions.getValue(); // Getting revisions is executed here
        //endregion
    }
    {
        //region lazy_CompareExchange        
        const session = documentStore.openSession({ transactionMode: "ClusterWide" });
        
        // Create compare-exchange value:
        session.advanced.clusterTransaction.createCompareExchangeValue("someKey", "someValue");
        await session.saveChanges();
        
        // Get the compare-exchange value lazily:
        const lazyCmpXchg = session
            // Add a call to lazily 
            .advanced.clusterTransaction.lazily
            // Compare-exchange values will Not be fetched here, no server call is made
            .getCompareExchangeValue("someKey");
        
        // Usage is the same for the other method:
        // .getCompareExchangeValues()
        
        const cmpXchgValue =
            await lazyCmpXchg.getValue(); // Getting compare-exchange value is executed here            
        //endregion
    }
    {
        //region lazy_ExecuteAll_Implicit
        // Define multiple lazy requests
        const lazyUser1 = session.advanced.lazily.load("users/1-A");
        const lazyUser2 = session.advanced.lazily.load("users/2-A");

        const lazyEmployees = session.query({ collection: "employees" })
            .lazily();
        const lazyProducts = session.query({ collection: "products" })
            .search("Name", "Ch*")
            .lazily();

        // Accessing the value of ANY of the lazy instances will trigger
        // the execution of ALL pending lazy requests held up by the session
        // This is done in a SINGLE server call
        const user1 = await lazyUser1.getValue();

        // ALL the other values are now also available
        // No additional server calls are made when accessing these values
        const user2 = await lazyUser2.getValue();
        const employees = await lazyEmployees.getValue();
        const products = await lazyProducts.getValue();
        //endregion
    }
    {
        //region lazy_ExecuteAll_Explicit
        // Define multiple lazy requests
        const lazyUser1 = session.advanced.lazily.load("users/1-A");
        const lazyUser2 = session.advanced.lazily.load("users/2-A");

        const lazyEmployees = session.query({ collection: "employees" })
            .lazily();
        const lazyProducts = session.query({ collection: "products" })
            .search("Name", "Ch*")
            .lazily();

        // Explicitly call 'executeAllPendingLazyOperations'
        // ALL pending lazy requests held up by the session will be executed in a SINGLE server call
        await session.advanced.eagerly.executeAllPendingLazyOperations();

        // ALL values are now available
        // No additional server calls are made when accessing the values
        const user1 = await lazyUser1.getValue();
        const user2 = await lazyUser2.getValue();
        const employees = await lazyEmployees.getValue();
        const products = await lazyProducts.getValue();
        //endregion
    }
    {
        //region lazy_productClass
        // Sample product document
        class Product {
            constructor(name, supplierId) {
                this.id = null;
                this.name = name;
                this.supplierId = supplierId; // The related document ID
            }
        }
        //endregion

        class Employee {
            constructor(name) {
                this.id = null;
                this.name = name;
            }
        }
    }
}


