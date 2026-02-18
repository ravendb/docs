import React, { ReactNode } from "react";
import { useLanguage } from "./LanguageStore";

interface LanguageContentProps {
    language: string;
    children: ReactNode;
}

export default function LanguageContent({ language, children }: LanguageContentProps) {
    const { language: current } = useLanguage();
    return current === language ? <>{children}</> : null;
}
