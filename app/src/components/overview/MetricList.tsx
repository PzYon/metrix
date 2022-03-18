import React, { useEffect, useState } from "react";
import { IMetric } from "../../serverApi/IMetric";
import { MetricListItem } from "./MetricListItem";
import { ServerApi } from "../../serverApi/ServerApi";
import { Grid, Typography } from "@mui/material";
import { GridItem } from "./GridItem";
import { translations } from "../../i18n/translations";
import { Section } from "../layout/Section";
import { useAppContext } from "../../AppContext";

export const MetricList: React.FC = () => {
  const [metrics, setMetrics] = useState<IMetric[]>([]);

  const { setAppAlert } = useAppContext();

  useEffect(() => {
    ServerApi.getMetrics()
      .then((data) => {
        debugger;
        setMetrics(data);
      })
      .catch((e) => {
        // setMetrics([]);
        setAppAlert({
          message: e.message,
          title: e.message,
          type: "todo",
        });
      });
  }, []);

  return (
    <Section>
      <Grid container rowSpacing={2} columnSpacing={2}>
        <GridItem reactKey={"add"} key={"add"} targetUrl={"/metrics/create"}>
          <Typography variant={"h4"}>+</Typography>
          <Typography variant={"subtitle1"}>{translations.create}</Typography>
        </GridItem>
        {metrics.map((m) => (
          <GridItem
            reactKey={m.key}
            key={m.key}
            targetUrl={`/metrics/view/${m.key}`}
          >
            <MetricListItem metric={m} />
          </GridItem>
        ))}
      </Grid>
    </Section>
  );
};
