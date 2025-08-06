import type { ReactNode } from "react";
import Heading from "@theme/Heading";
import UseCaseItem from "@site/src/components/Homepage/UseCases/UseCaseItem";
import Link from "@docusaurus/Link";
import { Icon } from "../../Common/Icon";

const cloudUseCases = [
  {
    title: "Connecting C# application to Cloud",
    imgSrc:
      "https://ravendb.net/wp-content/uploads/2025/01/connecting-c-application-article-cover.jpg",
    description: (
      <>
        Learn how to connect to your Cloud instance with C#, and how to confirm
        your connection is established.
      </>
    ),
    url: "https://ravendb.net/articles/connecting-c-application-to-ravendb-cloud",
  },
  {
    title: "Connecting Node.js application to Cloud",
    imgSrc:
      "https://ravendb.net/wp-content/uploads/2025/01/connecting-nodejs-to-ravendb-article-cover.jpg",
    description: (
      <>
        Learn how to connect to your Cloud instance with Node.js, and how to
        confirm your connection is established.
      </>
    ),
    url: "https://ravendb.net/articles/connecting-node-js-application-to-ravendb-cloud",
  },
];

export default function CloudUseCases(): ReactNode {
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
        {cloudUseCases.map((props, idx) => (
          <UseCaseItem key={idx} {...props} />
        ))}
      </div>
    </section>
  );
}
