import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { PublicClientApplication } from "@azure/msal-browser";
import { MsalProvider } from "@azure/msal-react";
import "./index.css";
import App from "./App.tsx";
import { AuthGuard } from "./auth/AuthGuard.tsx";
import { msalConfig } from "./auth/authConfig.ts";

const msalInstance = new PublicClientApplication(msalConfig);

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <MsalProvider instance={msalInstance}>
      <AuthGuard>
        <App />
      </AuthGuard>
    </MsalProvider>
  </StrictMode>
);
