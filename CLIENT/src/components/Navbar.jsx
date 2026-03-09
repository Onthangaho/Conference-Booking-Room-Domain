function Navbar({ onLogout, isAuthenticated }) {
  return (
    <nav className="navbar">
      <span>Conference Booking System</span>
      <div>
        {isAuthenticated ? (
          <button className="btn danger" onClick={onLogout}>Sign Out</button>
        ) : (
          <span>Please log in</span>
        )}
      </div>
    </nav>
  );
}

export default Navbar;
