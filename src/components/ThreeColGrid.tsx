import * as React from "react";
import clsx from "clsx";

export interface ThreeColGridProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
  className?: string;
  gap?: "sm" | "md" | "lg";
  equalHeight?: boolean;
}

const gapClasses: Record<NonNullable<ThreeColGridProps["gap"]>, string> = {
  sm: "gap-4",
  md: "gap-6",
  lg: "gap-8",
};

export default function ThreeColGrid({
  children,
  className = "",
  gap = "sm",
  equalHeight = false,
  ...props
}: ThreeColGridProps) {
  return (
    <div
      className={clsx(
        "grid [grid-template-columns:repeat(auto-fit,minmax(15rem,1fr))]",
        gapClasses[gap],
        className,
      )}
      {...props}
    >
      {children}
    </div>
  );
}
