import * as assert from "assert";
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = store.openSession();

async function creationApi() {

    {
        //region create_syntax_1
        // Available overloads:
        // ====================
        
        create(options);
        
        create(options, database);
        
        create(documentType);
        //endregion
    }
    {
        //region create_syntax_2
        // The SubscriptionCreationOptions object:
        // =======================================
        {
            name;
            query;
            includes;
            changeVector;
            mentorNode;
            pinToMentorNode;
            disabled;
            documentType;
        }
        //endregion
    }
    {
        //region create_syntax_3
        // Available overloads:
        // ====================

        createForRevisions(options);

        createForRevisions(options, database);
        //endregion
    }
    {
        //region update_syntax_1
        // Available overloads:
        // ====================

        update(options);

        update(options, database);
        //endregion
    }
    {
        //region update_syntax_2
        // The SubscriptionUpdateOptions object:
        // =====================================
        {
            id;
            createNew;
        }
        //endregion
    }
    {
        //region include_syntax_1
        includeDocuments(path);
        //endregion
    }
    {
        //region include_syntax_2
        // Include a single counter
        includeCounter(name);

        // Include multiple counters
        includeCounters(names);

        // Include ALL counters from ALL documents that match the subscription criteria
        includeAllCounters();
        //endregion
    }
    {
        //region include_syntax_3
        includeTimeSeries(name, type, time);
        includeTimeSeries(name, type, count);
        
        includeTimeSeries(names, type, time);
        includeTimeSeries(names, type, count);

        includeAllTimeSeries(type, time);
        includeAllTimeSeries(type, count);
        //endregion
    }
}
