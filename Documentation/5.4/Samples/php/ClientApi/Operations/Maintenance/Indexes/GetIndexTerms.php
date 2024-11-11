<?php

namespace RavenDB\Samples\ClientApi\Operations\Maintenance\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Indexes\GetTermsOperation;
use RavenDB\Type\StringArrayResult;

class GetIndexTerms
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            # region get_index_terms
            // Define the get terms operation
            // Pass the requested index-name, index-field, start value & page size
            $getTermsOp = new GetTermsOperation("Orders/Totals", "Employee", "employees/5-a", 10);

            // Execute the operation by passing it to Maintenance.Send
            /** @var StringArrayResult $fieldTerms */
            $fieldTerms = $store->maintenance()->send($getTermsOp);

            // fieldTerms will contain the all terms that come after term 'employees/5-a' for index-field 'Employee'
            # endregion
        } finally {
            $store->close();
        }
    }
}

/*
interface IFoo
{
    # region get_index_terms_syntax
    GetTermsOperation(?string $indexName, ?string $field, ?string $fromValue, ?int $pageSize = null)
    # endregion
}
*/
