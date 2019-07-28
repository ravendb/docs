import * as assert from "assert";
import { DocumentStore, DocumentConventions } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    //region conventions_1
    const store = new DocumentStore();
    const conventions = store.conventions;
    
    // customizations go here

    store.initialize();
    //endregion
}

function examples() {
    const conventions = new DocumentConventions();

    //region MaxHttpCacheSize
    conventions.maxHttpCacheSize = 256 * 1024 * 1024;
    //endregion

    //region MaxNumberOfRequestsPerSession
    conventions.maxNumberOfRequestsPerSession = 10;
    //endregion

    //region UseOptimisticConcurrency
    conventions.useOptimisticConcurrency = true;
    //endregion

    //region DisableTopologyUpdates
    conventions.disableTopologyUpdates = false;
    //endregion

    //region disable_cache
    conventions.maxHttpCacheSize = 0;
    //endregion

    //region UseCompression
    conventions.useCompression = true;
    //endregion

    //region PropertyCasing
    conventions.remoteEntityFieldNameConvention = "pascal";
    conventions.entityFieldNameConvention = "camel";

    //endregion

    //region StoreDatesWithTimezoneInfo
    conventions.storeDatesWithTimezoneInfo = true;
    //endregion

    //region StoreDatesInUtc
    conventions.storeDatesInUtc = true;
    //endregion
}
