"use client";

import { createContext, useState, useEffect, useCallback } from "react";
import apiClient, { setUnauthorizedHandler } from "../api/apiClient";

const VALID_ROLES = ["admin", "employee", "receptionist"];

interface AuthContextType {
  isAuthenticated: boolean;
  token: string | null;
  role: string | null;
  username: string | null;
  loading: boolean;
  login: (user: string, password: string) => Promise<void>;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export function AuthProvider({ children }) {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [token, setToken] = useState<string | null>(null);
  const [role, setRole] = useState(null);
  const [username, setUsername] = useState(null);
  const [loading, setLoading] = useState(true);

  // Hydrate auth state from localStorage on mount (persists across page refreshes)
  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    const userRole = localStorage.getItem("role")?.toLowerCase();
    const userName = localStorage.getItem("username");

    if (storedToken && userRole && VALID_ROLES.includes(userRole)) {
      setIsAuthenticated(true);
      setToken(storedToken);
      setRole(userRole);
      setUsername(userName);
    } else {
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      localStorage.removeItem("username");
      setIsAuthenticated(false);
      setToken(null);
      setRole(null);
      setUsername(null);
    }
    setLoading(false);
  }, []);

  // useCallback ensures logout is stable — safe to pass as a dependency
  const logout = useCallback(() => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("username");
    setIsAuthenticated(false);
    setToken(null);
    setRole(null);
    setUsername(null);
  }, []);

  // Register logout as the global 401 handler so apiClient can call it
  useEffect(() => {
    setUnauthorizedHandler(logout);
  }, [logout]);

  const login = async (user, password) => {
    try {
      const response = await apiClient.post("/auth/login", { username: user, password }) as any;

      const newToken = response?.token;
      const roles = response?.roles;
      const normalizedRole = Array.isArray(roles) && roles.length > 0
        ? String(roles[0]).toLowerCase()
        : null;

      if (!newToken || !normalizedRole || !VALID_ROLES.includes(normalizedRole)) {
        throw new Error("Invalid login response from server");
      }

      localStorage.setItem("token", newToken);
      localStorage.setItem("role", normalizedRole);
      localStorage.setItem("username", response?.username || user);

      setIsAuthenticated(true);
      setToken(newToken);
      setRole(normalizedRole);
      setUsername(response?.username || user);
    } catch (error) {
      localStorage.removeItem("token");
      localStorage.removeItem("role");
      localStorage.removeItem("username");
      setIsAuthenticated(false);
      setToken(null);
      setRole(null);
      setUsername(null);
      throw error;
    }
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, token, role, username, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
