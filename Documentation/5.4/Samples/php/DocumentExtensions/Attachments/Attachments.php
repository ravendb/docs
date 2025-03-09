<?php

namespace RavenDB\Samples\DocumentExtensions\Attachments;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Operations\Attachments\AttachmentName;
use RavenDB\Documents\Operations\Attachments\AttachmentNameArray;
use RavenDB\Documents\Operations\Attachments\CloseableAttachmentResult;
use RavenDB\ServerWide\DatabaseRecord;
use RavenDB\ServerWide\Operations\DeleteDatabaseCommandParameters;
use RavenDB\ServerWide\Operations\DeleteDatabasesOperation;
use RavenDB\ServerWide\Operations\CreateDatabaseOperation;
use RavenDB\Type\StringArray;

interface IFoo
{
    # region StoreSyntax
    public function store(object|string $idOrEntity, ?string $name, mixed $stream, ?string $contentType = null): void;

    public function storeFile(object|string $idOrEntity, ?string $name, string $filePath): void;
    # endregion

    # region GetSyntax
    function get(object|string $idOrEntity, ?string $name): CloseableAttachmentResult;
    function getNames(?object $entity): AttachmentNameArray;
    function exists(?string $documentId, ?string $name): bool;
    function getRevision(?string $documentId, ?string $name, ?string $changeVector): CloseableAttachmentResult;
    # endregion

    # region DeleteSyntax
    /**
     * Marks the specified document's attachment for deletion.
     * The attachment will be deleted when saveChanges is called.
     */
    public function delete(object|string $idOrEntity, ?string $name): void;
    # endregion
}


class Attachments
{
    public function storeAttachment(): void
    {
        $store = new DocumentStore();
        try {

        # region StoreAttachment
            $session = $store->openSession();
            try {
                $file1 = file_get_contents("001.jpg");
                $file2 = file_get_contents("002.jpg");
                $file3 = file_get_contents("003.jpg");
                $file4 = file_get_contents("004.mp4");

                $album = new Album();
                $album->setName("Holidays");
                $album->setDescription("Holidays travel pictures of the all family");
                $album->setTags(["Holidays Travel", "All Family"]);

                $session->store($album, "albums/1");

                $session->advanced()->attachments()->store("albums/1", "001.jpg", $file1, "image/jpeg");
                $session->advanced()->attachments()->store("albums/1", "002.jpg", $file2, "image/jpeg");
                $session->advanced()->attachments()->store("albums/1", "003.jpg", $file3, "image/jpeg");
                $session->advanced()->attachments()->store("albums/1", "004.mp4", $file4, "video/mp4");

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion

        } finally {
            $store->close();
        }
    }

    public function GetAttachment(): void
    {
        $store = new DocumentStore();
        try {
            # region GetAttachment
            $session = $store->openSession();
            try {
                $album = $session->load(Album::class, "albums/1");

                $file1 = $session->advanced()->attachments()->get($album, "001.jpg");
                $file2 = $session->advanced()->attachments()->get("albums/1", "002.jpg");

                $data = $file1->getData();

                $attachmentDetails = $file1->getDetails();
                $name = $attachmentDetails->getName();
                $contentType = $attachmentDetails->getContentType();
                $hash = $attachmentDetails->getHash();
                $size = $attachmentDetails->getSize();
                $documentId = $attachmentDetails->getDocumentId();
                $changeVector = $attachmentDetails->getChangeVector();

                $attachmentNames = $session->advanced()->attachments()->getNames($album);
                /** @var AttachmentName $attachmentName */
                foreach ($attachmentNames as $attachmentName)
                {
                    $name = $attachmentName->getName();
                    $contentType = $attachmentName->getContentType();
                    $hash = $attachmentName->getHash();
                    $size = $attachmentName->getSize();
                }

                $exists = $session->advanced()->attachments()->exists("albums/1", "003.jpg");
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }

    public function deleteAttachment(): void
    {
        $store = new DocumentStore();
        try {
            # region DeleteAttachment
            $session = $store->openSession();
            try {
                $album = $session->load(Album::class, "albums/1");
                $session->advanced()->attachments()->delete($album, "001.jpg");
                $session->advanced()->attachments()->delete("albums/1", "002.jpg");

                $session->saveChanges();
            } finally {
                $session->close();
            }
            # endregion
        } finally {
            $store->close();
        }
    }

    public function getDocumentStore(): DocumentStore
    {
        $store = new DocumentStore(["http://localhost:8080"], "TestDatabase");
        $store->initialize();

        $parameters = new DeleteDatabaseCommandParameters();
        $parameters->setDatabaseNames(["TestDatabase"]);
        $parameters->setHardDelete(true);

        $store->maintenance()->server()->send(new DeleteDatabasesOperation($parameters));
        $store->maintenance()->server()->send(new CreateDatabaseOperation(new DatabaseRecord("TestDatabase")));

        return $store;
    }
}

class Album
{
    private ?string $id = null;
    private ?string $name = null;
    private ?string $description = null;
    private ?StringArray $tags = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getDescription(): ?string
    {
        return $this->description;
    }

    public function setDescription(?string $description): void
    {
        $this->description = $description;
    }

    public function getTags(): ?StringArray
    {
        return $this->tags;
    }

    public function setTags(StringArray|array|null $tags): void
    {
        if (is_array($tags)) {
            $tags = StringArray::fromArray($tags);
        }
        $this->tags = $tags;
    }
}

class User
{
    private ?string $id = null;
    private ?string $name = null;
    private ?string $lastName = null;
    private ?string $addressId = null;
    private ?int $count = null;
    private ?int $age = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getLastName(): ?string
    {
        return $this->lastName;
    }

    public function setLastName(?string $lastName): void
    {
        $this->lastName = $lastName;
    }

    public function getAddressId(): ?string
    {
        return $this->addressId;
    }

    public function setAddressId(?string $addressId): void
    {
        $this->addressId = $addressId;
    }

    public function getCount(): ?int
    {
        return $this->count;
    }

    public function setCount(?int $count): void
    {
        $this->count = $count;
    }

    public function getAge(): ?int
    {
        return $this->age;
    }

    public function setAge(?int $age): void
    {
        $this->age = $age;
    }
}
