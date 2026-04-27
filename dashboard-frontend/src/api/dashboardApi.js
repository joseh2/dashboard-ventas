import axios from "axios";

const api = axios.create({
  baseURL: "https://dashboard-ventas-ccom.onrender.com/swagger/",
});

export const getDashboard = (anio, mes) => api.get(`/dashboard/${anio}/${mes}`);
export const saveMeta = (payload) => api.post("/metas", payload);
export const saveVenta = (payload) => api.post("/ventas", payload);