import React, { useEffect, useState } from "react";
import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";
import Captions from "yet-another-react-lightbox/plugins/captions";
import { useLocation } from "@docusaurus/router";
import { Share } from "yet-another-react-lightbox/plugins";

function decodeHTML(html: string) {
  const txt = document.createElement("textarea");
  txt.innerHTML = html;
  return txt.value;
}

export default function MarkdownImageLightbox() {
  const [open, setOpen] = useState(false);
  const [slides, setSlides] = useState<{ src: string; description?: string }[]>(
    [],
  );
  const [currentIndex, setCurrentIndex] = useState(0);
  const location = useLocation();

  useEffect(() => {
    const images = Array.from(document.querySelectorAll("img"));

    const imageList: { src: string; description?: string }[] = [];

    images.forEach((img, index) => {
      const src = img.getAttribute("src");
      if (!src) {return;}

      const altRaw = img.getAttribute("alt") || "";
      const alt = decodeHTML(altRaw);

      imageList.push({ src, description: alt });

      if (img.classList.contains("lightbox-bound")) {return;}

      img.style.cursor = "zoom-in";
      img.classList.add("lightbox-bound");

      img.addEventListener("click", (e) => {
        e.preventDefault();
        setCurrentIndex(index);
        setOpen(true);
      });
    });

    setSlides(imageList);
  }, [location.pathname]);

  return (
    <Lightbox
      open={open}
      close={() => setOpen(false)}
      index={currentIndex}
      slides={slides}
      plugins={[Share, Captions]}
      captions={{
        descriptionTextAlign: "center", // ✅ center the caption
        descriptionMaxLines: 2, // ✅ limit lines if needed
        showToggle: false, // ✅ hides toggle icon
      }}
    />
  );
}
