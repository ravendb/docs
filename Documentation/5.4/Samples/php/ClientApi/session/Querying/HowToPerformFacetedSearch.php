<?php

use RavenDB\Documents\Queries\Facets\AggregationArray;
use RavenDB\Documents\Queries\Facets\AggregationDocumentQueryInterface;
use RavenDB\Documents\Queries\Facets\FacetOperationsInterface;
use RavenDB\Documents\Queries\Facets\FacetOptions;
use RavenDB\Documents\Queries\Facets\FacetResult;
use RavenDB\Documents\Queries\Facets\FacetSetup;
use RavenDB\Documents\Queries\Facets\FacetTermSortMode;
use RavenDB\Documents\Queries\Facets\RangeBuilder;
use RavenDB\Documents\Queries\Query;
use RavenDB\Documents\Session\Tokens\FacetToken;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Type\StringArray;


//region facet_7_3
abstract class FacetBase
{
    /** @SerializedName("DisplayFieldName") */
    private ?string $displayFieldName = null;

    /** @SerializedName("Options") */
    private ?FacetOptions $options = null;

    /** @SerializedName("Aggregations") */
    private ?AggregationArray $aggregations = null;

    public function __construct()
    {
        $this->aggregations = new AggregationArray();
    }

    public function getDisplayFieldName(): ?string
    {
        return $this->displayFieldName;
    }

    public function setDisplayFieldName(?string $displayFieldName): void
    {
        $this->displayFieldName = $displayFieldName;
    }

    public function getOptions(): ?FacetOptions
    {
        return $this->options;
    }

    public function setOptions(?FacetOptions $options): void
    {
        $this->options = $options;
    }

    public function getAggregations(): ?AggregationArray
    {
        return $this->aggregations;
    }

    public function setAggregations(null|array|StringArray $aggregations): void
    {
        if (is_array($aggregations)) {
            $aggregations = AggregationArray::fromArray($aggregations);
        }
        $this->aggregations = $aggregations;
    }

    public abstract function toFacetToken(Closure $addQueryParameter): FacetToken;
}

class Facet extends FacetBase
{
    /** @SerializedName("FieldName") */
    private string $fieldName;

    public function getFieldName(): string
    {
        return $this->fieldName;
    }

    public function setFieldName(string $fieldName): void
    {
        $this->fieldName = $fieldName;
    }

    public function toFacetToken(Closure $addQueryParameter): FacetToken
    {
        return FacetToken::create($this, $addQueryParameter);
    }
}
//endregion


//region facet_7_4
class RangeFacet extends FacetBase
{
    private ?FacetBase $parent = null;

    /**
     * @SerializedName("Ranges")
     */
    private StringArray $ranges;

    public function __construct(?FacetBase $parent = null)
    {
        parent::__construct();

        $this->parent = $parent;
        $this->ranges = new StringArray();
    }

    public function getRanges(): StringArray
    {
        return $this->ranges;
    }

    /**
     * @param array|StringArray $ranges
     */
    public function setRanges(array|StringArray $ranges): void
    {
        if (is_array($ranges)) {
            $ranges = StringArray::fromArray($ranges);
        }
        $this->ranges = $ranges;
    }

    public function toFacetToken($addQueryParameter): FacetToken
    {
        if ($this->parent != null) {
            return $this->parent->toFacetToken($addQueryParameter);
        }

        return FacetToken::create($this, $addQueryParameter);
    }
}
//endregion

//region facet_7_5
class FacetAggregation
{
    public const NONE = "NONE";
    public const MAX  = "MAX";
    public const MIN = "MIN";
    public const AVERAGE = "AVERAGE";
    public const SUM = "SUM";
}
//endregion

interface FooInterface
{
    //region facet_1
    /**
     * Usage
     *   - aggregateBy(Callable $builder);
     *   - aggregateBy(FacetBase $facet);
     *   - aggregateBy(Facet... $facets);
     *
     * @param Callable|FacetBase $builderOrFacets
     *
     * @return AggregationDocumentQueryInterface
     */
    public function aggregateBy(...$builderOrFacets): AggregationDocumentQueryInterface;

    public function aggregateUsing(?string $facetSetupDocumentId): AggregationDocumentQueryInterface;
    //endregion

    //region facet_7_1
    public function byRanges(?RangeBuilder $range, ?RangeBuilder ...$ranges): FacetOperationsInterface;

    public function byField(string $fieldName): FacetOperationsInterface;
    public function withDisplayName(string $displayName): FacetOperationsInterface;

    public function withOptions(FacetOptions $options): FacetOperationsInterface;

