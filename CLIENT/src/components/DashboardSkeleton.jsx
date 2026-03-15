function DashboardSkeleton() {
  return (
    <main className="container" aria-hidden="true">
      <section className="dashboard-header skeleton-block">
        <div className="skeleton-line skeleton-title" />
        <div className="skeleton-line skeleton-subtitle" />
        <div className="stats-grid">
          <div className="stat-card skeleton-card" />
          <div className="stat-card skeleton-card" />
          <div className="stat-card skeleton-card" />
        </div>
      </section>

      <section className="booking-tools skeleton-tools">
        <div className="skeleton-input" />
        <div className="skeleton-input" />
        <div className="skeleton-input" />
        <div className="skeleton-input" />
      </section>

      <section className="bookings-list">
        {Array.from({ length: 6 }, (_, index) => (
          <article key={index} className="booking-card skeleton-card-large" />
        ))}
      </section>
    </main>
  );
}

export default DashboardSkeleton;