import { useDocsVersion, useActivePlugin } from "@docusaurus/plugin-content-docs/client";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import { SeeAlsoItemType } from "./types";

export function useVersionedLink() {
    const versionMetadata = useDocsVersion();
    const activePlugin = useActivePlugin();
    const { siteConfig } = useDocusaurusContext();
    const latestVersion = siteConfig.customFields?.latestVersion as string;
    const currentPluginId = activePlugin?.pluginId || "default";

    const getVersionPath = (): string => {
        if (!versionMetadata) {
            return latestVersion;
        }

        if (versionMetadata.version === "current") {
            return versionMetadata.label;
        }

        return versionMetadata.version;
    };

    const currentVersionPath = getVersionPath();

    const getVersionedLink = (item: SeeAlsoItemType): string => {
        if (item.source !== "docs") {
            return item.link;
        }

        const link = item.link.startsWith("/") ? item.link : `/${item.link}`;
        const hasVersionPrefix = /^\/\d+\.\d+\//.test(link);

        if (hasVersionPrefix) {
            return link;
        }

        const isUnversionedSection = /^\/(cloud|guides|templates)(\/|$)/.test(link);

        if (isUnversionedSection) {
            return link;
        }

        const versionToUse =
            currentPluginId === "cloud" || currentPluginId === "guides" || currentPluginId === "templates"
                ? latestVersion
                : currentVersionPath;

        return `/${versionToUse}${link}`;
    };

    return { getVersionedLink };
}
