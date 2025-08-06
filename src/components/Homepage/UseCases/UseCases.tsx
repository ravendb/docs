import type { ReactNode } from "react";
import Heading from "@theme/Heading";
import UseCaseItem from "@site/src/components/Homepage/UseCases/UseCaseItem";
import Link from "@docusaurus/Link";
import { Icon } from "../../Common/Icon";

const useCases = [
  {
    title: "Short introduction to data modeling in non-relational database",
    imgSrc:
      "https://ravendb.net/wp-content/uploads/2025/06/article-cover-genai.png",
    description: (
      <>
        Learn how to harness the power of Generative AI by pairing it with a
        high-performance document database.
      </>
    ),
    url: "https://ravendb.net/articles/survive-the-ai-tidal-wave-with-ravendb-genai",
  },
  {
    title: "Processing invoices using Data Subscriptions in RavenDB",
    imgSrc:
      "https://ravendb.net/wp-content/uploads/2024/12/processing-invoices-article-cover.jpg",
    description: (
      <>
        Learn how to process invoices in asynchronous manner using the data
        subscriptions feature
      </>
    ),
    url: "https://ravendb.net/articles/processing-invoices-using-data-subscriptions-in-ravendb",
  },
];

export default function UseCases(): ReactNode {
  return (
    <section className="mb-8">
      <div className="flex justify-between items-baseline">
        <Heading as="h3">Use cases</Heading>
        <Link
          to="https://ravendb.net/articles"
          className="inline-flex text-base/4 gap-2"
        >
          See all <Icon icon="newtab" size="xs" />
        </Link>
      </div>
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
        {useCases.map((props, idx) => (
          <UseCaseItem key={idx} {...props} />
        ))}
      </div>
    </section>
  );
}
