import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function conventions() {
    {
        //region conventions_1
        const documentStore = new DocumentStore(["serverUrl_1", "serverUrl_2", "..."], "DefaultDB");

        // Set conventions HERE, e.g.:
        documentStore.conventions.maxNumberOfRequestsPerSession = 50;
        documentStore.conventions.disableTopologyUpdates = true;

        documentStore.initialize();

        // * Here you can interact with the RavenDB store:
        //   open sessions, create or query for documents, perform operations, etc.

        // * Conventions CANNOT be set here after calling initialize()
        //endregion
    }
   
    //region syntaxDefinitions 

    //region customFetchSyntax
    // Returns an object
    get customFetch();
    // Set an object bound to worker with type: mtls_certificate
    set customFetch(customFetch);
    //endregion
    
    //region disableAtomicDocumentWritesInClusterWideTransactionSyntax
    // Returns a boolean value
    get disableAtomicDocumentWritesInClusterWideTransaction();
    // Set a boolean value
    set disableAtomicDocumentWritesInClusterWideTransaction(
        disableAtomicDocumentWritesInClusterWideTransaction
    );
    //endregion

    //region disableTopologyUpdatesSyntax
    // Returns a boolean value
    get disableTopologyUpdates();
    // Set a boolean value
    set disableTopologyUpdates(value);
    //endregion

    //region findCollectionNameSyntax
    // Returns a method
    get findCollectionName();
    // Set a method
    set findCollectionName(value);
    //endregion

    //region findJsTypeSyntax
    // Returns a method
    get findJsType();
    // Set a method
    set findJsType(value);
    //endregion

    //region findJsTypeNameSyntax
    // Returns a method
    get findJsTypeName();
    // Set a method
    set findJsTypeName(value);
    //endregion

    //region firstBroadcastAttemptTimeoutSyntax
    // Returns a number
    get firstBroadcastAttemptTimeout();
    // Set a number
    set firstBroadcastAttemptTimeout(firstBroadcastAttemptTimeout);
    //endregion

    //region identityPartsSeparatorSyntax
    // Returns a string
    get identityPartsSeparator();
    // Set a string
    set identityPartsSeparator(value);
    //endregion

    //region maxHttpCacheSizeSyntax
    // Returns a number
    get maxHttpCacheSize();
    // Set a number
    set maxHttpCacheSize(value);
    //endregion

    //region maxNumberOfRequestsPerSessionSyntax
    // Returns a number
    get maxNumberOfRequestsPerSession();
    // Set a number
    set maxNumberOfRequestsPerSession(value);
    //endregion

    //region requestTimeoutSyntax
    // Returns a number
    get requestTimeout();
    // Set a number
    set requestTimeout(value);
    //endregion

    //region secondBroadcastAttemptTimeoutSyntax
    // Returns a number
    get secondBroadcastAttemptTimeout();
    // Set a number
    set secondBroadcastAttemptTimeout(timeout);
    //endregion

    //region sendApplicationIdentifierSyntax
    // Returns a boolean
    get sendApplicationIdentifier();
    // Set a boolean
    set sendApplicationIdentifier(sendApplicationIdentifier)
    //endregion
    
    //region storeDatesInUtcSyntax
    // Returns a boolean
    get storeDatesInUtc();
    // Set a boolean
    set storeDatesInUtc(value);
    //endregion

    //region storeDatesWithTimezoneInfoSyntax
    // Returns a boolean
    get storeDatesWithTimezoneInfo();
    // Set a boolean
    set storeDatesWithTimezoneInfo(value);
    //endregion
    
    //region syncJsonParseLimitSyntax
    // Returns a number
    get syncJsonParseLimit();
    // Set a number
    set syncJsonParseLimit(value);
    //endregion

    //region throwIfQueryPageSizeIsNotSetSyntax
    // Returns a boolean
    get throwIfQueryPageSizeIsNotSet();
    // Set a boolean
    set throwIfQueryPageSizeIsNotSet(value);
    //endregion
    
    //region transformClassCollectionNameToDocumentIdPrefixSyntax
    // Returns a method
    get transformClassCollectionNameToDocumentIdPrefix();
    // Set a method
    set transformClassCollectionNameToDocumentIdPrefix(value);
    //endregion

    //region useCompressionSyntax
    // Returns a boolean
    get useCompression();
    // Set a boolean
    set useCompression(value);
    //endregion

    //region useJsonlStreamingSyntax
    // Returns a boolean
    get useJsonlStreaming();
    // Set a boolean
    set useJsonlStreaming(value);
    //endregion

    //region useOptimisticConcurrencySyntax
    // Returns a boolean
    get useOptimisticConcurrency();
    // Set a boolean
    set useOptimisticConcurrency(value);
    //endregion

    //region waitForIndexesAfterSaveChangesTimeoutSyntax
    // Returns a number
    get waitForIndexesAfterSaveChangesTimeout();
    // Set a number
    set waitForIndexesAfterSaveChangesTimeout(value);
    //endregion

    //region waitForNonStaleResultsTimeoutSyntax
    // Returns a number
    get waitForNonStaleResultsTimeout();
    // Set a number
    set waitForNonStaleResultsTimeout(value);
    //endregion

    //region waitForReplicationAfterSaveChangesTimeoutSyntax
    // Returns a number
    get waitForReplicationAfterSaveChangesTimeout();
    // Set a number
    set waitForReplicationAfterSaveChangesTimeout(value);
    //endregion
    
    //endregion 
   
}
