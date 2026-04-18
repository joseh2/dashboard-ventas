import { useState } from "react";

export default function SalesForm({ anio, mes, onSave }) {
    const fechaInicial = `${anio}-${String(mes).padStart(2, "0")}-01`;
    const [fecha, setFecha] = useState(fechaInicial);
    const [monto, setMonto] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        await onSave({ fecha, monto: Number(monto) });
        setMonto("");
    };

    return (
        <form className="card form-card" onSubmit={handleSubmit}>
            <h3>Ingresar venta del día</h3>
            <input
                type="date"
                value={fecha}
                onChange={(e) => setFecha(e.target.value)}
                required
            />
            <input
                type="number"
                step="0.01"
                value={monto}
                onChange={(e) => setMonto(e.target.value)}
                placeholder="Ej. 2733.25"
                required
            />
            <button type="submit">Guardar venta</button>
        </form>
    );
}