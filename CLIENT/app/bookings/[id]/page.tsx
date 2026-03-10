import { BookingDetailPageClient } from "../../../src/components/BookingDetailPageClient";

export default function BookingDetailPage({ params }) {
  return <BookingDetailPageClient bookingId={params.id} />;
}
