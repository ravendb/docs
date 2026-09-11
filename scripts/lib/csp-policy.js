/**
 * Content-Security-Policy for docs.ravendb.net, pushed by scripts/sync-csp.ps1.
 *
 * Served by the CloudFront response headers policy, not by Docusaurus, so a local build never
 * exercises it and a blocked tag only shows up as a console violation in production.
 *
 * Every host is justified by a tag that runs on the site: the GTM-TDH4JWF2 container, the Reo
 * snippet in docusaurus.config.ts, and Algolia DocSearch. Several container tags are Custom HTML,
 * i.e. arbitrary JavaScript published from the GTM console, and default-src carries
 * 'unsafe-inline', so this policy constrains which hosts a tag can reach, not what it does to the
 * page. Treat the allowlist as an inventory, not a sandbox, and update it in the same change that
 * touches the container.
 *
 * Gotchas: `*.example.com` does not match the apex `example.com`; an absent directive falls back
 * to default-src, which carries no third-party hosts; and a host source with no scheme does not
 * cover a wss: URL in Chrome, even on an https document.
 *
 * CommonJS for cross-consumer interop.
 */

// Directive order is preserved in the emitted header, so keep related directives adjacent for
// readability in the CloudFront console.
const CSP_DIRECTIVES = {
    // Site's own bundles. 'unsafe-inline'/'unsafe-eval' are inherited by
    // script-src-attr and style-src: Docusaurus emits an inline theme bootstrap and
    // GTM evaluates its container at runtime.
    "default-src": ["'self'", "'unsafe-inline'", "'unsafe-eval'"],

    // Tracking pixels are images from arbitrary ad hosts. Enumerating them is not
    // practical and an image is a low-value sink.
    "img-src": ["'self'", "data:", "*"],

    // <script src> elements. Anything missing here is a tag that never runs.
    "script-src-elem": [
        "'self'",
        "'unsafe-inline'",
        // Google Tag Manager loader + GA4 + the Google tag
        "www.googletagmanager.com",
        // Google Ads conversion tracking (conversion_async.js)
        "www.googleadservices.com",
        // Google Ads remarketing
        "pagead2.googlesyndication.com",
        // Google Ads view-through conversion pings, injected as <script> by the Google
        // tag. The conversion tags raise script-src-elem violations without it.
        "*.g.doubleclick.net",
        // Google tag "taggy" agent (cross-domain conversion measurement)
        "cct.google",
        // CookieYes CMP: script.js, which in turn loads banner.js
        "cdn-cookieyes.com",
        // Meta Pixel: fbevents.js
        "connect.facebook.net",
        // Meta CAPI parameter builder, injected by the Meta Pixel template
        "capi-automation.s3.us-east-2.amazonaws.com",
        // Reo.dev (snippet lives in docusaurus.config.ts headTags)
        "static.reo.dev",
        // LinkedIn Insight Tag: insight.min.js
        "snap.licdn.com",
        // X/Twitter pixel: uwt.js
        "static.ads-twitter.com",
        // Reddit pixel: pixel.js
        "www.redditstatic.com",
        // Zoho SalesIQ chat widget and Marketing Automation, both Custom HTML tags.
        // The widget URL is only a loader; its bundle comes from zohocdn.com, a
        // separate registrable domain that the zoho.com wildcard does NOT cover.
        "*.zoho.com",
        "*.zohostatic.com",
        "*.zohocdn.com",
        // Common Room signals.js (Custom HTML tag, visitor identification)
        "*.cr-relay.com",
        // InfoQ beacon.min.js (Custom HTML tag, paid-placement measurement)
        "*.infoq.com",
    ],

    // fetch / XHR / sendBeacon. This is where measurement hits actually land, so a
    // gap here silently drops data rather than breaking a page.
    "connect-src": [
        "'self'",
        // Algolia DocSearch. The client falls back from <appid>-dsn.algolia.net to
        // <appid>-{1,2,3}.algolianet.com on error, so both apexes are needed.
        "*.algolia.net",
        "*.algolianet.com",
        // GTM container fetches
        "*.googletagmanager.com",
        // GA4 collect (www. and region1. hosts)
        "*.google-analytics.com",
        // Google signals. The apex is a separate host from the wildcard.
        "analytics.google.com",
        "*.analytics.google.com",
        // Google Ads / Floodlight measurement
        "*.g.doubleclick.net",
        "ad.doubleclick.net",
        "pagead2.googlesyndication.com",
        "ade.googlesyndication.com",
        // Google Ads user lists, enhanced conversions, conversion linker
        "www.google.com",
        "www.googleadservices.com",
        "adservice.google.com",
        "cct.google",
        // CookieYes: config/translation JSON, consent log, and the geo-IP lookup the
        // banner awaits before it renders.
        "cdn-cookieyes.com",
        "log.cookieyes.com",
        "directory.cookieyes.com",
        // Meta Pixel event delivery
        "connect.facebook.net",
        "www.facebook.com",
        // Reo.dev ingestion
        "*.reo.dev",
        // LinkedIn Insight Tag conversion beacons
        "px.ads.linkedin.com",
        "*.linkedin.com",
        // X/Twitter pixel event delivery
        "analytics.twitter.com",
        "t.co",
        // Reddit pixel event delivery
        "*.reddit.com",
        "*.redditstatic.com",
        // Zoho SalesIQ. The chat holds a long-lived socket, which connect-src governs
        // for ws:/wss: as well as fetch.
        "*.zoho.com",
        "*.zohopublic.com",
        "*.zohostatic.com",
        "*.zohocdn.com",
        // SalesIQ's visitor-tracking socket (vts.zohopublic.com/watchws). A host
        // source with no scheme does NOT cover a wss: URL in Chrome, even on an https
        // document, so the socket needs its own scheme-qualified entry. Listing only
        // `*.zohopublic.com` gets the connection refused with a connect-src violation.
        "wss://*.zoho.com",
        "wss://*.zohopublic.com",
        // Common Room signal ingestion
        "*.cr-relay.com",
        // InfoQ beacon delivery
        "*.infoq.com",
    ],

    // Iframes injected by tags: the GTM <noscript> fallback and the cookie-sync
    // frames Google Ads remarketing and Meta drop after a conversion.
    "frame-src": [
        "'self'",
        "www.googletagmanager.com",
        "*.g.doubleclick.net",
        "td.doubleclick.net",
        "www.google.com",
        "www.facebook.com",
        // Zoho SalesIQ renders the chat panel in an iframe
        "*.zoho.com",
    ],

    // Stylesheets, fonts and workers pulled in by third-party widgets (Zoho SalesIQ
    // is the main one). These sinks are deliberately permissive: the controls that
    // matter are script-src-elem and connect-src, and pinning every vendor's asset
    // CDN buys little while guaranteeing console noise the next time one of them
    // moves a file.
    "style-src": ["'self'", "'unsafe-inline'", "*"],

    "font-src": ["'self'", "data:", "*"],

    "worker-src": ["'self'", "blob:"],
};

function buildCspHeader(directives = CSP_DIRECTIVES) {
    return Object.entries(directives)
        .map(([name, sources]) => `${name} ${sources.join(" ")}`)
        .join("; ");
}

module.exports = { CSP_DIRECTIVES, buildCspHeader };

// Runnable directly: `node scripts/lib/csp-policy.js` prints the header value.
if (require.main === module) {
    console.log(buildCspHeader());
}
