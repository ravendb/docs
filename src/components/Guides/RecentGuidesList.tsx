import React from "react";
import RecentGuidesListItem from "./RecentGuidesListItem";

interface RecentGuidesListProps {
    guides: {
        title: string;
        url: string;
        tags: { label: string; permalink: string }[];
        time: string;
        lastUpdatedAt: number;
    }[];
}

export default function RecentGuidesList({ guides }: RecentGuidesListProps) {
    return (
        <>
            {guides.map((guide, index) => (
                <RecentGuidesListItem key={guide.url} {...guide} isLast={index === guides.length - 1} />
            ))}
        </>
    );
}
