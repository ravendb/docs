import {DocumentStore} from "ravendb";


describe("node.js sandbox", function () {
    it("should work", async function () {
        const store = new DocumentStore("http://live-test.ravendb.net", "docs");

        store.initialize();

        const session = store.openSession();

        await session.store({
            test: 1
        });
        await session.saveChanges();
    });
})
