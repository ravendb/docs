# Cloud What's New

Each folder here is one entry: what one RavenDB Cloud deployment brought to customers, with the text in `index.mdx` and
the images next to it. The entries are listed at
[docs.ravendb.net/cloud/whats-new](https://docs.ravendb.net/cloud/whats-new), and the docs build publishes them as a
JSON feed at `/cloud/whats-new/feed.json`, which the Cloud Portal reads.

## What the customer sees

The docs list page shows every entry, newest first, and for each one:

- the title and the date,
- a lead image, where there is one,
- a bulleted list of the features, each as its name and one line about it,
- a **Read more** link to the full entry here.

The Cloud Portal shows only the newest entry, the same way; the history stays here. It opens a **What's new** window
when an entry newer than the one the customer saw last is published, and **What's new** in the side menu opens the
newest entry again. A customer who opens the portal for the first time sees no window: the newest entry is just recorded
as seen. The portal refreshes the feed about every 15 minutes, so an entry shows up there within that time after the
docs site is deployed with it.

Everything below the `{/* truncate */}` marker appears only on the entry's own page here, never in the portal.

## When to add an entry

Add one entry per Cloud deployment, after that deployment reached PROD. Merging earlier announces features customers
don't have yet, and a customer who already saw the entry is not shown it again when the features do arrive.

## Naming an entry

An entry is named after its deployment, as the deployment is named in YouTrack: `October 2026` for the first deployment
of a month and `October II 2026` for a second one. The first keeps its name, because when it ships nobody knows yet
whether the next deployment will be October II or November.

|          | First deployment in October 2026 | Second deployment in October 2026 |
| -------- | -------------------------------- | --------------------------------- |
| Folder   | `2026-10-08-october-2026/`       | `2026-10-26-october-ii-2026/`     |
| `title`  | `October 2026 - <headline>`      | `October II 2026 - <headline>`    |
| `slug`   | `october-2026`                   | `october-ii-2026`                 |

The date in the folder name is the day the deployment reached PROD, whatever month the deployment is named after: a
deployment named February 2026 that reached PROD on 12 March 2026 goes to `2026-03-12-february-2026/`.

## Adding an entry

1. Create the entry's folder, `cloud-whats-new/YYYY-MM-DD-<deployment>/`, and an `index.mdx` in it.
2. Fill in the frontmatter.
3. Write the excerpt: a lead image and a bullet list of features.
4. Add the `{/* truncate */}` marker.
5. Below it, write a `##` section per feature.
6. Put the images in the entry's folder.
7. Check it locally and open a pull request.

### Frontmatter

```yaml
---
title: "October 2026 - Sign in with your own identity provider"
description: "Sign in with your own identity provider, change storage without losing what you've tuned, and more."
slug: october-2026
---
```

| Field         | What it is                                                                                                                                                                                      |
| ------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `title`       | `<deployment> - <the headline change>`. Shown on the list, on the entry page, in the side list of all updates and in the Cloud Portal.                                                         |
| `description` | One sentence summing up the deployment. Used by search engines and as the feed item's `summary`; the portal shows the excerpt instead.                                                          |
| `slug`        | The URL: `/cloud/whats-new/<slug>`, the deployment's name in lowercase with dashes. Without it the URL would carry the date. It must be unique: a repeated slug puts two entries on one URL. |

Don't set `date`, `authors` or `tags`. The date comes from the folder name, and the entries have no authors or tags.

### Excerpt

The excerpt is everything above `{/* truncate */}`. It is a lead image, where one helps, followed by a bullet list with
one item per feature: the feature name in bold, a colon, and one line on what the customer can now do.

```mdx
![Sign in with Google, GitHub, Microsoft or your own identity provider](./cover-oct2026.webp)

- **Sign in with your own identity provider:** Let your team sign in with the work accounts they already use, and let your identity provider manage who gets in.
- **A simpler profile:** Your profile is now three pages, with two-factor authentication behind a single switch.

{/* truncate */}
```

- Write each feature as one bullet, `- **Name:** description`. The feed carries each bullet as a `feature` block (its
  `name` and `text`), and the list page and the portal both render them as bullets.
- Put the colon inside the bold (`**Name:**`) so the name reads as a label; the feed drops it from `name`.
- Keep each line to one sentence. The details belong below the marker, behind Read more.

### Below the truncate marker

Add a `##` section for each feature, named like its bold line, with the details: what changed, why it matters, any limits
(plan, cloud provider, region), and a link to the Cloud page that covers it. Add an image to a section where it helps.

### Images

- Put them in the entry's folder, next to `index.mdx`, and reference them with a relative path: `./<file>.webp`. Each
  deployment keeps its own images, so two deployments in one month never mix them.
- Use `.webp` and keep each file lean (re-encode at quality ~80; covers land well under 100 KB). Name the lead image
  `cover-<month><year>.webp` (e.g. `cover-oct2026.webp`), so two deployments never collide on one `cover` file.
- Copy a screenshot from `cloud/assets` or `cloud/portal/assets`, or take a new one. Never reference those folders
  directly: an entry records one deployment and must not change when the docs re-shoot a figure.
- Give each image alt text that says what it shows.
- An image in the excerpt also appears in the portal. Put an image below the marker if it should appear only here.

### Links

Link to Cloud pages by URL (`/cloud/cloud-sso`), not by file path: links to files of the `cloud` docs are not resolved
from an entry. Take `#anchors` from the built page, not from a guessed heading slug.

### Checking it locally

```bash
npm run start:current
```

The entry appears at `http://localhost:3000/cloud/whats-new`. The JSON feed is written only by a build:

```bash
npm run build:current
npm run serve
```

Then open `http://localhost:3000/cloud/whats-new/feed.json`. Each item carries the excerpt the portal shows in
`_ravendb.excerpt`, as `feature` (name + text), `paragraph` and `image` blocks.

## Rules that are easy to miss

- **One entry per day.** The portal picks the newest entry and decides whether it is new by date, so a second entry
  with the same date is not shown as new to a customer who already saw the first.
- **Never date an entry in the future.** It is published right away and stays the newest entry, so the portal keeps
  showing it instead of any entry merged after it with an earlier date.
- **Don't change the date of a published entry.** Moving it later shows it again to everyone who already saw it.
  Fixing the text or an image is safe.
