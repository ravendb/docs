const js = require("@eslint/js");
const tseslint = require("@typescript-eslint/eslint-plugin");
const tsparser = require("@typescript-eslint/parser");
const reactPlugin = require("eslint-plugin-react");
const reactHooksPlugin = require("eslint-plugin-react-hooks");
const globals = require("globals");

module.exports = [
    js.configs.recommended,
    {
        files: ["**/*.{ts,tsx,js,jsx}"],
        languageOptions: {
            parser: tsparser,
            parserOptions: {
                ecmaFeatures: {
                    jsx: true,
                },
                ecmaVersion: "latest",
                sourceType: "module",
            },
            globals: {
                ...globals.browser,
                ...globals.node,
                ...globals.es2021,
            },
        },
        plugins: {
            "@typescript-eslint": tseslint,
            react: reactPlugin,
            "react-hooks": reactHooksPlugin,
        },
        rules: {
            "@typescript-eslint/no-unused-vars": [
                "error",
                {
                    argsIgnorePattern: "^_",
                    varsIgnorePattern: "^_",
                    caughtErrorsIgnorePattern: "^_",
                },
            ],
            "no-unused-vars": "off",

            "no-warning-comments": [
                "warn",
                {
                    terms: ["fixme", "xxx", "hack"],
                    location: "anywhere",
                },
            ],
            "spaced-comment": [
                "error",
                "always",
                {
                    line: {
                        markers: ["/"],
                        exceptions: ["-", "+"],
                    },
                    block: {
                        markers: ["!"],
                        exceptions: ["*"],
                        balanced: true,
                    },
                },
            ],

            "react/prop-types": "off",
            "react/jsx-no-target-blank": "off",
            "react/react-in-jsx-scope": "off",
            "react-hooks/exhaustive-deps": "off",
            "react/jsx-curly-brace-presence": ["warn", { props: "never", children: "never" }],

            "@typescript-eslint/no-var-requires": "off",
            "@typescript-eslint/triple-slash-reference": "off",
            "@typescript-eslint/no-explicit-any": "off",
            "@typescript-eslint/prefer-namespace-keyword": "off",

            "no-console": ["warn", { allow: ["warn", "error"] }],
            "no-debugger": "error",
            "no-alert": "warn",
            "no-var": "error",
            "prefer-const": "error",
            "no-duplicate-imports": "error",
            "no-multiple-empty-lines": ["error", { max: 2, maxEOF: 1 }],
            "eol-last": "error",
            "no-trailing-spaces": "error",
            curly: "warn",
        },
        settings: {
            react: {
                pragma: "React",
                fragment: "Fragment",
                version: "detect",
            },
        },
    },
    // `no-undef` is a JS rule and produces false positives for TS type-only globals  like EventListener.
    // In TS files, unresolved symbols are handled by TypeScript.
    {
        files: ["**/*.ts", "**/*.tsx"],
        rules: {
            "no-undef": "off",
        },
    },
    {
        files: ["**/*.ts", "**/*.tsx"],
        rules: {
            "no-undef": "off",
        },
    },
    {
        ignores: [
            "node_modules/",
            "build/",
            "dist/",
            "static/",
            "versioned_docs/",
            "docs/",
            "*.config.js",
            "*.config.ts",
        ],
    },
];
