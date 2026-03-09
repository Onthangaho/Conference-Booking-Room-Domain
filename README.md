# Conference Booking Room Domain

Full-stack conference room booking system with:
- ASP.NET Core Web API + PostgreSQL + Identity/JWT
- React + Vite frontend
- Role-based access (Admin, Employee, Receptionist)
- Real-time booking sync via SignalR

## Features

### Authentication & Authorization
- Login endpoint issues JWT token (`/api/Auth/login`)
- Frontend stores token/role in `localStorage`
- Axios request interceptor auto-attaches `Authorization: Bearer <token>`
- Axios response interceptor handles `401` globally and clears auth state
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
- Inline field messages are shown under inputs

### Business Rules
- End time must be after start time
- No overlap in same room for confirmed bookings
- Booking must reference existing room
- Room must be active for booking
- Employee can only edit own booking
- Cancelled bookings cannot be edited
- Deleting booking is restricted by backend rule flow

### Dashboard UX
- Role-aware dashboard (`Logged in as ...`)
- Admin sees all active bookings
- Employee/Receptionist sees own active bookings + booking form
- Search + room type filter + sort + pagination
- Persisted dashboard preferences in `localStorage`
- Reset preferences button

### Real-Time Sync (SignalR)
- Hub endpoint: `/hubs/bookings`
- Broadcasts on create/update/cancel/delete
- Group-targeted delivery:
  - all admins (`role:admin`)
  - booking owner (`user:{username}`)
- React listener updates booking list live across tabs
- Cleanup included (`off` + `stop`) to avoid memory leaks

## Tech Stack
- Backend: .NET 8, ASP.NET Core, EF Core, PostgreSQL, Identity, JWT, SignalR
- Frontend: React 19, Vite, Axios, React Toastify, SignalR JS client

## Project Structure
- `API/` — ASP.NET Core API, auth, controllers, middleware, SignalR hub
- `Domain/` — domain entities and business logic
- `CLIENT/` — React frontend

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

API swagger (dev):
- `http://localhost:5247/swagger`

### 2) Run Client
```bash
cd CLIENT
npm install
npm run dev
```

Client default:
- `http://localhost:5173`

## Important Notes
- All client network calls use the preconfigured Axios singleton (no native fetch).
- Booking and auth logic are implemented in custom hooks (`useBookings`, `useAuth`).
- State updates follow immutable patterns.

## Author
- Onthangaho Magoro
