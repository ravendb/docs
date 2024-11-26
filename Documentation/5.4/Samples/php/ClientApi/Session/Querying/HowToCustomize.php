<?php

use RavenDB\Documents\Queries\ProjectionBehavior;
use RavenDB\Documents\Session\BeforeQueryEventArgs;
use RavenDB\Documents\Session\DocumentQueryCustomizationInterface;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Type\Duration;

interface FooInterface {
    # region customize_1_0
    public function addBeforeQueryExecutedListener(Closure $action): DocumentQueryCustomizationInterface;
    public function removeBeforeQueryExecutedListener(Closure $action): DocumentQueryCustomizationInterface;
    # endregion

    # region customize_1_0_0
    public function addAfterQueryExecutedListener(Closure $action): DocumentQueryCustomizationInterface;
    public function removeAfterQueryExecutedListener(Closure $action): DocumentQueryCustomizationInterface;
    # endregion

    # region customize_2_0
    public function noCaching(): DocumentQueryCustomizationInterface
    # endregion

    # region customize_3_0
    public function noTracking(): DocumentQueryCustomizationInterface
    # endregion

    # region customize_4_0
    public function randomOrdering(): DocumentQueryCustomizationInterface;
    public function randomOrdering(?string $seed): DocumentQueryCustomizationInterface;
    # endregion

    # region customize_8_0
    public function waitForNonStaleResults(): DocumentQueryCustomizationInterface;
    public function waitForNonStaleResults(Duration $waitTimeout): DocumentQueryCustomizationInterface;
    # endregion

    # region projectionbehavior
    public function projection(ProjectionBehavior $projectionBehavior): DocumentQueryCustomizationInterface;

    class ProjectionBehavior
    {
        public static function default(): ProjectionBehavior;
        public static function fromIndex(): ProjectionBehavior;
        public static function fromIndexOrThrow(): ProjectionBehavior;
        public static function fromDocument(): ProjectionBehavior;
        public static function fromDocumentOrThrow(): ProjectionBehavior;
    }
    # endregion
}

class HowToCustomize
{
    public function showSamples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                # region customize_1_1
                $session->advanced()->addBeforeQueryListener(function ($sender, BeforeQueryEventArgs $event) {
                    $event->getQueryCustomization()
                        ->addBeforeQueryExecutedListener(function($q) {
                            // set 'pageSize' to 10
                            $q->setPageSize(10);
                        });
                });

                $session->query(Employee::class)->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region customize_1_1_0
                $queryDuration = null;

                $session->query(Employee::class)
                    ->addAfterQueryExecutedListener(function($result) use (&$queryDuration) {
                        $queryDuration = Duration::ofMillis($result->getDurationInMs());
                    })
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region customize_2_1
                $session->advanced()
                    ->addBeforeQueryListener(function($sender, BeforeQueryEventArgs $event) {
                        $event->getQueryCustomization()->noCaching();
                    });

                $results = $session->query(Employee::class)
                    ->whereEquals("FirstName", "Robert")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region customize_3_1
                $session->advanced()
                    ->addBeforeQueryListener(function($sender, BeforeQueryEventArgs $event) {
                        $event->getQueryCustomization()->noTracking();
                    });

                $results = $session->query(Employee::class)
                    ->whereEquals("FirstName", "Robert")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region customize_4_1
                $session->advanced()
                    ->addBeforeQueryListener(function($sender, BeforeQueryEventArgs $event) {
                        $event->getQueryCustomization()->randomOrdering();
                    });

                // Result will be ordered randomly each time
                $results = $session->query(Employee::class)
                    ->whereEquals("FirstName", "Robert")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region customize_8_1
                $session->advanced()
                    ->addBeforeQueryListener(function($sender, BeforeQueryEventArgs $event) {
                        $event->getQueryCustomization()->waitForNonStaleResults();
                    });

                $results = $session->query(Employee::class)
                    ->whereEquals("FirstName", "Robert")
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region projectionbehavior_query
                $session->advanced()
                    ->addBeforeQueryListener(function($sender, BeforeQueryEventArgs $event) {
                        $event->getQueryCustomization()->projection(ProjectionBehavior::default());
                    });

                $results = $session->query(Employee::class)
                    ->selectFields(Employee::class, "name")
                    ->toList();
                # endregion

                $session->saveChanges();
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}
