import axios from "axios";

axios.defaults.baseURL = "http://localhost:5131";

export interface Item {
  id: number;
  name: string;
  price: number;
  description: string;
  category: number;
}

export type CreateItemRequest = Omit<Item, "id">;

export const getItems = async (): Promise<Item[]> => {
  const response = await axios.get<Item[]>("api/Item", {
    headers: {
      accept: "text/json",
    },
  });
  return response.data;
};

export const createItem = async (item: CreateItemRequest): Promise<Item> => {
  const response = await axios.post<Item>("api/Item", item, {
    headers: {
      accept: "text/json",
      "Content-Type": "application/json",
    },
  });
  return response.data;
};

export const updateItem = async (
  id: number,
  itemData: Omit<Item, "id">,
): Promise<Item> => {
  const response = await axios.put<Item>("api/Item", itemData, {
    params: { id },
    headers: {
      accept: "text/json",
      "Content-Type": "application/json",
    },
  });
  return response.data;
};

export const deleteItem = async (id: number): Promise<void> => {
  await axios.delete("api/Item", {
    params: { id },
    headers: {
      accept: "*/*",
    },
  });
};

export default { getItems, createItem, updateItem, deleteItem };
