#Conventions Related to Serialization

##Customize ObjectMapper

If you need to customize Jackson `ObjectMapper` object used by the client when sending entities to the server you can access and modify its instance:

{CODE:java customize_object_mapper@ClientApi\Configuration\Serialization.java /}

##Numbers (de)serialization

RavenDB client supports out of the box all common numeric value types: `int`, `long`, `double` etc.  
Note that although the (de)serialization of `decimals` is fully supported, there are [server side limitations](../../server/kb/numbers-in-ravendb) to numbers in that range.  
Other number types like `BigInteger` must be treated using custom (de)serialization.
