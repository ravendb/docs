import * as assert from "assert";
import { DocumentStore } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

//region get_document_id_3
class Comment {
    constructor (author, message) {
        this.author = author;
        this.message = message;
    }
}
//endregion

{
    let entity;
    //region get_document_id_1
    session.advanced.getDocumentId(entity);
    //endregion
}

async function sample() {
    let comment;

    {
        const session = store.openSession();
        //region get_document_id_2
        const commentId = session
            .advanced
            .getDocumentId(comment);    // e.g. comments/1-A
        //endregion
    }
}
