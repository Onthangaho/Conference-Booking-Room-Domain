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

Open: `http://localhost:3000`

## Author
Onthangaho Magoro
