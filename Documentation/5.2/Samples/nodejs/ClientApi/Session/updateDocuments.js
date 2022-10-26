import {DocumentStore} from "ravendb";

const documentStore = new DocumentStore();

async function updateDocuments() {
    {
        //region sample_document
        // Sample company document structure
        class Address {
            constructor(code) {
                this.postalCode = code;
            }
        }

        class Company {
            constructor(name, code) {
                this.name = name;
                this.address = new Address(code);
            }
        }
        //endregion
    }
    {
        //region load-company-and-update
        const session = documentStore.openSession();

        // Load a company document
        const company = await session.load("companies/1-A");

        // Update the company's postalCode
        company.address.postalCode = "TheNewPostalCode";

        // Apply changes
        await session.saveChanges();
        //endregion
    }
    {
        //region query-company-and-update
        const session = documentStore.openSession();

        // Query: find companies with the specified postalCode
        const matchingCompanies = await session.query({collection: "Companies"})
            .whereEquals("address.postalCode", "12345")
            .all();

        // Update the postalCode for the resulting company documents
        matchingCompanies.forEach(c => c.address.postalCode = "TheNewPostalCode");

        // Apply changes
        await session.saveChanges();
        //endregion
    }
}
