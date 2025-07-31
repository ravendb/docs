import CardWithIcon, {
  CardWithIconProps,
} from "@site/src/components/Common/CardWithIcon";

export default function StartingPoint(props: CardWithIconProps) {
  const { title, description, url } = props;
  return <CardWithIcon title={title} description={description} url={url} />;
}
