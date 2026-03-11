"use client";

import Link from "next/link";
import { useAuth } from "../../src/hooks/useAuth";

export function ShellHeader() {
  const { isAuthenticated, username, loading, logout } = useAuth();

  return (
    <header className="shell-header">
      <h1>Conference Booking System</h1>
      {!loading && (
        <div className="header-auth">
          {isAuthenticated ? (
            <>
              <span className="user-name">Welcome, <strong>{username}</strong></span>
              <button className="btn danger" onClick={logout}>Logout</button>
            </>
          ) : (
            <Link className="btn primary" href="/login">Login</Link>
          )}
        </div>
      )}
    </header>
  );
}
