import Navbar from "./components/Navbar";
import Header from "./components/Header";
import Dashboard from "./components/Dashboard";
import Footer from "./components/Footer";
import LoginForm from "./components/LoginForm";
import AppErrorBoundary from "./components/AppErrorBoundary";
import DashboardSkeleton from "./components/DashboardSkeleton";
import { ToastContainer } from "react-toastify";
import { useAuth } from "./hooks/useAuth";

function App() {
  const { isAuthenticated, role, username, loading, login, logout } = useAuth();

  if (loading) {
    return (
      <>
        <Navbar onLogout={logout} isAuthenticated={false} username={username} />
        <Header />
        <DashboardSkeleton />
        <Footer />
        <ToastContainer position="top-right" autoClose={3000} theme="colored" />
      </>
    );
  }

  return (
    <>
      <Navbar onLogout={logout} isAuthenticated={isAuthenticated} username={username} />
      <Header />
      <AppErrorBoundary>
        {isAuthenticated ? (
          <Dashboard role={role} />
        ) : (
          <LoginForm onLogin={login} />
        )}
      </AppErrorBoundary>
      <Footer />
      <ToastContainer position="top-right" autoClose={3000} theme="colored" />
    </>
  );
}

export default App;