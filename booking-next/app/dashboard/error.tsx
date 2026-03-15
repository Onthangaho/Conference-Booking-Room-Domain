"use client";

import { useEffect } from "react";

export default function DashboardError({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    console.error("Dashboard route error:", error);
  }, [error]);

  return (
    <main className="container">
      <section className="error-state" role="alert">
        <h2>Something went wrong in the dashboard</h2>
        <p>The page hit an error. You can reset and try loading it again.</p>
        <button className="btn primary" type="button" onClick={() => reset()}>
          Reset
        </button>
      </section>
    </main>
  );
}
