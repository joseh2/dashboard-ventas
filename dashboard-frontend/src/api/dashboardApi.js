import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5187/api",
});

export const getDashboard = (anio, mes) => api.get(`/dashboard/${anio}/${mes}`);
export const saveMeta = (payload) => api.post("/metas", payload);
export const saveVenta = (payload) => api.post("/ventas", payload);