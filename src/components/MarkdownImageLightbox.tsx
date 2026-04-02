import React, { useEffect, useState } from "react";
import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";
import Captions from "yet-another-react-lightbox/plugins/captions";
import { useLocation } from "@docusaurus/router";
import { Share } from "yet-another-react-lightbox/plugins";

type Slide = { src: string; description?: string };
type CandidateElement = HTMLDivElement | HTMLImageElement;
type GalleryItem = { element: CandidateElement; slide: Slide };

function decodeHTML(html: string) {
    const txt = document.createElement("textarea");
    txt.innerHTML = html;
    return txt.value;
}

function isIdealImageHost(element: unknown): element is HTMLDivElement {
    return element instanceof HTMLDivElement && element.classList.contains("ideal-image-lightbox-host");
}

function isImageElement(element: unknown): element is HTMLImageElement {
    return element instanceof HTMLImageElement;
}

function getLightboxCandidates(): CandidateElement[] {
    const candidatesElements = Array.from(
        document.querySelectorAll(".theme-doc-markdown img, .theme-doc-markdown .ideal-image-lightbox-host")
    );

    return candidatesElements.filter((element) => isImageElement(element) || isIdealImageHost(element));
}

function getLightboxSrc(element: CandidateElement): string | null {
    if (isImageElement(element)) {
        return element.currentSrc || element.getAttribute("src");
    }

    return element.getAttribute("data-lightbox-src");
}

function getLightboxDescription(element: CandidateElement): string {
    if (isImageElement(element)) {
        return decodeHTML(element.getAttribute("alt") || "");
    }

    return element.getAttribute("data-lightbox-description") || "";
}

function buildGallery(candidates: CandidateElement[]): GalleryItem[] {
    return candidates.reduce<GalleryItem[]>((gallery, candidate) => {
        const src = getLightboxSrc(candidate);
        if (!src) {
            return gallery;
        }

        gallery.push({
            element: candidate,
            slide: {
                src,
                description: getLightboxDescription(candidate),
            },
        });

        return gallery;
    }, []);
}

export default function MarkdownImageLightbox() {
    const [open, setOpen] = useState(false);
    const [slides, setSlides] = useState<Slide[]>([]);
    const [currentIndex, setCurrentIndex] = useState(0);
    const location = useLocation();

    useEffect(() => {
        const boundElements = new Map<CandidateElement, EventListener>();

        const openGallery = (target: CandidateElement) => {
            const gallery = buildGallery(getLightboxCandidates());
            const index = gallery.findIndex(({ element }) => element === target);

            if (index === -1) {
                return;
            }

            setSlides(gallery.map(({ slide }) => slide));
            setCurrentIndex(index);
            setOpen(true);
        };

        const bindCandidates = () => {
            getLightboxCandidates().forEach((candidate) => {
                if (boundElements.has(candidate)) {
                    return;
                }

                candidate.style.cursor = "zoom-in";

                const clickHandler: EventListener = (event) => {
                    event.preventDefault();
                    event.stopPropagation();
                    openGallery(candidate);
                };

                candidate.addEventListener("click", clickHandler);
                boundElements.set(candidate, clickHandler);
            });
        };

        bindCandidates();

        const observer = new window.MutationObserver(() => {
            bindCandidates();
        });

        document.querySelectorAll(".theme-doc-markdown").forEach((root) => {
            observer.observe(root, { childList: true, subtree: true });
        });

        return () => {
            observer.disconnect();

            boundElements.forEach((clickHandler, element) => {
                element.removeEventListener("click", clickHandler);
            });
        };
    }, [location.pathname]);

    return (
        <Lightbox
            open={open}
            close={() => setOpen(false)}
            index={currentIndex}
            slides={slides}
            plugins={[Share, Captions]}
            captions={{
                descriptionTextAlign: "center",
                descriptionMaxLines: 2,
                showToggle: false,
            }}
        />
    );
}
