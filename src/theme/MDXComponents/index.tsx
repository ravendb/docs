import MDXComponents from "@theme-original/MDXComponents";
import MDXImg from "./MDXImg";

export default {
    ...MDXComponents,
    img: MDXImg,
} satisfies typeof MDXComponents;
