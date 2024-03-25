import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function suggestions() {
    {
        //region suggest_1
        // This dynamic query on the 'Products' collection has NO resulting documents
        const products = await session
            .query({ collection: "Products" })
            .whereEquals("Name", "Chai")
            .all();
        //endregion
    }

    {
        //region suggest_2
        // Query for suggested terms for single term:
        // ==========================================

        const suggestions = await session
             // Make a dynamic query on collection 'Products'
            .query({ collection: "Products" })
             // Call 'suggestUsing'
            .suggestUsing(x => x
                 // Request to get terms from field 'Name' that are similar to 'chaig' 
                .byField("Name", "chaig"))
            .execute();
        //endregion
    }

    {
        //region suggest_3
        // The resulting suggested terms:
        // ==============================

        console.log("Suggested terms in field 'Name' that are similar to 'chaig':");
        suggestions["Name"].suggestions.forEach(suggestedTerm => {
            console.log("\t" + suggestedTerm);
        });

        // Suggested terms in field 'Name' that are similar to 'chaig':
        //     chai
        //     chang
        //endregion
    }

    {
        //region suggest_4
        // Query for suggested terms for multiple terms:
        // =============================================

        const suggestions = await session
             // Make a dynamic query on collection 'Products'
            .query({ collection: "Products" })
             // Call 'suggestUsing'
            .suggestUsing(x => x
                 // Request to get terms from field 'Name' that are similar to 'chaig' OR 'tof' 
                .byField("Name", ["chaig", "tof"]))
            .execute();
        //endregion
    }

    {
        //region suggest_5
        // The resulting suggested terms:
        // ==============================
        
        // Suggested terms in field 'Name' that are similar to 'chaig' OR to 'tof':
        //     chai
        //     chang
        //     tofu
        //endregion
    }

    {
        //region suggest_6
        // Query for suggested terms in multiple fields:
        // =============================================
        
        const suggestions = await session
             // Make a dynamic query on collection 'Companies'
            .query({ collection: "Companies" })
             // Call 'suggestUsing' to get suggestions for terms that are 
             // similar to 'chop-soy china' in first document field (e.g. 'Name') 
            .suggestUsing(x => x
                .byField("Name", "chop-soy china"))
             // Call 'AndSuggestUsing' to get suggestions for terms that are 
             // similar to 'maria larson' in an additional field (e.g. 'Contact.Name')
            .andSuggestUsing(x => x
                .byField("Contact.Name", "maria larson"))
            .execute();        
        //endregion
    }

    {
        //region suggest_7
        // The resulting suggested terms:
        // ==============================

        // Suggested terms in field 'Name' that is similar to 'chop-soy china':
        //     chop-suey chinese
        
        // Suggested terms in field 'Contact.Name' that are similar to 'maria larson':
        //     maria larsson
        //     marie bertrand
        //     aria cruz
        //     paula wilson
        //     maria anders
        //endregion
    }

    {
        //region suggest_8
        // Query for suggested terms - customize options and display name:
        // ===============================================================
        
        const suggestions = await session
             // Make a dynamic query on collection 'Products'
            .query({ collection: "Products" })
             // Call 'suggestUsing'
            .suggestUsing(x => x
                .byField("Name", "chaig")
                 // Customize suggestions options
                .withOptions({
                    accuracy: 0.4,
                    pageSize: 5,
                    distance: "JaroWinkler",
                    sortMode: "Popularity"
                })
                 // Customize display name for results
                .withDisplayName("SomeCustomName"))
            .execute();
        //endregion
    }

    {
        //region suggest_9
        // The resulting suggested terms:
        // ==============================

        console.log("Suggested terms:");
        // Results are available under the custom name entry
        suggestions["SomeCustomName"].suggestions.forEach(suggestedTerm => {
            console.log("\t" + suggestedTerm);
        });

        // Suggested terms:
        //     chai
        //     chang
        //     chartreuse verte
        //endregion
    }
}

//region syntax
{    
    {
        //region syntax_1
        // Requesting suggestions for term(s) in a field: 
        suggestUsing(action);
        
        // Requesting suggestions for term(s) in another field in the same query:
        andSuggestUsing(action);
        //endregion
    }
    {
        //region syntax_2
        byField(fieldName, term);
        byField(fieldName, terms);

        withDisplayName(displayName);
        withOptions(options);
        //endregion
    }
}
//endregion
