import { ReactNode } from "react";
import { useActiveDocContext } from "@docusaurus/plugin-content-docs/client";

interface VersionConditionalProps {
  minimumVersion: string;
  children: ReactNode;
}

export default function VersionConditional({
  minimumVersion,
  children,
}: VersionConditionalProps) {
  const pluginId = "default";
  const { activeVersion } = useActiveDocContext(pluginId);

  if (minimumVersion > activeVersion.label) {
    return null;
  }

  return <>{children}</>;
}