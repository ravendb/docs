#Custom Serialization/Deserialization

When document is saved, all public and private properties and all public fields are serialized. You can customize this behavior in one of two ways:

* Using the attributes from Newtonsoft.Json.Serialization to control serialization.
* Providing your own JsonContractSerializer. For example, here is how you can specify that only public fields & properties will be serialized:

    `store.Conventions.JsonContractResolver = new DefaultContractResolver(shareCache: true)  
    {  
              DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.Instance  
    };`

RavenDB usually handles the serialization and deserialization of your documents to/from JSON without requiring you to do any kind of manual mapping. But sometimes you may need to tweak the way your document gets serialized. e.g.:
 

* Ignoring properties which don't contain data, but maybe just provide some data manipulation in their getter or setter
* Customize the serialization of a specific class or property
* Allow "self" references

Most of your special serialization needs can be controlled by decorating your classes with attributes from the Newtonsoft.Json namespace (see [JSON documentation](http://james.newtonking.com/projects/json/help/)). Besides this Raven allows to use your own JsonContractResolver so you can provide a custom JsonContract for specific types.
 
Here are some examples:

##Ignoring a Property
This is trivial. Just add a JsonIgnore attribute.

    public class Carrot 
    { 
        public string Id { get; set; } 
    
        public decimal Length { get; set; } 
    
        [JsonIgnore] 
        public decimal LengthInInch 
        { 
            get 
            { 
               //...
            } 
    
            set 
            { 
                //...
            } 
        } 
    }

Now only Length, but not LengthInInch will be serialized.

##Customizing the serialization of a property
The JsonProperty attribute allows you to specify a custom converter class for a specific property. For example, to change the name of the serialized property:

    public class Recipe
    {
        public string Id { get; set; }
    
        [JsonProperty(PropertyName = "dishes")]
        public IList<IVegetable> SideDishes { get; set; }
    }

##Allow self references
Raven does not try to resolve associations between documents, because this would have a big impact on scaling or sharding and would probably require to introduce such nasty things like lazy loading. For self references within a document there's no such restriction. It doesn't work out of the box, but once again, JSON provides a handy attribute for this:

    [JsonObject(IsReference = true)] 
    public class Category
    {
         public string Id { get; set; }
         public string Name { get; set; }
         public Category Parent { get; set; }
         public List<Category> Children { get; set; }
    
         public Category()
         {
             Children = new List<Category>();
         }
    
         public void Add(Category category)
         {
             category.Parent = this;
             Children.Add(category);
         }
    }

Now the children's Parent property will be a reference instead of a full copy of Category (which would result in an endless recursion, if JSON wouldn't detect it and throw an exception).