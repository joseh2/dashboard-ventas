import axios from "axios";

const api = axios.create({
  baseURL: "https://dashboard-ventas-ccom.onrender.com/api",
});

export const getDashboard = (anio, mes) => api.get(`/Dashboard/${anio}/${mes}`);
export const saveMeta = (payload) => api.post("/Metas", payload);
export const saveVenta = (payload) => api.post("/Ventas", payload);
export const updateVenta = (id, payload) => api.put(`/Ventas/${id}`, payload);
export const deleteVenta = (id) => api.delete(`/Ventas/${id}`);
