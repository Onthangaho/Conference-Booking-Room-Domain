function Navbar({ onLogout, isAuthenticated, username }) {
  return (
    <nav className="navbar">
      <span>Conference Booking System</span>
      <div>
        {isAuthenticated ? (
          <div className="navbar-user">
            <span className="user-name">Welcome, <strong>{username}</strong></span>
            <button className="btn danger" onClick={onLogout}>Sign Out</button>
          </div>
        ) : (
          <span>Please log in</span>
        )}
      </div>
    </nav>
  );
}

export default Navbar;
