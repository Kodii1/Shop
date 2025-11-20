import { Routes, Route } from "react-router-dom";
import Register from "./pages/Register";
import ItemList from "./components/ItemList.tsx";
import ProductManagement from "./pages/ProductManagement";

export default function App() {
  return (
    <Routes>
      <Route path="/register" element={<Register />} />
      <Route path="/" element={<ItemList />} />
      <Route path="/ProductManagement" element={<ProductManagement />} />
    </Routes>
  );
}
