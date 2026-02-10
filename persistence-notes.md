# Persistence Notes

### Why In-Memory Storage Is Not Suitable for Production
In-memory storage (lists or JSON files) is volatile: data is lost when the application restarts, and it cannot scale to handle concurrent users or large datasets. It also lacks transactional guarantees, making it unsafe for critical business operations.

### What DbContext Represents
`DbContext` is the central class in Entity Framework Core that manages database connections and tracks changes to entities. It acts as a bridge between your domain models (`Booking`, `ConferenceRoom`) and the underlying database, enabling queries and persistence.

### How EF Core Fits Into the Architecture
EF Core provides the persistence layer of the system.  
- **Domain**: contains business rules and entities.  
- **Persistence**: uses EF Core and `DbContext` to store and retrieve domain entities.  
- **API**: wires everything together and exposes endpoints.  

This separation ensures the domain logic remains independent of infrastructure concerns.

### Preparing the System for Growth
Using EF Core and `DbContext` prepares the system for:
- **Relationships**: bookings linked to rooms, users, and roles.  
- **Ownership**: enforcing rules like “a booking belongs to a user.”  
- **Frontend Usage**: consistent APIs backed by a reliable database, enabling web or mobile clients to interact with persistent data.  

By adopting EF Core, the system is ready to evolve beyond simple demos into a production-ready application.