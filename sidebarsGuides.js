// This file has to be in JavaScript, otherwise @docusaurus/plugin-content-docs doesn't work properly
export default {
    guides: [
        {
            type: 'category',
            label: 'Guides',
            link: {
                type: 'doc',
                id: 'home',
            },
            items: [
                {
                    type: 'doc',
                    id: 'example-guide',
                    label: 'Example Guide'
                },
                {
                    type: 'doc',
                    id: 'other-guide',
                    label: 'Other Guide'
                },
            ],
        },
    ],
};
