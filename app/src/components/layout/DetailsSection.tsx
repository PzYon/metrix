import React from "react";
import { Paper, styled, Typography } from "@mui/material";

export const DetailsSection: React.FC<{
  title?: string;
  subTitle?: string;
}> = ({ title, children }) => {
  return (
    <Host>
      {title ? <Typography sx={{ flexShrink: 0 }}>{title}</Typography> : null}
      {children}
    </Host>
  );
};

const Host = styled(Paper)`
  margin: ${(p) => p.theme.spacing(2)} 0;
  padding: ${(p) => p.theme.spacing(2)};
`;