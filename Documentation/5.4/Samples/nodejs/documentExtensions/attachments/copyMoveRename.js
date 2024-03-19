import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function copyMoveRenameAttachment() {
    {
        //region copy_1
        // Load entities
        const employee1 = await session.load("employees/1-A");
        const employee2 = await session.load("employees/2-A");

        // Call method 'copy'
        // Copy attachment from employee1 to employee2
        session.advanced.attachments.copy(employee1, "photo.jpg", employee2, "photo-copy.jpg");

        // Attachment will be copied on the server-side only when saveChanges is called
        await session.saveChanges();
        //endregion
    }
    {
        //region copy_2
        // Call method 'copy'
        // Copy attachment from "employees/1-A" to "employees/2-A"
        session.advanced.attachments.copy("employees/1-A", "photo.jpg", "employees/2-A", "photo-copy.jpg");

        // Attachment will be copied on the server-side only when saveChanges is called
        await session.saveChanges();
        //endregion
    }
    {
        //region move_1
        // Load entities 
        const employee1 = await session.load("employees/1-A");
        const employee2 = await session.load("employees/2-A");

        // Call method 'move'
        // Move attachment from employee1 to employee2
        session.advanced.attachments.move(employee1, "photo.jpg", employee2, "photo.jpg");

        // Attachment will be moved on the server-side only when saveChanges is called
        await session.saveChanges();
        //endregion
    }
    {
        //region move_2
        // Call method 'move'
        // Move attachment from "employees/1-A" to "employees/2-A"
        session.advanced.attachments.move("employees/1-A", "photo.jpg", "employees/2-A", "photo.jpg");

        // Attachment will be moved on the server-side only when saveChanges is called
        await session.saveChanges();
        //endregion
    }
    {
        //region rename_1
        // Load entity
        const employee = await session.load("employees/1-A");

        // Call method 'rename'
        // Rename "photo.jpg"
        session.advanced.attachments.rename(employee, "photo.jpg", "photo-new.jpg");

        // Attachment will be renamed on the server-side only when saveChanges is called
        await session.saveChanges();
        //endregion
    }
    {
        //region rename_2
        // Call method 'rename'
        // Rename "photo.jpg"
        session.advanced.attachments.rename("employees/1-A", "photo.jpg", "photo-new.jpg");

        // Attachment will be renamed on the server-side only when saveChanges is called
        await session.saveChanges();
        //endregion
    }
}

//region syntax_copy
// Copy - available overloads:
// ===========================
copy(sourceEntity, sourceName, destinationEntity, destinationName);
copy(sourceDocumentId, sourceName, destinationDocumentId, destinationName);
//endregion

//region syntax_move
// Move - a vailable overloads:
// ============================
move(sourceEntity, sourceName, destinationEntity, destinationName);
move(sourceDocumentId, sourceName, destinationDocumentId, destinationName);
//endregion

//region syntax_rename
// Rename - available overloads:
// =============================
rename(entity, name, newName);
rename(documentId, name, newName);
//endregion
