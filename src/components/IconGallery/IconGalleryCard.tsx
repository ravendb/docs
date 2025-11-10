import React, { useState } from "react";
import { Icon } from "../Common/Icon";
import { IconName } from "@site/src/typescript/iconName";

export interface IconCardProps {
  iconName: IconName;
  onCopy?: (iconName: string) => void;
}

export default function IconCard({ iconName, onCopy }: IconCardProps) {
  const [copied, setCopied] = useState(false);

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(iconName);
      setCopied(true);
      onCopy?.(iconName);
      window.setTimeout(() => setCopied(false), 2000);
    } catch (_err) {
      // Ignore copy errors
    }
  };

  return (
    <div
      className="group flex flex-col items-center justify-center p-4 rounded-lg border border-black/10 dark:border-white/10 bg-muted/40 hover:bg-muted/60 transition-all cursor-pointer"
      onClick={handleCopy}
      title={`Click to copy: ${iconName}`}
    >
      <div className="mb-3 flex items-center justify-center w-16 h-16">
        <Icon icon={iconName} size="lg" />
      </div>
      <div className="text-center">
        <div className="text-xs font-mono text-gray-700 dark:text-gray-300 break-all">
          {iconName}
        </div>
        <div className="text-xs text-gray-500 dark:text-gray-400 mt-1">
          {copied ? "Copied!" : "Click to copy"}
        </div>
      </div>
    </div>
  );
}

