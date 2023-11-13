import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function seedIdentity() {
    {
        //region seed_identity_1
        // Seed a higher identity value on the server:
        // ===========================================

        // Define the seed identity operation. Pass:
        //   * The collection name (can be with or without a pipe)
        //   * The new value to set
        const seedIdentityOp = new SeedIdentityForOperation("companies|", 23);

        // Execute the operation by passing it to maintenance.send
        // The latest value on the server will be incremented to "23"
        // and the next document created with an identity will be assigned "24"
        const seededValue = await store.maintenance.send(seedIdentityOp);

        // Create a document with an identity ID:
        // ======================================

        const company = new Company();
        company.name = "RavenDB";

        await session.store(company, "companies|");
        await session.saveChanges();
        // => Document "companies/24" will be created
        //endregion
    }
    {
        //region seed_identity_2
        // Force a smaller identity value on the server:
        // =============================================
        
        // Define the seed identity operation. Pass:
        //   * The collection name (can be with or without a pipe)
        //   * The new value to set
        //   * Pass 'true' to force the update
        const seedIdentityOp = new SeedIdentityForOperation("companies|", 5, true);

        // Execute the operation by passing it to maintenance.send
        // The latest value on the server will be decremented to "5"
        // and the next document created with an identity will be assigned "6"
        const seededValue = await store.maintenance.send(seedIdentityOp);

        // Create a document with an identity ID:
        // ======================================

        const company = new Company();
        company.name = "RavenDB";

        await session.store(company, "companies|");
        await session.saveChanges();
        // => Document "companies/6" will be created
        //endregion
    }
}

{
    //region syntax
    const seedIdentityOp = new SeedIdentityForOperation(name, value, forceUpdate);
    //endregion
}
