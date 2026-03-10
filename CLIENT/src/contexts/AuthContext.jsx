"use client";

import { createContext, useState, useEffect } from "react";
import apiClient from "../api/apiClient";

const VALID_ROLES = ["admin", "employee", "receptionist"];

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [role, setRole] = useState(null);
  const [username, setUsername] = useState(null);
  const [loading, setLoading] = useState(true);

  // Initialize auth state from localStorage on mount
  useEffect(() => {
    const token = localStorage.getItem("token");
    const userRole = localStorage.getItem("role")?.toLowerCase();
    const userName = localStorage.getItem("username");

    if (token && userRole && VALID_ROLES.includes(userRole)) {
      setIsAuthenticated(true);
      setRole(userRole);
      setUsername(userName);
    } else {
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      localStorage.removeItem("username");
      setIsAuthenticated(false);
      setRole(null);
      setUsername(null);
    }
    setLoading(false);
  }, []);

  const login = async (user, password) => {
    try {
      const response = await apiClient.post("/auth/login", { username: user, password });

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
      localStorage.setItem("username", response?.username || user);

      setIsAuthenticated(true);
      setRole(normalizedRole);
      setUsername(response?.username || user);
    } catch (error) {
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      localStorage.removeItem("username");
      setIsAuthenticated(false);
      setRole(null);
      setUsername(null);
      throw error;
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("username");
    setIsAuthenticated(false);
    setRole(null);
    setUsername(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, role, username, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
