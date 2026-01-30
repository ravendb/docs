import React, { useMemo } from "react";
import IconGalleryCard from "./IconGalleryCard";
import { ALL_ICON_NAMES } from "@site/src/typescript/iconName";

export default function IconGallery() {
  const iconNames = useMemo(() => {
    return [...ALL_ICON_NAMES].sort();
  }, []);

  return (
    <div className="icon-gallery mb-4">
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
        {iconNames.map((iconName) => (
          <IconGalleryCard key={iconName} iconName={iconName} />
        ))}
      </div>
    </div>
  );
}
