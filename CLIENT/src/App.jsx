import Navbar from "./components/Navbar";
import Header from "./components/Header";
import Dashboard from "./components/Dashboard";
import Footer from "./components/Footer";
import { ToastContainer } from "react-toastify";

function App() {
  return (
    <>
      <Navbar />
      <Header />
      <Dashboard /> {/* ✅ Dashboard handles everything */}
      <Footer />
      <ToastContainer position="top-right" autoClose={3000} theme="colored" />
    </>
  );
}

export default App;
