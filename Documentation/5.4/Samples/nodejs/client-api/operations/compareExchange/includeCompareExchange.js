import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();

async function putCompareExchange() {
    {
        //region sample_data
        {
            // Create some company documents:
            // ==============================

            const session = documentStore.openSession();

            const company1 = new Company();
            company1.id = "companies/1";
            company1.name = "Apple";
            company1.supplier = "suppliers/1";
            company1.workers = ["employees/1", "employees/2"];

            const company2 = new Company("companies/2", "Google", "suppliers/2", ["employees/3", "employees/4"]);
            const company3 = new Company("companies/3", "Microsoft", "suppliers/3", ["employees/6", "employees/5"]);

            await session.store(company1);
            await session.store(company2);
            await session.store(company3);

            await session.saveChanges();
        }
        {
            // Create some CmpXchg items:
            // ==========================

            // Open a session with cluster-wide mode so that we can call 'createCompareExchangeValue'
            const session = documentStore.openSession({
                transactionMode: "ClusterWide"
            });

            session.advanced.clusterTransaction.createCompareExchangeValue("employee/1", "content for employee 1 ..");
            session.advanced.clusterTransaction.createCompareExchangeValue("employee/2", "content for employee 2 ..");
            session.advanced.clusterTransaction.createCompareExchangeValue("employee/3", "content for employee 3 ..");

            session.advanced.clusterTransaction.createCompareExchangeValue("suppliers/1", "content for supplier 1 ..");
            session.advanced.clusterTransaction.createCompareExchangeValue("suppliers/2", "content for supplier 2 ..");
            session.advanced.clusterTransaction.createCompareExchangeValue("suppliers/3", "content for supplier 3 ..");
            
            await session.saveChanges();
        }
        //endregion
    }
    {
        //region include_1
        // Open a session with cluster-wide mode to enable calling 'includeCompareExchangeValue'
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        // Load a company document + include a CmpXchg item:
        // =================================================

        // Call 'includeCompareExchangeValue' within the 'load' options,
        // pass the PATH of the document property that contains the key of the CmpXchg item to include
        const company1 = await session.load("companies/1", {
            documentType: Company,
            // "supplier" is the document property that holds the cmpXchg key 
            includes: i => i.includeCompareExchangeValue("supplier")
        });

        // Calling 'load' has triggered a server call
        const numberOfRequests = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests, 1);

        // Access the included CmpXchg item:
        // =================================

        // Call 'getCompareExchangeValue' to access the content of the included CmpXchg item,
        // pass the cmpXchg item KEY. This will NOT trigger another server call.
        const item = await session.advanced.clusterTransaction
            .getCompareExchangeValue(company1.supplier);

        // You can assert that no further server calls were made
        assert.equal(session.advanced.numberOfRequests, numberOfRequests);

        // The cmpXchg item value is available
        const value = item.value;
        //endregion
    }
    {
        //region include_2
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        // Load a company document + include multiple CmpXchg items:
        // =========================================================

        // Call 'includeCompareExchangeValue' within the 'load' options,
        // pass the PATH of the document property that contains the list keys of the CmpXchg items to include
        const company1 = await session.load("companies/1", {
            documentType: Company,
            // "workers" is the document property that holds the list of keys 
            includes: i => i.includeCompareExchangeValue("workers")
        });

        const numberOfRequests = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests, 1);

        // Access the included CmpXchg items:
        // ==================================

        // Call 'getCompareExchangeValues' to access the content of the included CmpXchg items, 
        // pass the list of KEYS. This will NOT trigger another server call.
        const items = await session.advanced.clusterTransaction
            .getCompareExchangeValues(company1.workers);

        assert.equal(session.advanced.numberOfRequests, numberOfRequests);

        // The value of each item is available
        const value1 = items["employees/1"].value;
        const value2 = items["employees/2"].value;
        //endregion
    }
    {
        //region include_3
        // Open a session with cluster-wide mode to enable calling 'includeCompareExchangeValue'
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        // Make a dynamic query + include CmpXchg items:
        // =============================================

        const companies = await session.query({ collection: "companies" })
            // Call 'include' with 'includeCompareExchangeValue'
            // pass the PATH of the document property that contains the key of the CmpXchg item to include  
            .include(x => x.includeCompareExchangeValue("supplier"))
            .all();

        // Making the query has triggered a server call
        const numberOfRequests = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests, 1);

        // Access the included CmpXchg items:
        // ==================================

        const cmpXchgItems = [];

        for (let i = 0; i < companies.length; i++) {
            // Call 'getCompareExchangeValues' to access the content of the included CmpXchg items,
            // pass the KEY. This will NOT trigger another server call.
            const item = await session.advanced.clusterTransaction
                .getCompareExchangeValue(companies[i].supplier);

            cmpXchgItems.push(item);
        }

        // You can assert that no further server calls were made
        assert.equal(session.advanced.numberOfRequests, numberOfRequests);
        //endregion
    }
    {
        //region include_4
        // Open a session with cluster-wide mode to enable calling 'include cmpxchg'
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        // Make a raw query + include CmpXchg items:
        // =========================================

        // In the provided RQL:
        // * Call 'include' with 'cmpxchg'
        // * Pass the PATH of the document property that contains the key of the CmpXchg item to include  
        const companies = await session.advanced
            .rawQuery(`from companies as c
                       select c
                       include cmpxchg(c.supplier)`)
            .all();

        const numberOfRequests = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests, 1);

        // Access the included CmpXchg items:
        // ==================================

        const cmpXchgItems = [];

        for (let i = 0; i < companies.length; i++) {
            // Call 'getCompareExchangeValues' to access the content of the included CmpXchg items, pass the KEY,
            // this will NOT trigger another server call
            const item = await session.advanced.clusterTransaction
                .getCompareExchangeValue(companies[i].supplier);

            cmpXchgItems.push(item);
        }
        
        assert.equal(session.advanced.numberOfRequests, numberOfRequests);
        //endregion
    }
    {
        //region include_5
        // Open a session with cluster-wide mode to enable calling 'includes.cmpxchg'
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        // Make a raw query + include CmpXchg items using Javascript method:
        // =================================================================

        // In the provided RQL:
        // * Call 'includes.cmpxchg'
        // * Pass the PATH of the document property that contains the key of the CmpXchg item to include  
        const companies = await session.advanced
            .rawQuery(`declare function includeCmpXchg(company) {
                          includes.cmpxchg(company.supplier);
                          return company;
                     }
                     
                    from companies as c
                    select includeCmpXchg(c)`)
            .all();

        const numberOfRequests = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests, 1);

        // Access the included CmpXchg items:
        // ==================================

        const cmpXchgItems = [];

        for (let i = 0; i < companies.length; i++) {
            // Call 'getCompareExchangeValues' to access the content of the included CmpXchg items,
            // pass the KEY. This will NOT trigger another server call
            const item = await session.advanced.clusterTransaction
                .getCompareExchangeValue(companies[i].supplier);

            cmpXchgItems.push(item);
        }

        assert.equal(session.advanced.numberOfRequests, numberOfRequests);
        //endregion
    }
    {
        //region include_6
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });

        // Make an index query + include CmpXchg items:
        // ============================================

        // Call 'include' with 'includeCompareExchangeValue'
        // pass the PATH of the document property that contains the key of the CmpXchg item to include
        const companies = await session.query({ indexName: "Companies/ByName" })
            .include(x => x.includeCompareExchangeValue("supplier"))
            .all();
        
        const numberOfRequests = session.advanced.numberOfRequests;
        assert.equal(numberOfRequests, 1);

        // Access the included CmpXchg items:
        // ==================================

        const cmpXchgItems = [];

        for (let i = 0; i < companies.length; i++) {
            // Call 'getCompareExchangeValues' to access the content of the included CmpXchg items,
            // pass the KEY. This will NOT trigger another server call
            const item = await session.advanced.clusterTransaction
                .getCompareExchangeValue(companies[i].supplier);

            cmpXchgItems.push(item);
        }

        assert.equal(session.advanced.numberOfRequests, numberOfRequests);
        //endregion
    }
}

//region syntax

//region syntax_1 
includeCompareExchangeValue(path);
//endregion

//region syntax_2 
// Return value of store.operations.send(putCmpXchgOp)
// ===================================================
class CompareExchangeResult {
    successful;
    value;
    index;
}
//endregion

//region sample_class
class Company {
    constructor(
        id = null,
        name = "",
        supplier = "",
        workers = []

    ) {
        Object.assign(this, {
            id,
            name,
            supplier,
            workers
        });
    }
}
//endregion

//region index
class Companies_ByName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();

        this.map('companies', company => {
            return {
                name: company.name
            };
        });
    }
}
//endregion

//endregion
