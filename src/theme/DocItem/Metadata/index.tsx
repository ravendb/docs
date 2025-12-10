import React, {type ReactNode} from 'react';
import Metadata from '@theme-original/DocItem/Metadata';
import type MetadataType from '@theme/DocItem/Metadata';
import type {WrapperProps} from '@docusaurus/types';
import {useDoc} from "@docusaurus/plugin-content-docs/client";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Head from "@docusaurus/Head";

type Props = WrapperProps<typeof MetadataType>;

export default function MetadataWrapper(props: Props): ReactNode {
  const {siteConfig} = useDocusaurusContext();
  const {metadata} = useDoc();
  const docSlug = metadata.slug;
  const canonicalUrl = `${siteConfig.url}${docSlug}`;

  return (
    <>
      <Metadata {...props} />
      <Head>
        <link rel="canonical" href={canonicalUrl} />
      </Head>
    </>
  );
}
