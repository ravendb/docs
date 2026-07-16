import { useEffect } from "react";
import { useLocation } from "@docusaurus/router";
import {
    type DocsLanguage,
    useLanguage,
    getStoredLanguage,
    getLanguageFromSearch,
    DEFAULT_LANGUAGE,
} from "./LanguageStore";

type LanguageUrlSyncProps = {
    supportedLanguages?: DocsLanguage[];
};

// Renders nothing. Keeps "?lang=" in sync with the active language (re-stamped after navigation)
// and persists an incoming "?lang=" so the choice carries across navigation and future sessions.
export default function LanguageUrlSync({ supportedLanguages }: LanguageUrlSyncProps): null {
    const { setLanguage } = useLanguage();
    const location = useLocation();

    useEffect(() => {
        if (!supportedLanguages || supportedLanguages.length === 0) {
            return;
        }

        // Resolve from the real sources (URL then storage), not the store's hydration-unstable `language`,
        // which briefly reports the default and would clobber a stored non-default preference.
        const urlLanguage = getLanguageFromSearch(location.search);
        const active = urlLanguage ?? getStoredLanguage();
        const desired = supportedLanguages.includes(active) ? active : supportedLanguages[0];
        const expectedUrlLanguage = desired === DEFAULT_LANGUAGE ? null : desired;

        // Guards make this a no-op once URL and storage agree, so there is no render loop.
        if (urlLanguage !== expectedUrlLanguage || getStoredLanguage() !== desired) {
            setLanguage(desired);
        }
    }, [location.pathname, location.search, supportedLanguages, setLanguage]);

    return null;
}
