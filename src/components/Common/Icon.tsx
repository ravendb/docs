import React from "react";
import { IconName } from "../../typescript/iconName";
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
  | "primary"
  | "ifm-menu";

export interface IconProps {
  icon: IconName;
  size?: IconSize;
  color?: IconColor;
  className?: string;
}

export function Icon({
  icon,
  color = "current",
  size = "sm",
  className,
}: IconProps) {
  const iconBase64 = require(`@site/static/icons/${icon}.svg`).default;

  const colorClass = `fill-${color}`;
  const sizeClass = getSizeClass(size);

  return (
    <div
      dangerouslySetInnerHTML={{
        __html: getSvg(iconBase64, sizeClass, colorClass),
      }}
      className={clsx(className)}
    />
  );
}

function getSvg(
  base64String: string,
  sizeClass: string,
  colorClass: string
): string {
  const svgContent = atob(base64String.split("base64,")[1]);
  const svgWithClasses = svgContent.replace(
    /<svg([^>]*)>/,
    `<svg$1 class="${sizeClass} ${colorClass}">`
  );
  return svgWithClasses;
}

function getSizeClass(size?: IconSize): `w-${number} h-${number}` {
  switch (size) {
    case "xs":
      return "w-4 h-4";
    case "sm":
      return "w-6 h-6";
    case "md":
      return "w-8 h-8";
    case "lg":
      return "w-10 h-10";
    case "xl":
      return "w-12 h-12";
    default:
      return "w-6 h-6";
  }
}
