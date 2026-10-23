import { PathType, type PathTypeValue } from "@site/src/typescript/pathUtils";

export const PRODUCT_BRAND_COLORS: Partial<Record<PathTypeValue, string>> = {
    [PathType.Documentation]: "text-[#0c2fa5] dark:text-[#388ee9]",
    [PathType.Cloud]: "text-[#417c5a] dark:text-[#63ffa6]",
    [PathType.Quill]: "text-[#c6432e] dark:text-[#ff775f]",
};
