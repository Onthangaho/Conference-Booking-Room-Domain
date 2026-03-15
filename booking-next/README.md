# Conference Booking System – Next.js Frontend

Next.js 15 App Router frontend for the Conference Booking System.

## Routes
| Path | Description |
|---|---|
| `/` | Landing page |
| `/login` | Login page — redirects to dashboard if already authenticated |
| `/dashboard` | Protected dashboard — redirects to login if unauthenticated |
| `/bookings/[id]` | Dynamic booking detail page |

## Architecture

### Global Authentication (React Context)
- `src/contexts/AuthContext.tsx` — stores `token`, `role`, `username`, `isAuthenticated`, `loading`
- State is hydrated from `localStorage` on mount so sessions survive page refresh
- `src/hooks/useAuth.ts` — custom hook for consuming auth state anywhere without prop drilling
- `app/providers.tsx` — client boundary that wraps children with `AuthProvider` and `ToastContainer`
- `app/layout.tsx` — root layout stays a Server Component; delegates client state to `providers.tsx`

### Route Protection
- `src/components/AuthGuard.tsx` — reusable wrapper that redirects unauthenticated users to `/login` using `next/navigation`
- `DashboardPageClient` wraps its content in `<AuthGuard>` to enforce protection

### API Client
- `src/api/apiClient.ts` — Axios instance with:
  - Request interceptor: attaches `Authorization: Bearer <token>` from localStorage
  - Response interceptor: on `401`, calls registered `logout()` from AuthContext (no `window.location.href`)

### Shell Layout
- `app/shared/ShellHeader.tsx` — shows `Login` for guests; `Welcome, <name>` + `Logout` for authenticated users
- `app/shared/ShellSidebar.tsx` — shows `Home` + `Login` for guests; `Dashboard` only when authenticated
- `app/shared/ShellFooter.tsx` — persistent footer

### Component Split Convention
- `*PageClient` components handle route concerns (auth guard, navigation, route params)
- Base components (`Dashboard`, `BookingListClient`, etc.) handle pure UI rendering

### Real-Time Updates
- `useBookings` hook connects to SignalR hub for live booking updates across tabs

## Getting Started

```bash
npm install
npm run dev
```

Create `.env.local`:
```
NEXT_PUBLIC_API_BASE_URL=http://localhost:5248/api
```

For production, set `NEXT_PUBLIC_API_BASE_URL` in your hosting platform environment settings (do not commit production URLs into source control).

## Assignment 2.4 Polish Notes

### Performance Optimizations Applied
- `Dashboard` uses `useMemo` for active bookings, room type options, room/date sorting, pagination, and page number generation.
- `BookingListClient` is wrapped in `memo` and receives stable callbacks via `useCallback` from `useBookings` and `Dashboard`.
- Sorting logic runs only when booking data or sort option changes; typing in search does not trigger re-sorting until the debounced API response updates data.

### Debounced Search Strategy
- Search input triggers API filtering with a 400ms debounce via `useDebouncedValue`.
- Requests are sent through Axios singleton (`apiClient`) and include `searchTerm` query param.
- This reduces request flood while keeping the UI responsive.

### Resilient UI
- Added `app/dashboard/loading.tsx` with booking-card skeleton placeholders.
- Added `app/dashboard/error.tsx` route-level error boundary with `Reset` action.
- Added dashboard retry UI for recoverable API outages without full browser refresh.

### Full Request Traceability
1. Next.js page (`app/dashboard/page.tsx`) renders client dashboard.
2. Client hook (`useBookings`) calls service functions in `src/services/api.ts`.
3. Axios interceptors (`src/api/apiClient.ts`) attach JWT and handle 401 globally.
4. .NET endpoint (`API/Controllers/BookingsController.cs`) validates and queries EF Core.
5. EF Core persists and reads PostgreSQL through `ConferenceBookingDbContext`.

### Bottleneck Audit (React DevTools)
- Identified repeated list rerenders during dashboard interactions (search typing and control changes).
- Resolved by stabilizing prop references (`useCallback`, `memo`) and isolating expensive computations with `useMemo`.

Open: `http://localhost:3000`

## CI Verification

Run a strict local CI-style check before submission:

```bash
npm run ci:verify
```

This command runs strict linting (warnings fail the run) and then production build.

## Author
Onthangaho Magoro
