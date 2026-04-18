import { useState } from "react";

export default function MetaForm({ anio, mes, onSave }) {
    const [meta, setMeta] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        await onSave({ anio, mes, meta: Number(meta) });
        setMeta("");
    };

    return (
        <form className="card form-card" onSubmit={handleSubmit}>
            <h3>Guardar meta mensual</h3>
            <input
                type="number"
                step="0.01"
                value={meta}
                onChange={(e) => setMeta(e.target.value)}
                placeholder="Ej. 81000"
                required
            />
            <button type="submit">Guardar meta</button>
        </form>
    );
}
