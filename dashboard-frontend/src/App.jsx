import { useEffect, useMemo, useState } from "react";

import { getDashboard, saveMeta, saveVenta, updateVenta, deleteVenta } from "./api/dashboardApi";
import SummaryCard from "./components/SummaryCard";
import MetaForm from "./components/MetaForm";
import SalesForm from "./components/SalesForm";
import ComparisonCard from "./components/ComparisonCard";
import DailyTable from "./components/DailyTable";
import SalesChart from "./components/SalesChart";

import "./styles.css";

const money = new Intl.NumberFormat("es-SV", {
  style: "currency",
  currency: "USD",
});

export default function App() {
  const today = new Date();
  const [anio, setAnio] = useState(today.getFullYear());
  const [mes, setMes] = useState(today.getMonth() + 1);
  const [dashboard, setDashboard] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

const editarVenta = async (id, payload) => {
  await updateVenta(id, payload);
  alert("Venta modificada correctamente");
  await cargarDashboard();
};

const borrarVenta = async (id) => {
  const confirmar = confirm("¿Seguro que deseas borrar esta venta?");
  if (!confirmar) return;

  await deleteVenta(id);
  alert("Venta borrada correctamente");
  await cargarDashboard();
};



  const meses = useMemo(
    () => [
      "Enero",
      "Febrero",
      "Marzo",
      "Abril",
      "Mayo",
      "Junio",
      "Julio",
      "Agosto",
      "Septiembre",
      "Octubre",
      "Noviembre",
      "Diciembre",
    ],
    []
  );

  const cargarDashboard = async () => {
    try {
      setLoading(true);
      setError("");
      const response = await getDashboard(anio, mes);
      setDashboard(response.data);
    } catch (err) {
      setDashboard(null);
      setError(err?.response?.data?.mensaje || "No se pudo cargar el dashboard.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    cargarDashboard();
  }, [anio, mes]);

  const guardarMeta = async (payload) => {
    await saveMeta(payload);
    await cargarDashboard();
  };

  const guardarVenta = async (payload) => {
    await saveVenta(payload);
    await cargarDashboard();
  };

   return (
    <div className="page">
      <div className="container">
        <header>
          <h1>Dashboard de ventas</h1>
          <p>
            Control mensual con meta, acumulado, porcentaje al día, porcentaje total,
            diferencia, mes pasado y año pasado.
          </p>
        </header>

        <section className="filters">
          <select value={mes} onChange={(e) => setMes(Number(e.target.value))}>
            {meses.map((nombre, index) => (
              <option key={nombre} value={index + 1}>
                {nombre}
              </option>
            ))}
          </select>

          <input
            type="number"
            value={anio}
            onChange={(e) => setAnio(Number(e.target.value))}
          />

          <button onClick={cargarDashboard}>Actualizar</button>
        </section>

        <section className="form-grid">
          <MetaForm anio={anio} mes={mes} onSave={guardarMeta} />
          <SalesForm anio={anio} mes={mes} onSave={guardarVenta} />
        </section>

        {loading && <p>Cargando datos...</p>}
        {error && <p className="error">{error}</p>}








        {dashboard && (
          <>
            <section className="summary-grid">
              <SummaryCard title="Venta acumulada" value={money.format(dashboard.ventaAcumulada)} accent="#2563eb" />
              <SummaryCard title="Al día" value={money.format(dashboard.alDia)} accent="#0f766e" />
              <SummaryCard title="Porcentaje al día" value={`${dashboard.porcentajeAlDia.toFixed(2)}%`} accent="#f59e0b" />
              <SummaryCard title="Diferencia" value={money.format(dashboard.diferencia)} accent={dashboard.diferencia >= 0 ? "#15803d" : "#b91c1c"} />
              <SummaryCard title="Falta" value={money.format(dashboard.falta)} accent="#dc2626" />
              <SummaryCard title="Diaria para meta" value={money.format(dashboard.diariaParaMeta)} accent="#7c3aed" />
              <SummaryCard title="Meta diaria" value={money.format(dashboard.metaDiaria)} accent="#0891b2" />
              <SummaryCard title="Meta mes" value={money.format(dashboard.metaMensual)} accent="#111827" />
              <SummaryCard title="Porcentaje total" value={`${dashboard.porcentajeTotal.toFixed(2)}%`} accent="#14b8a6" />
            </section>

            <section className="comparison-grid">
              <ComparisonCard title="Año pasado" data={dashboard.anioPasado} />
              <ComparisonCard title="Mes pasado" data={dashboard.mesPasado} />
              <div className="card">
                <h3>Proyección diaria</h3>
                <p><strong>Tienda:</strong> {money.format(dashboard.proyeccionTienda)}</p>
                <p><strong>Vendedor:</strong> {money.format(dashboard.proyeccionVendedor)}</p>
                <p><strong>Mayoreo:</strong> {money.format(dashboard.proyeccionMayoreo)}</p>
              </div>
            </section>

            <SalesChart rows={dashboard.detalle} />
          <DailyTable 
  rows={dashboard.detalle} 
  onEdit={editarVenta}
  onDelete={borrarVenta}
/>
          </>
        )}
      </div>
      </div>
   );
}