import assert from "node:assert/strict";
import { describe, it } from "node:test";
import type { BlogFeedItem, BlogPost } from "@docusaurus/plugin-content-blog";
import { createWhatsNewFeedItems, toExcerptBlocks } from "../index";

const SSO_IMAGE = "https://docs.ravendb.net/assets/images/portal-sso-wizard-a9dfab7ec31352f7.png";
const STORAGE_IMAGE = "https://docs.ravendb.net/assets/images/portal-edit-storage-0123456789abcdef.png";

const CONTENT_HTML =
    `<p><strong>SSO</strong></p><span><img width="1960" height="820" src="${SSO_IMAGE}" alt=""></span>` +
    `<h2>Storage</h2><img src="${STORAGE_IMAGE}" alt="">`;

describe("toExcerptBlocks", () => {
    it("turns each feature into its heading, description and image, in order", () => {
        const source =
            "**Sign in with your own identity provider**\n\n" +
            "Let your team sign in with the work accounts they\nalready use.\n\n" +
            "![SSO](./portal-sso-wizard.png)\n\n" +
            "**Change storage**\n\n" +
            "Keep your IOPS.\n\n" +
            "![Storage](./portal-edit-storage.png)\n\n" +
            "{/* truncate */}\n\n## Sign in with your own identity provider";

        assert.deepEqual(toExcerptBlocks(source, CONTENT_HTML), [
            { type: "heading", text: "Sign in with your own identity provider" },
            { type: "paragraph", text: "Let your team sign in with the work accounts they already use." },
            { type: "image", url: SSO_IMAGE },
            { type: "heading", text: "Change storage" },
            { type: "paragraph", text: "Keep your IOPS." },
            { type: "image", url: STORAGE_IMAGE },
        ]);
    });

    it("keeps a feature without an image", () => {
        const source = "**Sign in with Microsoft**\n\nNext to Google and GitHub.\n\n{/* truncate */}";

        assert.deepEqual(toExcerptBlocks(source, CONTENT_HTML), [
            { type: "heading", text: "Sign in with Microsoft" },
            { type: "paragraph", text: "Next to Google and GitHub." },
        ]);
    });

    it("keeps a paragraph that only starts with bold text as a paragraph", () => {
        const source = "**SCIM** keeps your users in sync, and **groups** map to roles.\n\n{/* truncate */}";

        assert.deepEqual(toExcerptBlocks(source, CONTENT_HTML), [
            { type: "paragraph", text: "SCIM keeps your users in sync, and groups map to roles." },
        ]);
    });

    it("leaves out everything below the truncate marker", () => {
        const source = "__SSO__\n\nSummary.\n\n<!-- truncate -->\n\n![Storage](./portal-edit-storage.png)";

        assert.deepEqual(toExcerptBlocks(source, CONTENT_HTML), [
            { type: "heading", text: "SSO" },
            { type: "paragraph", text: "Summary." },
        ]);
    });

    it("turns Markdown into plain text", () => {
        const source = "Use **SCIM** and [group mapping](/cloud/cloud-sso) with `openid` scopes.\n\n{/* truncate */}";

        assert.deepEqual(toExcerptBlocks(source, CONTENT_HTML), [
            { type: "paragraph", text: "Use SCIM and group mapping with openid scopes." },
        ]);
    });

    it("keeps an absolute image URL as it is", () => {
        const source = "![Logo](https://ravendb.net/logo.png)\n\n{/* truncate */}";

        assert.deepEqual(toExcerptBlocks(source, CONTENT_HTML), [
            { type: "image", url: "https://ravendb.net/logo.png" },
        ]);
    });

    it("treats the whole entry as the excerpt when there is no truncate marker, as the list page does", () => {
        const source = "**SSO**\n\nSummary.\n\n![SSO](./portal-sso-wizard.png)";

        assert.deepEqual(toExcerptBlocks(source, CONTENT_HTML), [
            { type: "heading", text: "SSO" },
            { type: "paragraph", text: "Summary." },
            { type: "image", url: SSO_IMAGE },
        ]);
    });
});

describe("createWhatsNewFeedItems", () => {
    it("adds the excerpt blocks to each item and keeps the default fields", async () => {
        const blogPosts = [
            { content: "**SSO**\n\nSummary.\n\n![SSO](./portal-sso-wizard.png)\n\n{/* truncate */}" },
            {
                content: "**Alerts**\n\nSummary.\n\n{/* truncate */}\n\n![Storage](./portal-edit-storage.png)",
            },
        ] as unknown as BlogPost[];
        const defaultItems: BlogFeedItem[] = [
            { title: "October", id: "october", link: "october", date: new Date("2026-10-26"), content: CONTENT_HTML },
            {
                title: "September",
                id: "september",
                link: "september",
                date: new Date("2026-09-08"),
                content: CONTENT_HTML,
            },
        ];

        const items = await createWhatsNewFeedItems({
            blogPosts,
            siteConfig: {} as never,
            outDir: "build",
            defaultCreateFeedItems: async () => defaultItems,
        });

        assert.deepEqual(
            items.map((item) => [item.title, item.link, item.extensions?.[0].objects.excerpt]),
            [
                [
                    "October",
                    "october",
                    [
                        { type: "heading", text: "SSO" },
                        { type: "paragraph", text: "Summary." },
                        { type: "image", url: SSO_IMAGE },
                    ],
                ],
                [
                    "September",
                    "september",
                    [
                        { type: "heading", text: "Alerts" },
                        { type: "paragraph", text: "Summary." },
                    ],
                ],
            ]
        );
    });
});
