import React, { ReactNode } from "react";

interface SampleLayoutProps {
    children: ReactNode;
    details: ReactNode;
    gallery?: ReactNode;
}

export default function SampleLayout({ children, details, gallery }: SampleLayoutProps) {
    return (
        <div className="flex flex-col-reverse lg:flex-row flex-wrap gap-8">
            <div className="flex-1 min-w-0">
                <div className="hidden lg:block">{gallery}</div>
                {children}
            </div>
            <div className="lg:w-[300px] shrink-0">
                <div className="lg:hidden">{gallery}</div>
                {details}
            </div>
        </div>
    );
}
