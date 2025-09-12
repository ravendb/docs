import * as React from "react";
import clsx from "clsx";

export interface OneColGridProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
  className?: string;
  gap?: "sm" | "md" | "lg";
  equalHeight?: boolean;
}

const gapClasses: Record<NonNullable<OneColGridProps["gap"]>, string> = {
  sm: "gap-4",
  md: "gap-6",
  lg: "gap-8",
};

export default function OneColGrid({
  children,
  className = "",
  gap = "sm",
  equalHeight = false,
  ...props
}: OneColGridProps) {
  return (
    <div
      className={clsx(
        "grid grid-cols-1",
        gapClasses[gap],
        className,
      )}
      {...props}
    >
      {children}
    </div>
  );
}


