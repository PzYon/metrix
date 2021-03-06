import React from "react";
import { MetricType } from "../../serverApi/MetricType";
import { styled } from "@mui/material";
import { MetricTypeFactory } from "../../metricTypes/MetricTypeFactory";

export enum MetricTypeIconStyle {
  Overview,
  PageTitle,
}

export const MetricTypeIcon: React.FC<{
  type: MetricType;
  style: MetricTypeIconStyle;
}> = ({ type, style }) => {
  const Host =
    style === MetricTypeIconStyle.Overview ? OverviewHost : PageTitleHost;

  return <Host>{MetricTypeFactory.create(type).getIcon()}</Host>;
};

const PageTitleHost = styled("div")`
  svg {
    border-radius: 100%;
    height: 0.8em;
    width: 0.8em;
    color: ${(p) => p.theme.palette.text.primary};
    border-color: ${(p) => p.theme.palette.text.primary};
    border: 2px solid ${(p) => p.theme.palette.text.primary};
    margin-top: 5px;
    margin-right: 10px;
    padding: 2px;
  }
`;

const OverviewHost = styled("div")`
  svg {
    border-radius: 100%;
    border: 2px solid ${(p) => p.theme.palette.primary.main};
    padding: 8px;
    color: ${(p) => p.theme.palette.primary.main};
    background-color: ${(p) => p.theme.palette.background.default};
    width: 0.8em;
    height: 0.8em;
  }
`;
