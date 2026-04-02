export interface Tag {
    key: string;
    label: string;
    category?: string;
    count?: number;
}

export interface Sample {
    id: string;
    title: string;
    description?: string;
    permalink: string;
    image?: string;
    img_alt?: string;
    tags: Array<{ label: string; key: string; category?: string }>;
}

export interface PluginData {
    samples: Sample[];
    tags: Tag[];
}
