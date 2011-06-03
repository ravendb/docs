# Partial document updates using the Patching API

This Patching API allows to modify a document without having to get and put it in full. This can save bandwidth, as well as reduce the potential for concurrency conflicts. However, this is not a transactional operation, and as such will consider only the latest write.

