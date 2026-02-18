//this is a reusable button component that can be used throughout the application. It accepts a label, an onClick handler, and a variant for styling.

function Button({ label, onClick, variant = "primary", type = "button" }) {
  return (
    <button 
      type={type} 
      className={`btn ${variant}`} 
      onClick={onClick}
    >
      {label}
    </button>
  );
}

export default Button;