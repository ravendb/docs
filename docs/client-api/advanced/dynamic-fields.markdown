# Dynamic fields

While strongly typed entities are well processed by Linq expressions, some scenarios demand the use of dynamic properties. To support searching in object graphs that cannot have their entire structure be declared upfront, RavenDB exposes the following low-level API for creating fields from within index definitions.

With this feature, you can search for documents even with properties which were created on the fly. For example, consider a `Product` object that is declared as follows:

{CODE-START:csharp /}
		public class Product
		{
			public string Id { get; set; }
			public List<Attribute> Attributes { get; set; }
		}

		public class Attribute
		{
			public string Name { get; set; }
			public string Value { get; set; }
		}
{CODE-END /}

This way, properties such as color, size, weight and the like are added only to those products which they are indeed valid properties of. However, while they are easily stored, they cannot be easily searched on.

This is where dynamic fields come in. With the following index definition, RavenDB will index the attribute value under the attribute name in its own field:

{CODE-START:csharp /}
		public class Product_ByAttribute : AbstractIndexCreationTask<Product>
		{
			public Product_ByAttribute()
			{
				Map = products =>
					from p in products
					select new
					{
						_ = p.Attributes.Select(attribute =>
              new Field(attribute.Name, attribute.Value, Field.Store.NO, Field.Index.ANALYZED))
					};
			}
		}
{CODE-END /}

The underscore used for defining the index name is just a convention - the call to `.Select(at => new Field(...))` will generate index fields based on the properties in the provided collection, without creating any field with the name specified there, hence the underscore.

Obviously, this index can have more attributes defined in it for indexing, just like any other ordinary index.

{INFO Field options like Store.NO and Index.ANALYZED are configurable also with dynamic fields. /}

After creating the index, we can easily look for documents using the attribute name as a field to look on, as if it was a real object property. Since it is not really a property, there is no Linq support for it, hence it can only be queried using the `LuceneQuery<>()` API:

{CODE-START:csharp /}
					var products = session.Advanced.LuceneQuery<Product>("Product/ByAttribute")
						.WhereEquals("Color", "Red")
						.ToList();
{CODE-END /}

## Numeric fields

If the `Product` class had an integer property instead of a string one, we would have to use a `NumericField` in the index definition instead, to enable searches with numeric operators like `WhereGreaterThan()` and the like:

{CODE-START:csharp /}
		public class Product_ByNumericAttributeUsingField : AbstractIndexCreationTask<Product>
		{
			public Product_ByNumericAttributeUsingField()
			{
				Map = products =>
					from p in products
					select new
					{
						_ = p.Attributes.Select(attribute =>
              new NumericField(attribute.Name, attribute.NumericValue.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS))
					};
			}
		}
{CODE-END /}

If you expect to have both numeric and string values, you can trigger dynamic field indexing on both types, like so:

{CODE-START:csharp /}
		public class Product_ByNumericAttributeUsingField : AbstractIndexCreationTask<Product>
		{
			public Product_ByNumericAttributeUsingField()
			{
				Map = products =>
					from p in products
					select new
					{
						_ = p.Attributes.Select(attribute =>
              new Field(attribute.Name, attribute.Value, Field.Store.NO, Field.Index.ANALYZED))
						__ = p.Attributes.Select(attribute =>
              new NumericField(attribute.Name, attribute.NumericValue.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS))
					};
			}
		}
{CODE-END /}