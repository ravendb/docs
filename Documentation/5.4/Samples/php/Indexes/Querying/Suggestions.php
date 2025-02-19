<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldIndexing;
use RavenDB\Documents\Queries\Suggestions\StringDistanceTypes;
use RavenDB\Documents\Queries\Suggestions\SuggestionOptions;
use RavenDB\Documents\Queries\Suggestions\SuggestionResult;
use RavenDB\Documents\Queries\Suggestions\SuggestionSortMode;
use RavenDB\Documents\Queries\Suggestions\SuggestionWithTerm;
use RavenDB\Documents\Queries\Suggestions\SuggestionWithTerms;
use RavenDB\Samples\Indexes\Product;

class Suggestions
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region suggestions_2

                // This query on index 'Products/ByName' has NO resulting documents
                /** @var array<Product> $products */
                $products = $session
                    ->query(Products_ByName_IndexEntry::class, Products_ByName::class)
                    ->search("ProductName", "chokolade")
                    ->ofType(Product::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_3
                // Query the index for suggested terms for single term:
                // ====================================================

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                     // Query the index
                    ->query(Products_ByName_IndexEntry::class, Products_ByName::class)
                     // Call 'SuggestUsing'
                     ->suggestUsing(function ($builder) {
                         // Request to get terms from index-field 'ProductName' that are similar to 'chokolade'
                         return $builder->byField("ProductName", "chokolade");
                     })
                    ->execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_5
                // Define the suggestion request for single term
                $suggestionRequest = new SuggestionWithTerm("ProductName");
                $suggestionRequest->setTerm("chokolade");

                // Query the index for suggestions
                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                    ->query(Products_ByName_IndexEntry::class, Products_ByName::class)
                     // Call 'SuggestUsing' - pass the suggestion request
                    ->suggestUsing($suggestionRequest)
                    ->Execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_6
                // Query the index for suggested terms for single term:
                // ====================================================

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session->advanced()
                     // Query the index
                    ->documentQuery(Products_ByName_IndexEntry::class, Products_ByName::class)
                     // Call 'SuggestUsing'
                    ->suggestUsing(function($builder) {
                        // Request to get terms from index-field 'ProductName' that are similar to 'chokolade'
                        return $builder->byField("ProductName", "chokolade");
                     })
                    ->execute();
                # endregion

                # region suggestions_7
                // The resulting suggested terms:
                // ==============================

                echo "Suggested terms in index-field 'ProductName' that are similar to 'chokolade':";
                foreach ($suggestions["ProductName"]->getSuggestions() as $suggestedTerm)
                {
                    echo "\t" . $suggestedTerm;
                }

                // Suggested terms in index-field 'ProductName' that are similar to 'chokolade':
                //     schokolade
                //     chocolade
                //     chocolate
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_8
                // Query the index for suggested terms for multiple terms:
                // =======================================================

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                     // Query the index
                    ->query(Products_ByName_IndexEntry::class, Products_ByName::class)
                     // Call 'SuggestUsing'
                    ->suggestUsing(function($builder) {
                         return $builder
                             // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' OR 'syrop'
                             ->ByField("ProductName", ["chokolade", "syrop"]);
                     })
                    ->execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_10
                // Define the suggestion request for multiple terms
                $suggestionRequest = new SuggestionWithTerms("ProductName");
                $suggestionRequest->setTerms([ "chokolade", "syrop" ]);

                // Query the index for suggestions
                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                    ->query(Products_ByName_IndexEntry::class, Products_ByName::class)
                     // Call 'SuggestUsing' - pass the suggestion request
                    ->suggestUsing($suggestionRequest)
                    ->execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_11
                // Query the index for suggested terms for multiple terms:
                // =======================================================

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session->advanced()
                     // Query the index
                    ->documentQuery(Products_ByName_IndexEntry::class, Products_ByName::class)
                     // Call 'SuggestUsing'
                    ->suggestUsing(function($builder) {
                        return $builder
                             // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' OR 'syrop'
                            ->byField("ProductName", [ "chokolade", "syrop" ]);
                    })
                    ->execute();
                # endregion

                # region suggestions_12
                // The resulting suggested terms:
                // ==============================

                // Suggested terms in index-field 'ProductName' that are similar to 'chokolade' OR to 'syrop':
                //     schokolade
                //     chocolade
                //     chocolate
                //     sirop
                //     syrup
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_13
                // Query the index for suggested terms in multiple fields:
                // =======================================================

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                     // Query the index
                    ->query(Companies_ByNameAndByContactName_IndexEntry::class, Companies_ByNameAndByContactName::class)
                     // Call 'SuggestUsing' to get suggestions for terms that are
                     // similar to 'chese' in first index-field (e.g. 'CompanyName')
                    ->suggestUsing(function($builder) {
                        return $builder
                        ->byField("CompanyName", "chese" );
                    })
                     // Call 'AndSuggestUsing' to get suggestions for terms that are
                     // similar to 'frank' in an additional index-field (e.g. 'ContactName')
                    ->andSuggestUsing(functioN($builder) {
                        return $builder
                            ->byField("ContactName", "frank");
                    })
                    ->execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_15
                // Define suggestion requests for multiple fields:

                $request1 = new SuggestionWithTerm("CompanyName");
                // Looking for terms from index-field 'CompanyName' that are similar to 'chese'
                $request1->setTerm("chese");

                $request2 = new SuggestionWithTerm("ContactName");
                // Looking for terms from nested index-field 'ContactName' that are similar to 'frank'
                $request2->setTerm("frank");

                // Query the index for suggestions
                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                    ->query(Companies_ByNameAndByContactName_IndexEntry::class, Companies_ByNameAndByContactName::class)
                     // Call 'SuggestUsing' - pass the suggestion request for the first index-field
                    ->suggestUsing($request1)
                     // Call 'AndSuggestUsing' - pass the suggestion request for the second index-field
                    ->andSuggestUsing($request2)
                    ->execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_16
                // Query the index for suggested terms in multiple fields:
                // =======================================================

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session->advanced()
                     // Query the index
                    ->documentQuery(Companies_ByNameAndByContactName_IndexEntry::class, Companies_ByNameAndByContactName::class)
                     // Call 'SuggestUsing' to get suggestions for terms that are
                     // similar to 'chese' in first index-field (e.g. 'CompanyName')
                    ->suggestUsing(function($builder) {
                            return $builder
                                ->ByField("CompanyName", "chese" );
                    })
                     // Call 'AndSuggestUsing' to get suggestions for terms that are
                     // similar to 'frank' in an additional index-field (e.g. 'ContactName')
                    ->andSuggestUsing(function($builder) {
                        return $builder
                            ->byField("ContactName", "frank");
                    })
                    ->execute();
                # endregion

                # region suggestions_17
                // The resulting suggested terms:
                // ==============================

                // Suggested terms in index-field 'CompanyName' that is similar to 'chese':
                //     cheese
                //     chinese

                // Suggested terms in index-field 'ContactName' that are similar to 'frank':
                //     fran
                //     franken
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_18
                // Query the index for suggested terms - customize options and display name:
                // =========================================================================

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                     // Query the index
                    ->query(Products_ByName_IndexEntry::class, Products_ByName::class)
                     // Call 'SuggestUsing'
                    ->suggestUsing(function($builder) {
                         $suggestionOptions = new SuggestionOptions();
                         $suggestionOptions->setAccuracy(0.3);
                         $suggestionOptions->setPageSize(5);
                         $suggestionOptions->setDistance(StringDistanceTypes::nGram());
                         $suggestionOptions->setSortMode(SuggestionSortMode::popularity());

                         $builder
                            ->byField("ProductName", "chokolade")
                             // Customize suggestions options
                            ->withOptions($suggestionOptions)
                             // Customize display name for results
                            ->withDisplayName("SomeCustomName");
                    })
                    ->execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_20
                // Define the suggestion request
                $suggestionRequest = new SuggestionWithTerm("ProductName");
                // Looking for terms from index-field 'ProductName' that are similar to 'chokolade'
                $suggestionRequest->setTerm("chokolade");

                // Customize options
                $options = new SuggestionOptions();
                $options->setAccuracy(0.3);
                $options->setPageSize(5);
                $options->setDistance(StringDistanceTypes::nGram());
                $options->setSortMode(SuggestionSortMode::popularity());

                $suggestionRequest->setOptions($options);

                // Customize display name
                $suggestionRequest->setDisplayField("SomeCustomName");


                // Query the index for suggestions
                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session
                    ->query(Products_ByName_IndexEntry::class, Products_ByName::class)
                    // Call 'SuggestUsing' - pass the suggestion request
                    ->suggestUsing($suggestionRequest)
                    ->execute();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region suggestions_21
                // Query the index for suggested terms - customize options and display name:
                // =========================================================================

                /** @var array<SuggestionResult> $suggestions */
                $suggestions = $session->advanced()
                     // Query the index
                    ->documentQuery(Products_ByName_IndexEntry::class, Products_ByName::class)
                     // Call 'SuggestUsing'
                    ->suggestUsing(function($builder) {
                         $options = new SuggestionOptions();
                         $options->setAccuracy(0.3);
                         $options->setPageSize(5);
                         $options->setDistance(StringDistanceTypes::nGram());
                         $options->setSortMode(SuggestionSortMode::popularity());

                         return $builder
                             ->byField("ProductName", "chokolade")
                             // Customize suggestions options
                             ->withOptions($options)
                             // Customize display name for results
                            ->withDisplayName("SomeCustomName");
                    })
                    ->execute();
                # endregion

                # region suggestions_22
                // The resulting suggested terms:
                // ==============================

                echo "Suggested terms:";
                // Results are available under the custom name entry
                foreach ($suggestions["SomeCustomName"]->getSuggestions() as $suggestedTerm)
                {
                    echo "\t" . $suggestedTerm;
                }

                // Suggested terms:
                //     chocolade
                //     schokolade
                //     chocolate
                //     chowder
                //     marmalade
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region suggestions_index_1
// The IndexEntry class defines the index-fields
class Products_ByName_IndexEntry
{
    private ?string $productName = null;

    public function getProductName(): ?string
    {
        return $this->productName;
    }

    public function setProductName(?string $productName): void
    {
        $this->productName = $productName;
    }
}
class Products_ByName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // The 'Map' function defines the content of the index-fields
        $this->map = "from product in docs.Products " .
            "select new " .
            "{ " .
            "  product.Name " .
            "} ";

        // Configure index-field 'ProductName' for suggestions
        $this->suggestion("Name"); // configuring suggestions

        // Optionally: set 'Search' on this field
        // This will split the field content into multiple terms allowing for a full-text search
        $this->index("Name", FieldIndexing::search()); // (optional) splitting name into multiple tokens

    }
}
# endregion

# region suggestions_index_2
// The IndexEntry class defines the index-fields.
class Companies_ByNameAndByContactName_IndexEntry
{
    private ?string $companyName = null;
    private ?string $contactName = null;

    public function getCompanyName(): ?string
    {
        return $this->companyName;
    }

    public function setCompanyName(?string $companyName): void
    {
        $this->companyName = $companyName;
    }

    public function getContactName(): ?string
    {
        return $this->contactName;
    }

    public function setContactName(?string $contactName): void
    {
        $this->contactName = $contactName;
    }
}

class Companies_ByNameAndByContactName extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // The 'Map' function defines the content of the index-fields
        $this->map= "from company in docs.Companies" .
            "select new { " .
                "CompanyName = company.Name, " .
                "ContactName = company.Contact.Name " .
            "}";

        // Configure the index-fields for suggestions
        $this->suggestion("CompanyName");
        $this->suggestion("ContactName");

        // Optionally: set 'Search' on the index-fields
        // This will split the fields' content into multiple terms allowing for a full-text search
        $this->index("CompanyName", FieldIndexing::search());
        $this->index("ContactName", FieldIndexing::search());
    }
}
# endregion
