# Setting up authentication 

`Certificate` property allows to pass certificate which will be used by RavenDB client to connect to server. 

{NOTE If your RavenDB instance is running on 'https' then your application has to use a client certificate in order to be able to access the server. You can find more information [here](../server/security/overview). /}

## Example I

{CODE client_cert@ClientApi\Certificate.cs /}


## Related Articles

- [How to create **secured server**?](../server/security/overview)
