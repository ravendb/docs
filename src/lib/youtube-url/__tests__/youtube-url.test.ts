import { test } from "node:test";
import assert from "node:assert/strict";

import { youTubeVideoId } from "../lib/youtube-url.js";

const ID = "qMJfgQicjwk";

test("resolves the URL forms the docs use to the video ID", () => {
    for (const input of [
        ID,
        `https://www.youtube.com/watch?v=${ID}`,
        `https://www.youtube.com/watch?v=${ID}&feature=shared`,
        `https://youtu.be/${ID}?si=Fxk7alyTQ9bDcf5O`,
    ]) {
        assert.equal(youTubeVideoId(input), ID, input);
    }
});

test("rejects anything that is not a single YouTube video", () => {
    for (const input of [
        "qMJfgQicjw", // 10 characters
        "qMJfgQicjw!",
        `https://www.youtube.com/embed/${ID}`,
        `https://vimeo.com/${ID}`,
        `https://youtube.com.evil.example.com/watch?v=${ID}`,
        "https://www.youtube.com/playlist?list=PL1234567890",
        "https://www.youtube.com/@ravendb_net",
    ]) {
        assert.equal(youTubeVideoId(input), null, input);
    }
});
