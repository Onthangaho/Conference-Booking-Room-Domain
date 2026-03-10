"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import Dashboard from "./Dashboard";
import { useAuth } from "../hooks/useAuth";
// This component serves as the main dashboard page for authenticated users. It checks if the user is authenticated and redirects to the login page if not. If the user is authenticated, it displays the dashboard content and includes a sign-out button that calls the logout function from the authentication hook.
export function DashboardPageClient() {
  // Access router for navigation and authentication context for user info and logout function
  const router = useRouter();

  // Destructure authentication status, user role, and logout function 
  const { isAuthenticated, role, logout } = useAuth();
 // On component mount and whenever authentication status changes, check if the user is authenticated. If not, redirect to the login page.
  useEffect(() => {
    if (!isAuthenticated) {
      router.replace("/login");
    }
    
  }, [isAuthenticated, router]);
 // If authentication status is still being determined, show a loading message. Otherwise, render the dashboard content with a sign-out button.
  if (!isAuthenticated) {
    return <p className="container">Checking authentication...</p>;
  }

  return (
    <main className="container">
      <div className="dashboard-actions">
        <button className="btn danger" onClick={logout}>Sign Out</button>
      </div>
      <Dashboard role={role} />
    </main>
  );
}
