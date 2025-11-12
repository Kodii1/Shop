import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import "bootswatch/dist/darkly/bootstrap.min.css";
import MyNavbar from "./components/Navbar.tsx";
import { AuthProvider } from "./contexts/AuthContext.tsx";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <MyNavbar />
      </AuthProvider>
    </BrowserRouter>
  </StrictMode>,
);
