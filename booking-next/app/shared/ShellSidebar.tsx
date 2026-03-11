"use client";

import Link from "next/link";
import { useAuth } from "../../src/hooks/useAuth";

export function ShellSidebar() {
  const { isAuthenticated, loading } = useAuth();

  // Don't render anything until we know the auth state
  if (loading) {
    return <aside className="shell-sidebar"></aside>;
  }

  return (
    <aside className="shell-sidebar">
      {/* Next <Link> gives client-side transitions without full page refresh. */}
      {!isAuthenticated && (
        <>
          <Link href="/">Home</Link>
          <Link href="/login">Login</Link>
        </>
      )}
      {isAuthenticated && <Link href="/dashboard">Dashboard</Link>}
    </aside>
  );
}
