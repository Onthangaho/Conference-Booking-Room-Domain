"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import Dashboard from "./Dashboard";
import { useAuth } from "../hooks/useAuth";

export function DashboardPageClient() {
  const router = useRouter();
  const { isAuthenticated, role, logout } = useAuth();

  useEffect(() => {
    if (!isAuthenticated) {
      router.replace("/login");
    }
  }, [isAuthenticated, router]);

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
