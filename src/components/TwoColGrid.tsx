import * as React from "react";
import clsx from "clsx";

export interface TwoColGridProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
  className?: string;
  gap?: "sm" | "md" | "lg";
  equalHeight?: boolean;
}

const gapClasses: Record<NonNullable<TwoColGridProps["gap"]>, string> = {
  sm: "gap-4",
  md: "gap-6",
  lg: "gap-8",
};

export default function TwoColGrid({
  children,
  className = "",
  gap = "sm",
  equalHeight = false,
  ...props
}: TwoColGridProps) {
  return (
    <div
      className={clsx(
        "grid grid-cols-1 sm:grid-cols-2",
        gapClasses[gap],
        className,
      )}
      {...props}
    >
      {children}
    </div>
  );
}


