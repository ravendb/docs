# How to Ignore Entity Changes

---

{NOTE: }

* By default, each session tracks the changes made in all the entities it has ever stored, loaded, or queried for.  
  All changes are then persisted when `saveChanges` is called.

* To ignore entity changes when calling `saveChanges`, **disable entity tracking** by any of the following:
    * [Disable tracking for a specific entity in session](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-a-specific-entity-in-session)
    * [Disable tracking for all entities in session](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-all-entities-in-session)
    * [Disable tracking for query results](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-query-results)
    * [Customize tracking in conventions](../../../client-api/session/configuration/how-to-disable-tracking#customize-tracking-in-conventions)
{NOTE/}

---

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
