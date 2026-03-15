export default function DashboardLoading() {
  return (
    <main className="container">
      <section className="dashboard-header">
        <h2>Conference Booking Dashboard</h2>
      </section>

      <section className="booking-skeleton-list" aria-hidden="true">
        {Array.from({ length: 6 }, (_, index) => (
          <article key={`dashboard-loading-skeleton-${index}`} className="booking-skeleton-card">
            <div className="booking-skeleton-line title" />
            <div className="booking-skeleton-line" />
            <div className="booking-skeleton-line" />
            <div className="booking-skeleton-line short" />
          </article>
        ))}
      </section>
    </main>
  );
}
