# Conference Booking Room Domain

Full-stack conference room booking system with:
- ASP.NET Core Web API + PostgreSQL + Identity/JWT
- Vite + React frontend
- Role-based access (Admin, Employee, Receptionist)
- Real-time booking sync via SignalR
- Global authentication via React Context API

## Features

### Authentication & Authorization
- Login endpoint issues JWT token (`/api/Auth/login`)
- Global `AuthContext` stores `token`, `role`, `username`, `isAuthenticated`, `loading`
- Auth state hydrated from `localStorage` on app load — session survives page refresh
- `useAuth()` custom hook exposes auth state and actions to any component
- `AuthGuard` component protects private routes — redirects guests to `/login`
- Axios request interceptor auto-attaches `Authorization: Bearer <token>`
- Axios response interceptor handles `401` by calling `logout()` from context
- Role-protected endpoints (`[Authorize(Roles = ...)]`)

### Booking Mutations (POST/PUT)
- Create booking: `POST /api/Bookings`
- Update booking: `PUT /api/Bookings/{id}` (Employee/Receptionist, owner-only)
- Cancel booking: `PUT /api/Bookings/{id}/cancel`
- Delete booking (admin rule): `DELETE /api/Bookings/{id}`
- UI uses pessimistic/local state update after successful mutation

### Validation Handshake
- Backend enforces fail-fast rules (invalid time, overlap, invalid room, etc.)
- Invalid requests return `400` with `ValidationProblemDetails` for field-level errors
- Frontend parses and maps validation errors to form inputs
- Inline field messages shown under inputs

### Business Rules
- End time must be after start time
- No overlap in same room for confirmed bookings
- Booking must reference an existing active room
- Employee can only edit their own booking
- Cancelled bookings cannot be edited

### Dashboard UX
- Role-aware dashboard
- Admin sees all active bookings
- Employee/Receptionist sees own active bookings + booking form
- Search + room type filter + sort + pagination
- Persisted dashboard preferences in `localStorage`
- Reset preferences button

### Dynamic Navigation
- Sidebar shows `Home` + `Login` for guests; `Dashboard` only when authenticated
- Header toggles between `Login` link (guest) and `Welcome, <name>` + `Logout` button (authenticated)

### Real-Time Sync (SignalR)
- Hub endpoint: `/hubs/bookings`
- Broadcasts on create/update/cancel/delete
- Group-targeted delivery:
  - All admins (`role:admin`)
  - Booking owner (`user:{username}`)
- React listener updates booking list live across tabs
- Cleanup on unmount (`off` + `stop`) to avoid memory leaks

## Tech Stack
- **Backend:** .NET 8, ASP.NET Core, EF Core, PostgreSQL, Identity, JWT, SignalR
- **Frontend:** Vite, React 19, JavaScript, Axios, React Toastify, SignalR JS client

## Project Structure
```
API/             — ASP.NET Core API (auth, controllers, middleware, SignalR hub)
Domain/          — Domain entities and business logic
CLIENT/          — Active Vite/React frontend
  src/
    api/         — Axios client with interceptors
    contexts/    — AuthContext (global auth state)
    hooks/       — useAuth, useBookings, useRooms
    components/  — Dashboard, booking UI, error boundary, skeleton states
    services/    — API service functions
    styles/      — Global CSS
booking-next/    — Separate Next.js experiment/reference folder
```

## Setup

### Prerequisites
- .NET SDK 8
- Node.js 18+
- PostgreSQL

### 1) Run API
```bash
cd API
dotnet restore
dotnet run
```
API runs at `http://localhost:5248`  
Swagger (dev): `http://localhost:5248/swagger`

### 2) Run Client
```bash
cd CLIENT
npm install
npm run dev
```
Client runs at `http://localhost:5173`

Create `CLIENT/.env.local`:
```
VITE_API_BASE_URL=http://localhost:5248/api
```

You can copy the default from `CLIENT/.env.example`.

## Module Polish Phase

The current codebase now includes the missing performance and resiliency improvements requested for the polish assignment:

- Debounced booking search from the React client to the .NET API, instead of filtering only in-memory.
- Stable derived dashboard state with `useMemo` for filtering, sorting, and pagination.
- Loading skeletons so the dashboard does not flash empty content while data is loading.
- A resettable React error boundary so a rendering failure does not force a full browser refresh.
- Memoized booking list/card rendering to reduce unnecessary child re-renders.
- Retryable load errors for bookings and visible room-load failures in the booking form.

### Notes for Demonstration

- The active app being demonstrated is the Vite React client in `CLIENT/`.
- The `booking-next/` folder exists in the repo, but it is not the active frontend used by the current booking flow.
- React DevTools can show extra renders in development because Strict Mode intentionally re-runs parts of the render lifecycle. What matters here is that the expensive dashboard derivations are memoized and the search requests are debounced before hitting the API.

## Test Accounts
| Role | Username | Password |
|---|---|---|
| Admin | admin | Admin@123 |
| Employee | employee | Employee@123 |
| Receptionist | receptionist | Receptionist@123 |

## Author
Onthangaho Magoro
