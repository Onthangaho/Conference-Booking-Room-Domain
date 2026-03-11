"use client";

import { useEffect } from "react";
import { useRouter, usePathname } from "next/navigation";
import { useAuth } from "../hooks/useAuth";

// This component checks if the user is authenticated before allowing access to its children. If the user is not authenticated, it redirects them to the login page. It also handles the loading state while checking authentication.
export function AuthGuard({ children }: { children: React.ReactNode }) {
  const { isAuthenticated, loading } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  // If we're still loading the auth state, we don't want to redirect yet. Once loading is done, if the user is not authenticated, we redirect to the login page.
  useEffect(() => {
    // Only redirect if we're not loading and the user is not authenticated. This prevents a flash of the protected content while we're still checking auth.
    if (!loading && !isAuthenticated) {
      router.replace(`/login`);
    }
  }, [isAuthenticated, loading, router, pathname]);

  // While we're loading the auth state, we can show a loading message or spinner. Once loading is complete, if the user is not authenticated, we return null (or could show a message). If authenticated, we render the children.
  if (loading) return <main className="container"><p>Loading...</p></main>;
  if (!isAuthenticated) return null;

  
  return <>{children}</>;
}
