"use client";

import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { AuthProvider } from "../src/contexts/AuthContext";

export function AppProviders({ children }) {
  return (
    <AuthProvider>
      {children}
      <ToastContainer position="top-right" autoClose={3000} theme="colored" />
    </AuthProvider>
  );
}
