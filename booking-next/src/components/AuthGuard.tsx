"use client";

import { useEffect } from "react";
import { useRouter, usePathname } from "next/navigation";
import { useAuth } from "../hooks/useAuth";

export function AuthGuard({ children }: { children: React.ReactNode }) {
  const { isAuthenticated, loading } = useAuth();
  const router = useRouter();
  const pathname = usePathname();

  useEffect(() => {
    if (!loading && !isAuthenticated) {
      router.replace(`/login`);
    }
  }, [isAuthenticated, loading, router, pathname]);

  if (loading) return <main className="container"><p>Loading...</p></main>;
  if (!isAuthenticated) return null;

  return <>{children}</>;
}
