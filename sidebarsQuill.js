// This file has to be in JavaScript, otherwise @docusaurus/plugin-content-docs doesn't work properly
export default {
    quill: [
        {
            type: "category",
            label: "Quill",
            link: {
                type: "doc",
                id: "home",
            },
            collapsible: false,
            items: [
                {
                    type: "doc",
                    id: "home",
                    label: "Home",
                    className: "hidden", // <-- This makes the default item invisible, while still rendering the sidebar
                },
                {
                    type: "doc",
                    id: "quill-overview",
                    label: "Overview",
                },
            ],
        },
    ],
};
