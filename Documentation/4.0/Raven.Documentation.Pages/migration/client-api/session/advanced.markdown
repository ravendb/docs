# Changes related to IAdvancedDocumentSessionOperations

Changes in advanced session operations accessible by `session.Advanced`.

{PANEL:GetEtagFor}

As [documents don't have etags any longer](../../../migration/client-api/introduction#etags) but they use change vectors instead the method `GetEtagFor` has been replaced by `GetChangeVectorFor`.

{PANEL/}


{PANEL:MarkReadOnly}

Marking documents as read-only isn't supported. 

{PANEL/}


{PANEL:ExplicitlyVersion}

The method has been removed. Document Revisions configuration don't use `ExcludeUnlessExplicit` option anymore.

{PANEL/}

{PANEL:Dealing with Non Authoritative Information}

DTC support has been dropped.

{PANEL/}



