<?php

namespace RavenDB\Samples\Indexes\Querying;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\FieldStorage;
use RavenDB\Documents\Queries\MoreLikeThis\MoreLikeThisOptions;
use RavenDB\Documents\Queries\MoreLikeThis\MoreLikeThisStopWords;

class MoreLikeThis
{
    public function Sample(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region more_like_this_1
                /** @var array<Article> $articles */
                $articles = $session
                    ->query(Article::class, Articles_ByArticleBody::class)
                    ->moreLikeThis(function($builder) {
                        return $builder
                            ->usingDocument(function($x) {
                                return $x->whereEquals("id()", "articles/1");
                            });
                    })
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region more_like_this_1_1
                /** @var array<Article> $articles */
                $articles = $session->advanced()
                    ->documentQuery(Article::class, Articles_ByArticleBody::class)
                    ->moreLikeThis(function($builder) {
                        return $builder
                            ->usingDocument(function($x) {
                                return $x->whereEquals("id()", "articles/1");
                            });
                    })
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region more_like_this_2
                /** @var array<Article> $articles */
                $articles = $session
                    ->query(Article::class, Articles_ByArticleBody::class)
                    ->moreLikeThis(function($builder) {
                        $mlt = new MoreLikeThisOptions();
                        $mlt->setFields(["ArticleBody"]);

                        return $builder
                            ->usingDocument(function($x) {
                                return $x->whereEquals("id()", "articles/1");
                            })
                            ->withOptions($mlt);
                    })
                    ->toList();
                # endregion

                # region more_like_this_3
                $mlt = new MoreLikeThisStopWords();
                $mlt->setId("Config/Stopwords");
                $mlt->setStopWords(["I", "A", "Be"]);
                $session->store($mlt);
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region more_like_this_2_1
                /** @var array<Article> $articles */
                $articles = $session->advanced()
                    ->documentQuery(Article::class, Articles_ByArticleBody::class)
                    ->moreLikeThis(function($builder) {
                        $mlt = new MoreLikeThisOptions();
                        $mlt->setFields(["ArticleBody"]);

                        return $builder
                            ->usingDocument(function($x) {
                                return $x->whereEquals("id()", "articles/1");
                            })
                            ->withOptions($mlt);
                    })
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

# region more_like_this_4
class Article
{
    public ?string $id = null;
    public ?string $name = null;
    public ?string $articleBody = null;

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

    public function getArticleBody(): ?string
    {
        return $this->articleBody;
    }

    public function setArticleBody(?string $articleBody): void
    {
        $this->articleBody = $articleBody;
    }
}

class Articles_ByArticleBody extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from doc in docs.Articles select { doc.ArticleBody }";

        $this->store("ArticleBody", FieldStorage::yes());
        $this->analyze("ArticleBody", "StandardAnalyzer");
    }
}
# endregion

