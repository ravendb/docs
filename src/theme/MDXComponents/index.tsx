import MDXComponents from "@theme-original/MDXComponents";
import Video from "@site/src/components/Common/Video";
import MDXImg from "./MDXImg";

export default {
    ...MDXComponents,
    img: MDXImg,
    Video,
} satisfies typeof MDXComponents;
