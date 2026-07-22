import { useEffect } from "react";
import { useLocation } from "@docusaurus/router";
import {
    type DocsLanguage,
    useSetLanguage,
    getStoredLanguage,
    getLanguageFromSearch,
    DEFAULT_LANGUAGE,
} from "./LanguageStore";

type LanguageUrlSyncProps = {
    supportedLanguages?: DocsLanguage[];
};

// Renders nothing. Keeps ?lang= in sync with the active language and persists an incoming ?lang=.
export default function LanguageUrlSync({ supportedLanguages }: LanguageUrlSyncProps): null {
    const setLanguage = useSetLanguage();
    const location = useLocation();

    useEffect(() => {
        if (!supportedLanguages || supportedLanguages.length === 0) {
            return;
        }

        // Resolve from the real sources (URL then storage), not the hydration-unstable store value.
        const urlLanguage = getLanguageFromSearch(location.search);
        const active = urlLanguage ?? getStoredLanguage();
        const desired = supportedLanguages.includes(active) ? active : supportedLanguages[0];
        const expectedUrlLanguage = desired === DEFAULT_LANGUAGE ? null : desired;

        if (urlLanguage !== expectedUrlLanguage || getStoredLanguage() !== desired) {
            setLanguage(desired);
        }
    }, [location.pathname, location.search, supportedLanguages, setLanguage]);

    return null;
}
