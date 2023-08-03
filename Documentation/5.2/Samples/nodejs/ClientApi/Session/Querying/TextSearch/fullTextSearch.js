import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function fullTextSearch() {
    {
        //region fts_1
        const employees = await session
             // Make a dynamic query on 'Employees' collection    
            .query({ collection: "Employees" })
             // * Call 'search' to make a Full-Text search
             // * Search is case-insensitive
             // * Look for documents containing the term 'University' within their 'Notes' field
            .search("Notes", "University")
            .all();

        // Results will contain Employee documents that have
        // any case variation of the term 'university' in their 'Notes' field.
        //endregion
    }
    {
        //region fts_2
        const employees = await session
            .query({ collection: "Employees" })
             // * Pass multiple terms in a single string, separated by spaces.
             // * Pass 'AND' as the third parameter
            .search("Notes", "College German", "AND")
            .all();

        // * Results will contain Employee documents that have BOTH 'College' AND 'German'
        //   in their 'Notes' field.
        //   
        // * Search is case-insensitive.
        //endregion
    }
    {
        //region fts_3
        const employees = await session
            .query({ collection: "Employees" })
             // * Pass multiple terms in a single string, separated by spaces.
             // * Pass 'OR' as the third parameter (or don't pass this param at all) 
            .search("Notes", "University Sales Japanese", "OR")
            .all();

        // * Results will contain Employee documents that have
        //   either 'University' OR 'Sales' OR 'Japanese' within their 'Notes' field
        //
        // * Search is case-insensitive.
        //endregion
    }
    {
        //region fts_4
        const employees = await session  
            .query({ collection: "Employees" })
            .search("Notes", "French")
             // Operator OR will be used between the two 'Search' calls by default
            .search("Title", "President")
            .all();

        // * Results will contain Employee documents that have:
        //   ('French' in their 'Notes' field) OR ('President' in their 'Title' field)
        //
        // * Search is case-insensitive.
        //endregion
    }
    {
        //region fts_5
        const companies = await session
            .query({ collection: "Companies" })
            .whereEquals("Contact.Title", "Owner")
             // Operator AND will be used with previous 'where' predicate
             // Call 'openSubclause' to open predicate block
            .openSubclause()
            .search("Address.Country", "France")
             // Operator OR will be used between the two 'Search' calls by default
            .search("Name", "Markets")
             // Call 'closeSubclause' to close predicate block
            .closeSubclause()
            .all();

        // * Results will contain Company documents that have:
        //   ('Owner' as the 'Contact.Title')
        //   AND
        //   (are located in 'France' OR have 'Markets' in their 'Name' field)
        //
        // * Search is case-insensitive
        //endregion
    }
    {
        //region fts_6
        const employees = await session
            .query({ collection: "Employees" })
            .search("Notes", "French")
             // Call 'andAlso' so that operator AND will be used with previous 'search' call
            .andAlso()
            .search("Title", "Manager")
            .all();
        
        // * Results will contain Employee documents that have:
        //   ('French' in their 'Notes' field)
        //   AND
        //   ('Manager' in their 'Title' field)
        //
        // * Search is case-insensitive
        //endregion
    }
    {
        //region fts_7
        const employees = await session
            .query({ collection: "Employees" })
            .search("Notes", "French")
            .andAlso()
             // Call 'openSubclause' to open predicate block
            .openSubclause()
             // Call 'not' to negate the next search call
            .not()
            .search("Title", "Manager")
             // Call 'closeSubclause' to close predicate block
            .closeSubclause()
            .all();

        // * Results will contain Employee documents that have:
        //   ('French' in their 'Notes' field)
        //   AND
        //   (do NOT have 'Manager' in their 'Title' field)
        //
        // * Search is case-insensitive
        //endregion
    }
    {
        //region fts_8
        const companies = await session
            .query({ collection: "Companies" })
             // * Look for documents that contain:
             //   the term 'USA' OR 'London' in any field within the complex 'Address' object
            .search("Address", "USA London")
            .all();

        // * Results will contain Company documents that are located either in 'USA' OR in 'London'.
        // * Search is case-insensitive.
        //endregion
    }
    {
        //region fts_9
        const employees = await session
            .query({ collection: "Employees" })
             // Use '*' to replace one ore more characters
            .search("Notes", "art*")
            .search("Notes", "*logy")
            .search("Notes", "*mark*")
            .all();

        // Results will contain Employee documents that have in their 'Notes' field:
        // (terms that start with 'art')  OR
        // (terms that end with 'logy') OR
        // (terms that have the text 'mark' in the middle) 
        //
        // * Search is case-insensitive
        //endregion
    }
}

{
    //region syntax
    // Available overloads:
    search(fieldName, searchTerms);
    search(fieldName, searchTerms, operator);
    //endregion
}
