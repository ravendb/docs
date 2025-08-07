import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

async function patchRequests() {
    {
        //region patch_firstName_session
        // Modify FirstName to Robert using the 'patch' method
        // ===================================================
        
        session.advanced.patch("employees/1-A", "FirstName", "Robert");        
        await session.saveChanges();
        //endregion
    }
    {
        //region patch_firstName_defer
        // Modify FirstName to Robert using 'defer' with 'PatchCommandData'
        // ================================================================
        
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.FirstName = args.FirstName;";
        patchRequest.values = { FirstName: "Robert" };
                
        const patchCommand = new PatchCommandData("employees/1-A", null, patchRequest);        
        session.advanced.defer(patchCommand);
        
        await session.saveChanges();
        //endregion
    }
    {
        //region patch_firstName_operation
        // Modify FirstName to Robert via 'PatchOperation' on the documentStore
        // ====================================================================
        
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.FirstName = args.FirstName;";
        patchRequest.values = { FirstName: "Robert" };
        
        const patchOp = new PatchOperation("employees/1-A", null, patchRequest);        
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region patch_firstName_and_lastName_session
        // Modify FirstName to Robert and LastName to Carter in single request
        // ===================================================================
        
        // The two Patch operations below are sent via 'saveChanges()' which complete transactionally,
        // as this call generates a single HTTP request to the database. 
        // Either both will succeed - or both will be rolled back - since they are applied within the same
        // transaction.
        
        // However, on the server side, the two Patch operations are still executed separately.
        // To achieve atomicity at the level of a single server-side operation, use 'defer' or an 'operation'.

        session.advanced.patch("employees/1-A", "FirstName", "Robert");
        session.advanced.patch("employees/1-A", "LastName", "Carter");
        
        await session.saveChanges();
        //endregion
    }
    {
        //region patch_firstName_and_lastName_defer
        // Modify FirstName to Robert and LastName to Carter in single request
        // ===================================================================

        // Note that here we do maintain the operation's atomicity
        const patchRequest = new PatchRequest();
        patchRequest.script = `this.FirstName = args.FirstName;
                               this.LastName = args.LastName;`;
        patchRequest.values = { 
            FirstName: "Robert",
            LastName: "Carter"
        };
 
        const patchCommand = new PatchCommandData("employees/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region patch_firstName_and_lastName_operation
        // Modify FirstName to Robert and LastName to Carter in single request
        // ===================================================================

        // Note that here we do maintain the operation's atomicity
        const patchRequest = new PatchRequest();
        patchRequest.script = `this.FirstName = args.FirstName;
                               this.LastName = args.LastName;`;
        patchRequest.values = {
            FirstName: "Robert",
            LastName: "Carter"
        };

        const patchOp = new PatchOperation("employees/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region increment_value_session
        // Increment UnitsInStock property value by 10
        // ===========================================
        
        session.advanced.increment("products/1-A", "UnitsInStock", 10);
        await session.saveChanges();
        //endregion
    }
    {
        //region increment_value_defer
        // Increment UnitsInStock property value by 10
        // ===========================================
        
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.UnitsInStock += args.UnitsToAdd;";
        patchRequest.values = {
            UnitsToAdd: 10
        };

        const patchCommand = new PatchCommandData("products/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region increment_value_operation
        // Increment UnitsInStock property value by 10
        // ===========================================
        
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.UnitsInStock += args.UnitsToAdd;";
        patchRequest.values = {
            UnitsToAdd: 10
        };

        const patchOp = new PatchOperation("products/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region add_or_increment
        // An entity that will be used in case the specified document is not found:
        const newUser = new User();
        newUser.firstName = "John";
        newUser.lastName = "Doe";
        newUser.loginCount = 1;

        session.advanced.addOrIncrement(
            // Specify document id on which the operation should be performed
            "users/1",
            // Specify an entity,
            // if the specified document is Not found, a new document will be created from this entity
            newUser,
            // The field that should be incremented
            "loginCount",
            // Increment the specified field by this value
            2);
        
        await session.saveChanges();
        //endregion
    }
    {
        //region add_or_patch
        // An entity that will be used in case the specified document is not found:
        const newUser = new User();
        newUser.firstName = "John";
        newUser.lastName = "Doe";
        newUser.lastLogin = new Date(2024, 0, 1);

        session.advanced.addOrPatch(
            // Specify document id on which the operation should be performed
            "users/1",
            // Specify an entity,
            // if the specified document is Not found, a new document will be created from this entity
            newUser,
            // The field that should be patched
            "lastLogin",
            // Set the current date and time as the new value for the specified field
            new Date());

        await session.saveChanges();        
        //endregion
    }
    {
        //region add_or_patch_array
        // An entity that will be used in case the specified document is not found:
        const newUser = new User();
        newUser.firstName = "John";
        newUser.lastName = "Doe";
        newUser.loginTimes = [new Date(2024, 0, 1)];
        
        session.advanced.addOrPatchArray(
            // Specify document id on which the operation should be performed
            "users/1",
            // Specify an entity,
            // if the specified document is Not found, a new document will be created from this entity
            newUser,
            // The array field that should be patched
            "loginTimes",
            // Add values to the list of the specified array field
            a => a.push(new Date(2024, 2, 2), new Date(2024, 3, 3)));
        
        await session.saveChanges();
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region add_item_to_array_session
        // Add a new comment to an array
        // =============================
        
        // The new comment to add:
        const newBlogComment = new BlogComment();
        newBlogComment.content = "Some content";
        newBlogComment.title = "Some title";
        
        // Call 'patchArray':
        session.advanced.patchArray(
            "blogPosts/1",  // Document id to patch
            "comments",     // The array to add the comment to
            comments => {   // Adding the new comment
                comments.push(newBlogComment); 
            });
        
        await session.saveChanges();
        //endregion
    }
    {
        //region add_item_to_array_defer
        // Add a new comment to an array
        // =============================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.comments.push(args.comment);";
        patchRequest.values = {
            comment: {
                title: "Some title",
                content: "Some content",
            }
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("blogPosts/1", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region add_item_to_array_operation
        // Add a new comment to an array
        // =============================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.comments.push(args.comment);";
        patchRequest.values = {
            comment: {
                title: "Some title",
                content: "Some content",
            }
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("blogPosts/1", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region insert_item_in_array_defer
        // Insert a new comment at position 1
        // ==================================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.comments.splice(1, 0, args.comment);";
        patchRequest.values = {
            comment: {
                title: "Some title",
                content: "Some content",
            }
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("blogPosts/1", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region insert_item_in_array_operation
        // Insert a new comment at position 1
        // ==================================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.comments.splice(1, 0, args.comment);";
        patchRequest.values = {
            comment: {
                title: "Some title",
                content: "Some content",
            }
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("blogPosts/1", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region modify_item_in_array_defer
        // Modify comment at position 3
        // ============================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.comments.splice(3, 1, args.comment);";
        patchRequest.values = {
            comment: {
                title: "Some title",
                content: "Some content",
            }
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("blogPosts/1", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region modify_item_in_array_operation
        // Modify comment at position 3
        // ============================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = "this.comments.splice(3, 1, args.comment);";
        patchRequest.values = {
            comment: {
                title: "Some title",
                content: "Some content",
            }
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("blogPosts/1", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region filter_items_from_array_defer
        // Remove all comments that contain the word "wrong" in their content
        // ==================================================================
        
        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = `this.comments = this.comments.filter(comment => 
                               !comment.content.includes(args.text));`;
        patchRequest.values = {
            text: "wrong"
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("blogPosts/1", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region filter_items_from_array_operation
        // Remove all comments that contain the word "wrong" in their content
        // ==================================================================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = `this.comments = this.comments.filter(comment => 
                               !comment.content.includes(args.text));`;
        patchRequest.values = {
            text: "wrong"
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("blogPosts/1", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region load_and_update_defer
        // Load a related document and update a field
        // ==========================================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = `this.Lines.forEach(line => { 
                                   const productDoc = load(line.Product);
                                   line.ProductName = productDoc.Name;
                               });`;

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("orders/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region load_and_update_operation
        // Load a related document and update a field
        // ==========================================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = `this.Lines.forEach(line => { 
                                   const productDoc = load(line.Product);
                                   line.ProductName = productDoc.Name;
                               });`;

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("orders/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    /////////////////////////////////////////////////////////////////////////
    {
        //region remove_property_defer
        // Remove a document property
        // ==========================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = `delete this.Address.PostalCode;`;

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("employees/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region remove_property_operation
        // Remove a document property
        // ==========================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = `delete this.Address.PostalCode;`;

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("employees/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region rename_property_defer
        // Rename property Name to ProductName
        // ===================================
        
        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = `const propertyValue = this[args.currentProperty];
                               delete this[args.currentProperty];
                               this[args.newProperty] = propertyValue;`;
        patchRequest.values = {
            currentProperty: "Name",
            newProperty: "ProductName"
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("products/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region rename_property_operation
        // Rename property Name to ProductName
        // ===================================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        patchRequest.script = `const propertyValue = this[args.currentProperty];
                               delete this[args.currentProperty];
                               this[args.newProperty] = propertyValue;`;
        patchRequest.values = {
            currentProperty: "Name",
            newProperty: "ProductName"
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("products/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region add_document_defer
        // Add a new document
        // ==================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Add a new document (projects/1) to collection Projects
        // The id of the patched document (employees/1-A) is used as content for ProjectLeader property
        patchRequest.script = `put('projects/1', { 
                                   ProjectLeader: id(this),
                                   ProjectDesc: 'Some desc..', 
                                   '@metadata': { '@collection': 'Projects'} 
                               });`;

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("employees/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region add_document_operation
        // Add a new document
        // ==================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Add a new document (projects/1) to collection Projects
        // The id of the patched document (employees/1-A) is used as content for ProjectLeader property
        patchRequest.script = `put('projects/1', { 
                                   ProjectLeader: id(this),
                                   ProjectDesc: 'Some desc..', 
                                   '@metadata': { '@collection': 'Projects'} 
                               });`;

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("employees/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region clone_document_defer
        // Clone a document
        // ================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // The new document will be in the same collection as 'employees/1-A'
        // By specifying 'employees/' the server will generate a "server-side ID' to the new document
        patchRequest.script = `put('employees/', this);`;

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("employees/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region clone_document_operation
        // Clone a document
        // ================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // The new document will be in the same collection as 'employees/1-A'
        // By specifying 'employees/' the server will generate a "server-side ID' to the new document
        patchRequest.script = `put('employees/', this);`;

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("employees/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region increment_counter_session
        // Increment/Create counter
        // ========================
        
        // Increase counter "Likes" by 10, or create it with a value of 10 if it doesn't exist
        session.countersFor("products/1-A").increment("Likes", 10);
        await session.saveChanges();
        //endregion
    }
    {
        //region increment_counter_defer
        // Create/Increment counter
        // ========================
        
        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Use the 'incrementCounter' method to create/increment a counter 
        patchRequest.script = `incrementCounter(this, args.counterName, args.counterValue);`;
        patchRequest.values = {
            counterName: "Likes",
            counterValue: 10
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("products/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region increment_counter_operation
        // Create/Increment counter
        // ========================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Use the 'incrementCounter' method to create/increment a counter 
        patchRequest.script = `incrementCounter(this, args.counterName, args.counterValue);`;
        patchRequest.values = {
            counterName: "Likes",
            counterValue: 10
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("products/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region delete_counter_session
        // Delete counter
        // ==============

        session.countersFor("products/1-A").delete("Likes");
        await session.saveChanges();
        //endregion
    }
    {
        //region delete_counter_defer
        // Delete counter
        // ==============
        
        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Use the 'deleteCounter' method to delete a counter 
        patchRequest.script = `deleteCounter(this, args.counterName);`;
        patchRequest.values = {
            counterName: "Likes"
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("products/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region delete_counter_operation
        // Delete counter
        // ==============

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Use the 'deleteCounter' method to delete a counter 
        patchRequest.script = `deleteCounter(this, args.counterName);`;
        patchRequest.values = {
            counterName: "Likes"
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("products/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region get_counter_session
        // Get counter value
        // =================
        
        const counters = await session.counterFor("products/1-A").get("Likes");
        //endregion
    }
    {
        //region get_counter_defer
        // Get counter value
        // =================
        
        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();
        
        // Use the 'counter' method to get the value of the specified counter 
        // and then put the results into a new document 'productLikes/'
        patchRequest.script = `const numberOfLikes = counter(this, args.counterName);
                               put('productLikes/', {ProductName: this.Name, Likes: numberOfLikes});`;
        
        patchRequest.values = {
            counterName: "Likes"
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("products/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        //endregion
    }
    {
        //region get_counter_operation
        // Get counter value
        // =================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Use the 'counter' method to get the value of the specified counter 
        // and then put the results into a new document 'productLikes/'
        patchRequest.script = `const numberOfLikes = counter(this, args.counterName);
                               put('productLikes/', {ProductName: this.Name, Likes: numberOfLikes});`;

        patchRequest.values = {
            counterName: "Likes"
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("products/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        //endregion
    }
    //////////////////////////////////////////////////////////////////////////
    {
        //region Patching_usingFunction
        // Modify value using inline string compilation
        // ============================================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Define the script:
        patchRequest.script = `
            // Give a discount if the product is low in stock:
            const functionBody = "return doc.UnitsInStock < lowStock ? " + 
                "doc.PricePerUnit * discount :" + 
                "doc.PricePerUnit;";

            // Define a function that processes the document and returns the price:
            const calcPrice = new Function("doc", "lowStock", "discount", functionBody);

            // Update the product's PricePerUnit based on the function:
            this.PricePerUnit = calcPrice(this, args.lowStock, args.discount);`;

        patchRequest.values = {
            discount: "0.8",
            lowStock: "10"
        };

        // Define the 'PatchCommandData':
        const patchCommand = new PatchCommandData("products/1-A", null, patchRequest);
        session.advanced.defer(patchCommand);

        await session.saveChanges();
        
        // The same can be applied using the 'operations' syntax.
        //endregion
    }
    {
        //region Patching_usingEval
        // Modify value using inline string compilation
        // ============================================

        // Define the 'PatchRequest':
        const patchRequest = new PatchRequest();

        // Define the script:
        patchRequest.script = `
            // Give a discount if the product is low in stock:
            const discountExpression = "this.UnitsInStock < args.lowStock ? " + 
                "this.PricePerUnit * args.discount :" + 
                "this.PricePerUnit";
            
            // Call 'eval', pass the string expression that contains your logic:
            const price = eval(discountExpression);
            
            // Update the product's PricePerUnit:
            this.PricePerUnit = price;`;

        patchRequest.values = {
            discount: "0.8",
            lowStock: "10"
        };

        // Define and send the 'PatchOperation':
        const patchOp = new PatchOperation("products/1-A", null, patchRequest);
        await documentStore.operations.send(patchOp);
        
        // The same can be applied using the 'session defer' syntax.
        //endregion
    }

//region syntax
    
    {
        //region patch_syntax
        patch(id, path, value);
        patch(entity, path, value);
        //endregion
    }
    {
        //region add_or_patch_syntax
        addOrPatch(id, entity, pathToObject, value);
        //endregion
    }
    {
        //region increment_syntax
        increment(id, path, valueToAdd);
        increment(entity, path, valueToAdd);
        //endregion
    }
    {
        //region add_or_increment_syntax
        addOrIncrement(id, entity, pathToObject, valToAdd);
        //endregion
    }
    {
        //region patch_array_syntax
        patchArray(id, pathToArray, arrayAdder);
        patchArray(entity, pathToArray, arrayAdder);
        //endregion
    }
    {
        //region add_or_patch_array_syntax
        addOrPatchArray(id, entity, pathToObject, arrayAdder);
        //endregion
    }
    {
        //region class_JavaScriptArray
        class JavaScriptArray {
            push(...u);      // Add a list of values to add to the array
            removeAt(index); // Remove an item from position 'index' in the array
        }
        //endregion
    }
    {
        //region defer_syntax
        session.advanced.defer(...commands);
        //endregion
    }
    {
        //region class_PatchCommandData
        class PatchCommandData {
            // ID of document to be patched
            id; // string
            
            // Change vector of document to be patched, can be null.
            // Used to verify that the document was not changed before the patch is executed.
            changeVector // string;
            
            // Patch request to be performed on the document
            patch; // A PatchRequest object
            
            // Patch request to perform if no document with the specified ID was found
            patchIfMissing; // A PatchRequest object
        }
        //endregion
    }
    {
        //region class_PatchRequest
        class PatchRequest {
            //  The JavaScript code to be run on the server
            script; // string
            
            // Parameters to be passed to the script
            values:; // Dictionary<string, object>

            // It is highly recommend to use the script with the parameters. 
            // This allows RavenDB to cache scripts and boost performance.
            // The parameters are accessed in the script via the `args` object.
        }
        //endregion
    }
    {
        //region operations_syntax
        const patchOperation = new PatchOperation(id, changeVector, patch);
        
        const patchOperation = new PatchOperation(id, changeVector, patch, patchIfMissing,
            skipPatchIfChangeVectorMismatch);
        //endregion
    }

//endregion

//region user_class
    class User {
        constructor(
            id = null,
            firstName = "",
            lastName = "",
            loginCount = 0,
            lastLogin = new Date(),
            loginTimes = []
        ) {
            Object.assign(this, {
                id,
                firstName,
                lastName,
                loginCount,
                lastLogin,
                loginTimes
            });
        }
    }
//endregion

//region blog_classes
    class BlogPost {
        constructor(
            id = null,
            title = "",
            body = "",
            comments = []
        ) {
            Object.assign(this, {
                id,
                title,
                body,
                comments
            });
        }
    }
    
    class BlogComment {
        constructor(
            title = "",
            content = ""
        ) {
            Object.assign(this, {
                title,
                content
            });
        }
    }
//endregion
