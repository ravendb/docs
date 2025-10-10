import React, {type ReactNode} from 'react';
import clsx from 'clsx';
import {ThemeClassNames, HtmlClassNameProvider} from '@docusaurus/theme-common';
import renderRoutes from '@docusaurus/renderRoutes';
import Layout from '@theme/Layout';

import type {Props} from '@theme/DocVersionRoot';
import { LanguageProvider } from '@site/src/components/LanguageContext';

export default function DocsRoot(props: Props): ReactNode {
  return (
      <LanguageProvider>
        <HtmlClassNameProvider className={clsx(ThemeClassNames.wrapper.docsPages)}>
          <Layout>{renderRoutes(props.route.routes!)}</Layout>
        </HtmlClassNameProvider>
      </LanguageProvider>
  );
}
