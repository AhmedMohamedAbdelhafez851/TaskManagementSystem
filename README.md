# TaskFlow - Task Management System

ASP.NET WebForms application with Clean Architecture, Repository Pattern, and role-based access control.

---

## Quick Overview

| Feature | Status |
|---------|--------|
| Clean Architecture | ✅ Domain → DAL → BLL → Web |
| Repository Pattern | ✅ |
| Password Hashing | ✅ SHA256 + Salt |
| SQL Injection Protection | ✅ Parameterized Queries |
| Role-Based Access | ✅ Admin / Member |
| File Upload | ✅ PDF, DOC, DOCX, JPG, PNG (Max 5MB) |
| Logging | ✅ FileLogger |
| UI Framework | ✅ Bootstrap 5 |

---


---

## Admin Features

- Dashboard with task statistics
- Create Task (title, description, assignment, file)
- Search Tasks (filter by member + status)
- Edit Task (Title/Description only if Status = New)
- Auto reset Assigned Date when assignment changes

## Member Features

- Dashboard shows only assigned tasks
- Overdue notifications (3+ days, Status = New)
- View Task Details
- Change Status only (no edit)
- Download attachments

---

## Login Credentials

| Role | Username | Password |
|------|----------|----------|
| Admin | `admin` | `Admin@123` |
| Member | `ahmed.ali` | `Member@123` |
| Member | `sara.hassan` | `Member@123` |
| Member | `mohamed.kamal` | `Member@123` |

---

## Tech Stack

- ASP.NET WebForms (.NET 4.7.2)
- C#
- SQL Server
- Bootstrap 5
- Font Awesome 6
- jQuery

---

## How to Run

1. Open `TaskManagementSystem.sln` in Visual Studio 2022
2. Run `DatabaseScript.sql` in SQL Server
3. Update connection string in `Web.config` if needed
4. Build → Rebuild Solution
5. Press F5

---

## Project Structure
