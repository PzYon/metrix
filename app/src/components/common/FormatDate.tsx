import { format, formatDistanceToNow } from "date-fns";

export enum DateFormat {
  relativeToNow,
  numerical,
  full,
  ticks,
}

export const formatDate = (
  value: string | number | Date,
  dateFormat?: DateFormat
): string => {
  let date;
  if (typeof value === "string" || typeof value === "number") {
    date = new Date(value);
  } else if (value instanceof Date) {
    date = value;
  } else {
    throw new Error(`'${value}' is an invalid date.`);
  }

  switch (dateFormat) {
    case DateFormat.full:
      return format(date, "PPPPpppp");
    case DateFormat.numerical:
      return format(date, "Pp");
    case DateFormat.ticks:
      return format(date, "T");
    case DateFormat.relativeToNow:
    default:
      return formatDistanceToNow(date, {
        addSuffix: true,
        includeSeconds: true,
      });
  }
};

export const FormatDate = (props: {
  value: Date | string | number;
  dateFormat?: DateFormat;
}) => {
  if (!props.value) {
    return null;
  }

  return (
    <span
      title={formatDate(
        props.value,
        props.dateFormat === DateFormat.relativeToNow
          ? DateFormat.full
          : DateFormat.relativeToNow
      )}
    >
      {formatDate(props.value, props.dateFormat)}
    </span>
  );
};