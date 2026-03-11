"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import LoginForm from "./LoginForm";
import { useAuth } from "../hooks/useAuth";

export function LoginPageClient() {
  const router = useRouter();
  const { isAuthenticated, loading, login } = useAuth();

  // Only redirect if loading is complete and user is authenticated
  useEffect(() => {
    if (!loading && isAuthenticated) {
      router.replace("/dashboard");
    }
  }, [isAuthenticated, loading, router]);

  // Show loading during initial auth check
  if (loading) {
    return <main className="container"><p>Loading...</p></main>;
  }

  // Show login form if not authenticated
  if (!isAuthenticated) {
    return (
      <main className="container">
        <LoginForm
          onLogin={async (username, password) => {
            await login(username, password);
            router.push("/dashboard");
          }}
        />
      </main>
    );
  }

  return null;
}
