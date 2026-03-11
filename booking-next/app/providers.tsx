"use client";

import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { AuthProvider } from "../src/contexts/AuthContext";

// This component wraps the entire app with necessary providers (like AuthProvider) and includes the ToastContainer for notifications. It ensures that all pages have access to authentication state and can show toast messages.
export function AppProviders({ children }) {
  return (
    <AuthProvider>
      {children}
      <ToastContainer position="top-right" autoClose={3000} theme="colored" />
    </AuthProvider>
  );
}
