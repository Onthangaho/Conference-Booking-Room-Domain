"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import LoginForm from "./LoginForm";
import { useAuth } from "../hooks/useAuth";

export function LoginPageClient() {
  const router = useRouter();
  const { isAuthenticated, login } = useAuth();

  useEffect(() => {
    if (isAuthenticated) {
      router.replace("/dashboard");
    }
  }, [isAuthenticated, router]);

  return (
    <main className="container">
      {/* LoginForm remains interactive Client UI. */}
      <LoginForm
        onLogin={async (username, password) => {
          await login(username, password);
          router.push("/dashboard");
        }}
      />
    </main>
  );
}
