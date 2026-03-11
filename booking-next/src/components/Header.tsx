import Heading from "./Heading";
import ConnectionStatus from "./ConnectionStatus";

function Header() {
  return (
    <header style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
      <Heading text="Conference Booking Dashboard" />
      <ConnectionStatus />
    </header>
  );
}

export default Header;

