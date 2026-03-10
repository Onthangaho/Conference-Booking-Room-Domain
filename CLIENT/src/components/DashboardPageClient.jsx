"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import Dashboard from "./Dashboard";
import { useAuth } from "../hooks/useAuth";

export function DashboardPageClient() {
  const router = useRouter();
  const { isAuthenticated, role, loading, logout } = useAuth();

  // Only redirect to login if loading is complete and user is NOT authenticated
  useEffect(() => {
    if (!loading && !isAuthenticated) {
      router.replace("/login");
    }
  }, [isAuthenticated, loading, router]);

  // Show loading during initial auth check
  if (loading) {
    return <main className="container"><p>Loading...</p></main>;
  }

  // Show dashboard if authenticated
  if (isAuthenticated) {
    return (
      <main className="container">
        <div className="dashboard-actions">
          <button className="btn danger" onClick={logout}>Sign Out</button>
        </div>
        <Dashboard role={role} />
      </main>
    );
  }

  // Redirecting to login
  return null;
}
