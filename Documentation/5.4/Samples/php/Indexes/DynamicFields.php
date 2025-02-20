<?php

namespace RavenDB\Samples\Indexes;

/* EXAMPLE 1
interface ICreateField
{
    # region syntax
    object CreateField(string name, object value);

    object CreateField(string name, object value, bool stored, bool analyzed);

    object CreateField(string name, object value, CreateFieldOptions options);
    # endregion
}

# region dynamic_fields_1
use Ds\Map as DSMap;

class Product
{
    private ?string $id = null;

    // The KEYS under the Attributes object will be dynamically indexed
    // Fields added to this object after index creation time will also get indexed
    public ?DSMap $attributes = null;
}
# endregion

# region dynamic_fields_2
class Products_ByAttributeKey extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from p in docs.Products select new {" .
            "_ = p.attributes.Select(item => CreateField(item.Key, item.Value))" .
            "}";
    }
}
# endregion

# region dynamic_fields_2_JS
class Products_ByAttributeKey_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Products', function (p) { " .
            "    return { " .
            "        _: Object.keys(p.attributes).map(key => createField(key, p.attributes[key], " .
            "            { indexing: 'Search', storage: false, termVector: null })) " .
            "    }; " .
            "}) "
        ]);
    }
}
# endregion

class DynamicFields
{
    public function queryDynamicFields(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region dynamic_fields_3
                $matchingDocuments = $session
                    ->advanced()
                    ->documentQuery(Product::class, Products_ByAttributeKey::class)
                    // 'Size' is a dynamic-index-field that was indexed from the Attributes object
                    ->whereEquals("Size", 42)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
*/


/* Example 2

# region dynamic_fields_4
class Product
{
    private ?string $id = null;

    // All KEYS in the document will be dynamically indexed
    // Fields added to the document after index creation time will also get indexed
    public ?string $firstName = null;
    public ?string $lastName = null;
    public ?string $title = null;

    // ... getters and setters
}
# endregion

# region dynamic_fields_5_JS
class Products_ByAnyField_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // This will index EVERY FIELD under the top level of the document
        $this->setMaps([
            "map('Products', function (p) {
                return {
                    _: Object.keys(p).map(key => createField(key, p[key],
                        { indexing: 'Search', storage: true, termVector: null }))
                }
            })"
        ]);
    }
}
# endregion

class DynamicFields
{
    public function QueryDynamicFields(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->OpenSession();
            try {
                # region dynamic_fields_6
                $matchingDocuments = $session
                    ->advanced()
                    ->documentQuery(Product::class, Products_ByAnyField_JS::class)
                     // 'LastName' is a dynamic-index-field that was indexed from the document
                    ->whereEquals("LastName", "Doe")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

*/

/*  Example3
# region dynamic_fields_7
class Product
{
    public ?string $id = null;

    // The VALUE of ProductType will be dynamically indexed
    public ?string $productType = null;
    public ?int $pricePerUnit = null;

    // ... getters and setters
}
# endregion

# region dynamic_fields_8
class Products_ByProductType extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // Call 'CreateField' to generate the dynamic-index-fields
        // The field name will be the value of document field 'ProductType'
        // The field terms will be derived from document field 'PricePerUnit'
        $this->map = "docs.Products.Select(p => new { " .
            "    _ = this.CreateField(p.productType, p.pricePerUnit) " .
            "})";
    }
}
# endregion

# region dynamic_fields_8_JS
class Products_ByProductType_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Products', function (p) {
                return {
                    _: createField(p.ProductType, p.PricePerUnit,
                        { indexing: 'Search', storage: true, termVector: null })
                };
            })"
        ]);
    }
}
# endregion

class DynamicFields
{
    public function QueryDynamicFields(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->OpenSession();
            try {
                # region dynamic_fields_9
                $matchingDocuments = $session
                    ->advanced()
                    ->documentQuery(Product::class, Products_ByProductType::class)
                // 'Electronics' is the dynamic-index-field that was indexed from document field 'ProductType'
                ->whereEquals("Electronics", 23)
                ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
*/

/* Example4 */
# region dynamic_fields_10
class Product
{
    public ?string $id = null;
    public ?string $name = null;

    // For each element in this list, the VALUE of property 'PropName' will be dynamically indexed
    // e.g. Color, Width, Length (in ex. below) will become dynamic-index-fields
    public ?AttributeList $attributes = null;

    // ... getters and setters
}

class Attribute
{
    public ?string $propName = null;
    public ?string $propValue = null;

    // ... getters and setters
}

class AttributeList extends TypedList
{
    protected function __construct()
    {
        parent::__construct(Attribute::class);
    }
}
# endregion

# region dynamic_fields_11
class Attributes_ByName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // Define the dynamic-index-fields by calling CreateField
        // A dynamic-index-field will be generated for each item in the Attributes list

        // For each item, the field name will be the value of field 'PropName'
        // The field terms will be derived from field 'PropValue'

        $this->map =
            "docs.Products.Select(p => new { " .
            "    _ = p.attributes.Select(item => this.CreateField(item.propName, item.propValue)), " .
            "    Name = p.name " .
            "})";
    }
}
# endregion

# region dynamic_fields_11_JS
class Attributes_ByName_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Products', function (p) {
                return {
                    _: p.Attributes.map(item => createField(item.PropName, item.PropValue,
                        { indexing: 'Search', storage: true, termVector: null })),
                   Name: p.Name
                };
            })"
        ]);
    }
}
# endregion

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\AbstractJavaScriptIndexCreationTask;
use RavenDB\Type\TypedList;

class DynamicFields
{
    public function queryDynamicFields(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region dynamic_fields_12
                /** @var array<Product> $matchingDocuments */
                $matchingDocuments = $session
                    ->advanced()
                    ->documentQuery(Product::class, Attributes_ByName::class)
                     // 'Width' is a dynamic-index-field that was indexed from the Attributes list
                    ->whereEquals("Width", 10)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
