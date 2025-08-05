import Heading from "@theme/Heading";
import FeatureItem from "@site/src/components/Homepage/Features/FeatureItem";

export default function CloudFeatures() {
  return (
    <section className="mb-8">
      <Heading as="h3">Browse by features</Heading>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
        <FeatureItem
          title="Tiers and Instances"
          icon="dbgroup"
          url="/cloud/cloud-instances"
          description="Pick Free, Development, or Production-graded products, with premium or standard storage options"
        />
        <FeatureItem
          title="Pricing, Payment and Billing"
          icon="price-tag"
          url="/cloud/cloud-pricing-payment-billing"
          description="Learn more about on-demand & yearly pricing, billing, and payment configuration"
        />
        <FeatureItem
          title="Backup And Restore"
          icon="backup-history"
          url="/cloud/cloud-backup-and-restore"
          description="Keep your data safe, configure one-click restores"
        />
        <FeatureItem
          title="Migration"
          icon="import"
          url="/cloud/cloud-migration"
          description="Import live servers or export files into Cloud with a certificate swap. "
        />
        <FeatureItem
          title="Scaling"
          icon="autoscaling"
          url="/cloud/cloud-scaling"
          description="Auto or manual scale-up/down without service interruption - add nodes, swap disks & more"
        />
        <FeatureItem
          title="Security"
          icon="lock"
          url="/cloud/cloud-security"
          description="TLS 1.2+, X.509 mutual auth, full at-rest encryption."
        />
        <FeatureItem
          title="API"
          icon="api-keys"
          url="/cloud/cloud-api"
          description="OpenAPI/Swagger endpoints plus SDKs for complete automation."
        />
        <FeatureItem
          title="Product Features"
          icon="features"
          url="/cloud/cloud-features"
          description="Enable RavenDB capabilities on demand"
        />
        <FeatureItem
          title="Settings"
          icon="settings"
          url="/cloud/cloud-settings"
          description="Fine-tune your Cloud instance"
        />
        <FeatureItem
          title="Support"
          icon="support"
          url="/cloud/cloud-support"
          description="Pick your plan & learn about support form"
        />
        <FeatureItem
          title="Maintenance & Troubleshooting"
          icon="studio-config"
          url="/cloud/cloud-maintenance-troubleshooting"
          description="Advanced monitoring system tracking critical performance metrics"
        />
        <FeatureItem
          title="Microsoft Azure Marketplace"
          icon="azure"
          url="/cloud/cloud-microsoft-azure-marketplace"
          description="Subscribe via Azure billing"
        />
        <FeatureItem
          title="AWS Marketplace"
          icon="aws"
          url="/cloud/cloud-aws-marketplace"
          description="Subscribe via AWS billing"
        />
      </div>
    </section>
  );
}
