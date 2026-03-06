import { IconName } from "@site/src/typescript/iconName";
import { useLatestVersion } from "@site/src/hooks/useLatestVersion";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import CardWithIcon from "../Common/CardWithIcon";

export interface GuideItem {
    title: string;
    description: string;
    icon: IconName;
    url: string;
}

interface GuideProps {
    guide: GuideItem;
}

function useDocAbsoluteUrl(relativeUrl: string): string {
    const latestVersion = useLatestVersion();
    const { siteConfig } = useDocusaurusContext();

    return `${siteConfig.url}/${latestVersion}${relativeUrl}`;
}

export default function Guide({ guide }: GuideProps) {
    const absoluteUrl = useDocAbsoluteUrl(guide.url);

    return <CardWithIcon title={guide.title} icon={guide.icon} description={guide.description} url={absoluteUrl} />;
}
