# Student Registration Web App — Setup & Run Instructions

## Project Repository
- **GitHub:** https://github.com/arjundroid12/student-registration-webapp
- **Framework:** ASP.NET Core 8 MVC + Entity Framework Core + Identity
- **Database:** SQL Server LocalDB

---

## Step 1: Clone & Open

```bash
git clone https://github.com/arjundroid12/student-registration-webapp.git
cd student-registration-webapp
```

Open `StudentRegistrationWebApp.csproj` in Visual Studio 2022.

---

## Step 2: Install NuGet Packages

If packages are missing, run in Package Manager Console (Tools → NuGet Package Manager → Package Manager Console):

```
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 8.0.8
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.8
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.8
Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 8.0.4
```

---

## Step 3: Create Database (EF Core Migrations)

The project uses Code-First migrations. The database auto-creates on first run via `DbInitializer`, but to do it manually:

### Option A: Package Manager Console (Visual Studio)
```
Add-Migration InitialCreate
Update-Database
```

### Option B: .NET CLI (terminal)
```bash
dotnet tool install --global dotnet-ef    # if not installed
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This creates the `StudentRegistrationWebApp` database in LocalDB with:
- **Identity tables:** AspNetUsers, AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetUserTokens, AspNetUserLogins, AspNetRoleClaims
- **Application tables:** Courses, CourseRegistrations

### Seeded Data (auto-inserted on first run):
- **Roles:** "Admin", "Student"
- **Admin account:** Email = `admin@studentapp.com`, Password = `Admin@123`
- **Sample courses:** CS101, CS201, CS301, CS202, CS401

---

## Step 4: Run the Application

Press **F5** (debug) or **Ctrl+F5** (run without debugging) in Visual Studio.

Or via CLI:
```bash
dotnet run
```

The app launches at `https://localhost:7045` or `http://localhost:5045`.

---

## Step 5: Test All Features

### 5.1 Admin Flow
1. Login with `admin@studentapp.com` / `Admin@123`
2. Verify navbar shows: **Home, Courses, Students, Logout**
3. Verify email `admin@studentapp.com` shows in navbar
4. Go to **Courses** → Click **"+ Add New Course"** → Create a course
5. Go to **Courses** → Click **Edit** on a course → Modify it → Save
6. Go to **Courses** → Click **Delete** on a course → Confirm deletion
7. Go to **Students** → View list of all registered students with their courses

### 5.2 Student Flow
1. **Logout** from admin
2. Click **Register** → Fill: Full Name, Email, Password, Phone → Submit
3. Verify welcome message: "Welcome, [Name]! Your account has been created successfully. You are registered as a Student."
4. Verify navbar shows: **Home, Courses, Register for Course, My Profile, Logout**
5. Verify email shows in navbar
6. Go to **Courses** → Verify "Register" button appears next to each course (NOT Edit/Delete)
7. Click **Register** on a course → Verify success message
8. Go to **My Profile** → Verify it shows registered course name
9. Go to **Register for Course** again → Verify it redirects to profile (already registered)
10. Click **Edit Profile** → Change name/phone/address → Save → Verify update
11. **Logout**

### 5.3 Anonymous Flow
1. Verify navbar shows: **Home, Courses, Register, Login** (no Profile, no Students, no Logout)
2. Go to **Courses** → Verify list is visible (no Register/Edit/Delete buttons)
3. Try accessing `/Profile` directly in URL → Should redirect to Login page
4. Try accessing `/Students` directly in URL → Should redirect to Login page
5. Try accessing `/Courses/Create` directly → Should redirect to Login page

### 5.4 Access Denied Test
1. Login as **Student**
2. Try accessing `/Students` by typing URL directly → Should show **Access Denied** page
3. Try accessing `/Courses/Create` by typing URL → Should show **Access Denied** page (or redirect to login)

---

## Step 6: Take Screenshots for Submission

Take these 7 screenshots:

1. **Administrator Navigation** — Login as admin, screenshot the navbar
2. **Student Navigation** — Login as student, screenshot the navbar
3. **Anonymous Navigation** — Logout, screenshot the navbar
4. **Successful Login** — Login as any user, screenshot the welcome message
5. **Successful Course Registration** — As student, register for a course, screenshot success message
6. **Access Denied page** — As student, try accessing `/Students`, screenshot the Access Denied page
7. **Student Profile page** — As student, go to My Profile, screenshot the profile

### Additional Screenshot:
8. **SQL Server Database** — Open SQL Server Object Explorer in Visual Studio (View → SQL Server Object Explorer) → Expand `StudentRegistrationWebApp` database → Screenshot showing Identity tables (AspNetUsers, AspNetRoles, etc.) and application tables (Courses, CourseRegistrations)

---

## Step 7: Prepare Submission

Compile these into a single PDF or DOCX:
1. **Source code** — All `.cs` files (controllers, models, data) or the full project folder
2. **SQL Server screenshot** — Database with Identity + application tables
3. **7 UI screenshots** listed above

