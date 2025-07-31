import React, { useEffect, useState } from "react";
import { useColorMode } from "@docusaurus/theme-common";

export default function NavbarColorModeToggle({
  className,
}: {
  className?: string;
}) {
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
        className="absolute top-0 left-0 lg:left-[0.5px] w-full h-[200%] flex flex-col items-center justify-center transition-transform duration-300"
        style={{ transform: `translateY(${isDark ? "-50%" : "0%"})` }}
      >
        <div className="flex items-center justify-center h-8">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            className="h-5 w-5 text-ifm-menu"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            strokeWidth={1}
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M12 3v1m0 16v1m8.66-8.66h-1M4.34 12H3m15.07-5.07l-.71.71M6.34 17.66l-.71.71m12.02 0l-.71-.71M6.34 6.34l-.71-.71M12 8a4 4 0 100 8 4 4 0 000-8z"
            />
          </svg>
        </div>

        <div className="flex items-center justify-center h-8">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            className="h-5 w-5 text-ifm-menu"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            strokeWidth={1}
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M21 12.79A9 9 0 1111.21 3a7 7 0 109.79 9.79z"
            />
          </svg>
        </div>
      </div>
    </button>
  );
}
