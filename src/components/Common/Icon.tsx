import React from "react";
import { IconName } from "../../generatedTypes/iconName";
import clsx from "clsx";

export type IconSize = "xs" | "sm" | "md" | "lg" | "xl";
export type IconColor =
  | "current"
  | "red"
  | "blue"
  | "green"
  | "yellow"
  | "purple"
  | "pink"
  | "indigo"
  | "gray"
  | "slate"
  | "zinc"
  | "neutral"
  | "stone"
  | "orange"
  | "amber"
  | "lime"
  | "emerald"
  | "teal"
  | "cyan"
  | "sky"
  | "violet"
  | "fuchsia"
  | "rose"
  | "primary";

export interface IconProps {
  icon: IconName;
  size?: IconSize;
  color?: IconColor;
  className?: string;
}

export function Icon({ icon, color, size = "md", className }: IconProps) {
  const iconBase64 = require(`@site/static/icons/${icon}.svg`).default;

  const colorClass = color ? `fill-${color}` : "fill-current";
  const sizeClass = size === "xs" ? "w-4 h-4" :
                   size === "sm" ? "w-6 h-6" :
                   size === "md" ? "w-8 h-8" :
                   size === "lg" ? "w-10 h-10" :
                   size === "xl" ? "w-12 h-12" :
                   "w-6 h-6";

  return (
    <div
      dangerouslySetInnerHTML={{ __html: getSvg(iconBase64, sizeClass, colorClass) }}
      className={clsx("inline-block", className)}
    />
  );
}

function getSvg(base64String: string, sizeClass: string, colorClass: string): string {
  const svgContent = atob(base64String.split("base64,")[1]);
  const svgWithClasses = svgContent.replace(
    /<svg([^>]*)>/,
    `<svg$1 class="${sizeClass} ${colorClass}">`
  );
  return svgWithClasses;
}
