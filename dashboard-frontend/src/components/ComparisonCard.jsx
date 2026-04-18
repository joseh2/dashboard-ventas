const money = new Intl.NumberFormat("es-SV", {
    style: "currency",
    currency: "USD",
});

export default function ComparisonCard({ title, data }) {
    const color = data.diferencia >= 0 ? "#15803d" : "#b91c1c";

    return (
        <div className="card">
            <h3>{title}</h3>
            <p><strong>Venta acumulada:</strong> {money.format(data.ventaAcumulada || 0)}</p>
            <p><strong>Porcentaje:</strong> {(data.porcentaje || 0).toFixed(2)}%</p>
            <p style={{ color }}><strong>Diferencia:</strong> {money.format(data.diferencia || 0)}</p>
        </div>
    );
}