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

export default function Features() {
  return (
    <section className="mb-8">
      <Heading as="h3">Browse by features</Heading>
      <div className="mb-8">
        <AiFeaturesGrid />
      </div>
      <div className="mb-8">
        <IndexesFeaturesGrid />
      </div>
      <div className="mb-8">
        <QueryingFeaturesGrid />
      </div>
      <div className="mb-8">
        <DocumentExtensionsFeaturesGrid />
      </div>
      <div className="mb-8">
        <BackgroundTasksFeaturesGrid />
      </div>
      <div className="mb-8">
        <ClustersFeaturesGrid />
      </div>
      <div className="mb-8">
        <OngoingTasksFeaturesGrid />
      </div>
      <div className="mb-8">
        <SecurityFeaturesGrid />
      </div>
      <div className="mb-8">
        <MonitoringFeaturesGrid />
      </div>
      <div className="mb-8">
        <AdministrationFeaturesGrid />
      </div>
      <div className="mb-8">
        <IntegrationFeaturesGrid />
      </div>
    </section>
  );
}
