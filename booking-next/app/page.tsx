import Link from "next/link";

export default function LandingPage() {
  return (
    <main className="container">
      <h2>Welcome</h2>
      <p>This is the landing page for the Conference Booking System.</p>
      <div className="landing-actions">
        <Link className="btn primary" href="/login">Go to Login</Link>
        <Link className="btn secondary" href="/dashboard">Go to Dashboard</Link>
      </div>
    </main>
  );
}
