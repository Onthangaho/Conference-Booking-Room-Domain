# Conference Booking System – Frontend

This frontend is migrated to **Next.js App Router**.

## Routes
- `/` landing page
- `/login` login page
- `/dashboard` dashboard page
- `/bookings/[id]` dynamic booking detail page

## Architecture Highlights
- Root shell is in `app/layout.tsx` (persistent Header + Sidebar + Footer).
- Route entry points use the App Router `page.tsx` convention.
- Interactivity uses `"use client"` only where needed (forms, hooks, click handlers).
- Navigation uses Next.js `Link` for client-side transitions.
- API base URL uses Next env variable: `NEXT_PUBLIC_API_BASE_URL` from `.env.local`.

## Run
```bash
cd CLIENT
npm install
npm run dev
```

Open: `http://localhost:3000`

## Author
ONTHANGAHO MAGORO
