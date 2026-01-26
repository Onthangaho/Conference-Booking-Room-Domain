# Conference-Booking-Room-Domain
C# domain model for a Conference Room Booking System, focusing on business rules, validation, and clean design for future API development.

## Table of Contents
1. [Project Overview](#project-overview)  
2. [What This Project Is About](#what-this-project-is-about)  
3. [Domain Concepts](#domain-concepts)  
4. [How the System Works](#how-the-system-works)  
5. [Design Decisions](#design-decisions)  
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
- Has a room type (Small, Medium, Large)
- Determines its capacity based on room type
- Validates whether a booking can fit in the room

---

### Booking
Represents a booking request.

It:
- Stores who made the booking
- Stores start and end time
- Stores number of attendees
- Controls booking status (Pending, Approved, Cancelled)

---

### BookingStatus (Enum)
Defines valid booking states:
- Pending
- Approved
- Rejected
- Cancelled

Using an enum prevents invalid booking states.

---

### RoomType (Enum)
Defines room size categories:
- Small
- Medium
- Large

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

## Error Handling and Validation

The system prevents invalid states by checking:
- Booking start time is not in the past
- End time is after start time
- Number of attendees is greater than zero
- Room capacity is not exceeded
- Booking status changes are valid

Exceptions are used to stop invalid operations.

---

## Console Application Usage

The console application is used only to:
- Create domain objects
- Test validation rules
- Display results


---

## Installation and Setup

### Requirements
- .NET SDK 8

### Steps
1. Download and install **.NET SDK 8**
2. Clone the repo
3. Change directory to the repo
4. Create the console application:
   ```bash
   dotnet new console
5. Enter code . to open VScode
6. run the application:
```bash
  dotnet run
```
## Future Improvents
- Web APIs
- USer authentication
- Booking conflict detection

## Created by
- Onthangaho Magoro
