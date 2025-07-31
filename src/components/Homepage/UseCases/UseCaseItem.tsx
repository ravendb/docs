import CardWithImage, {
  CardWithImageProps,
} from "@site/src/components/Common/CardWithImage";

export default function UseCaseItem(props: CardWithImageProps) {
  const { title, url, description, imgSrc, imgAlt, ctaLabel } = props;
  return (
    <CardWithImage
      title={title}
      description={description}
      url={url}
      imgSrc={imgSrc}
      imgAlt={imgAlt}
      ctaLabel={ctaLabel}
    />
  );
}
