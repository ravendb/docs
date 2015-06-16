#Replication stops working after a node was recreated 
##Symptoms:
- A node in RavenDB 2.5  Replication configuration that has been removed, and then readded, as a fresh node, without any previous data, but the same name
- The resurrected node receives, but  does not send any new messages to existing nodes

##Cause:
###tldr: 
Inconsistency between last replicated etag in resurrected node and last received etags in the existing nodes.

###Full explanation:

In replication, a database asks other nodes in the configuration about the last etag they receive from it, in order to decide, whether to send them data or not. In our case, the nodes in the configuration has the "original" database last etag stored, although the "resurrected" last etag is nullified.  As explained, the resurrected DB will ask the other nodes for the last etag they received from it, but it'll receive an etag that is greater and therefore it will conclude that there is nothing new to send.

##Resolution:
Removing and adding the configuration of the replication to the "resurrected" node in the other nodes. This will cause the "resurrected" node to resend all it's data to the other nodes
