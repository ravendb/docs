<?php

namespace RavenDB\Samples\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\IndexDefinition;
use RavenDB\Documents\Operations\Indexes\PutIndexesOperation;
use RavenDB\Type\StringArray;
use RavenDB\Type\TypedList;

class IndexingHierarchicalData
{
    public function sample(): void
    {
        $store = new DocumentStore();
        try {
            # region indexes_3
            $indexDefinition = new IndexDefinition();
            $indexDefinition->setName("BlogPosts/ByCommentAuthor");
            $indexDefinition->setMaps([
                "from blogpost in docs.BlogPosts
                from comment in Recurse(blogpost, (Func<dynamic, dynamic>)(x => x.Comments))
                select new
                {
                    Author = comment.Author
                }"
            ]);

            $store->maintenance()->send(new PutIndexesOperation($indexDefinition));
            # endregion

            $session = $store->openSession();
            try {
                # region indexes_4
                /** @var array<BlogPost> $results */
                $results = $session
                    ->query(BlogPosts_ByCommentAuthor_Result::class, BlogPosts_ByCommentAuthor::class)
                    ->whereEquals("authors", "john")
                    ->ofType(BlogPost::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region indexes_5
                /** @var array<BlogPost> $results */
                $results = $session
                        ->advanced()
                        ->documentQuery(BlogPost::class, BlogPosts_ByCommentAuthor::class)
                        ->whereEquals("authors", "John")
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

# region indexes_1
class BlogPost
{
    private ?string $author = null;
    private ?string $title = null;
    private ?string $text = null;

    // Blog post readers can leave comments
    public ?BlogPostCommentList $comments = null;

    public function getAuthor(): ?string
    {
        return $this->author;
    }

    public function setAuthor(?string $author): void
    {
        $this->author = $author;
    }

    public function getTitle(): ?string
    {
        return $this->title;
    }

    public function setTitle(?string $title): void
    {
        $this->title = $title;
    }

    public function getText(): ?string
    {
        return $this->text;
    }

    public function setText(?string $text): void
    {
        $this->text = $text;
    }

    public function getComments(): ?BlogPostCommentList
    {
        return $this->comments;
    }

    public function setComments(?BlogPostCommentList $comments): void
    {
        $this->comments = $comments;
    }
}

class BlogPostComment
{
    private ?string $author = null;
    private ?string $text = null;

    // Comments can be left recursively
    private ?BlogPostCommentList $comments = null;

    public function getAuthor(): ?string
    {
        return $this->author;
    }

    public function setAuthor(?string $author): void
    {
        $this->author = $author;
    }

    public function getText(): ?string
    {
        return $this->text;
    }

    public function setText(?string $text): void
    {
        $this->text = $text;
    }

    public function getComments(): ?BlogPostCommentList
    {
        return $this->comments;
    }

    public function setComments(?BlogPostCommentList $comments): void
    {
        $this->comments = $comments;
    }
}

class BlogPostCommentList extends TypedList
{
    public function __construct()
    {
        parent::__construct(BlogPost::class);
    }
}
# endregion

# region indexes_2
class BlogPosts_ByCommentAuthor_Result
{
    private ?StringArray $authors = null;

    public function getAuthors(): ?StringArray
    {
        return $this->authors;
    }

    public function setAuthors(?StringArray $authors): void
    {
        $this->authors = $authors;
    }
}

class BlogPosts_ByCommentAuthor extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from blogpost in docs.Blogposts let authors = Recurse(blogpost, x => x.comments) select new { authors = authors.Select(x => x.author) }";
    }
}
# endregion

