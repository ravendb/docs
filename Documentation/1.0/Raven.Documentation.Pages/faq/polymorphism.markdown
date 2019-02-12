# Polymorphism in RavenDB

RavenDB stores document in JSON format, which make it very flexible, but also make some code patterns harder to work with. In particular, the RavenDB Client API will not, by default, record type information into embedded parts of the JSON document. That makes for a much easier to read JSON, but it means that using polymorphism for objects that are embedded inside another document requires some modification.

{NOTE There is no problem with polymorphism for entities that are stored as documents, only with embedded documents. /}

That modification happens entirely at the [JSON.Net](https://www.newtonsoft.com/json) layer, which is responsible for serializing and deserializing documents. The problem is when you have a model such as this:

{CODE-START:csharp /}
public class Sale
{
    public Sale()
    {
        Items = new List<SaleItem>();
    }
    public string Id { get; set; }
    public List<SaleItem> Items { get; private set; }
}
&nbsp;
public abstract class SaleItem
{
    public decimal Amount { get; set; }
}
&nbsp;
public class ProductSaleItem : SaleItem
{
    public string ProductNumber { get; set; }
}
&nbsp;
public class DiscountSaleItem : SaleItem
{
    public string DiscountText { get; set; }
}
{CODE-END/}

And you want to store the following data:

{CODE-START:csharp /}
using (var session = documentStore.OpenSession())
{
    var sale = new Sale();
    sale.Items.Add(new ProductSaleItem { Amount = 1.99m, ProductNumber = "123" });
    sale.Items.Add(new DiscountSaleItem { Amount = -0.10m, DiscountText = "Hanukkah Discount" });
    session.Store(sale);
    session.SaveChanges();
}
{CODE-END/}

With the default JSON.Net behavior, you can serialize this object graph, but you can't deserialize it, because there isn't enough information in the JSON to do so.

RavenDB gives you the following extension point to handle that:

{CODE-START:csharp /}
documentStore.Conventions.CustomizeSerializer = serializer => serializer.TypeNameHandling = TypeNameHandling.All;
{CODE-END/}
