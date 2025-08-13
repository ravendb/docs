import React, { type ComponentType, type ReactNode } from "react";
import clsx from "clsx";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Translate from "@docusaurus/Translate";
import Button from "@site/src/components/Common/Button";
import {
  useDocsPreferredVersion,
  useDocsVersion,
  useActivePlugin,
  useDocVersionSuggestions,
  type GlobalVersion,
} from "@docusaurus/plugin-content-docs/client";
import { ThemeClassNames } from "@docusaurus/theme-common";
import type { Props } from "@theme/DocVersionBanner";
import type {
  VersionBanner,
  PropVersionMetadata,
} from "@docusaurus/plugin-content-docs";
import { Icon } from "@site/src/components/Common/Icon";

type BannerLabelComponentProps = {
  siteTitle: string;
  versionMetadata: PropVersionMetadata;
};

function UnreleasedVersionLabel({
  siteTitle,
  versionMetadata,
}: BannerLabelComponentProps) {
  return (
    <Translate
      id="theme.docs.versions.unreleasedVersionLabel"
      description="The label used to tell the user that he's browsing an unreleased doc version"
      values={{
        siteTitle,
        versionLabel: <b>{versionMetadata.label}</b>,
      }}
    >
      {
        "This is unreleased documentation for {siteTitle} {versionLabel} version."
      }
    </Translate>
  );
}

function UnmaintainedVersionLabel({
  siteTitle,
  versionMetadata,
}: BannerLabelComponentProps) {
  return (
    <div className="flex items-start gap-2">
      <div className="text-blue-500 flex-shrink-0">
        <Icon icon="info" size="xs" />
      </div>
      <div className="flex-1 min-w-0">
        <Translate
          id="theme.docs.versions.unmaintainedVersionLabel"
          description="The label used to tell the user that he's browsing an unmaintained doc version"
          values={{
            siteTitle,
            versionLabel: <b>{versionMetadata.label}</b>,
          }}
        >
          {
            "You're currently reading RavenDB {versionLabel} Documentation."
          }
        </Translate>
      </div>
    </div>
  );
}

const BannerLabelComponents: {
  [banner in VersionBanner]: ComponentType<BannerLabelComponentProps>;
} = {
  unreleased: UnreleasedVersionLabel,
  unmaintained: UnmaintainedVersionLabel,
};

function BannerLabel(props: BannerLabelComponentProps) {
  const BannerLabelComponent =
    BannerLabelComponents[props.versionMetadata.banner!];
  return <BannerLabelComponent {...props} />;
}

function LatestVersionSuggestionLabel({
  to,
  onClick,
}: {
  to: string;
  onClick: () => void;
  versionLabel: string;
}) {
  return (
    <div>
      <Button
        url={to}
        onClick={onClick}
        variant="outline"
        size="sm"
        className="self-start sm:self-center"
      >
        <Translate
          id="theme.docs.versions.latestVersionLinkLabel"
          description="The label used for the latest version suggestion link label"
        >
          See latest version
        </Translate>
      </Button>
    </div>
  );
}

function DocVersionBannerEnabled({
  className,
  versionMetadata,
}: Props & {
  versionMetadata: PropVersionMetadata;
}): ReactNode {
  const {
    siteConfig: { title: siteTitle },
  } = useDocusaurusContext();
  const { pluginId } = useActivePlugin({ failfast: true })!;

  const getVersionMainDoc = (version: GlobalVersion) =>
    version.docs.find((doc) => doc.id === version.mainDocId)!;

  const { savePreferredVersionName } = useDocsPreferredVersion(pluginId);

  const { latestDocSuggestion, latestVersionSuggestion } =
    useDocVersionSuggestions(pluginId);

  // Try to link to same doc in latest version (not always possible), falling
  // back to main doc of latest version
  const latestVersionSuggestedDoc =
    latestDocSuggestion ?? getVersionMainDoc(latestVersionSuggestion);

  return (
    <div
      className={clsx(
        className,
        ThemeClassNames.docs.docVersionBanner,
        "card mb-4 gap-4 rounded-2xl border border-black/10 dark:border-white/10 bg-muted/40 p-4 transition-colors"
      )}
      role="alert"
    >
      <div className="flex items-start">
        <div className="flex grow flex-wrap flex-row justify-between items-center">
          <div className="text-sm dark:text-gray-200">
            <BannerLabel
              siteTitle={siteTitle}
              versionMetadata={versionMetadata}
            />
          </div>

          <div className="mt-1 text-sm">
            <LatestVersionSuggestionLabel
              versionLabel={latestVersionSuggestion.label}
              to={latestVersionSuggestedDoc.path}
              onClick={() =>
                savePreferredVersionName(latestVersionSuggestion.name)
              }
            />
          </div>
        </div>
      </div>
    </div>
  );
}

export default function DocVersionBanner({ className }: Props): ReactNode {
  const versionMetadata = useDocsVersion();
  if (versionMetadata.banner) {
    return (
      <DocVersionBannerEnabled
        className={className}
        versionMetadata={versionMetadata}
      />
    );
  }
  return null;
}
