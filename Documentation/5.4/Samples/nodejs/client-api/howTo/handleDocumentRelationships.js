import { DocumentStore } from "ravendb";
import assert from "assert";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

//region classes

//region order
class Order {
    constructor(
        customerId = '',
        supplierIds = [],
        referral = null,
        lineItems = [],
        totalPrice = 0
    ) {
        Object.assign(this, {
            customerId,
            supplierIds,
            referral,
            lineItems,
            totalPrice
        });
    }
}
//endregion

//region customer
class Customer {
    constructor(
        id = '',
        name = ''
    ) {
        Object.assign(this, {
            id,
            name
        });
    }
}
//endregion

//region denormalized_customer
class DenormalizedCustomer {
    constructor(
        id = '',
        name = '',
        address = ''
    ) {
        Object.assign(this, {
            id,
            name,
            address
        });
    }
}
//endregion

//region referral
class Referral {
    constructor(
        customerId = '',
        commissionPercentage = 0
    ) {
        Object.assign(this, {
            customerId,
            commissionPercentage
        });
    }
}
//endregion

//region line_item
class LineItem {
    constructor(
        productId = '',
        name = '',
        quantity = 0
    ) {
        Object.assign(this, {
            productId,
            name,
            quantity
        });
    }
}
//endregion

//region order_2
class Order2 {
    constructor(
        customer = {},
        supplierIds = '',
        referral = null,
        lineItems = [],
        totalPrice = 0
    ) {
        Object.assign(this, {
            customer,
            supplierIds,
            referral,
            lineItems,
            totalPrice
        });
    }
}
//endregion

//region person_1
class Person {
    constructor(
        id = '',
        name = '',
        // attributes will be assigned a plain object containing key-value pairs
        attributes = {}
    ) {
        Object.assign(this, {
            id,
            name,
            attributes
        });
    }
}
//endregion

//region person_2
class PersonWithAttribute {
    constructor(
        id = '',
        name = '',
        // attributes will be assigned a complex object
        attributes = {}
    ) {
        Object.assign(this, {
            id,
            name,
            attributes
        });
    }
}

class Attribute {
    constructor(
        ref = ''
    ) {
        Object.assign(this, {
            ref
        });
    }
}
//endregion

//endregion

