import "../src/styles/main.css";
import { AppProviders } from "./providers";
import { ShellHeader } from "./shared/ShellHeader";
import { ShellSidebar } from "./shared/ShellSidebar";
import { ShellFooter } from "./shared/ShellFooter";

export const metadata = {
  title: "Conference Booking System",
  description: "Secure conference booking with role-based workflows",
};

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <body suppressHydrationWarning>
        <AppProviders>
          {/* Layout stays mounted between route transitions (App Router behavior). */}
          <div className="app-shell">
            <ShellHeader />
            <div className="shell-content">
              <ShellSidebar />
              <section className="shell-main">{children}</section>
            </div>
            <ShellFooter />
          </div>
        </AppProviders>
      </body>
    </html>
  );
}
