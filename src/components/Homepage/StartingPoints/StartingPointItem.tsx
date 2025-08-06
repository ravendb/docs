import CardWithIcon, {
  CardWithIconProps,
} from "@site/src/components/Common/CardWithIcon";

export default function StartingPoint(props: CardWithIconProps) {
  const { title, icon, description, url } = props;
  return <CardWithIcon title={title} icon={icon} description={description} url={url} />;
}
