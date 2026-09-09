# Registering a license

Read this only after the license check in `install.md` says the server is unlicensed **and** the user has said yes to licensing it. A license applies to the whole server, so on a server the user does not own the decision belongs to the owner.

## Getting a key

A free **Developer** key is requested at <https://ravendb.net/download> → *Developer*, and arrives by email at the address given. Which limits the key grants are carried in the key itself — read them off `/license/status` after activation rather than assuming. A Developer license is licensed for development, not production.

**The details are the user's**: real name, email, company, country, job title, industry, intended use. Collect them in one interaction, say plainly that the key goes to the address they give and that the details reach RavenDB's sales team, and never invent, guess, or reuse details from elsewhere. Filling that form with anything the user did not supply is not an option, whoever is doing the typing.

Then either they submit it themselves, or — with a browser tool — you drive it for them. If you have no browser tool, say so and offer the choice:

> I can fill the license form for you, but I need Playwright to reach the page: `npm i playwright && npx playwright install chromium`. Otherwise, go to <https://ravendb.net/download>, pick *Developer*, fill in the form, and paste the key here when the email arrives.

## Driving the form

Three things about the page are structural rather than cosmetic, and worth knowing before you start: a cookie banner may sit in front of it and swallow the first click; **the license form does not exist until a license tier is chosen**, the fields rendering only after the tier card is clicked; and **the page carries other forms whose controls look like the license form's** — a site-wide search box, and a download-package section with its own terms checkbox and submit button.

Everything else you derive from the page in front of you. This is a marketing page: any selector, label, or option list written down here would be stale before it was read.

1. Pick the tier by the **text of its card**, never by position. Then confirm the license form rendered by a field only it has — a name or email input. A terms checkbox or a submit button proves nothing, since the download section has both already.
2. **Scope the rest to the license form's own container**, then enumerate its visible inputs and read their `name`, `id`, and placeholder off the DOM; map the user's answers onto them by meaning, and read each value back to confirm it landed — a value that did not stick means you addressed something outside the form. Prefer `name` over a generated-looking `id`, which is a per-render value.
3. For each dropdown, **open it and read the options that are actually there**, then choose the closest match to what the user said and tell them which one you picked. Never hardcode an option list; they are re-worded regularly, and picking a stale label silently sends the wrong answer. Do not assume a select-by-value helper works — check whether the control is a real `<select>` first; a scripted one needs its option clicked while the menu is open, because the options unmount on blur.
4. Check the terms checkbox belonging to the license form, leave any marketing-consent checkbox alone unless the user asked for it, and submit with the button whose text offers to generate the license — a sibling button in the same form also submits, so the type alone does not identify it. A successful submission navigates away to a confirmation page rather than answering inline, so read the response there and report it back verbatim rather than declaring success yourself.

If a handle you derived stops matching, read the page again rather than retrying it.

**The form arrives with an invisible reCAPTCHA, which scores the session rather than asking anything, so finding one is not a failure.** It appears alongside the fields, sits in a body-level badge container outside the form, and needs no interaction. What ends the attempt is an interactive challenge or a rate limit — hand the user the manual path rather than working around either.

## Why there is no headless route

The server has endpoints for the same request (`POST /license/free/send-verification-code`, then `POST /license/free/download` with the emailed code, `LicenseType` `Developer` or `Community`), but both refuse outside **setup mode** — on a server already past setup they answer `403` with "RavenDB has already been setup". A dev server started with `Setup.Mode=None` is past setup by definition, so the website form is the only route to a new key.

## Activating the key you were given

The email carries a JSON object: `{"Id": "…", "Name": "…", "Keys": ["…"]}`. Either the user pastes it into Studio's *About* page, or you save it to a file and post **that object as the request body**, unwrapped — the endpoint deserializes the body itself, so a `{"License": …}` wrapper is not what it expects:

```bash
curl -s -o /dev/null -w '%{http_code}\n' -X POST http://localhost:8080/admin/license/activate \
  -H 'Content-Type: application/json' --data-binary @license.json      # 204 on success
```

- `405` means the server runs with `License.CanActivate=false`; no body gets past that.
- A key the server cannot validate comes back as **500** with `InvalidDataException: Could not validate license!` — not a 4xx, so a status-code check that only looks for `>= 400` reads it as a server fault. The wrapped form fails the same way with a different exception (`ArgumentNullException` from the validator), which is how a `{"License": …}` body announces itself. Neither attempt changes the server's license state.
- Activation is a cluster-admin operation: on a secured server it needs a client certificate with that clearance.

Then verify, and report what you find rather than assuming it worked:

```bash
curl -s http://localhost:8080/license/status      # expect Type to have changed, and the Has* flags the app needs to be true
```

An unchanged `Type` means the key did not apply — say so. Keep the key out of the repository and out of shell history: read it from a file, and delete the file when done.
