import { useState } from "react";
import { useAuth } from "./contexts/AuthContext";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";

const Register = () => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [repeatEmail, setRepeatEmail] = useState("");
  const [password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState([]);

  const { register } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError([]);

    if (email !== repeatEmail) {
      setError(["Emails doesn't match."]);
      setLoading(false);
      return;
    }
    if (password !== repeatPassword) {
      setError(["Passwords doesn't match."]);
      setLoading(false);
      return;
    }

    try {
      const result = await register({ firstName, lastName, email, password });

      if (result.success) {
        setFirstName("");
        setLastName("");
        setEmail("");
        setRepeatEmail("");
        setPassword("");
        setRepeatPassword("");
        setError([]);
      } else {
        setError(result.errors || [result.error]);
      }
    } catch (err) {
      setError(["Error: " + err]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Form className="px-3 py-3" onSubmit={handleSubmit}>
      <Form.Group className="mb-3" controlId="registerFirstName">
        <Form.Label>First name</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter first name"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          required
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="registerLastName">
        <Form.Label>Last name</Form.Label>
        <Form.Control
          type="text"
          placeholder="Enter last name"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          required
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="registerEmail">
        <Form.Label>Email address</Form.Label>
        <Form.Control
          type="email"
          placeholder="Enter email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="registerRepeatEmail">
        <Form.Label>Repeat email address</Form.Label>
        <Form.Control
          type="email"
          placeholder="Repeat email"
          value={repeatEmail}
          onChange={(e) => setRepeatEmail(e.target.value)}
          required
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="registerPassword">
        <Form.Label>Password</Form.Label>
        <Form.Control
          type="password"
          placeholder="Enter password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </Form.Group>

      <Form.Group className="mb-3" controlId="registerRepeatPassword">
        <Form.Label>Repeat Password</Form.Label>
        <Form.Control
          type="password"
          placeholder="Repeat password"
          value={repeatPassword}
          onChange={(e) => setRepeatPassword(e.target.value)}
          required
        />
      </Form.Group>

      {error.length > 0 && (
        <div style={{ color: "red", marginBottom: "10px" }}>
          {error.map((errMsg, i) => (
            <div key={i}>{errMsg}</div>
          ))}
        </div>
      )}

      <Button type="submit" variant="primary" disabled={loading}>
        {loading ? "Signing up..." : "Sign up"}
      </Button>
    </Form>
  );
};

export default Register;
