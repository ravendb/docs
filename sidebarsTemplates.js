// This file has to be in JavaScript, otherwise @docusaurus/plugin-content-docs doesn't work properly
export default {
    templates: [
        {
            type: "category",
            label: "Templates",
            link: {
                type: "doc",
                id: "home",
            },
            collapsible: false,
            items: [
                {
                    type: "doc",
                    id: "introduction",
                    label: "Introduction",
                },
                {
                    type: "category",
                    label: "Guides authoring",
                    items: [
                        {
                            type: "doc",
                            id: "new-guides",
                            label: "Adding new guides",
                        },
                        {
                            type: "doc",
                            id: "featured-guides",
                            label: "Featured guides",
                        },
                        {
                            type: "doc",
                            id: "tags",
                            label: "Tags",
                        },
                    ],
                },
                {
                    type: "doc",
                    id: "frames",
                    label: "Frames",
                },
                {
                    type: "doc",
                    id: "icon-gallery",
                    label: "Icon gallery",
                },
                {
                    type: "doc",
                    id: "themed-images",
                    label: "Themed images",
                },
                {
                    type: "doc",
                    id: "see-also",
                    label: "See also",
                },
            ],
        },
    ],
};
