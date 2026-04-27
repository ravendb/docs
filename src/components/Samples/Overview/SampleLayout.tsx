import React, { ReactNode } from "react";
import SampleMetadataColumn from "./Partials/SampleMetadataColumn";
import Gallery from "@site/src/components/Common/Gallery";
import { useDoc } from "@docusaurus/plugin-content-docs/client";

interface SampleLayoutProps {
    children: ReactNode;
}

export default function SampleLayout({ children }: SampleLayoutProps) {
    const { frontMatter } = useDoc();
    const gallery = frontMatter.gallery?.length ? <Gallery images={frontMatter.gallery} /> : null;

    return (
        <div className="flex flex-col-reverse lg:flex-row flex-wrap gap-8">
            <div className="flex-1 min-w-0">
                <div className="hidden lg:block">{gallery}</div>
                {children}
            </div>
            <div className="lg:w-[300px] shrink-0">
                <div className="lg:hidden">{gallery}</div>
                <SampleMetadataColumn />
            </div>
        </div>
    );
}
