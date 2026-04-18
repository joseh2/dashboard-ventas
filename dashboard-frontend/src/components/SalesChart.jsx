import {
    CartesianGrid,
    Legend,
    Line,
    LineChart,
    ResponsiveContainer,
    Tooltip,
    XAxis,
    YAxis,
} from "recharts";

export default function SalesChart({ rows }) {
    const data = rows.map((row) => ({
        dia: row.dia,
        ventaAcumulada: row.ventaAcumulada,
        metaAcumulada: row.metaAcumulada,
    }));

    return (
        <div className="card">
            <h3>Ventas acumuladas vs meta acumulada</h3>
            <div style={{ width: "100%", height: 320 }}>
                <ResponsiveContainer>
                    <LineChart data={data}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="dia" />
                        <YAxis />
                        <Tooltip />
                        <Legend />
                        <Line
                            type="monotone"
                            dataKey="ventaAcumulada"
                            name="Venta acumulada"
                            stroke="#2563eb"
                            strokeWidth={3}
                        />
                        <Line
                            type="monotone"
                            dataKey="metaAcumulada"
                            name="Meta acumulada"
                            stroke="#f59e0b"
                            strokeWidth={3}
                        />
                    </LineChart>
                </ResponsiveContainer>
            </div>
        </div>
    );
}