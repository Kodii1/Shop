import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import "bootswatch/dist/darkly/bootstrap.min.css";
import MyNavbar from "./components/Navbar.tsx";
import { AuthProvider } from "./contexts/AuthContext.tsx";
import App from "./App.tsx";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <MyNavbar />
        <App />
      </AuthProvider>
    </BrowserRouter>
  </StrictMode>,
);
