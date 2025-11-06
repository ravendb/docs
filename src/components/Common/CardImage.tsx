import React from "react";
import ThemedImage from "@theme/ThemedImage";

export interface CardImageProps {
  imgSrc: string | { light: string; dark: string };
  imgAlt?: string;
  className?: string;
}

export default function CardImage({
  imgSrc,
  imgAlt = "",
  className = "",
}: CardImageProps) {
  const isThemedImage = typeof imgSrc === "object" && imgSrc !== null;

  if (isThemedImage) {
    return (
      <ThemedImage
        alt={imgAlt}
        sources={{
          light: imgSrc.light,
          dark: imgSrc.dark,
        }}
        className={className}
      />
    );
  }

  return (
    <img
      src={imgSrc as string}
      alt={imgAlt}
      className={className}
    />
  );
}

