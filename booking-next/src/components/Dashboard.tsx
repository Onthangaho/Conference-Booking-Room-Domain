"use client";

import Heading from "./Heading";
import BookingListClient from "./BookingListClient";
import BookingForm from "./BookingForm";
import { useBookings } from "../hooks/useBookings";
import { useCallback, useEffect, useMemo, useState } from "react";

const PAGE_SIZE_OPTIONS = [6, 12, 24];

function Dashboard({ role }) {
  const normalizedRole = role?.toLowerCase(); // normalize role
  const [searchTerm, setSearchTerm] = useState(() => localStorage.getItem("dashboard.searchTerm") || "");
  const { bookings, loading, loadError, errors, addBooking, editBooking, deleteBooking, retryLoad } = useBookings(normalizedRole, searchTerm);
  const [roomTypeFilter, setRoomTypeFilter] = useState(() => localStorage.getItem("dashboard.roomTypeFilter") || "all");
  const [sortBy, setSortBy] = useState(() => localStorage.getItem("dashboard.sortBy") || "startAsc");
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(() => {
    const saved = Number(localStorage.getItem("dashboard.pageSize"));
    return PAGE_SIZE_OPTIONS.includes(saved) ? saved : 6;
  });

  const isEmployee = normalizedRole === "employee" || normalizedRole === "receptionist";

  const activeBookings = useMemo(
    () => (Array.isArray(bookings) ? bookings.filter((booking) => !booking.isCancelled && booking.status !== "Cancelled") : []),
    [bookings]
  );

  const roomTypes = useMemo(
    () => [...new Set(activeBookings.map((booking) => booking.roomType).filter(Boolean))],
    [activeBookings]
  );

  const filteredBookings = useMemo(() => {
    return activeBookings.filter((booking) => {
      const matchesRoomType = roomTypeFilter === "all" || booking.roomType === roomTypeFilter;

      return matchesRoomType;
    });
  }, [activeBookings, roomTypeFilter]);

  const sortedBookings = useMemo(() => {
    const sorted = [...filteredBookings];

    sorted.sort((a, b) => {
      if (sortBy === "startAsc") return new Date(a.start).getTime() - new Date(b.start).getTime();
      if (sortBy === "startDesc") return new Date(b.start).getTime() - new Date(a.start).getTime();
      if (sortBy === "roomAsc") return (a.roomName || "").localeCompare(b.roomName || "");
      if (sortBy === "roomDesc") return (b.roomName || "").localeCompare(a.roomName || "");
      if (sortBy === "createdBy") return (a.createdBy || "").localeCompare(b.createdBy || "");
      return 0;
    });

    return sorted;
  }, [filteredBookings, sortBy]);

  const totalPages = Math.max(1, Math.ceil(sortedBookings.length / pageSize));
  const currentPageSafe = Math.min(currentPage, totalPages);

  const pagedBookings = useMemo(() => {
    const startIndex = (currentPageSafe - 1) * pageSize;
    return sortedBookings.slice(startIndex, startIndex + pageSize);
  }, [sortedBookings, currentPageSafe, pageSize]);

  const handleSearchChange = useCallback((value) => {
    setSearchTerm(value);
    setCurrentPage(1);
  }, []);

  const handleRoomTypeChange = useCallback((value) => {
    setRoomTypeFilter(value);
    setCurrentPage(1);
  }, []);

  const handleSortChange = useCallback((value) => {
    setSortBy(value);
    setCurrentPage(1);
  }, []);

  const handlePageSizeChange = useCallback((value) => {
    setPageSize(Number(value));
    setCurrentPage(1);
  }, []);

  const resetDashboardPreferences = useCallback(() => {
    localStorage.removeItem("dashboard.searchTerm");
    localStorage.removeItem("dashboard.roomTypeFilter");
    localStorage.removeItem("dashboard.sortBy");
    localStorage.removeItem("dashboard.pageSize");

    setSearchTerm("");
    setRoomTypeFilter("all");
    setSortBy("startAsc");
    setPageSize(6);
    setCurrentPage(1);
  }, []);

  useEffect(() => {
    localStorage.setItem("dashboard.sortBy", sortBy);
  }, [sortBy]);

  useEffect(() => {
    localStorage.setItem("dashboard.pageSize", String(pageSize));
  }, [pageSize]);

  useEffect(() => {
    localStorage.setItem("dashboard.searchTerm", searchTerm);
  }, [searchTerm]);

  useEffect(() => {
    localStorage.setItem("dashboard.roomTypeFilter", roomTypeFilter);
  }, [roomTypeFilter]);

  useEffect(() => {
    if (roomTypeFilter !== "all" && !roomTypes.includes(roomTypeFilter)) {
      setRoomTypeFilter("all");
    }
  }, [roomTypeFilter, roomTypes]);

  const pageNumbers = useMemo(() => {
    if (totalPages <= 7) {
      return Array.from({ length: totalPages }, (_, index) => index + 1);
    }

    const pages: (number | string)[] = [1];
    const start = Math.max(2, currentPageSafe - 1);
    const end = Math.min(totalPages - 1, currentPageSafe + 1);

    if (start > 2) pages.push("...");
    for (let i = start; i <= end; i += 1) pages.push(i);
    if (end < totalPages - 1) pages.push("...");
    pages.push(totalPages);

    return pages;
  }, [totalPages, currentPageSafe]);

  if (loadError) {
    return (
      <main className="container">
        <section className="error-state" role="alert">
          <h3>Could not load bookings</h3>
          <p>{loadError}</p>
          <button className="btn primary" type="button" onClick={retryLoad}>Try Again</button>
        </section>
      </main>
    );
  }

  if (loading) {
    return (
      <main className="container">
        <section className="booking-skeleton-list" aria-hidden="true">
          {Array.from({ length: 4 }, (_, index) => (
            <article key={`booking-skeleton-${index}`} className="booking-skeleton-card">
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

  return (
    <main className="container">
      <section className="dashboard-header">
        <Heading title="Conference Booking Dashboard" />
        <p className="role-indicator">Logged in as: <strong>{normalizedRole || "guest"}</strong></p>
        <div className="stats-grid">
          <div className="stat-card">
            <p>Active Bookings</p>
            <h3>{activeBookings.length}</h3>
          </div>
          <div className="stat-card">
            <p>Visible Results</p>
            <h3>{sortedBookings.length}</h3>
          </div>
          <div className="stat-card">
            <p>Page</p>
            <h3>{currentPageSafe}/{totalPages}</h3>
          </div>
        </div>
      </section>

      <section className="booking-tools">
        <input
          type="text"
          placeholder="Search by room, status, or user..."
          value={searchTerm}
          onChange={(e) => handleSearchChange(e.target.value)}
        />

        <select value={roomTypeFilter} onChange={(e) => handleRoomTypeChange(e.target.value)}>
          <option value="all">All room types</option>
          {roomTypes.map((roomType) => (
            <option key={roomType} value={roomType}>
              {roomType}
            </option>
          ))}
        </select>

        <select value={sortBy} onChange={(e) => handleSortChange(e.target.value)}>
          <option value="startAsc">Start Time (Oldest First)</option>
          <option value="startDesc">Start Time (Newest First)</option>
          <option value="roomAsc">Room Name (A-Z)</option>
          <option value="roomDesc">Room Name (Z-A)</option>
          <option value="createdBy">Created By (A-Z)</option>
        </select>

        <select value={pageSize} onChange={(e) => handlePageSizeChange(e.target.value)}>
          {PAGE_SIZE_OPTIONS.map((size) => (
            <option key={size} value={size}>
              {size} per page
            </option>
          ))}
        </select>

        <button className="btn danger" onClick={resetDashboardPreferences} type="button">
          Reset Dashboard Preferences
        </button>
      </section>

      {isEmployee && (
        <>
          <Heading title="Create Booking" />
          <BookingForm addBooking={addBooking} errors={errors} />
          <Heading title="My Active Bookings" />
          <BookingListClient
            bookings={pagedBookings}
            role={normalizedRole}
            editBooking={editBooking}
            deleteBooking={deleteBooking}
          />
        </>
      )}
      {normalizedRole === "admin" && (
        <>
          <Heading title="All Active Bookings" />
          <BookingListClient
            bookings={pagedBookings}
            role={normalizedRole}
            deleteBooking={deleteBooking}
            editBooking={editBooking}
          />
        </>
      )}

      {sortedBookings.length > 0 && (
        <section className="pagination-controls">
          <button
            className="btn secondary"
            onClick={() => setCurrentPage((prev) => Math.max(1, prev - 1))}
            disabled={currentPageSafe === 1}
          >
            Previous
          </button>
          <div className="page-numbers">
            {pageNumbers.map((page, index) =>
              page === "..." ? (
                <span key={`ellipsis-${index}`} className="page-ellipsis">...</span>
              ) : (
                <button
                  key={page}
                  className={`btn secondary page-btn ${currentPageSafe === page ? "active" : ""}`}
                  onClick={() => setCurrentPage(page as number)}
                >
                  {page}
                </button>
              )
            )}
          </div>
          <span>Page {currentPageSafe} of {totalPages}</span>
          <button
            className="btn secondary"
            onClick={() => setCurrentPage((prev) => Math.min(totalPages, prev + 1))}
            disabled={currentPageSafe === totalPages}
          >
            Next
          </button>
        </section>
      )}
    </main>
  );
}

export default Dashboard;