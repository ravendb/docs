import React, { ComponentProps } from "react";
import LazyImage from "@site/src/components/Common/LazyImage";

export default function MDXImg(props: ComponentProps<typeof LazyImage>) {
    return <LazyImage {...props} />;
}
