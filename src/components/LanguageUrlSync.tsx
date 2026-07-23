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

        // Read the real sources (URL then storage), not the store's hydration-unstable `language`.
        const urlLanguage = getLanguageFromSearch(location.search);
        const activeLanguage = urlLanguage ?? getStoredLanguage();

        // Page doesn't offer the active language: leave the preference and URL alone (display clamps it).
        if (!supportedLanguages.includes(activeLanguage)) {
            return;
        }

        // Persist a ?lang arrival and keep it in the URL; guarded so it doesn't loop.
        const expectedUrlLanguage = activeLanguage === DEFAULT_LANGUAGE ? null : activeLanguage;
        if (urlLanguage !== expectedUrlLanguage || getStoredLanguage() !== activeLanguage) {
            setLanguage(activeLanguage);
        }
    }, [location.pathname, location.search, supportedLanguages, setLanguage]);

    return null;
}
