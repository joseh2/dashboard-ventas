export default function SummaryCard({ title, value, accent = "#1f2937" }) {
  return (
    <div className="card summary-card" style={{ borderLeft: `6px solid ${accent}` }}>
      <div className="summary-title">{title}</div>
      <div className="summary-value">{value}</div>
    </div>
  );
}