---

## Project Structure

```
student-registration-webapp/
├── Controllers/
│   ├── HomeController.cs                    — Home page (anonymous)
│   ├── AccountController.cs                 — Register, Login, Logout, AccessDenied
│   ├── CoursesController.cs                 — CRUD (Admin), View (all), Details (all)
│   ├── StudentsController.cs                — Student list (Admin only)
│   ├── ProfileController.cs                 — View/Edit own profile (Student only)
│   └── CourseRegistrationController.cs      — Register/Drop course (Student only)
├── Data/
│   ├── ApplicationDbContext.cs              — DbContext + Identity + relationships
│   └── DbInitializer.cs                    — Seeds roles, admin, courses on startup
├── Models/
│   ├── ApplicationUser.cs                   — extends IdentityUser (FullName, Phone, Address)
│   ├── Course.cs                            — Course entity (Code, Name, Credits, Instructor)
│   ├── CourseRegistration.cs                — Student-Course 1:1 mapping
│   └── ErrorViewModel.cs
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml                   — Role-based navigation menu + email display
│   │   └── Error.cshtml
│   ├── Home/Index.cshtml                    — Welcome page
│   ├── Account/
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   └── AccessDenied.cshtml              — Access Denied page
│   ├── Courses/
│   │   ├── Index.cshtml                     — Course list (role-based buttons)
│   │   ├── Details.cshtml
│   │   ├── Create.cshtml                    — Admin only
│   │   ├── Edit.cshtml                      — Admin only
│   │   └── Delete.cshtml                    — Admin only
│   ├── Students/Index.cshtml                — Admin: all students with courses
│   ├── Profile/
│   │   ├── Index.cshtml                     — Student: own profile
│   │   └── Edit.cshtml                      — Student: edit profile
│   └── CourseRegistration/
│       └── Register.cshtml                  — Student: register for one course
├── wwwroot/
│   ├── css/site.css
│   └── js/site.js
├── Properties/
│   └── launchSettings.json
├── Program.cs                               — Identity + DI + middleware + DB seed
├── appsettings.json                         — Connection string (LocalDB)
├── StudentRegistrationWebApp.csproj          — .NET 8 + NuGet packages
├── .gitignore
└── README.md
```

---

## Authorization Summary

| Controller | Action | Attribute | Who Can Access |
|---|---|---|---|
| Home | Index | `[AllowAnonymous]` | Everyone |
| Courses | Index | `[AllowAnonymous]` | Everyone |
| Courses | Details | `[AllowAnonymous]` | Everyone |
| Courses | Create | `[Authorize(Roles="Admin")]` | Admin only |
| Courses | Edit | `[Authorize(Roles="Admin")]` | Admin only |
| Courses | Delete | `[Authorize(Roles="Admin")]` | Admin only |
| Students | Index | `[Authorize(Roles="Admin")]` | Admin only |
| Profile | Index | `[Authorize(Roles="Student")]` | Student only |
| Profile | Edit | `[Authorize(Roles="Student")]` | Student only |
| CourseRegistration | Register | `[Authorize(Roles="Student")]` | Student only |
| CourseRegistration | Drop | `[Authorize(Roles="Student")]` | Student only |
| Account | Login | `[AllowAnonymous]` | Everyone |
| Account | Register | `[AllowAnonymous]` | Everyone |
| Account | AccessDenied | `[AllowAnonymous]` | Everyone |
| Account | Logout | `[Authorize]` | Authenticated users |

---

## Default Credentials

| Role | Email | Password |
|---|---|---|
| Admin | admin@studentapp.com | Admin@123 |

Students create their own accounts via the Register page.

---

## Troubleshooting

### "Cannot connect to LocalDB"
- Open Visual Studio Installer → Modify → ensure "SQL Server Express LocalDB" is installed
- Or install from: https://www.microsoft.com/en-us/sql-server/sql-server-downloads (scroll to "Express" edition)

### "No migrations found"
- Run `Add-Migration InitialCreate` in Package Manager Console first, then `Update-Database`

### "Access Denied on all pages after login"
- The `DbInitializer` may not have seeded roles. Check SQL Server Object Explorer → AspNetRoles table. Should have "Admin" and "Student" rows.

### Port already in use
- Edit `Properties/launchSettings.json` and change the port numbers

---

## Assignment Requirements Checklist

- [x] Part 1 — Authentication (Identity, Register, Login, auto-assign Student role, email in navbar)
- [x] Part 2 — Authorization (Admin: CRUD courses + view students; Student: view courses + register one + profile)
- [x] Part 3 — Navigation Menu (role-based: Anonymous, Student, Admin)
- [x] Part 4 — Security ([Authorize] attributes, no URL bypass, Access Denied page)
- [x] Part 5 — UI (welcome message, email in navbar, success messages, Access Denied page)
- [x] Submission — source code, DB screenshot, 7 UI screenshots
