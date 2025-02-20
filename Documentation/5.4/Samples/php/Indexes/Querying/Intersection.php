<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Type\TypedList;

class Intersection
{
    public function sample(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region intersection_3

                // first tShirt
                $tShirt = new TShirt();
                $tShirt->setId("tshirts/1");
                $tShirt->setManufacturer("Raven");
                $tShirt->setReleaseYear(2010);

                $types = TShirtTypeList::fromArray([
                    new TShirtType(color: "Blue", size: "Small"),
                    new TShirtType(color: "Black", size: "Small"),
                    new TShirtType(color: "Black", size: "Medium"),
                    new TShirtType(color: "Gray", size: "Large")
                ]);
                $tShirt->setTypes($types);

                $session->store($tShirt);

                // second tShirt
                $tShirt = new TShirt();
                $tShirt->setId("tshirts/2");
                $tShirt->setManufacturer("Wolf");
                $tShirt->setReleaseYear(2011);

                $types = TShirtTypeList::fromArray([
                    new TShirtType(color: "Blue", size: "Small"),
                    new TShirtType(color: "Black", size: "Large"),
                    new TShirtType(color: "Black", size: "Medium")
                ]);
                $tShirt->setTypes($types);

                $session->store($tShirt);

                // third tShirt
                $tShirt = new TShirt();
                $tShirt->setId("tshirts/3");
                $tShirt->setManufacturer("Raven");
                $tShirt->setReleaseYear(2011);

                $types = TShirtTypeList::fromArray([
                    new TShirtType(color: "Yellow", size: "Small"),
                    new TShirtType(color: "Gray", size: "Large")
                ]);
                $tShirt->setTypes($types);

                $session->store($tShirt);

                // fourth tShirt
                $tShirt = new TShirt();
                $tShirt->setId("tshirts/4");
                $tShirt->setManufacturer("Raven");
                $tShirt->setReleaseYear(2012);

                $types = TShirtTypeList::fromArray([
                    new TShirtType(color: "Blue", size: "Small"),
                    new TShirtType(color: "Gray", size: "Large")
                ]);
                $tShirt->setTypes($types);

                $session->store($tShirt);
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region intersection_4
                /** @var array<TShirt> $results */
                $results = $session->query(TShirts_ByManufacturerColorSizeAndReleaseYear_Result::class, TShirts_ByManufacturerColorSizeAndReleaseYear::class)
                    ->whereEquals("Manufacturer", "Raven")
                    ->intersect()
                    ->whereEquals("Color", "Blue")
                    ->andAlso()
                    ->whereEquals("Size", "Small")
                    ->intersect()
                    ->whereEquals("Color", "Gray")
                    ->andAlso()
                    ->whereEquals("Size", "Large")
                    ->ofType(TShirt::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region intersection_5
                /** @var array<TShirt> $results */
                $results = $session
                    ->advanced()
                    ->documentQuery(TShirt::class, TShirts_ByManufacturerColorSizeAndReleaseYear::class)
                    ->whereEquals("Manufacturer", "Raven")
                    ->intersect()
                    ->whereEquals("Color", "Blue")
                    ->andAlso()
                    ->whereEquals("Size", "Small")
                    ->intersect()
                    ->whereEquals("Color", "Gray")
                    ->andAlso()
                    ->whereEquals("Size", "Large")
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

# region intersection_1
class TShirt
{
    private ?string $id = null;
    private ?int $releaseYear = null;
    private ?string $manufacturer = null;
    private ?TShirtTypeList $types = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getReleaseYear(): ?int
    {
        return $this->releaseYear;
    }

    public function setReleaseYear(?int $releaseYear): void
    {
        $this->releaseYear = $releaseYear;
    }

    public function getManufacturer(): ?string
    {
        return $this->manufacturer;
    }

    public function setManufacturer(?string $manufacturer): void
    {
        $this->manufacturer = $manufacturer;
    }

    public function getTypes(): ?TShirtTypeList
    {
        return $this->types;
    }

    public function setTypes(?TShirtTypeList $types): void
    {
        $this->types = $types;
    }
}

class TShirtType
{
    private ?string $color = null;
    private ?string $size = null;

    public function __construct(?string $color, ?string $size)
    {
        $this->color = $color;
        $this->size = $size;
    }

    public function getColor(): ?string
    {
        return $this->color;
    }

    public function setColor(?string $color): void
    {
        $this->color = $color;
    }

    public function getSize(): ?string
    {
        return $this->size;
    }

    public function setSize(?string $size): void
    {
        $this->size = $size;
    }
}

class TShirtTypeList extends TypedList
{
    protected function __construct()
    {
        parent::__construct(TShirtType::class);
    }
}
# endregion

# region intersection_2
class TShirts_ByManufacturerColorSizeAndReleaseYear_Result
{
    private ?string $manufacturer = null;
    private ?string $color = null;
    private ?string $size = null;
    private ?int $releaseYear = null;
    // ... getters and setters
}
class TShirts_ByManufacturerColorSizeAndReleaseYear extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.TShirts.SelectMany(tshirt => tshirt.types, (tshirt, type) => new {" .
            "    manufacturer = tshirt.manufacturer," .
            "    color = type.color," .
            "    size = type.size," .
            "    releaseYear = tshirt.releaseYear" .
            "})";
    }
}
# endregion
