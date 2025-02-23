import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function forceRevisionCreation() {
    {
        //region force_revision_creation_by_entity
        // Force revision creation by entity
        // =================================

        const company = new Company();
        company.name = "CompanyName";

        const session = documentStore.openSession();
        await session.store(company);
        await session.saveChanges();

        // Forcing the creation of a revision by entity can be performed 
        // only when the entity is tracked, after the document is stored.
        await session.advanced.revisions.forceRevisionCreationFor(company);

        // Must call 'saveChanges' for the revision to be created
        await session.saveChanges();

        // Get existing revisions:
        const revisions = await session.advanced.revisions.getFor(company.id);
        const revisionsCount = revisions.length;

        assert.equal(revisionsCount, 1);
        //endregion
    }
    {
        //region force_revision_creation_by_id
        const company = new Company();
        company.name = "CompanyName";

        const session = documentStore.openSession();
        await session.store(company);
        await session.saveChanges();
       
        // Force revision creation by ID
        const companyId = company.id;
        await session.advanced.revisions.forceRevisionCreationFor(companyId);
        await session.saveChanges();

        const revisions = await session.advanced.revisions.getFor(company.id);
        const revisionsCount = revisions.length;

        assert.equal(revisionsCount, 1);
        //endregion
    }
}

//region syntax_1
// Available overloads:
// ====================
forceRevisionCreationFor(entity);
forceRevisionCreationFor(entity, strategy);
forceRevisionCreationFor(id);
forceRevisionCreationFor(id, strategy);
//endregion
