import { useEffect, useState } from "react";
import apiClient from "../api/apiClient";

const VALID_ROLES = ["admin", "employee", "receptionist"];

export function useAuth() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [role, setRole] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const userRole = localStorage.getItem("role")?.toLowerCase();

    if (token && userRole && VALID_ROLES.includes(userRole)) {
      setIsAuthenticated(true);
      setRole(userRole);
      return;
    }

    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("username");
    setIsAuthenticated(false);
    setRole(null);
  }, []);

  const login = async (username, password) => {
    const response = await apiClient.post("/Auth/login", { username, password });

    const token = response?.token;
    const roles = response?.roles;
    const normalizedRole = Array.isArray(roles) && roles.length > 0
      ? String(roles[0]).toLowerCase()
      : null;

    if (!token || !normalizedRole || !VALID_ROLES.includes(normalizedRole)) {
      throw new Error("Invalid login response from server");
    }

    localStorage.setItem("token", token);
    localStorage.setItem("role", normalizedRole);
    localStorage.setItem("username", response?.username || username);

    setIsAuthenticated(true);
    setRole(normalizedRole);
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("username");
    setIsAuthenticated(false);
    setRole(null);
  };

  return { isAuthenticated, role, login, logout };
}
