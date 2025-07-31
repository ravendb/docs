import Heading from "@theme/Heading";
import AiFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/AiFeatures";
import IndexesFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/IndexesFeatures";
import QueryingFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/QueryingFeatures";
import DocumentExtensionsFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/DocumentExtensionsFeatures";
import BackgroundTasksFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/BackgroundTasksFeatures";
import ClustersFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/ClustersFeatures";
import OngoingTasksFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/OngoingTasksFeatures";
import SecurityFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/SecurityFeatures";
import MonitoringFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/MonitoringFeatures";
import AdministrationFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/AdministrationFeatures";
import IntegrationFeaturesGrid from "@site/src/components/Homepage/Features/FeaturesLists/IntegrationFeatures";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

export default function CloudFeatures() {
  return (
    <section className="mb-8">
      <Heading as="h3">Browse by features</Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        <FeatureItem
          title="Tiers and instances"
          description="Short description"
          url="/cloud/cloud-instances"
        />
        <FeatureItem
          title="Pricing, payment and billing"
          description="Short description"
          url="/cloud/cloud-pricing-payment-billing"
        />
        <FeatureItem
          title="Backup and restore"
          description="Short description"
          url="/cloud/cloud-backup-and-restore"
        />
        <FeatureItem
          title="Migration"
          description="Short description"
          url="/cloud/cloud-migration"
        />
        <FeatureItem
          title="Scaling"
          description="Short description"
          url="/cloud/cloud-scaling"
        />
        <FeatureItem
          title="Security"
          description="Short description"
          url="/cloud/cloud-security"
        />
        <FeatureItem
          title="API"
          description="Short description"
          url="/cloud/cloud-api"
        />
        <FeatureItem
          title="Product features"
          description="Short description"
          url="/cloud/cloud-features"
        />
        <FeatureItem
          title="Settings"
          description="Short description"
          url="/cloud/cloud-settings"
        />
        <FeatureItem
          title="Support"
          description="Short description"
          url="/cloud/cloud-support"
        />
        <FeatureItem
          title="Maintenance & troubleshooting"
          description="Short description"
          url="/cloud/cloud-maintenance-troubleshooting"
        />
        <FeatureItem
          title="Microsoft Azure Marketplace"
          description="Short description"
          url="/cloud/cloud-microsoft-azure-marketplace"
        />
        <FeatureItem
          title="AWS Marketplace"
          description="Short description"
          url="/cloud/cloud-aws-marketplace"
        />
      </div>
    </section>
  );
}
