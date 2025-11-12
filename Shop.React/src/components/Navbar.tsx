import { Navbar, Nav, Container } from "react-bootstrap";
import { useAuth } from "../contexts/AuthContext";
import Login from "./Login";
import UserMenu from "./UserMenu.tsx";
const MyNavbar = () => {
  const { isAuthenticated } = useAuth();
  return (
    <Navbar bg="white" expand="lg">
      <Container>
        <Navbar.Brand href="/">Shop</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="ms-auto">
            {isAuthenticated ? <UserMenu /> : <Login />}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default MyNavbar;
