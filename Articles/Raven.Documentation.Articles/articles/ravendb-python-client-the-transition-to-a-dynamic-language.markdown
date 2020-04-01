# RavenDB Python client - The transition to a dynamic language

When I started to work in Hibernating Rhinos, and taking part in RavenDB development, I have been asked  to create a RavenDB client in Python.
Python is a dynamic and flexible language, and because of that I could start developing and experimenting with RavenDB much more easily. I could build the REST methods in an instance and getting responses from the server in early stages. 

While developing the Python client I always had to follow the RavenDB basic core and never lose its idea. RavenDB is the premier NoSQL database for .NET. Open source, speed-obsessed and 
a joy to use. It's reliable, can do ACID transactions, fast, uses replication and much more.
The flexibility of document database like RavenDB gives us the ability to hold dynamic values easily , RavenDB can save any types we choose, change any individual document or many, add fields as we please or delete ones. For that and more it's just make sense in a dynamic language like python. 
For me, the most important thing was to make a client that will be easy to use just like the .Net client by "unleashing the power of python" and following the Python way of doing things I could achieve that without losing any functionality in the process .

In C#, we declare types in advance. The first issue I encountered was how to build my methods and how to let Python users know what are the correct variables to pass to the methods, without previous knowledge in the RavenDB Python client.
{CODE-BLOCK:csharp}
  public T Load<T>(string id)
{CODE-BLOCK/}

*<sup> ex. Load signature in .NET RavenDB

So I got two ways to solve it. The first is to use isinstance() to check every field and if I get false I would raise an exception. 
The second is to use duck typing (duck typing is an application of the duck test in type safety. It requires that type checking is deferred to runtime, and is implemented by means of dynamic typing or reflection) one of the advantage python have. 
I decided to use both solutions. I use the first where we won't get exceptions from duct taping or where we get unexplained ones. 
I use duck taping when I can be sure the user will understand the problem when he gets an exception.

{CODE-BLOCK:python} 
def load(self, key_or_keys, object_type=None, includes=None):
	if not key_or_keys:
		raise ValueError("None or empty key is invalid")
	if includes and not isinstance(includes, list):
		includes = [includes]
	if isinstance(key_or_keys, list):
		return self._multi_load(key_or_keys, object_type, includes)
{CODE-BLOCK/}
Example using ```isinstance()```.<br ><br >
{CODE-BLOCK:python} 
def save_entity(self, key, entity, original_metadata, metadata, document,force_concurrency_check=False):
	self._known_missing_ids.discard(key)
	if key not in self._entities_by_key:
		self._entities_by_key[key] = entity
		self._entities_and_metadata[self._entities_by_key[key]] = {
			"original_value": document.copy(), 
			"metadata": metadata,
			"original_metadata": original_metadata, 
			"etag": metadata.get("etag", None), 
			"key": key,
			"force_concurrency_check": force_concurrency_check}
{CODE-BLOCK/}
Example for duck taping. See ```document.copy()``` (document needs to be a dict).

RavenDB .Net client makes life easier by using reflection to create the correct object and for 
every variable that doesn't exist in the document it will return the default value 
(e.g. strings will get  an empty string). You can already see the problem with that in Python. 
How can I really know the default value of the variable if the user can assign different default values? 
How can I make sure that I get all the right fields for the object?
How can I make sure not to get any exceptions during the initialization?

That's take us to the second  issue:
Along with the document, RavenDB saves a dict (as metadata) with more information about the document. 
One of the properties stored inside the metadata is the Raven-Python-Type a property that I put in the metadata to help me solve the issue, 
In these property we save the class name and it's module as the value. Then, we can try to import it when we want to load or query a document 
("Raven-Python-Type": "__main__.Foo").
 <img style="float: left;"><br ><br >
 ![Metadata](images/pymetadata.png)

*<sup> ex. Document metadata<br >

{CODE-BLOCK:python}
def import_class(name):
	components = name.split('.')
	try:
		mod = __import__(components[0])
		for comp in components[1:]:
			mod = getattr(mod, comp)
		return mod
	except (ImportError, ValueError):
		pass
