# How to Check for Entity Changes
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) made to all entities that it has either loaded, stored, deleted, or queried for,  
  and persists to the server only what is needed when `saveChanges()` is called.

* This article describes how to check for changes made to a specific **entity** within a session.  
  To check for changes to **all** tracked entities, see [Check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session).

* In this page:
    * [Check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed#check-for-entity-changes)
    * [Syntax](../../../client-api/session/how-to/check-if-entity-has-changed#syntax)

{NOTE/}

---

{PANEL: Check for entity changes }

* The session's advanced property `hasChanged` indicates whether the specified entity was added, modified, or deleted within the session.

* Note: The _hasChanged_ property is cleared after calling `saveChanges()`.

---

{CODE:nodejs changes_1@client-api\session\howTo\entityChanges.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\session\howTo\entityChanges.js /}

{PANEL/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict entity from session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh entity](../../../client-api/session/how-to/refresh-entity)
