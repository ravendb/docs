import * as React from "react";
import clsx from "clsx";

export type GridColCount = 1 | 2 | 3;

export interface ColGridProps extends React.HTMLAttributes<HTMLDivElement> {
    children: React.ReactNode;
    className?: string;
    colCount: GridColCount;
}

function getGridColsClass(colCount: GridColCount): string {
    if (colCount === 1) {
        return "grid grid-cols-1";
    }
    if (colCount === 2) {
        return "grid grid-cols-1 sm:grid-cols-2";
    }
    if (colCount === 3) {
        return "grid grid-cols-1 xl:grid-cols-3";
    }
    return "";
}

export default function ColGrid({ children, className = "", colCount = 1, ...props }: ColGridProps) {
    return (
        <div className={clsx(getGridColsClass(colCount), "gap-4", className)} {...props}>
            {children}
        </div>
    );
}
