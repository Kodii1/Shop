import { useEffect, useState } from "react";
import Card from "react-bootstrap/Card";
import ProductsService from "../services/ProductsService";
import { Container, Row, Col } from "react-bootstrap";

const ItemsList = () => {
  const [items, setItems] = useState<ProductsService.Item[]>([]);

  useEffect(() => {
    ProductsService.getItems().then(setItems).catch(console.error);
  }, []);

  return (
    <Container className="d-flex justify-content-center">
      <Row className="justify-content-center">
        {items.map((item) => (
          <Col key={item.id} xs={12} sm={6} md={4} lg={3} className="mb-4">
            <Card style={{ width: "18rem" }}>
              <Card.Img variant="top" src={"https://via.placeholder.com/300"} />
              <Card.Body>
                <Card.Title>{item.name}</Card.Title>
                <Card.Text>Cena: {item.price} zł</Card.Text>
                <Card.Text>{item.description}</Card.Text>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    </Container>
  );
};

export default ItemsList;
