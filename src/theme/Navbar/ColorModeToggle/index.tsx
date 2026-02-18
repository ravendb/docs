import React, { useEffect, useState } from "react";
import { useColorMode } from "@docusaurus/theme-common";
import { Icon } from "@site/src/components/Common/Icon";

export default function NavbarColorModeToggle({ className }: { className?: string }) {
    const { colorMode, setColorMode } = useColorMode();
    const [hasMounted, setHasMounted] = useState(false);

    useEffect(() => {
        setHasMounted(true);
    }, []);

    const isDark = colorMode === "dark";

    if (!hasMounted) {
        return (
            <button
                className={`w-8 h-8 rounded-full border border-black/10 dark:border-white/10 ${className}`}
                aria-label="Toggle mode"
            />
        );
    }

    return (
        <button
            onClick={() => setColorMode(isDark ? "light" : "dark")}
            className={`relative w-8 h-8 max-w-[31.48px] max-h-[31.48px] rounded-full overflow-hidden border border-black/10 dark:border-white/10 flex items-center justify-center bg-ifm-background transition-colors duration-300 ms-3 lg:ms-0 cursor-pointer hover:bg-black/5 dark:hover:bg-white/5 ${className}`}
            aria-label="Toggle mode"
        >
            <div
                className="absolute top-0 left-0 w-full h-[200%] flex flex-col items-center justify-center transition-transform duration-300"
                style={{ transform: `translateY(${isDark ? "-50%" : "0%"})` }}
            >
                <div className="flex items-center justify-center h-8 text-ifm-menu">
                    <Icon icon="moon" size="xs" />
                </div>

                <div className="flex items-center justify-center h-8 text-ifm-menu">
                    <Icon icon="sun" size="xs" />
                </div>
            </div>
        </button>
    );
}
