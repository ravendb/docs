import React from "react";
import clsx from "clsx";
import { SeeAlsoItem } from "./SeeAlsoItem";
import { SeeAlsoItemType } from "./types";

interface Props {
    items: SeeAlsoItemType[];
    className?: string;
}

export default function SeeAlso({ items, className }: Props) {
    if (!items || items.length === 0) {
        return null;
    }

    return (
        <div className={clsx("flex flex-col gap-4 items-start pt-8 w-full", className)}>
            <h3 className="text-xl font-bold !mb-0 text-[var(--ifm-font-color-base)]">See also</h3>
            <div className="flex flex-col w-full">
                {items.map((item, idx) => (
                    <SeeAlsoItem key={idx} item={item} />
                ))}
            </div>
        </div>
    );
}