    public function sumOn(string $path, ?string $displayName = null): FacetOperationsInterface;
    public function minOn(string $path, ?string $displayName = null): FacetOperationsInterface;
    public function maxOn(string $path, ?string $displayName = null): FacetOperationsInterface;
    public function averageOn(string $path, ?string $displayName = null): FacetOperationsInterface;
    //endregion
}

class Foo1 {
    //region facet_7_2
    private FacetTermSortMode $termSortMode;
    private bool $includeRemainingTerms = false;
    private int $start = 0;
    private int $pageSize = 0;

    private const INT_32_MAX = 2147483647;

    public function __construct() {
        $this->pageSize = self::INT_32_MAX;
        $this->termSortMode = FacetTermSortMode::valueAsc();
    }

    //getters and setters
    //endregion
}

class HowToPerformFacetedSearch
{
    public function samples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {

            $session = $store->openSession();
            try {
                //region facet_2_1
                $facetOptions = new FacetOptions();
                $facetOptions->setTermSortMode(FacetTermSortMode::countDesc());

                $facet1 = new Facet();
                $facet1->setFieldName("manufacturer");
                $facet1->setOptions($facetOptions);

                $facet2 = new RangeFacet();
                $facet2->setRanges([
                    "cost < 200",
                    "cost between 200 and 400",
                    "cost between 400 and 600",
                    "cost between 600 and 800",
                    "cost >= 800"
                ]);
                $facet2->setAggregations([FacetAggregation::AVERAGE, "cost"]);

                $facet3 = new RangeFacet();
                $facet3->setRanges([
                    "megapixels < 3",
                    "megapixels between 3 and 7",
                    "megapixels between 7 and 10",
                    "megapixels >= 10"
                ]);

                /** @var array<FacetResult> $facets */
                $facets = $session
                    ->query(Camera::class, Query::index("Camera/Costs"))
                    ->aggregateBy($facet1)
                    ->andAggregateBy($facet2)
                    ->andAggregateBy($facet3)
                    ->execute();
                //endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                //region facet_3_1
                $options = new FacetOptions();
                $options->setTermSortMode(FacetTermSortMode::countDesc());

                $costBuilder = RangeBuilder::forPath("cost");
                $megapixelsBuilder = RangeBuilder::forPath("megapixels");

                /** @var array<FacetResult> $facetResult */
                $facetResult = $session
                    ->query(Camera::class, Query::index("Camera/Costs"))
                    ->aggregateBy(function($builder) use ($options) {
                        $builder
                            ->byField("manufacturer")
                            ->withOptions($options);
                    })
                    ->andAggregateBy(function($builder) use ($costBuilder) {
                        $builder
                            ->byRanges(
                                $costBuilder->isLessThan(200),
                                $costBuilder->isGreaterThanOrEqualTo(200)->isLessThan(400),
                                $costBuilder->isGreaterThanOrEqualTo(400)->isLessThan(600),
                                $costBuilder->isGreaterThanOrEqualTo(600)->isLessThan(800),
                                $costBuilder->isGreaterThanOrEqualTo(800)
                             )
                            ->averageOn("cost");
                    })
                    ->andAggregateBy(function($builder) use ($megapixelsBuilder) {
                        $builder
                            ->byRanges(
                                $megapixelsBuilder->isLessThan(3),
                                $megapixelsBuilder->isGreaterThanOrEqualTo(3)->isLessThan(7),
                                $megapixelsBuilder->isGreaterThanOrEqualTo(7)->isLessThan(10),
                                $megapixelsBuilder->isGreaterThanOrEqualTo(10)
                            );
                    })
                    ->execute();
                //endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                //region facet_4_1
                $facetSetup = new FacetSetup();

                $facetManufacturer = new Facet();
                $facetManufacturer->setFieldName("manufacturer");
                $facetSetup->setFacets([$facetManufacturer]);

                $cameraFacet = new RangeFacet();
                $cameraFacet->setRanges([
                    "cost < 200",
                    "cost between 200 and 400",
                    "cost between 400 and 600",
                    "cost between 600 and 800",
                    "cost >= 800"
                ]);

                $megapixelsFacet = new RangeFacet();
                $megapixelsFacet->setRanges([
                    "megapixels < 3",
                    "megapixels between 3 and 7",
                    "megapixels between 7 and 10",
                    "megapixels >= 10"
                ]);

                $facetSetup->setRangeFacets([$cameraFacet, $megapixelsFacet]);

                $session->store($facetSetup, "facets/CameraFacets");
                $session->saveChanges();

                /** @var array<FacetResult> $facets */
                $facets = $session
                    ->query(Camera::class, Query::index("Camera/Costs"))
                    ->aggregateUsing("facets/CameraFacets")
                    ->execute();
                //endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

class Camera
{

}