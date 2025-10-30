import React, {type ReactNode} from 'react';
import clsx from 'clsx';
import {ThemeClassNames, HtmlClassNameProvider} from '@docusaurus/theme-common';
import renderRoutes from '@docusaurus/renderRoutes';
import Layout from '@theme/Layout';

import type {Props} from '@theme/DocVersionRoot';

export default function DocsRoot(props: Props): ReactNode {
  return (
      <HtmlClassNameProvider className={clsx(ThemeClassNames.wrapper.docsPages)}>
        <Layout>{renderRoutes(props.route.routes!)}</Layout>
      </HtmlClassNameProvider>
  );
}
