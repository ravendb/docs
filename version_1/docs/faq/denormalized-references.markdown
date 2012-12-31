# Denormalized References

RavenDB doesn't allow references between documents in the sense that an RDBMS user will understand them. Moreover, while RavenDB offers advanced features like Includes and multi-maps, document isolation is still an important design feature when you build your data model.

Consider the following case:

![Figure 1: Denormalized References](images/denormalized_references_faq.png)

Order has a reference to the customer. However, the above is the logical model, which ignores the persistence concerns. The model we use to store the data in RavenDB is by necessity different. We can't hold a direct reference to the customer, because RavenDB will serialize the entire Customer with our Order, which isn't what we want.

On the other hand, holding just the document id (common in RDBMS systems) is usually going to be insufficient. Normally, a document should contain all the data that is required to perform all routine operations on it. In the case of the Order, that means displaying the Customer's name.

{NOTE From our experiences, it seems that the vast majority of cases requires just holding a name/id pair as a denormalized reference for a document. There are cases where we need more, certainly, but the method outline here works for those as well. /}

In other words, we need to hold in the Order class what we call a denormalized reference to the customer. That reference includes the document id, but it also includes any properties that are important for the Order. In this case, the name.

## What properties should be included in the denormalized reference?

* Properties which rarely change (such as name, email, etc).
* Properties that are required for common processing of the referencing document.
* Properties whose value needs to be separate than the current value in the owner document (Product.Cost is a good example where we want to copy the value to the OrderLine.Cost, so we can keep track if things have changed).

The following shows how we can do this easily using code. First, the model:

{CODE-START:csharp /}
    public interface INamedDocument
    {
        string Id { get; set; }
        string Name { get; set; }
    }

    public class Customer : INamedDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Order : INamedDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DenormalizedReference<Customer> Customer { get; set;}
    }
{CODE-END/}

Note that the Customer property on the Order is a DenormalizedReference of T. This class looks like this:

{CODE-START:csharp /}
    public class DenormalizedReference<T> where T : INamedDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        public static implicit operator DenormalizedReference<T> (T doc)
        {
            return new DenormalizedReference<T>
            {
                Id = doc.Id,
                Name = doc.Name
            }
        }
    }
{CODE-END/}

This is pretty simple, except that we use something that you might not have seen before, implicit operators. This is just a C# language feature that tells the compiler that whenever it sees a expression of type T passed where the expected type is DenormalizedReference<T>, the implicit operator will be called to convert it.

This allows the following code:

{CODE-START:csharp /}
    Customer customer = GetCurrentCustomer();
    var order = new Order
    {
       Customer = customer // implicit operator called here
    }
{CODE-END/}

The result is a more natural approach for working with the denormalized reference.
