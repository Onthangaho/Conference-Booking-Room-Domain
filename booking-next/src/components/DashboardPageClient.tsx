"use client";

import Dashboard from "./Dashboard";
import { useAuth } from "../hooks/useAuth";
import { AuthGuard } from "./AuthGuard";

export function DashboardPageClient() {
  const { role } = useAuth();

  return (
    <AuthGuard>
      <main className="container">
        <Dashboard role={role} />
      </main>
    </AuthGuard>
  );
}
