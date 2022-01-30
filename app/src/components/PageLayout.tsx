import React from "react";
import { Paper, styled } from "@mui/material";

export const PageLayout: React.FC = ({ children }) => {
  return <Host elevation={3}>{children}</Host>;
};

const Host = styled(Paper)`
  max-width: 1200px;
  min-height: calc(100vh - 80px);
  margin: 20px auto 20px auto;
  padding: 20px;
`;