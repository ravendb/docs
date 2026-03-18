import type { ReactNode } from "react";
import Heading from "@theme/Heading";
import UseCaseItem from "@site/src/components/Homepage/UseCases/UseCaseItem";
import Link from "@docusaurus/Link";
import { Icon } from "../../Common/Icon";
import surviveTheAiTidalWaveWithRavenDBGenAiImg from "@site/static/img/ravendb/article-cover-genai.png";
import processingInvoicesUsingDataSubscriptionsInRavenDbImg from "@site/static/img/ravendb/processing-invoices-article-cover.jpg";

const useCases = [
    {
        title: "Survive the AI tidal wave with RavenDB & GenAI",
        imgSrc: surviveTheAiTidalWaveWithRavenDBGenAiImg,
        imgAlt: "RavenDB GenAI integration",
        imgWidth: 640,
        imgHeight: 194,
        description: (
            <>
                Learn how to harness the power of Generative AI by pairing it with a high-performance document database.
            </>
        ),
        url: "/guides/survive-the-ai-tidal-wave-with-ravendb-genai",
    },
    {
        title: "Processing invoices using Data Subscriptions in RavenDB",
        imgSrc: processingInvoicesUsingDataSubscriptionsInRavenDbImg,
        imgAlt: "Processing invoices with RavenDB Data Subscriptions",
        imgWidth: 640,
        imgHeight: 194,
        description: <>Learn how to process invoices in asynchronous manner using the data subscriptions feature</>,
        url: "/guides/processing-invoices-using-data-subscriptions-in-ravendb",
    },
];

export default function UseCases(): ReactNode {
    return (
        <section className="mb-8">
            <div className="flex justify-between items-baseline">
                <Heading as="h2">Use cases</Heading>
                <Link to="/guides/tags/use-case" className="inline-flex text-base/4 gap-2">
                    See more <Icon icon="arrow-thin-right" size="xs" />
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
