#What are conventions?

Conventions give you an ability to customize the Client API behavior. They are accessible from `DocumentStore` object:

{CODE conventions_1@ClientApi\Configuration\WhatAreConventions.cs /}

You will find many settings to overwrite, allowing you to adjust the client according to your needs. The conventions apply to many different client behaviors so they are grouped and described in the separate articles of this section.


{INFO All customizations need to be set before `DocumentStore.Initialize` is called. /}

