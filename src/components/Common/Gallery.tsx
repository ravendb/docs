import React, { useRef, useState } from "react";
import clsx from "clsx";
import LazyImage from "./LazyImage";

export interface GalleryImage {
    src: string;
    alt?: string;
}

export interface GalleryProps {
    images: GalleryImage[];
    className?: string;
}

export default function Gallery({ images, className }: GalleryProps) {
    const thirdImageRef = useRef<HTMLDivElement>(null);
    const [currentSlide, setCurrentSlide] = useState(0);
    const carouselRef = useRef<HTMLDivElement>(null);

    if (!images || images.length === 0) {
        return null;
    }

    const visibleImages = images.slice(0, 3);
    const remainingCount = Math.max(0, images.length - 3);

    const handleOverlayClick = () => {
        const imgElement = thirdImageRef.current?.querySelector("img");
        if (imgElement) {
            imgElement.click();
        }
    };

    const handleScroll = () => {
        if (carouselRef.current) {
            const scrollLeft = carouselRef.current.scrollLeft;
            const slideWidth = carouselRef.current.offsetWidth;
            const newSlide = Math.round(scrollLeft / slideWidth);
            setCurrentSlide(newSlide);
        }
    };

    return (
        <>
            <div className={clsx("lg:hidden mb-6", className)}>
                <div
                    ref={carouselRef}
                    onScroll={handleScroll}
                    className={clsx("flex gap-3 overflow-x-auto snap-x snap-mandatory", "scrollbar-none", "-mx-4 px-4")}
                    style={{ scrollbarWidth: "none", msOverflowStyle: "none" }}
                >
                    {images.map((image, index) => (
                        <div
                            key={index}
                            className={clsx("group w-full snap-center shrink-0", "flex items-center justify-center")}
                        >
                            <LazyImage
                                imgSrc={image.src}
                                alt={image.alt || ""}
                                className={clsx(
                                    "w-full h-auto rounded-xl",
                                    "!transition-transform",
                                    "active:scale-[0.98]"
                                )}
                            />
                        </div>
                    ))}
                </div>
                {images.length > 1 && (
                    <div className="flex justify-center gap-1.5 mt-3">
                        {images.map((_, index) => (
                            <button
                                key={index}
                                onClick={() => {
                                    if (carouselRef.current) {
                                        carouselRef.current.scrollTo({
                                            left: index * carouselRef.current.offsetWidth,
                                            behavior: "smooth",
                                        });
                                    }
                                }}
                                className={clsx(
                                    "h-1.5 rounded-full transition-all",
                                    currentSlide === index
                                        ? "w-6 bg-primary"
                                        : "w-1.5 bg-black/20 dark:bg-white/20 hover:bg-black/30 dark:hover:bg-white/30"
                                )}
                                aria-label={`Go to slide ${index + 1}`}
                            />
                        ))}
                    </div>
                )}
            </div>

            <div className={clsx("hidden lg:flex gap-3 w-full mb-6", className)}>
                {visibleImages[0] && (
                    <div
                        className={clsx(
                            "group min-w-0 rounded-xl overflow-hidden",
                            visibleImages.length === 1 && "flex-1",
                            visibleImages.length === 2 && "flex-1",
                            visibleImages.length >= 3 && "flex-[592]"
                        )}
                    >
                        <LazyImage
                            imgSrc={visibleImages[0].src}
                            alt={visibleImages[0].alt || ""}
                            className={clsx(
                                "w-full h-full object-cover aspect-[592/331]",
                                "!transition-transform origin-bottom",
                                "group-hover:scale-105 group-hover:translate-y-1"
                            )}
                        />
                    </div>
                )}
                {visibleImages.length === 2 && visibleImages[1] && (
                    <div className="group flex-1 min-w-0 rounded-xl overflow-hidden">
                        <LazyImage
                            imgSrc={visibleImages[1].src}
                            alt={visibleImages[1].alt || ""}
                            className={clsx(
                                "w-full h-full object-cover aspect-[592/331]",
                                "!transition-transform origin-bottom",
                                "group-hover:scale-105 group-hover:translate-y-1"
                            )}
                        />
                    </div>
                )}
                {visibleImages.length >= 3 && (
                    <div className="flex-[284] min-w-0 flex flex-col gap-3">
                        {visibleImages[1] && (
                            <div className="group rounded-xl overflow-hidden">
                                <LazyImage
                                    imgSrc={visibleImages[1].src}
                                    alt={visibleImages[1].alt || ""}
                                    className={clsx(
                                        "w-full h-full object-cover aspect-[284/160]",
                                        "!transition-transform origin-bottom",
                                        "group-hover:scale-105 group-hover:translate-y-1"
                                    )}
                                />
                            </div>
                        )}

                        {visibleImages[2] && (
                            <div ref={thirdImageRef} className="group relative rounded-xl overflow-hidden">
                                <LazyImage
                                    imgSrc={visibleImages[2].src}
                                    alt={visibleImages[2].alt || ""}
                                    className={clsx(
                                        "w-full h-full object-cover aspect-[284/160]",
                                        "!transition-transform origin-bottom",
                                        "group-hover:scale-105 group-hover:translate-y-1"
                                    )}
                                />
                                {remainingCount > 0 && (
                                    <div
                                        className={clsx(
                                            "absolute inset-0",
                                            "bg-black/20 dark:bg-black/60",
                                            "flex items-center justify-center",
                                            "text-white font-bold text-2xl",
                                            "hover:bg-black/30 dark:hover:bg-black/70",
                                            "transition-colors",
                                            "cursor-pointer"
                                        )}
                                        onClick={handleOverlayClick}
                                    >
                                        +{remainingCount}
                                    </div>
                                )}
                            </div>
                        )}
                    </div>
                )}
            </div>

            {images.length > 3 && (
                <div className="hidden">
                    {images.slice(3).map((img, index) => (
                        <LazyImage key={index} imgSrc={img.src} alt={img.alt || ""} />
                    ))}
                </div>
            )}
        </>
    );
}
