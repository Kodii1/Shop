import { createContext, useContext, useState, useEffect } from "react";
import axios from "axios";
import { jwtDecode } from "jwt-decode";

axios.defaults.baseURL = "http://localhost:5131";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem("token"));
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const initializeAuth = () => {
      const savedToken = localStorage.getItem("token");

      if (savedToken) {
        try {
          const decoded = jwtDecode(savedToken);
          const currentTime = Date.now() / 1000;

          if (decoded.exp < currentTime) {
            logout();
          } else {
            setToken(savedToken);
            setUser({
              email: decoded.email,
              name: decoded.given_name,
              role: decoded.role,
            });
            axios.defaults.headers.common["Authorization"] =
              "Bearer " + savedToken;
          }
        } catch (error) {
          console.error("Token verification error:", error);
          logout();
        }
      }
      setIsLoading(false);
    };

    initializeAuth();
  }, []);

  useEffect(() => {
    if (token) {
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;
      localStorage.setItem("token", token);
    } else {
      delete axios.defaults.headers.common["Authorization"];
      localStorage.removeItem("token");
    }
  }, [token]);

  const login = async (credentials) => {
    try {
      const response = await axios.post("api/User/login", credentials);
      const receivedToken = response.data;

      if (receivedToken) {
        setToken(receivedToken);
        const decoded = jwtDecode(receivedToken);
        setUser({
          email: decoded.email,
          name: decoded.given_name,
          role: decoded.role,
        });
        return { success: true };
      } else {
        console.error("Token not found in response:", response.data);
        return { success: false, error: "Token not found in API response" };
      }
    } catch (error) {
      console.error("Login error:", error);
      const errorMessage =
        error.response?.data?.message ||
        error.response?.data?.error ||
        "Login failed. Please check your credentials.";
      return { success: false, error: errorMessage };
    }
  };
  const register = async (credentials) => {
    try {
      const response = await axios.post("api/User/register", credentials);
      const receivedToken = response.data;

      if (receivedToken) {
        setToken(receivedToken);
        const decoded = jwtDecode(receivedToken);
        setUser({
          email: decoded.email,
          name: decoded.given_name,
          role: decoded.role,
        });
        return { success: true };
      } else {
        return { success: false, error: ["Token not found in API response"] };
      }
    } catch (error) {
      const backendErrors = error.response?.data?.[""] || [
        error.response?.data?.message || "Register failed",
      ];
      return { success: false, errors: backendErrors };
    }
  };

  const logout = () => {
    setToken(null);
    setUser(null);
  };

  const contextValue = {
    token,
    user,
    register,
    login,
    logout,
    isLoading,
    isAuthenticated: !!token,
  };

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
