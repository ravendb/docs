import React from "react";
import Layout from "@theme/Layout";

export default function NoBreadcrumbsWrapper({ children, ...props }) {
  return <Layout {...props}>{children}</Layout>;
}
