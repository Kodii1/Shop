import { useEffect, useState } from "react";
import { Table, Button, Modal, Form, Container, Alert } from "react-bootstrap";
import ProductsService from "../services/ProductsService";

const ProductManagement = () => {
  const [items, setItems] = useState<ProductsService.Item[]>([]);
  const [showModal, setShowModal] = useState(false);
  const [editingItem, setEditingItem] = useState<ProductsService.Item | null>(
    null,
  );
  const [formData, setFormData] = useState({
    name: "",
    price: 0,
    description: "",
    category: 0,
  });
  const [error, setError] = useState("");

  useEffect(() => {
    loadItems();
  }, []);

  const loadItems = async () => {
    try {
      const data = await ProductsService.getItems();
      setItems(data);
    } catch (err) {
      console.error("Error loading:", err);
    }
  };

  const handleShowModal = (item: ProductsService.Item | null = null) => {
    setEditingItem(item);
    if (item) {
      setFormData({
        name: item.name,
        price: item.price,
        description: item.description,
        category: item.category,
      });
    } else {
      setFormData({ name: "", price: 0, description: "", category: 0 });
    }
    setShowModal(true);
    setError("");
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      if (editingItem && editingItem.id) {
        await ProductsService.updateItem(editingItem.id, formData);
      } else if (!editingItem) {
        await ProductsService.createItem(formData);
      } else {
        throw new Error("Missing ID for edited product");
      }

      setShowModal(false);
      loadItems();
    } catch (err) {
      console.error("Error:", err);
      setError(err.message || "Error while saving");
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm("Are you sure you want to delete?")) {
      try {
        await ProductsService.deleteItem(id);
        loadItems();
      } catch (err) {
        console.error("Delete error:", err);
        setError("Error while deleting product");
      }
    }
  };

  return (
    <Container className="mt-4">
      <div className="d-flex justify-content-between mb-3">
        <h2>Product Management</h2>
        <Button onClick={() => handleShowModal()}>+ Add Product</Button>
      </div>

      {error && <Alert variant="danger">{error}</Alert>}

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Price (PLN)</th>
            <th>Description</th>
            <th>Category</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {items.map((item) => (
            <tr key={item.id}>
              <td>{item.id}</td>
              <td>{item.name}</td>
              <td>{item.price}</td>
              <td>{item.description}</td>
              <td>{item.category}</td>
              <td>
                <Button
                  size="sm"
                  variant="outline-primary"
                  className="me-2"
                  onClick={() => handleShowModal(item)}
                >
                  Edit
                </Button>
                <Button
                  size="sm"
                  variant="outline-danger"
                  onClick={() => handleDelete(item.id)}
                >
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <Modal show={showModal} onHide={() => setShowModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>
            {editingItem ? `Edit: ${editingItem.name}` : "New Product"}
          </Modal.Title>
        </Modal.Header>
        <Form onSubmit={handleSubmit}>
          <Modal.Body>
            {error && <Alert variant="danger">{error}</Alert>}

            <Form.Group className="mb-3">
              <Form.Label>Name</Form.Label>
              <Form.Control
                type="text"
                value={formData.name}
                onChange={(e) =>
                  setFormData({ ...formData, name: e.target.value })
                }
                required
              />
            </Form.Group>

            <Form.Group className="mb-3">
              <Form.Label>Price (PLN)</Form.Label>
              <Form.Control
                type="number"
                value={formData.price}
                onChange={(e) =>
                  setFormData({ ...formData, price: Number(e.target.value) })
                }
                required
              />
            </Form.Group>

            <Form.Group className="mb-3">
              <Form.Label>Description</Form.Label>
              <Form.Control
                as="textarea"
                rows={3}
                value={formData.description}
                onChange={(e) =>
                  setFormData({ ...formData, description: e.target.value })
                }
                required
              />
            </Form.Group>

            <Form.Group className="mb-3">
              <Form.Label>Category</Form.Label>
              <Form.Control
                type="number"
                value={formData.category}
                onChange={(e) =>
                  setFormData({ ...formData, category: Number(e.target.value) })
                }
                required
              />
            </Form.Group>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={() => setShowModal(false)}>
              Cancel
            </Button>
            <Button variant="primary" type="submit">
              {editingItem ? "Save Changes" : "Add Product"}
            </Button>
          </Modal.Footer>
        </Form>
      </Modal>
    </Container>
  );
};

export default ProductManagement;
