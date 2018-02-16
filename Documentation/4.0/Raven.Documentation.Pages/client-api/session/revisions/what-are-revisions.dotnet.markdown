# What are Revisions?

RavenDB can create snapshots of the documents (and their attachments) upon every document (or attachment) update or deletion. This feature can be very useful when historic records of the documents are needed e.g. for full audit trails.

## Configuration

Configuration can be set either per [collection]() or for _all collections_ (known as 'default configuration'). The 'default configuration' is applied only if collection-specific configuration is not present or enabled.

Following three options are available per each configuration:

- **Purge on delete** - when this option is active then on document deletion all of the revisions will be also removed
- **Limit # of revisions** - database can limit number of revisions to the given number (old ones will be removed)
- **Limit # of revisions per age** - database will keep revisions at least for the provided time (takes precedence over '# of revisions' option)

## Related articles

- [Revisions : Loading](../../../client-api/session/revisions/loading)
