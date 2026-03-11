import { BookingDetailPageClient } from "../../../src/components/BookingDetailPageClient";

// This is a server component that simply renders the client component. We do this to keep the client-side code separate and only load it when needed.
export default function BookingDetailPage({ params }) {
  return <BookingDetailPageClient bookingId={params.id} />;
}