return None
{CODE-BLOCK/}
The next step will be to check and build the class from the document we got from the server.

{CODE-BLOCK:python}
class Foo(object):
    def __init__(self, name, dependencies=None, saved_in_version="2.7.9"):
        self.name = name
        self.dependencies = dependencies
        self.saved_in_version = saved_in_version
{CODE-BLOCK/}
{CODE-BLOCK:python}
{
    "name": "PyRavenDB",
    "dependencies": [
        "pycrypto >= 2.6.1",
        "requests >= 2.9.1",
        "inflector >= 2.0.11",
        "enum >= 0.4.6"
       ]
}
{CODE-BLOCK/}
here we have the class Foo with the document we get from the server. 
The class Foo has been modified after the document has been saved to the server.
We have to know what the variables in the class are. We also need to know
if we have them in the document and initialize them with the right value. 
At the end, we need to know the default values of those variables that we couldn't fetch from the document 
(their class has changed see save_in_version in class Foo). Unlike in C# that we can know the default value from the type.
{CODE-BLOCK:python}
Args, __, keywords, defaults = inspect.getargspec(entity.__class__.__init__)
{CODE-BLOCK/}
The above line will give me all the variables in the __init__ method and all the default values. Then we will execute the following code for making the match:
{CODE-BLOCK:python}
if (len(args) - 1) > len(document):
    remainder = len(args)
    if defaults:
        remainder -= len(defaults)
    for i in range(1, remainder):
        entity_initialize_dict[args[i]] = document.get(args[i], None)
    for i in range(remainder, len(args)):
        entity_initialize_dict[args[i]] = document.get(args[i], 
								defaults[i - remainder])
else:
    if keywords:
        entity_initialize_dict = document
    else:
        for key in document:
            if key in args:
                entity_initialize_dict[key] = document[key]
{CODE-BLOCK/}
{CODE-BLOCK:python}
 entity = entity.__class__(**entity_initialize_dict)
 {CODE-BLOCK/}
* see <a href="https://docs.python.org/2/library/inspect.html" rel="nofollow">https://docs.python.org/2/library/inspect.html</a> for more information.

After making the match we can use entity_initialize_dict to initialize our object. This action will solve many problems up ahead. 
For example, if our class inherits from another class and it doesn't contain all the fields of the base class in the __init__ method, 
then the getargspec method won't return them and in this case we can lose important information about the class 
(the result is a uncompleted object). 
The method will return a DynamicStructure if it fails to import the class or in case the object_type variable 
(will be explained later) is equal to None.

{CODE-BLOCK:python}
class _DynamicStructure(object):
	def __init__(self, **entries):
		self.__dict__.update(entries)
	def __str__(self):
		return str(self.__dict__)
{CODE-BLOCK/}
In RavenDB .Net client there are many usages of generics. RavenDB tries to be as much strongly typed as it can be, and we can understand why (no ones want errors)
{CODE-BLOCK:csharp}
 Foo foo = session.Load<Foo>("foos/1");
 {CODE-BLOCK/}
In Python it is a little different, Python doesn't need any of that because of its dynamic structure. I can add and change every value I want in every class I want during run-time. Still, I wanted to add the option to get any type of object or the actual type that is specified in the document metadata.  
For that I added the field object_type (None in default).

In object_type the user can put any class he wants and if the client found a match against the type specified in the document metadata (Raven-Python-Type), we will get the right class.  If we don't initialize object_type and Raven-Python-Type in the metadata we will get a dynamic entity (see _DynamicStructure). 
{CODE-BLOCK:python}
foo = session.load("foos/1", object_type=Foo)
{CODE-BLOCK/}
Finally, I could overcome all these issues and created the Python client in RavenDB (pyravendb)
that can handle most CRUD scenarios, including full support for replication, failover, dynamic queries, etc.


for more information please visit:<br >
<a href="https://github.com/ravendb/RavenDB-Python-Client" rel="nofollow">https://github.com/ravendb/RavenDB-Python-Client</a><br >
<a href="https://ravendb.net">https://ravendb.net</a><br >

Author : <b>Idan Haim Shalom</b>
