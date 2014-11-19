# Standard transactions support

Everything you do by using a session results in sending a batch request when `SaveChanges` is called. All operations made within a session are executed in a single transactional batch on the server side.

RavenDB supports ACID transaction model for document operations and BASE model for indexes. More details about ACID and BASE models you will find [here](../../faq/transaction-support).