import { test } from "node:test";
import assert from "node:assert/strict";

import { activeAnchorId, type AnchorBox, type TocViewport } from "../tocHighlight";

const NAVBAR = 60;
const HEIGHT = 1000;

function heading(id: string, top: number): AnchorBox {
    return { id, top, bottom: top + 40 };
}

function viewport(overrides: Partial<TocViewport> = {}): TocViewport {
    return { anchorTopOffset: NAVBAR, height: HEIGHT, atBottom: false, hashId: null, ...overrides };
}

test("a heading in the top half of the viewport is active", () => {
    const anchors = [heading("first", -800), heading("second", 120), heading("third", 1400)];
    assert.equal(activeAnchorId(anchors, viewport()), "second");
});

test("while the next heading is still in the bottom half, the previous one stays active", () => {
    const anchors = [heading("first", -800), heading("second", 700)];
    assert.equal(activeAnchorId(anchors, viewport()), "first");
});

test("at the bottom of the page, a clicked last heading in the bottom half is active", () => {
    // The page cannot scroll the short last section any higher than this.
    const anchors = [heading("daily", 300), heading("azure", 872)];
    assert.equal(activeAnchorId(anchors, viewport({ atBottom: true, hashId: "azure" })), "azure");
});

test("at the bottom of the page without a click, the last heading in view is active", () => {
    const anchors = [heading("daily", 300), heading("azure", 872)];
    assert.equal(activeAnchorId(anchors, viewport({ atBottom: true })), "azure");
});

test("at the bottom of the page, a clicked heading still in view keeps the highlight", () => {
    const anchors = [heading("daily", 300), heading("azure", 872)];
    assert.equal(activeAnchorId(anchors, viewport({ atBottom: true, hashId: "daily" })), "daily");
});

test("at the bottom of the page, a clicked heading scrolled out of view gives way to the last one in view", () => {
    const anchors = [heading("intro", -2000), heading("azure", 872)];
    assert.equal(activeAnchorId(anchors, viewport({ atBottom: true, hashId: "intro" })), "azure");
});

test("at the bottom of a page whose headings were all scrolled past, the last heading is active", () => {
    const anchors = [heading("first", -2000), heading("last", -400)];
    assert.equal(activeAnchorId(anchors, viewport({ atBottom: true })), "last");
});

test("before the first heading nothing is active", () => {
    const anchors = [heading("first", 800)];
    assert.equal(activeAnchorId(anchors, viewport()), null);
});
