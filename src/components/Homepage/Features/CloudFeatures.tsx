import Heading from "@theme/Heading";
import CoreServiceFeaturesGrid from "./FeaturesLists/Cloud/CoreServiceFeatures";
import OperationsFeaturesGrid from "./FeaturesLists/Cloud/OperationsFeatures";
import MarketplaceEcosystemFeaturesGrid from "./FeaturesLists/Cloud/MarketplaceEcosystemFeatures";

export default function CloudFeatures() {
    return (
        <section className="mb-8">
            <Heading as="h3">Browse</Heading>
            <div className="mb-8">
                <CoreServiceFeaturesGrid />
            </div>
            <div className="mb-8">
                <OperationsFeaturesGrid />
            </div>
            <div className="mb-8">
                <MarketplaceEcosystemFeaturesGrid />
            </div>
        </section>
    );
}
