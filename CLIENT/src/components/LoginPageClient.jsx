"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import LoginForm from "./LoginForm";
import { useAuth } from "../hooks/useAuth";

// This component represents the login page for the application. It checks if the user is already authenticated and redirects to the dashboard if so. If the user is not authenticated, it renders a LoginForm component and handles the login process by calling the login function from the authentication hook and then navigating to the dashboard upon successful login.
export function LoginPageClient() {
  const router = useRouter();
  const { isAuthenticated, login } = useAuth();

  // On component mount and whenever authentication status changes, check if the user is authenticated. If so, redirect to the dashboard page.
  useEffect(() => {
    // If user is already authenticated, redirect to dashboard
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
