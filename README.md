# Conference-Booking-Room-Domain
C# domain model for a Conference Room Booking System, focusing on business rules, validation, and clean design for future API development.

## Table of Contents
1. [Project Overview](#project-overview)  
2. [What This Project Is About](#what-this-project-is-about)  
3. [Domain Concepts](#domain-concepts)  
4. [How the System Works](#how-the-system-works)  
5. [Design Decisions](#design-decisions)  
6. [Design Justifications](#design-justifications)
6. [Error Handling and Validation](#error-handling-and-validation)  
7. [Console Application Usage](#console-application-usage)  
8. [Installation and Setup](#installation-and-setup)   
9. [Future Improvements](#future-improvements)  
10. [Author](#author)  

---

## Project Overview

This project is a **C# domain model** for a Conference Room Booking System.  
The goal of this assignment is to model the **core business rules** of the system, not to build a full application.

The domain model will be reused later when building APIs and other system parts.

---

## What This Project Is About

The system models how conference rooms are booked in real life.  
It focuses on:
- Room size and capacity
- Booking time rules
- Booking status changes
- Preventing invalid bookings

A console application is used **only to test and demonstrate** the domain logic.

---

## Domain Concepts

### ConferenceRoom
Represents a physical conference room.

It:
- Has a name
- Has a room type (Standard, Training, Boardroom)
- Determines its capacity 
- Defines its Location
- Show its availability status using isActive flag

---

### Booking
Represents a booking request.

It:
- Stores who made the booking
- Stores start and end time
- Stores number of attendees
- Controls booking status (Pending, Confirmed, Cancelled)

---

### BookingStatus (Enum)
Defines valid booking states:
- Pending
- Confirmed
- Cancelled

Using an enum prevents invalid booking states.

---

### RoomType (Enum)
Defines room size categories:
- Standard
- Training
- BoardRoom

Room type is used to determine room capacity.

---

### BookingRequest (Record)
Used to group booking input values together.  
It improves readability and keeps method calls clean.

---

## How the System Works

1. The user enters booking details (name, time, attendees)
2. The system suggests an appropriate room type based on attendees
3. A booking is created with validation
4. The room checks if it can accommodate the booking
5. The booking is approved to demonstrate behaviour

---

## Design Decisions

- Booking status can only be changed using methods
- Business rules are enforced inside domain classes
- `Program.cs` contains no business logic

This keeps the domain model clean and realistic.

---

## Design Justifications
### ConferenceRoom
- **Location (non‑nullable)**: Every room must have a physical location. Making this field non‑nullable enforces realism — a room without a location would be meaningless in practice.

- **IsActive (non‑nullable, default = true)**: Rooms are assumed available unless marked otherwise. Defaulting to `true` reflects the common case (most rooms are active), while allowing administrators to deactivate rooms when needed.

### Booking
- **CreatedAt (non‑nullable, default = DateTime.UtcNow)**: Every booking must have a creation timestamp. Defaulting to the current time ensures auditability and transparency without requiring manual input.  

- **CancelledAt (nullable)**: Not all bookings are cancelled. Making this field nullable avoids forcing a value when a booking is still active. It only stores a timestamp when cancellation occurs.  

- **Delete only if cancelled**: This rule depends on `CancelledAt` being nullable — if the field is null, the booking is active and cannot be deleted.

### Session
- **Title (non‑nullable)**: Every session must have a name to identify the event.  

- **Capacity (non‑nullable)**: A session must define how many attendees it can accommodate. 

- **StartTime / EndTime (non‑nullable)**: Sessions cannot exist without defined times. These fields are required to enforce scheduling.

- **Seeded defaults**: One example session is seeded with realistic values (title, capacity, start/end times) to demonstrate functionality without requiring user input.

---

### Why These Choices Matter
- **Non‑nullable fields** enforce business rules and prevent invalid states (e.g., a booking without a start time).  

- **Nullable fields** are used only when the value is optional or conditional (e.g., `CancelledAt` only exists if a booking is cancelled)
.  
- **Defaults** simplify usage and reflect real‑world expectations (e.g., rooms are active by default, bookings are created with the current timestamp).  

---


## Error Handling and Validation

The system prevents invalid states by checking:
- Booking start time is not in the past
- End time is after start time
- Number of attendees is greater than zero
- Room capacity is not exceeded
- Booking status changes are valid

Exceptions are used to stop invalid operations.

---



## Installation and Setup

### Requirements
- .NET SDK 8

### Steps
1. Download and install **.NET SDK 8**
2. Clone the repo
3. Change directory to the repo
4. Navigate to the API project folder:
   ```bash
  cd Conference-Booking-Room-Domain/API
5. run the API:
```bash
  dotnet run
```
6. Open Swagger UI in your browser
 - eg http://localhost:5247/swagger

## Web API USAGE
- Example Request and Response

### Create a Booking(Request)
 
POST /api/bookings
Content-Type: application/json

{
  "room": {
    "id": 1,
    "name": "Room A",
    "roomType": "Standard",
    "capacity": 10
  },
  "start": "2026-02-03T22:23:42",
  "endTime": "2026-02-03T23:23:42"
}

### Response(200 ok)
{
  "room": {
    "id": 1,
    "name": "Room A",
    "roomType": "Standard",
    "capacity": 10
  },
  "start": "2026-02-03T22:23:42",
  "endTime": "2026-02-03T23:23:42",
  "status": "Confirmed"
}
## Created by
- Onthangaho Magoro
