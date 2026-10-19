import { test } from "node:test";
import assert from "node:assert/strict";

import { getSidebarNav, type SidebarNav } from "../pathUtils";

const VERSION = "7.2";

function highlightedLabels(path: string): string[] {
    const nav = getSidebarNav(path, VERSION);
    return [...nav.links, ...nav.footer].filter((item) => item.isActive).map((item) => item.label);
}

function linkTo(nav: SidebarNav, label: string): string | undefined {
    return nav.links.find((item) => item.label === label)?.to;
}

test("a docs article leaves every entry above the page tree unhighlighted", () => {
    assert.deepEqual(highlightedLabels("/7.2/client-api/creating-document-store"), []);
    assert.deepEqual(highlightedLabels("/quill/logging"), []);
});

test("Start is highlighted on the landing page of each product", () => {
    assert.deepEqual(highlightedLabels("/7.2"), ["Start"]);
    assert.deepEqual(highlightedLabels("/cloud"), ["Start"]);
    assert.deepEqual(highlightedLabels("/quill"), ["Start"]);
});

test("a trailing slash still counts as the landing page", () => {
    assert.deepEqual(highlightedLabels("/7.2/"), ["Start"]);
});

test("the what's new page highlights What's new rather than Start", () => {
    assert.deepEqual(highlightedLabels("/7.2/whats-new"), ["What's new"]);
});

test("every guide highlights Guides, since guides have no page tree", () => {
    assert.deepEqual(highlightedLabels("/guides"), ["Guides"]);
    assert.deepEqual(highlightedLabels("/guides/backups-in-ravendb"), ["Guides"]);
});

test("the samples hub highlights Samples", () => {
    assert.deepEqual(highlightedLabels("/samples"), ["Samples"]);
});

test("the switcher shows the product the page belongs to", () => {
    assert.equal(getSidebarNav("/7.2/indexes/map-indexes", VERSION).currentProduct?.label, "RavenDB");
    assert.equal(getSidebarNav("/cloud/cloud-overview", VERSION).currentProduct?.label, "RavenDB Cloud");
    assert.equal(getSidebarNav("/quill/logging", VERSION).currentProduct?.label, "Quill");
});

test("guides and samples belong to no product", () => {
    assert.equal(getSidebarNav("/guides/backups-in-ravendb", VERSION).currentProduct, null);
    assert.equal(getSidebarNav("/samples", VERSION).currentProduct, null);
});

test("each product in the switcher links to its landing page", () => {
    const links = getSidebarNav("/7.2/indexes/map-indexes", VERSION).products.map((product) => [
        product.label,
        product.to,
    ]);
    assert.deepEqual(links, [
        ["RavenDB", "/7.2"],
        ["RavenDB Cloud", "/cloud"],
        ["Quill", "/quill"],
    ]);
});

test("Templates joins the switcher only while you are in it", () => {
    const nav = getSidebarNav("/templates/content-frame", VERSION);
    assert.deepEqual(
        nav.products.map((product) => product.label),
        ["RavenDB", "RavenDB Cloud", "Quill", "Templates"]
    );
    assert.equal(nav.currentProduct?.to, "/templates");
});

test("Start links to the landing page of the version you are reading", () => {
    assert.equal(linkTo(getSidebarNav("/6.2/client-api/creating-document-store", "6.2"), "Start"), "/6.2");
});

test("on guides and samples, Start leads to their own landing page", () => {
    assert.equal(linkTo(getSidebarNav("/guides/backups-in-ravendb", VERSION), "Start"), "/guides");
    assert.equal(linkTo(getSidebarNav("/samples/some-sample", VERSION), "Start"), "/samples");
});

test("the bottom bar adds What's new to Community only in the RavenDB docs", () => {
    const footer = (path: string) => getSidebarNav(path, VERSION).footer.map((item) => [item.label, item.to]);
    assert.deepEqual(footer("/7.2/indexes/map-indexes"), [
        ["What's new", "/7.2/whats-new"],
        ["Community", "https://ravendb.net/community"],
    ]);
    assert.deepEqual(footer("/quill/logging"), [["Community", "https://ravendb.net/community"]]);
    assert.deepEqual(footer("/guides"), [["Community", "https://ravendb.net/community"]]);
});

test("the section links stop at Samples everywhere", () => {
    const labels = (path: string) => getSidebarNav(path, VERSION).links.map((item) => item.label);
    assert.deepEqual(labels("/7.2/indexes/map-indexes"), ["Start", "Guides", "Samples"]);
    assert.deepEqual(labels("/guides/backups-in-ravendb"), ["Start", "Guides", "Samples"]);
    assert.deepEqual(labels("/samples"), ["Start", "Guides", "Samples"]);
});

test("on guides and samples, every product gets a docs link named after it", () => {
    const productDocs = (path: string) =>
        getSidebarNav(path, VERSION).productDocs.map((product) => [product.label, product.to]);
    const expected = [
        ["RavenDB Docs", "/7.2"],
        ["RavenDB Cloud Docs", "/cloud"],
        ["Quill Docs", "/quill"],
    ];
    assert.deepEqual(productDocs("/guides/backups-in-ravendb"), expected);
    assert.deepEqual(productDocs("/samples"), expected);
});

test("inside a product the docs links give way to the switcher", () => {
    assert.deepEqual(getSidebarNav("/7.2/indexes/map-indexes", VERSION).productDocs, []);
    assert.deepEqual(getSidebarNav("/quill/logging", VERSION).productDocs, []);
    assert.deepEqual(getSidebarNav("/templates/content-frame", VERSION).productDocs, []);
});
