import type { ReactNode } from "react";
import StartingPoint from "@site/src/components/Homepage/StartingPoints/StartingPointItem";
import Heading from "@theme/Heading";

const StartingPointsList = [
  {
    title: "Developer",
    url: "/start/getting-started#documentstore",
    description: (
      <>
        Learn how to create a client, connect to the server, handle documents
        and more
      </>
    ),
  },
  {
    title: "DevOps",
    url: "/start/getting-started",
    description: (
      <>
        Learn how to install RavenDB, set up a cluster, maintain the database
        and more
      </>
    ),
  },
];

export default function StartingPoints(): ReactNode {
  return (
    <section className="mb-8">
      <Heading as="h3">Starting points</Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
        {StartingPointsList.map((props, idx) => (
          <StartingPoint key={idx} {...props} />
        ))}
      </div>
    </section>
  );
}
