# Authorization Notes

## Why authorization should not live in controllers
Authorization logic embedded directly in controllers couples business rules to request handling. This makes controllers bloated, harder to test, and forces duplication across endpoints. By centralizing authorization in middleware or policies, controllers remain focused on orchestration and input/output, while access rules are enforced consistently across the system.

## Why roles belong in tokens
Roles must be embedded in JWT tokens so that every request carries the user’s identity and permissions. This avoids repeated database lookups, ensures stateless authentication, and allows downstream services to enforce role-based rules without needing direct access to the identity store.

## How this design prepares the system
- **Database relationships**: With roles and user IDs in tokens, bookings can be tied to specific users in relational tables. Ownership checks become simple comparisons between token claims and stored records.
- **Booking ownership**: The `ClaimTypes.Name` or `NameIdentifier` claim allows the API to record who created a booking. This enables fine-grained authorization (e.g., only the creator or an Admin can cancel).
- **Frontend integration**: Tokens containing roles let the frontend adapt dynamically — showing or hiding UI elements based on the user’s permissions, without extra API calls. This improves responsiveness and reduces complexity.

---

By keeping authorization out of controllers and embedding roles in tokens, the system remains modular, scalable, and ready for integration with both relational data and modern frontend applications.