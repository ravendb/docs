import useDocusaurusContext from "@docusaurus/useDocusaurusContext";

export const useLatestVersion = () => {
    const { siteConfig } = useDocusaurusContext();
    return siteConfig.customFields.latestVersion;
};
