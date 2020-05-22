# Migration: Changes Related to IAdvancedDocumentSessionOperations

Changes in advanced session operations are accessible by `session.Advanced`.

{PANEL:GetEtagFor}

[Documents don't have etags any longer](../../../migration/client-api/introduction#etags) but they use change vectors instead the method `GetEtagFor`. It has been replaced by `GetChangeVectorFor`.

{PANEL/}


{PANEL:MarkReadOnly}

Marking documents as read-only isn't supported. 

{PANEL/}


{PANEL:ExplicitlyVersion}

The method has been removed. Document revisions configuration doesn't use the `ExcludeUnlessExplicit` option anymore.

{PANEL/}

{PANEL:Dealing with Non Authoritative Information}

DTC support has been dropped.

{PANEL/}



