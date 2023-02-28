# Cloudflare Workers
---

{NOTE: What are Cloudflare Workers?}

[Cloudflare Edge Workers](https://cloudflare.com) let's you move compute workloads closer to your users with over 250+ metro data centers.

Edge Workers run on the V8 Runtime or on Deno. RavenDB provides support through its Node.js SDK and in this article,
you will learn how to get started quickly with the [RavenDB Cloudflare Worker template](https://github.com/ravendb/cloudflare-worker-template) connected to RavenDB Cloud.

{NOTE/}

{PANEL: Configure Cloudflare Account}

RavenDB uses client certificate authentication which is handled through Cloudflare mTLS authentication
for Workers. You will need to upload your certificate to your Cloudflare account so that it can be
accessed and bound to your Worker.

First, download your RavenDB client certificate (`.cer` and `.key` files) you use to authenticate.
Follow the guides for either Cloud or for self-hosting.

You will also need:

- A free or paid Cloudflare account
- cURL, Postman, or some other HTTP utility

{NOTE: Wrangler support coming soon}

As of February 2023, the `wrangler2` CLI utility from Cloudflare does not yet support
uploading client certificates from the command-line, so this guide will walkthrough
uploading manually through the Cloudflare API.

{NOTE/}

---

### Get a Cloudflare API Token

Login to your Cloudflare account.

Go to Settings => API Keys

{PANEL/}
