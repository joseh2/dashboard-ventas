import { useState } from "react";
import "../estilo.css";

const money = new Intl.NumberFormat("es-SV", {
    style: "currency",
    currency: "USD",
});

export default function DailyTable({ rows, onEdit, onDelete }) {
    const [editId, setEditId] = useState(null);
    const [nuevoMonto, setNuevoMonto] = useState("");

    const iniciarEdicion = (row) => {
        setEditId(row.id);
        setNuevoMonto(row.ventaDia);
    };

    const guardarEnter = async (e, row) => {
        if (e.key !== "Enter") return;

        await onEdit(row.id, {
            fecha: row.fecha,
            monto: Number(nuevoMonto),
        });

        alert("Venta modificada correctamente");
        setEditId(null);
        setNuevoMonto("");
    };

    return (
        <div className="card table-card">
            <h3>Detalle diario</h3>
            <div className="table-wrapper">
                <table>
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Fecha</th>
                            <th>Dato/día</th>
                            <th>Venta total</th>
                            <th>Meta/día</th>
                            <th>%</th>
                            <th>Diferencia</th>
                            <th>Acción</th>
                        </tr>
                    </thead>

                    <tbody>
                        {rows.map((row) => (
                            <tr key={row.id || row.dia}>
                                <td>{row.dia}</td>
                                <td>{new Date(row.fecha).toLocaleDateString("es-SV")}</td>

                                <td>
                                    {editId === row.id ? (
                                        <input
                                            type="number"
                                            step="0.01"
                                            value={nuevoMonto}
                                            autoFocus
                                            onChange={(e) => setNuevoMonto(e.target.value)}
                                            onKeyDown={(e) => guardarEnter(e, row)}
                                        />
                                    ) : (
                                        money.format(row.ventaDia)
                                    )}
                                </td>

                                <td>{money.format(row.ventaAcumulada)}</td>
                                <td>{money.format(row.metaAcumulada)}</td>
                                <td>{row.porcentajeDia.toFixed(2)}%</td>

                                <td className={row.diferenciaDia >= 0 ? "positive" : "negative"}>
                                    {money.format(row.diferenciaDia)}
                                </td>

                                <td>
                                    <button type="button" onClick={() => iniciarEdicion(row)}>
                                        ✏️
                                    </button>

                                    <button
                                        type="button"
                                        onClick={async () => {
                                            if (confirm("¿Seguro que deseas borrar esta venta?")) {
                                                await onDelete(row.id);
                                                alert("Venta borrada correctamente");
                                            }
                                        }}
                                    >
                                        🗑️
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            {editId && <p>Presiona Enter para guardar el cambio.</p>}
        </div>
    );
}