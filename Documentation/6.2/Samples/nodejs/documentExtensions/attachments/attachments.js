import * as assert from "assert";
import * as fs from "fs";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    let documentId, contentType, stream, entity, name, changeVector;
    //region StoreSyntax
    session.advanced.attachments.store(documentId, name, stream, [contentType]);

    session.advanced.attachments.store(entity, name, stream, [contentType]);
    //endregion

    //region GetSyntax
    session.advanced.attachments.getNames(entity);

    session.advanced.attachments.exists(documentId, name);

    session.advanced.attachments.get(documentId, name);

    session.advanced.attachments.get(entity, name);

    session.advanced.attachments.getRevision(documentId, name, changeVector);
    //endregion

    //region DeleteSyntax
    session.advanced.attachments.delete(documentId, name);

    session.advanced.attachments.delete(entity, name);
    //endregion
}

class Album {}

async function storeAttachment() {
    {
        //region StoreAttachment
        const session = store.openSession();

        const file1 = fs.createReadStream("001.jpg");
        const file2 = fs.createReadStream("002.jpg");
        const file3 = fs.createReadStream("003.jpg");
        const file4 = fs.createReadStream("004.mp4");

        const album = new Album();
        album.name = "Holidays";
        album.description = "Holidays travel pictures of the all family";
        album.tags = [ "Holidays Travel", "All Family" ];
        await session.store(album, "albums/1");

        session.advanced.attachments
            .store("albums/1", "001.jpg", file1, "image/jpeg");
        session.advanced.attachments
            .store("albums/1", "002.jpg", file2, "image/jpeg");
        session.advanced.attachments
            .store("albums/1", "003.jpg", file3, "image/jpeg");
        session.advanced.attachments
            .store("albums/1", "004.mp4", file4, "video/mp4");

        await session.saveChanges();
        //endregion
    }
}

async function getAttachment() {
    {
        //region GetAttachment
        const album = await session.load("albums/1");

        const file1 = await session.advanced.attachments.get(album, "001.jpg");
        const file2 = await session.advanced.attachments.get("albums/1", "002.jpg");

        const inputStream = file1.data;

        const attachmentDetails = file1.details;
        //     { 
        //       name: '001.jpg',
        //       documentId: 'albums/1',
        //       contentType: 'image/jpeg',
        //       hash: 'MvUEcrFHSVDts5ZQv2bQ3r9RwtynqnyJzIbNYzu1ZXk=',
        //       changeVector: '"A:3-K5TR36dafUC98AItzIa6ow"',
        //       size: 25793 
        //     }

        const attachmentNames = await session.advanced.attachments.getNames(album);
        for (const attachmentName of attachmentNames) {
            const name = attachmentName.name;
            const contentType = attachmentName.contentType;
            const hash = attachmentName.hash;
            const size = attachmentName.size;
        }

        const exists = session.advanced.attachments.exists("albums/1", "003.jpg");
        // true
        
        //endregion
    }
}

async function deleteAttachment() {
        //region DeleteAttachment
        const session = store.openSession();
        const album = await session.load("albums/1");
        session.advanced.attachments.delete(album, "001.jpg");

        session.advanced.attachments.delete("albums/1", "002.jpg");

        await session.saveChanges();
        //endregion
}
