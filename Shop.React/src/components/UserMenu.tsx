import { Image, Button, Dropdown } from "react-bootstrap";
import { useAuth } from "../contexts/AuthContext";
import avatarIcon from "../assets/avatar.png";
const UserMenu = () => {
  const { user, logout } = useAuth();
  return (
    <Dropdown align="end">
      {user?.name}
      <Dropdown.Toggle
        as={Button}
        variant="link"
        id="avatar-dropdown"
        className="p-0 border-0"
      >
        <Image
          src={avatarIcon}
          roundedCircle
          alt="Avatar"
          width={40}
          height={40}
        />
      </Dropdown.Toggle>
      <Dropdown.Menu>
        {user?.role === "Admin" && (
          <Dropdown.Item href="/ProductManagement">
            Modify Product
          </Dropdown.Item>
        )}
        <Dropdown.Item onClick={logout}>Logout</Dropdown.Item>
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default UserMenu;
