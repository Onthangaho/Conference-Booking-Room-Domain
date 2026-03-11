import Navbar from "./components/Navbar";
import Header from "./components/Header";
import Dashboard from "./components/Dashboard";
import Footer from "./components/Footer";
import LoginForm from "./components/LoginForm";
import { ToastContainer } from "react-toastify";
import { useAuth } from "./hooks/useAuth";

function App() {
  const { isAuthenticated, role, login, logout } = useAuth();

  return (
    <>
      <Navbar onLogout={logout} isAuthenticated={isAuthenticated} />
      <Header />
      {isAuthenticated ? (
        <Dashboard role={role} />
      ) : (
        <LoginForm onLogin={login} />
      )}
      <Footer />
      <ToastContainer position="top-right" autoClose={3000} theme="colored" />
    </>
  );
}

export default App;