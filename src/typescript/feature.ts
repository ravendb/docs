import { ReactNode } from "react";
import { IconName } from "./iconName";

export interface Feature {
  title: string;
  description?: ReactNode;
  icon: IconName;
  url: string;
} 