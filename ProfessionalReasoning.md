# — Professional Reasoning (Assignment 3.2)

## 1. Why is removing a column more dangerous than adding one?
Removing a column is more dangerous because it can lead to **data loss** and break existing queries, reports, or applications that depend on that column. Once removed, the historical data stored in that column is gone unless it was backed up. Adding a column, by contrast, is non‑destructive — it simply extends the schema without affecting existing data.

## 2. Why are migrations preferred over manual SQL changes?
Migrations are preferred because they provide a **versioned, repeatable, and trackable** way to evolve the database schema. They ensure that every developer and environment applies the same changes consistently. Manual SQL changes are error‑prone, harder to reproduce, and can lead to environments being out of sync.

## 3. What could go wrong if two developers modify the schema without migrations?
If two developers make manual schema changes without migrations, they risk **conflicting changes** and **inconsistent environments**. For example, one developer might rename a column while another adds constraints to it. Without migrations, these changes can overwrite each other, cause runtime errors, or make the database unreproducible across environments.

## 4. Which of your schema changes would be risky in production, and why?
The most risky change would be **removing or altering existing columns** such as `CancelledAt` or `IsActive`. In production, these fields may already store important business data. Dropping or changing them could break reports, invalidate historical records, or cause loss of audit information. Adding new fields like `CreatedAt` or seeding sessions is less risky because they extend the schema without destroying existing data.