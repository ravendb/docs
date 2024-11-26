<?php

use RavenDB\Constants\PhpClient;
use RavenDB\Documents\Queries\MoreLikeThis\MoreLikeThisBase;
use RavenDB\Documents\Queries\MoreLikeThis\MoreLikeThisOperationsInterface;
use RavenDB\Documents\Queries\MoreLikeThis\MoreLikeThisOptions;
use RavenDB\Documents\Queries\Query;
use RavenDB\Documents\Session\DocumentQueryInterface;

class Foo {
    # region more_like_this_2
    public const DEFAULT_MAXIMUM_NUMBER_OF_TOKENS_PARSED = 5000;
    public const DEFAULT_MINIMUM_TERM_FREQUENCY = 2;
    public const DEFAULT_MINIMUM_DOCUMENT_FREQUENCY = 5;
    public const DEFAULT_MAXIMUM_DOCUMENT_FREQUENCY = PhpClient::INT_MAX_VALUE;
    public const DEFAULT_BOOST = false;
    public const DEFAULT_BOOST_FACTOR = 1;
    public const DEFAULT_MINIMUM_WORD_LENGTH = 0;
    public const DEFAULT_MAXIMUM_WORD_LENGTH = 0;
    public const DEFAULT_MAXIMUM_QUERY_TERMS = 25;

    private ?int $minimumTermFrequency = null;
    private ?int $maximumQueryTerms = null;
    private ?int $maximumNumberOfTokensParsed = null;
    private ?int $minimumWordLength = null;
    private ?int $maximumWordLength = null;
    private ?int $minimumDocumentFrequency = null;
    private ?int $maximumDocumentFrequency = null;
    private ?int $maximumDocumentFrequencyPercentage = null;
    private ?bool $boost = null;
    private ?float $boostFactor = null;
    private ?string $stopWordsDocumentId = null;
    private ?array $fields = null;

    # endregion
}

interface FooInterface2 {
    # region more_like_this_1

    /**
     * Usage:
     *   - moreLikeThis(MoreLikeThisBase $moreLikeThis);
     *   - moreLikeThis(function(MoreLikeThisBuilder($builder) {...});
     *
     * @param MoreLikeThisBase|Closure|null $moreLikeThisOrBuilder
     * @return DocumentQueryInterface
     */
    public function moreLikeThis(null|MoreLikeThisBase|Closure $moreLikeThisOrBuilder): DocumentQueryInterface;
    # endregion

    # region more_like_this_3
    /**
     * Usage:
     *   - usingDocument();
     *   - usingDocument(string $documentJson);
     *   - usingDocument(function(MoreLikeThisBuilder $build) {...});
     *
     * @param string|Closure|null $documentJsonOrBuilder
     * @return MoreLikeThisOperationsInterface
     */
    function usingDocument(null|string|Closure $documentJsonOrBuilder): MoreLikeThisOperationsInterface;

    function usingDocumentWithJson(?string $documentJson): MoreLikeThisOperationsInterface; // same as calling usingDocument(string $documentJson)

    function usingDocumentWithBuilder(?Closure $builder): MoreLikeThisOperationsInterface; // same as calling usingDocument(function($builder) {...});

    function withOptions(MoreLikeThisOptions $options): MoreLikeThisOperationsInterface;
    # endregion
}

class Article {
    private ?string $id = null;
    private ?string $category = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getCategory(): ?string
    {
        return $this->category;
    }

    public function setCategory(?string $category): void
    {
        $this->category = $category;
    }
}

class MoreLikeThis
{
    public function samples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {

            $session = $store->openSession();
            try {
                # region more_like_this_4
                // Search for similar articles to 'articles/1'
                // using 'Articles/MoreLikeThis' index and search only field 'body'
                $options = new MoreLikeThisOptions();
                $options->setFields([ "body" ]);

                /** @var array<Article> $articles */
                $articles = $session
                    ->query(Article::class, Query::index("Articles/MoreLikeThis"))
                    ->moreLikeThis(function($builder) use ($options) {
                        $builder
                            ->usingDocument(function ($x) {
                                $x->whereEquals("id()", "articles/1");
                            })
                            ->withOptions($options);
                    })
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region more_like_this_6
                // Search for similar articles to 'articles/1'
                // using 'Articles/MoreLikeThis' index and search only field 'body'
                // where article category is 'IT'
                $options = new MoreLikeThisOptions();
                $options->setFields([ "body" ]);
                /** @var array<Article> $articles */
                $articles = $session
                    ->query(Article::class, Query::index("Articles/MoreLikeThis"))
                    ->moreLikeThis(function($builder) use ($options) {
                        $builder
                            ->usingDocument(function ($x) {
                                $x->whereEquals("id()", "articles/1");
                            })
                            ->withOptions($options);
                    })
                    ->whereEquals("category", "IT")
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
