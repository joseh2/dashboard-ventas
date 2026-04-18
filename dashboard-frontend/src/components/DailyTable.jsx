import "../estilo.css";
const money = new Intl.NumberFormat("es-SV", {
    style: "currency",
    currency: "USD",
});

export default function DailyTable({ rows }) {
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
                        </tr>
                    </thead>
                    <tbody>
                        {rows.map((row) => (
                            <tr key={row.dia}>
                                <td>{row.dia}</td>
                                <td>{new Date(row.fecha).toLocaleDateString("es-SV")}</td>
                                <td>{money.format(row.ventaDia)}</td>
                                <td>{money.format(row.ventaAcumulada)}</td>
                                <td>{money.format(row.metaAcumulada)}</td>
                                <td>{row.porcentajeDia.toFixed(2)}%</td>
                                <td className={row.diferenciaDia >= 0 ? "positive" : "negative"}>
                                    {money.format(row.diferenciaDia)}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}