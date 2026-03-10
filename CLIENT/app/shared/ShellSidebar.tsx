import Link from "next/link";

export function ShellSidebar() {
  return (
    <aside className="shell-sidebar">
      {/* Next <Link> gives client-side transitions without full page refresh. */}
      <Link href="/">Home</Link>
      <Link href="/login">Login</Link>
      <Link href="/dashboard">Dashboard</Link>
    </aside>
  );
}