async function handleDocumentRelationships() {
    {
        //region includes_1_0
        const order = await session
             // Call 'include'
             // Pass the path of the document property that holds document to include
            .include("customerId")
            .load("orders/1-A");
        
        const customer = await session
             // This call to 'load' will not require querying the server
             // No server request will be made
            .load(order.customerId);
        //endregion
    }
    {
        //region includes_2_0
        const orders = await session
            .include("customerId")
            .load(["orders/1-A", "orders/2-A"]);

        const orderEntities = Object.entries(orders);
        
        for (let i = 0; i < orderEntities.length; i++) {
            // This will not require querying the server
            const customer = await session.load(orderEntities[i][1].customerId);
        }
        //endregion
    }
    {
        //region includes_3_0
        const orders = await session
            .query({ collection: "orders" })
            .whereGreaterThan("totalPrice", 100)
            .include("customerId")
            .all();

        for (let i = 0; i < orders.length; i++) {
            // This will not require querying the server
            const customer = await session.load(orders[i].customerId);
        }
        //endregion
    }
    {
        //region includes_3_0_builder
        const orders = await session
            .query({ collection: "orders" })
            .whereGreaterThan("totalPrice", 100)
            .include(i => i
                .includeDocuments("customerId")        // include document
                .includeCounter("OrderUpdateCounter")) // builder can include counters as well 
            .all();
        
        for (let i = 0; i < orders.length; i++) {
            // This will not require querying the server
            const customer = await session.load(orders[i].customerId);
        }
        //endregion
    }
    {
        //region includes_4_0
        const order = await session
            .include("supplierIds")
            .load("orders/1-A");

        for (let i = 0; i < order.supplierIds.length; i++) {
            // This will not require querying the server
            const supplier = await session.load(order.supplierIds[i]);
        }
        //endregion
    }
    {
        //region includes_4_0_builder
        const order = await session
            .load("orders/1-A", {
                includes: i => i.includeDocuments("supplierIds")
            });

        for (let i = 0; i < order.supplierIds.length; i++) {
            // This will not require querying the server
            const supplier = await session.load(order.supplierIds[i]);
        }
        //endregion
    }
    {
        //region includes_5_0
        const orders = await session
            .include("supplierIds")
            .load(["orders/1-A", "orders/2-A"]);

        const orderEntities = Object.entries(orders);

        for (let i = 0; i < orderEntities.length; i++) {
            const suppliers = orderEntities[i][1].supplierIds;
            
            for (let j = 0; j < suppliers.length; j++) {
                // This will not require querying the server
                const supplier = await session.load(suppliers[j]);
            }
        }
        //endregion
    }
    {
        //region includes_6_0
        const order = await session
            .include("referral.customerId")
            .load("orders/1-A");

        // This will not require querying the server
        const customer = await session.load(order.referral.customerId);
        //endregion
    }
    {
        //region includes_6_0_builder
        const order = await session
            .load("orders/1-A", {
                includes: i => i.includeDocuments("referral.customerId")
            });

        // This will not require querying the server
        const customer = await session.load(order.referral.customerId);
        //endregion
    }
    {
        //region includes_7_0
        const order = await session
            .include("lineItems[].productId")
            .load("orders/1-A");

        for (let i = 0; i < order.lineItems.length; i++) {
            // This will not require querying the server
            const product = await session.load(order.lineItems[i].productId);
        }
        //endregion
    }
    {
        //region includes_7_0_builder
        const order = await session
            .load("orders/1-A", {
                includes: i => i.includeDocuments("lineItems[].productId")
            });

        for (let i = 0; i < order.lineItems.length; i++) {
            // This will not require querying the server
            const product = await session.load(order.lineItems[i].productId);
        }
        //endregion
    }
    {
        //region includes_9_0
        const order = await session
            .include("customer.id")
            .load("orders/1-A");

        // This will not require querying the server
        const customer = await session.load(order.customer.id);
        //endregion
    }
    {
        //region includes_10_0
        const person1 = new Person();
        person1.name = "John Doe";
        person1.id = "people/1";
        person1.attributes = {
            "mother": "people/2",
            "father": "people/3"
        }

        const person2 = new Person();
        person2.name = "Helen Doe";
        person2.id = "people/2";

        const person3 = new Person();
        person3.name = "George Doe";
        person3.id = "people/3";

        await session.store(person1);
        await session.store(person2);
        await session.store(person3);

        await session.saveChanges();
        //endregion
    }
    {
        //region includes_10_1
        const person = await session
            .include("attributes.$Values")
            .load("people/1");

        const mother = await session
            .load(person.attributes["mother"]);

        const father = await session
            .load(person.attributes["father"]);

        assert.equal(session.advanced.numberOfRequests, 1);
        //endregion
    }
    {
        //region includes_10_1_builder
        const person = await session
            .load("people/1", {
                includes: i => i.includeDocuments("attributes.$Values")
            });

        const mother = await session
            .load(person.attributes["mother"]);

        const father = await session
            .load(person.attributes["father"]);

        assert.equal(session.advanced.numberOfRequests, 1);
        //endregion
    }
    {
        //region includes_10_3
        const person = await session
            .include("attributes.$Keys")
            .load("people/1");
        //endregion
    }
    {
        //region includes_10_3_builder
        const person = await session
            .load("people/1", {
                includes: i => i.includeDocuments("attributes.$Keys")
            });
        //endregion
    }
    {
        //region includes_11_0
        const attr2 = new Attribute();
        attr2.ref = "people/2";
        const attr3 = new Attribute();
        attr3.ref = "people/3";
        
        const person1 = new PersonWithAttribute();
        person1.name = "John Doe";
        person1.id = "people/1";
        person1.attributes = {
            "mother": attr2,
            "father": attr3
        }

        const person2 = new Person();
        person2.name = "Helen Doe";
        person2.id = "people/2";

        const person3 = new Person();
        person3.name = "George Doe";
        person3.id = "people/3";

        await session.store(person1);
        await session.store(person2);
        await session.store(person3);

        await session.saveChanges();
        //endregion
    }
    {
        //region includes_11_1
        const person = await session
            .include("attributes.$Values[].ref")
            .load("people/1");

        const mother = await session
            .load(person.attributes["mother"].ref);

        const father = await session
            .load(person.attributes["father"].ref);

        assert.equal(session.advanced.numberOfRequests, 1);
        //endregion
    }
}
